using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace UnityMugen.Combat
{

    public class EntityCollection : IEnumerable<Entity>
    {
        public struct Enumerator : IEnumerator<Entity>
        {
            public Enumerator(EntityCollection collection)
            {
                if (collection == null) throw new ArgumentNullException(nameof(collection));

                m_collection = collection;
                m_current = null;
                m_index = 0;
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(m_collection);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (m_collection == null) return false;

                var firstcount = m_collection.m_entities.Count;
                var totalcount = firstcount + m_collection.m_addlist.Count;

                for (; m_index < totalcount; ++m_index)
                {
                    try
                    {
                        if (m_index < firstcount)
                            m_current = m_collection.m_entities.ElementAt(m_index).Value;
                        else
                            m_current = m_collection.m_addlist[m_index - firstcount];
                    }
                    catch
                    {
                        UnityEngine.Debug.LogWarning("Error ao ler um index inexistente.");
                    }
                    if (m_collection.m_removelist.Contains(m_current.UniqueID) == false)
                    {
                        ++m_index;
                        return true;
                    }
                }

                m_current = null;
                return false;



                //while ((uint)m_index < (uint)m_collection.m_entities.Count)
                //{
                //    if (m_collection.m_entities[m_index].UniqueID >= 0)
                //    {
                //        m_current = m_collection.m_entities[m_index];
                //        m_index++;
                //        return true;
                //    }
                //    m_index++;
                //}
                //m_index = m_collection.m_entities.Count + 1;
                //m_current = default(Entity);
                //return false;
            }

            public void Reset()
            {
                m_current = null;
                m_index = 0;
            }

            public Entity Current => m_current;

            object IEnumerator.Current => m_current;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly EntityCollection m_collection;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Entity m_current;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private int m_index;

        }


        public EntityCollection()
        {
            m_entities = new SortedDictionary<long, Entity>();
            m_addlist = new List<Entity>();
            m_removelist = new HashSet<long>();
            m_tempqueue = new List<Entity>();
            m_inupdate = false;
            m_drawordercomparer = DrawOrderComparer;
            m_updateordercomparer = UpdateOrderComparer;
            m_removecheck = DrawRemoveCheck;
        }

        public bool Contains(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            foreach (var e in this)
            {
                if (ReferenceEquals(e, entity))
                    return true;
            }

            return false;
        }

        public void Add(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (Contains(entity)) throw new ArgumentException("Entity is already part of collection");

            if (m_inupdate == false)
            {
                m_entities.Add(entity.UniqueID, entity);
            }
            else
            {
                m_addlist.Add(entity);
            }

            if (entity.typeEntity == TypeEntity.Helper)
            {
                var helper = (Helper)entity;

                List<Helper> helpers;
                if (helper.BasePlayer.Helpers.TryGetValue(helper.Id, out helpers) == false)
                {
                    helpers = new List<Helper>();
                    helper.BasePlayer.Helpers.Add(helper.Id, helpers);
                }

                helpers.Add(helper);
            }
            else if (entity.typeEntity == TypeEntity.Explod)
            {
                var explod = (Explod)entity;

                if (explod.Creator.Explods.TryGetValue(explod.Id, out List<Explod> explods) == false)
                {
                    explods = new List<Explod>();
                    explod.Creator.Explods.Add(explod.Id, explods);
                }

                explods.Add(explod);
            }
            else if (entity.typeEntity == TypeEntity.GraphicUI)
            {
                var graphicUI = (GraphicUIEntity)entity;

                if (graphicUI.Creator.GraphicUIs.TryGetValue(graphicUI.Id, out List<GraphicUIEntity> graphicUIs) == false)
                {
                    graphicUIs = new List<GraphicUIEntity>();
                    graphicUI.Creator.GraphicUIs.Add(graphicUI.Id, graphicUIs);
                }

                graphicUIs.Add(graphicUI);
            }
        }

        public void Remove(Entity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (Contains(entity) == false)
                throw new ArgumentException("Entity is not part of collection");
            if (m_removelist.Contains(entity.UniqueID))
                throw new ArgumentException("Entity is already set to be removed from collection");

            entity.AfterImages.ResetFE();

            if (m_inupdate == false)
            {
                m_entities.Remove(entity.UniqueID);
                m_addlist.Remove(entity);
            }
            else
            {
                m_removelist.Add(entity.UniqueID);
            }

            if (entity.typeEntity == TypeEntity.Helper)
            {
                var helper = (Helper)entity;
                helper.BasePlayer.Helpers[helper.Id].Remove(helper);
            }

            if (entity.typeEntity == TypeEntity.Explod)
            {
                var explod = (Explod)entity;
                if (explod.Creator.Explods.ContainsKey(explod.Id))
                {
                    explod.Creator.Explods[explod.Id].Remove(explod);
                    if (explod.Creator.Explods[explod.Id].Count == 0)
                        explod.Creator.Explods.Remove(explod.Id);
                }
            }

            if (entity.typeEntity == TypeEntity.GraphicUI)
            {
                var graphicUI = (GraphicUIEntity)entity;
                graphicUI.Creator.GraphicUIs[graphicUI.Id].Remove(graphicUI);
            }
        }

        public void Clear()
        {
            m_addlist.Clear();
            m_entities.Clear();
            m_removelist.Clear();
        }

        private void AddEntities()
        {
            foreach (var entity in m_addlist)
            {
                m_entities.Add(entity.UniqueID, entity);
            }

            m_addlist.Clear();
        }

        private void RemoveEntities()
        {
            foreach (var id in m_removelist)
            {
                m_addlist.Remove(m_entities[id]);
                m_entities.Remove(id);
            }

            m_removelist.Clear();
        }

        private void RemoveCheck()
        {
            foreach (var entity in this)
            {
                if (entity.RemoveCheck())
                    Remove(entity);
            }
        }

        public void CountEntities(out int players, out int helpers, out int explods, out int projectiles, out int graphicUIs)
        {
            players = 0;
            helpers = 0;
            explods = 0;
            projectiles = 0;
            graphicUIs = 0;

            foreach (var entity in this)
            {
                if (entity is Player) ++players;
                else if (entity is Helper) ++helpers;
                else if (entity is Explod) ++explods;
                else if (entity is Projectile) ++projectiles;
                else if (entity is GraphicUIEntity) ++graphicUIs;
            }
        }

        public void UpdateFE()
        {
            m_inupdate = true;

            AddEntities();
            RemoveCheck();
            RemoveEntities();

            m_tempqueue.Clear();

            foreach (var entity in this)
            {
                if (Engine.SuperPause.IsPaused(entity) || Engine.Pause.IsPaused(entity))
                    continue;
                m_tempqueue.Add(entity);
            }

            RunEntityUpdates(m_tempqueue);
            //m_tempqueue.Clear();

            while (m_addlist.Count > 0)
            {
                foreach (var entity in m_addlist)
                {
                    if (Engine.SuperPause.IsPaused(entity) || Engine.Pause.IsPaused(entity)) continue;
                    m_tempqueue.Add(entity);
                }

                AddEntities();

                RunEntityUpdates(m_tempqueue);
            }

            RemoveCheck();
            RemoveEntities();

            m_inupdate = false;
        }

        private void RunEntityUpdates(List<Entity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            entities.Sort(m_updateordercomparer);

            foreach (var entity in entities) entity.CleanUp();

            foreach (var entity in entities) entity.UpdateInput();

            foreach (var entity in entities) entity.UpdateAnimations();

            foreach (var entity in entities) entity.UpdateState();

            // Novo de acordo com IK | pode ser que isso 
            // tenha que ficar no final de todos os foreach
            foreach (var entity in entities) entity.ActionFinish();

            foreach (var entity in entities) entity.UpdateAfterImages();

            foreach (var entity in entities) entity.UpdatePhsyics();

            entities.Clear();
        }

        public void Draw(bool debug)
        {
            m_tempqueue.Clear();
            foreach (var entity in this)
            {
                if (entity.RemoveCheck()) continue;
                m_tempqueue.Add(entity);
            }

            m_tempqueue.Sort(m_drawordercomparer);

            //Point p = Engine.CameraFE.Location;
            //Camera.main.transform.position = new Vector3(p.X, -p.Y, Camera.main.transform.position.z);

            for (int i = 0; i < m_tempqueue.Count; i++)
                m_tempqueue[i].Draw(i);

            foreach (var entity in m_tempqueue)
                entity.DebugDraw();

            m_tempqueue.Clear();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<Entity> IEnumerable<Entity>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private int UpdateOrderComparer(Entity lhs, Entity rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            if (lhs == rhs) return 0;

            var order = lhs.UpdateOrder - rhs.UpdateOrder;
            if (order != 0) return order;

            if (lhs is Character && rhs is Character)
            {
                var clhs = lhs as Character;
                var crhs = rhs as Character;

                var tlhs = clhs.MoveType == MoveType.BeingHit;
                var trhs = crhs.MoveType == MoveType.BeingHit;

                if (tlhs && trhs == false)
                {
                    return 1;
                }

                if (tlhs == false && trhs)
                {
                    return -1;
                }
            }

            return 0;
            //return m_entities.IndexOf(lhs) - m_entities.IndexOf(rhs);
        }

        private int DrawOrderComparer(Entity lhs, Entity rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            if (lhs == rhs) return 0;

            var order = lhs.DrawOrder - rhs.DrawOrder;
            if (order != 0) return order;

            if (lhs is Player && rhs is Player == false) return 1;
            if (rhs is Player && lhs is Player == false) return -1;

            return 0;
            //return m_entities.IndexOf(lhs) - m_entities.IndexOf(rhs);
        }

        private bool DrawRemoveCheck(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return m_removelist.Contains(entity.UniqueID);// || Engine.EnvironmentColor.IsHidden(entity);
        }

        public bool TryGetValue(long index, out Entity value)
        {
            value = null;
            if (m_entities.TryGetValue(index, out Entity entity))
                value = entity;
            else
            {
                foreach (Entity entityAdd in m_addlist)
                {
                    if (entityAdd.UniqueID == index)
                    {
                        value = entityAdd;
                        break;
                    }
                }
            }

            if (value != null && m_removelist.Contains(value.UniqueID) == false)
            {
                return true;
            }

            return false;
        }

        public FightEngine Engine => LauncherEngine.Inst.mugen.Engine;
        #region Fields

        public int EntitiesCount => m_entities.Count;
        private SortedDictionary<long, Entity> m_entities;
        private List<Entity> m_addlist;
        public HashSet<long> m_removelist;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Entity> m_tempqueue;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Comparison<Entity> m_updateordercomparer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Comparison<Entity> m_drawordercomparer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Predicate<Entity> m_removecheck;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_inupdate;

        #endregion
    }

}