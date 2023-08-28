using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityMugen.Audio;
using UnityMugen.Commands;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat
{
    public abstract class Character : Entity
    {
        public abstract SoundManager SoundManager { get; }
        public abstract StateManager StateManager { get; }
        public abstract CharacterDimensions Dimensions { get; }
        public abstract ICommandManager CommandManager { get; }

        public Vector2 DrawScale { get; set; }
        public Vector2 DrawOffset { get; set; }
        public int RoundsExisted { get; set; }
        public bool CameraFollowX { get; set; }
        public bool CameraFollowY { get; set; }
        public bool ScreenBound { get; set; }
        public bool PushFlag { get; set; }
        public PlayerMode PlayerMode;// { get; set; }
        public float LastTickHitable { get; set; } // Usado no Modo Trainner
        public float VelOff { get; set; } // Novo Teste Tiago
        public ContactType ContactType { get; set; } // Novo Teste Tiago
        public int JugglePoints { get; set; }
        public PlayerButton CurrentInput { get; set; }
        public int FallTime { get; set; } // Novo Teste Tiago

        public override EntityUpdateOrder UpdateOrder => EntityUpdateOrder.Character;

        public ForceFeedbackJoy ForceFeedbackJoy => m_forceFeedbackJoy;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ForceFeedbackJoy m_forceFeedbackJoy;

        public CharacterBind Bind => m_bind;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CharacterBind m_bind;

        public CharacterVariables Variables { get => m_variables; set => m_variables = value; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CharacterVariables m_variables;

        public CharacterAssertions Assertions => m_assertions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CharacterAssertions m_assertions;

        public bool UpdatedAnimation => m_updatedanimation;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_updatedanimation;

        public StringBuilder Clipboard => m_clipboard;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StringBuilder m_clipboard;

        public Dictionary<long, List<Combat.Explod>> Explods { get => m_explods; set => m_explods = value; } 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<long, List<Combat.Explod>> m_explods;

        public Dictionary<long, List<GraphicUIEntity>> GraphicUIs => m_graphicUIs;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<long, List<GraphicUIEntity>> m_graphicUIs;

        public OffensiveInfo OffensiveInfo { get => m_offensiveinfo; set => m_offensiveinfo = value; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private OffensiveInfo m_offensiveinfo;

        public DefensiveInfo DefensiveInfo { get => m_defensiveinfo; set => m_defensiveinfo = value; }
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DefensiveInfo m_defensiveinfo;

        [NonSerialized] public PlayerID m_PlayerNumber;

        private MoveType m_movetype;
        private UnityMugen.Physic m_physics;
        private StateType m_statetype;
        private PlayerControl m_playercontrol;
        private float m_life;

        private Boolean m_inhitpause;
        private Boolean m_positionfreeze;


        public void IniciarCharacter()
        {
            m_statetype = StateType.Standing;
            m_playercontrol = PlayerControl.InControl;
            m_movetype = MoveType.Idle;
            m_physics = UnityMugen.Physic.Standing;
            m_life = 0;
            FallTime = 0;
            DrawOffset = new Vector2(0, 0);
            PositionFreeze = false;
            RoundsExisted = 0;
            m_bind = new CharacterBind(this as Character);
            m_assertions = new CharacterAssertions();
            JugglePoints = 0;
            m_variables = new CharacterVariables();
            m_clipboard = new StringBuilder();
            CurrentInput = PlayerButton.None;
            PushFlag = false;
            DrawScale = Vector2.one;
            m_offensiveinfo = new OffensiveInfo(this as Character);
            m_defensiveinfo = new DefensiveInfo(this as Character);
            m_updatedanimation = false;
            m_explods = new Dictionary<long, List<Explod>>();
            m_graphicUIs = new Dictionary<long, List<GraphicUIEntity>>();
            m_forceFeedbackJoy = new ForceFeedbackJoy(this);
        }


        public override void ResetFE()
        {
            base.ResetFE();

            m_statetype = StateType.Standing;
            m_playercontrol = PlayerControl.InControl;
            m_movetype = MoveType.Idle;
            m_physics = UnityMugen.Physic.Standing;
            m_life = 0;
            FallTime = 0;
            DrawOffset = new Vector2(0, 0);
            PositionFreeze = false;

#warning isso ainda nao foi aplicado e nao sei se irei aplicar
            RoundsExisted = 0;

            m_bind.ResetFE();
            m_assertions.ResetFE();
            JugglePoints = 0;
            m_variables.ResetFE();
            m_clipboard.Length = 0;
            CurrentInput = PlayerButton.None;
            PushFlag = false;
            DrawScale = Vector2.one;
            m_offensiveinfo.ResetFE();
            m_defensiveinfo.ResetFE();
            m_updatedanimation = false;
            m_explods.Clear();
            m_forceFeedbackJoy.ResetFE();
        }

        public override void Draw(int orderDraw)
        {
            // Usado no Modo Trainner
            if (MoveType == MoveType.BeingHit)
                LastTickHitable = Engine.TickCount;
            /////////

            base.Draw(orderDraw);
        }

        public override void DebugDraw()
        {
            base.DebugDraw();
        }

        public override void CleanUp()
        {
            if (OffensiveInfo.HitPauseTime > 1)
            {
                InHitPause = true;
                --OffensiveInfo.HitPauseTime;
            }
            else
            {
                InHitPause = false;
                OffensiveInfo.HitPauseTime = 0;
            }

            if (InHitPause == false)
            {
                base.CleanUp();

                CameraFollowX = true;
                CameraFollowY = true;
                ScreenBound = true;
                PositionFreeze = false;
                Assertions.ResetFE();
                Dimensions.ClearOverride();
                DrawOffset = Vector2.zero;
                PushFlag = false;
                DrawScale = Vector2.one;
                m_updatedanimation = false;

                DoFriction();
            }
        }

        public override void UpdateAnimations()
        {
            if (InHitPause == false)
            {
                base.UpdateAnimations();
                m_updatedanimation = true;
            }
            else
            {
                m_updatedanimation = false;
            }
        }

        public override void UpdateAfterImages()
        {
            if (InHitPause == false)
            {
                base.UpdateAfterImages();
            }
        }

        public override void UpdateState()
        {
            Bind.UpdateFE();

            // Em teste - Esta funcionando, só não sei se é o melhor lugar para colocar este codigo.
            if (InHitPause == false)
            {
                if (OffensiveInfo.P1NewState >= 0 && 
                    OffensiveInfo.MoveHit != 0)
                {
                    StateManager.ChangeState(OffensiveInfo.P1NewState);
                    OffensiveInfo.P1NewState = -1;
                }

                if (
                    OffensiveInfo.P2NewState >= 0 &&
                    OffensiveInfo.TargetList.Count > 0)
                {
                    foreach (Character c in OffensiveInfo.TargetList)
                    {
                        if (OffensiveInfo.HitDef.P2UseP1State)
                            c.StateManager.ForeignManager = StateManager;
                        else
                            c.StateManager.ForeignManager = null;

                        c.StateManager.ChangeState(OffensiveInfo.P2NewState);
                    }
                    OffensiveInfo.P2NewState = -1;
                }

            }
            // --Teste

            StateManager.Run(InHitPause);

            if (InHitPause == false)
            {
                ForceFeedbackJoy.UpdateFE();
                OffensiveInfo.UpdateFE();
                DefensiveInfo.UpdateFE();
            }
        }

        public override void ActionFinish()
        {
            if (InHitPause == false)
            {
                // Tem mais codigo pra cima | IK
                DefensiveInfo.HitDef.HitDamage = 0;
                DefensiveInfo.HitDef.GuardDamage = 0;
                DefensiveInfo.HitDef.P1GuardPowerAdjustment = 0;
                DefensiveInfo.HitDef.P1HitPowerAdjustment = 0;
                DefensiveInfo.HitDef.P2GuardPowerAdjustment = 0;
                DefensiveInfo.HitDef.P2HitPowerAdjustment = 0;
                // Tem mais codigo pra baixo | IK
            }
        }

        public override void UpdatePhsyics()
        {
            var aas = this;
            if (InHitPause || DefensiveInfo.HitShakeTime > 0) return;

            CurrentVelocity += CurrentAcceleration;

            float newVelOff = 0;
            if (Engine.SuperPause.IsActive == false)
            {
                newVelOff = VelOff;
            }

            if (PositionFreeze == false)
            {
                Vector2 newVel = new Vector2(newVelOff + CurrentVelocity.x, CurrentVelocity.y);
                base.Move(newVel);
            }

            switch (Physics)
            {
                case UnityMugen.Physic.Standing:
                    if (CurrentLocation.y >= 0) CurrentLocation = new Vector2(CurrentLocation.x, 0);
                    break;

                case UnityMugen.Physic.Crouching:
                    if (CurrentLocation.y >= 0) CurrentLocation = new Vector2(CurrentLocation.x, 0);
                    break;
            }

            HandleTurning();
            HandlePushing();

            if (Engine.SuperPause.IsActive == false)
            {
                VelOff *= (0.7f);

                if (Mathf.Abs(VelOff) < (1 * Constant.Scale))
                {
                    VelOff = 0;
                }
            }
        }

        public override void UpdateInput()
        {
            if (CommandManager.IsHuman())
                CommandManager.UpdateFE(CurrentInput, CurrentFacing, InHitPause);
            else if (Engine.RoundState != RoundState.PreIntro &&
                Engine.RoundState != RoundState.Intro)
                CommandManager.UpdateFE(CurrentInput, CurrentFacing, InHitPause);
        }

        //public virtual void RecieveInput(PlayerButton button, bool pressed)
        //{
        //    if (pressed)
        //    {
        //        m_currentinput |= button;
        //    }
        //    else
        //    {
        //        m_currentinput &= ~button;
        //    }
        //}

        private void DoFriction()
        {
            switch (Physics)
            {
                case UnityMugen.Physic.Standing:
                    CurrentVelocity *= (new Vector2(BasePlayer.playerConstants.Standfriction, 1));
                    ZeroCheckVelocity();
                    break;

                case UnityMugen.Physic.Crouching:
                    CurrentVelocity *= (new Vector2(BasePlayer.playerConstants.Crouchfriction, 1));
                    ZeroCheckVelocity();
                    break;

                case UnityMugen.Physic.Airborne:
                    CurrentVelocity += (new Vector2(0, BasePlayer.playerConstants.Vert_acceleration * Constant.Scale));
                    break;
            }
        }

        private void ZeroCheckVelocity()
        {
            var velocity = CurrentVelocity;

            velocity.x = velocity.x > (-1.0f * Constant.Scale) && velocity.x < (1.0f * Constant.Scale) ? 0 : velocity.x;

            CurrentVelocity = velocity;
        }

        protected virtual Boolean HandleTurning()
        {
            if (PlayerControl == PlayerControl.NoControl) return false;
            if (CurrentLocation.y != 0 || PlayerControl == PlayerControl.NoControl) return false;

            Player closest = GetOpponent();
            if (closest == null) return false;
            if (CurrentFacing == Facing.Right && CurrentLocation.x <= closest.CurrentLocation.x) return false;
            if (CurrentFacing == Facing.Left && CurrentLocation.x >= closest.CurrentLocation.x) return false;

            if (StateManager.CurrentState.number == StateNumber.Standing && AnimationManager.CurrentAnimation.Number != 5 && Assertions.NoAutoTurn == false)
            {
                CurrentFacing = FlipFacing(CurrentFacing);
                SetLocalAnimation(5, 0);
                return true;
            }

            if (StateManager.CurrentState.number == StateNumber.Walking && AnimationManager.CurrentAnimation.Number != 5 && Assertions.NoAutoTurn == false)
            {
                CurrentFacing = FlipFacing(CurrentFacing);
                SetLocalAnimation(5, 0);
                return true;
            }

            if (StateManager.CurrentState.number == StateNumber.Crouching && AnimationManager.CurrentAnimation.Number != 6 && Assertions.NoAutoTurn == false)
            {
                CurrentFacing = FlipFacing(CurrentFacing);
                SetLocalAnimation(6, 0);
                return true;
            }

            return false;
        }

        protected virtual void HandlePushing()
        {
            if (PushFlag == false) return;

            foreach (var entity in Engine.Entities)
            {
                var o = FilterEntityAsCharacter(entity, AffectTeam.Enemy);
                if (o == null) continue;

                if (o.PushFlag == false) continue;

                if (Collision.HasCollision(this, ClsnType.Type2Normal, o, ClsnType.Type2Normal) == false) continue;

                if (CurrentLocation.x > o.CurrentLocation.x)
                {
                    var rhs_pos = o.GetRightLocation();
                    var lhs_pos = GetLeftLocation();

                    var overlap = rhs_pos - lhs_pos;

                    if (overlap > 0)
                    {
                        var actualpush = o.MoveLeft(new Vector2(overlap * .5f, 0));

                        //if (actualpush.x != -overlap)
                        //{
                        //      var reversepush = overlap - actualpush.x;
                        //      MoveRight(new Vector2(reversepush, 0));
                        //}
                    }
                }
                else if (CurrentLocation.x < o.CurrentLocation.x)
                {
                    var lhs_pos = GetRightLocation();
                    var rhs_pos = o.GetLeftLocation();

                    var overlap = lhs_pos - rhs_pos;

                    if (overlap > 0)
                    {
                        var actualpush = o.MoveRight(new Vector2(overlap * .5f, 0));

                        //if (actualpush.x != overlap)
                        //{
                        //    var reversepush = overlap - actualpush.x;
                        //    MoveLeft(new Vector2(reversepush, 0));
                        //}
                    }
                }
                else if (CurrentLocation.x == o.CurrentLocation.x)
                {
                    if (CurrentFacing == Facing.Left)
                    {
                        MoveLeft(new Vector2(-1 * Constant.Scale, 0));
                        o.MoveRight(new Vector2(-1 * Constant.Scale, 0));
                    }
                    else if (CurrentFacing == Facing.Right)
                    {
                        o.MoveLeft(new Vector2(-1 * Constant.Scale, 0));
                        MoveRight(new Vector2(-1 * Constant.Scale, 0));
                    }
                }
            }

        }

        //public override Vector2 Move(Vector2 p)
        //{
        //    return base.Move(p);


        //    //if (PositionFreeze == false)
        //    //{
        //    //    return base.Move(p);
        //    //}

        //    //return new Vector2();

        //}

        protected override void Bounding()
        {
            base.Bounding();
            //Point p = Engine.Stage.PlayerBounds.Bound(new Point((int)CurrentLocation.X, (int)CurrentLocation.Y));
            //CurrentLocation = new Vector2(p.X, p.Y);

            if (ScreenBound)
            {
                var screenrect = Engine.CameraFE.ScreenBounds();

                float newleft = screenrect.xMin - GetLeftEdgePosition(true);
                float newright = screenrect.xMax - GetRightEdgePosition(true);

                // isso ainda nao esta certo dando problemas quando o 
                // personagem anda para tras e ele esta no canto da tela
                //     ele acaba arrastando o outro personagem junto 
                if (GetLeftEdgePosition(true) < screenrect.xMin)
                {
                    //CurrentLocation += new Vector2((newleft / 2), 0);
                    CurrentLocation += new Vector2(newleft, 0); //Original
                }

                if (GetRightEdgePosition(true) > screenrect.xMax)
                {
                    //CurrentLocation += new Vector2((newright / 2), 0);
                    CurrentLocation += new Vector2(newright, 0); //Original
                }
            }
        }

        public override Vector2 GetDrawLocation(bool startLoacation = false)
        {
            return CurrentLocation + Misc.GetOffset(Vector2.zero, CurrentFacing, DrawOffset);
        }

        public Vector2 GetDrawLocationYTransform()
        {
            Vector3 invertYChar = CurrentLocationYTransform();
            return new Vector2(invertYChar.x, invertYChar.y) + Misc.GetOffset(Vector2.zero, CurrentFacing, DrawOffset);
        }

        public Player GetOpponent()
        {
            foreach (var entity in Engine.Entities)
            {
                if (!(entity is Player)) continue;

                Player player = entity as Player;
                if (Team != player.Team)
                    return player;
            }

            return null;
        }

        public float GetLeftLocation()
        {
            if (CurrentFacing == Facing.Right)
            {
                return GetBackLocation();
            }

            return GetFrontLocation();
        }

        public float GetRightLocation()
        {
            if (CurrentFacing == Facing.Right)
            {
                return GetFrontLocation();
            }

            return GetBackLocation();
        }

        public float GetFrontLocation()
        {
            if (CurrentFacing == Facing.Right)
            {
                return CurrentLocation.x + Dimensions.GetFrontWidth(StateType);
            }

            return CurrentLocation.x - Dimensions.GetFrontWidth(StateType);
        }

        public float GetBackLocation()
        {
            if (CurrentFacing == Facing.Right)
            {
                return CurrentLocation.x - Dimensions.GetBackWidth(StateType);
            }

            return CurrentLocation.x + Dimensions.GetBackWidth(StateType);
        }

        public float GetLeftEdgePosition(bool body)
        {
            if (CurrentFacing == Facing.Left)
            {
                var position = CurrentLocation.x - Engine.stageScreen.Stage.LeftEdgeDistance;
                if (body) position -= Dimensions.FrontEdgeWidth;

                return position;
            }

            if (CurrentFacing == Facing.Right)
            {
                var position = CurrentLocation.x - Engine.stageScreen.Stage.LeftEdgeDistance;
                if (body) position -= Dimensions.BackEdgeWidth;

                return position;
            }

            return 0;
        }

        public float GetRightEdgePosition(bool body)
        {
            if (CurrentFacing == Facing.Left)
            {
                var position = CurrentLocation.x + Engine.stageScreen.Stage.RightEdgeDistance;
                if (body) position += Dimensions.BackEdgeWidth;

                return position;
            }

            if (CurrentFacing == Facing.Right)
            {
                var position = CurrentLocation.x + Engine.stageScreen.Stage.RightEdgeDistance;
                if (body) position += Dimensions.FrontEdgeWidth;

                return position;
            }

            return 0;
        }

        public Character FilterEntityAsCharacter(Entity entity, AffectTeam team)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var c = entity as Character;
            if (c == null || c == this) return null;

            if ((team & AffectTeam.Enemy) != AffectTeam.Enemy && Team != c.Team) return null;
            if ((team & AffectTeam.Friendly) != AffectTeam.Friendly && Team == c.Team) return null;

            return c;
        }

        public Projectile FilterEntityAsProjectile(Entity entity, int projid)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var proj = entity as Projectile;
            if (proj == null || proj.BasePlayer != this || projid > 0 && entity.Id != projid)
                return null;

            return proj;
        }

        public Character FilterEntityAsPartner(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity == this) return null;
            if (entity == BasePlayer) return null;
            if (entity.Team != Team) return null;

            if (entity is Helper)
            {
                var helper = entity as Helper;
                return helper.Data.Type == HelperType.Player ? helper : null;
            }

            if (entity is Player)
            {
                return entity as Player;
            }

            return null;
        }

        public List<Character> GetTargets(int target_id)
        {
            List<Character> add = new List<Character>();
            if (target_id >= 0)
            {

                foreach (var character in OffensiveInfo.TargetList)
                {
                    //if (character.AttackInfo.HitTime == 0) continue;
                    //if (character.MoveType != MoveType.BeingHit) continue;
                    //if (character.AttackInfo.Attacker != this) continue;

                    if (character.DefensiveInfo.HitDef.TargetId == target_id)
                        add.Add(character);
                }
            }
            else
            {
                foreach (var character in OffensiveInfo.TargetList)
                    add.Add(character);
            }
            return add;
        }

        public List<Explod> GetExplods(int id)
        {
            List<Explod> add = new List<Explod>();
            if (id >= 0)
            {
                if (Explods.TryGetValue(id, out List<UnityMugen.Combat.Explod> explods))
                {
                    foreach (var explod in explods)
                        add.Add(explod);
                }
            }
            else
            {
                foreach (var data in Explods)
                {
                    foreach (var explod in data.Value)
                        add.Add(explod);
                }
            }
            return add;
        }

        public List<GraphicUIEntity> GetGraphicUIs(long id)
        {
            List<GraphicUIEntity> add = new List<GraphicUIEntity>();
            if (id >= 0)
            {
                if (GraphicUIs.TryGetValue(id, out List<GraphicUIEntity> graphicUIs))
                {
                    foreach (GraphicUIEntity explod in graphicUIs)
                        add.Add(explod);
                }
            }
            else
            {
                foreach (var data in GraphicUIs)
                {
                    foreach (var graphicUI in data.Value)
                        add.Add(graphicUI);
                }
            }
            return add;
        }

        private void RemoveFromOthersTargetLists()
        {
            foreach (var entity in Engine.Entities)
            {
                var character = entity as Character;
                if (character == null) continue;

                character.OffensiveInfo.TargetList.Remove(this);
            }
        }








        public StateType StateType
        {
            get { return m_statetype; }

            set
            {
                if (value == StateType.None || value == StateType.Unchanged)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_statetype = value;
            }
        }
        public MoveType MoveType
        {
            get { return m_movetype; }

            set
            {
                if (value == MoveType.Unchanged)
                    throw new ArgumentOutOfRangeException(nameof(value));

                if (m_movetype == MoveType.BeingHit && value != MoveType.BeingHit)
                    RemoveFromOthersTargetLists();

                m_movetype = value;
            }
        }


        public float Life
        {
            get { return m_life; }

            set
            {
                // Se modo de Jogo igual  Trainner o personagem numca ira morrer
                if (Launcher.engineInitialization.Mode == CombatMode.Training ||
                    Assertions.NoKO == true)
                {
                    float maxHP = BasePlayer.playerConstants.MaximumLife;
                    if (Id == 0)
                        maxHP = BasePlayer.playerConstants.MaximumLife * (Launcher.trainnerSettings.percentP1HPMax * 10) / 100;
                    else
                        maxHP = BasePlayer.playerConstants.MaximumLife * (Launcher.trainnerSettings.percentP2HPMax * 10) / 100;

                    m_life = Misc.Clamp(value, 0.000000001f, maxHP);
                }
                else
                    m_life = Misc.Clamp(value, 0f, BasePlayer.playerConstants.MaximumLife);
            }
        }



        public PlayerControl PlayerControl
        {
            get { return m_playercontrol; }

            set
            {
                if (value != PlayerControl.InControl && value != PlayerControl.NoControl)
                    throw new ArgumentException("Value must be InControl or NoControl", "value");
                m_playercontrol = value;
            }
        }

        public static Facing FlipFacing(Facing input)
        {
            if (input == Facing.Left) return Facing.Right;

            if (input == Facing.Right) return Facing.Left;

            throw new ArgumentException("Not valid Facing", "input");
        }



        public Physic Physics
        {
            get { return m_physics; }
            set { m_physics = value; }
        }
        public Boolean InHitPause
        {
            get { return m_inhitpause; }
            set { m_inhitpause = value; }
        }
        public Boolean PositionFreeze
        {
            get { return m_positionfreeze; }
            set { m_positionfreeze = value; }
        }


        public void InstanceHelper(HelperData data, long? uniqueID = null)
        {
            GameObject GO = new GameObject("Helper");
            GO.tag = "Entity";
            Helper helper = GO.AddComponent<Helper>();
            helper.Iniciar(this, data);
            GO.transform.localPosition = helper.GetStartLocation();
            helper.transform.SetParent(Engine.stageScreen.Entities);
            if (uniqueID.HasValue) data.UniqueID = uniqueID.Value;
            helper.UniqueID = uniqueID.HasValue ? uniqueID.Value : Engine.GenerateCharacterId();
            Engine.Entities.Add(helper);

            // Adicionado Novo Teste Tiago
            if (this is Helper helper2)
            {
                helper2.Childrens.Add(helper);
            }
            ///////////////////////////////
        }

        public void InstanceProjectile(ProjectileData data, long? uniqueID = null)
        {
            GameObject GO = new GameObject("Projectile");
            GO.tag = "Entity";

            Projectile projectile = GO.AddComponent<Projectile>();
            projectile.Iniciar(this, data);
            GO.transform.localPosition = projectile.GetStartLocation();
            projectile.transform.SetParent(Engine.stageScreen.Entities);
            if (uniqueID.HasValue) data.UniqueID = uniqueID.Value;
            projectile.UniqueID = uniqueID.HasValue ? uniqueID.Value : Engine.GenerateCharacterId();
            Engine.Entities.Add(projectile);
        }

        public Explod InstanceExplod(ExplodData data, long? uniqueID = null)
        {
            GameObject GO = new GameObject("Explod");
            GO.tag = "Entity";
            Explod explod = GO.AddComponent<Explod>();
            explod.Iniciar(data);
            if (explod.IsValid)
            {
                GO.transform.localPosition = explod.GetDrawLocation(true);
                explod.transform.SetParent(Engine.stageScreen.Entities);
                if (uniqueID.HasValue) data.UniqueID = uniqueID.Value;
                explod.UniqueID = uniqueID.HasValue ? uniqueID.Value : Engine.GenerateCharacterId();
                Engine.Entities.Add(explod);
            }
            else
                Destroy(explod);

            return explod;
        }

        public void InstanceGraphicUI(string name, GraphicUIData data, long? uniqueID = null)
        {
            GameObject GO = new GameObject(name);
            GO.tag = "Entity";

            GraphicUIEntity graphicUIEntity = GO.AddComponent<GraphicUIEntity>();
            graphicUIEntity.Iniciar(this, data);

            if (graphicUIEntity.IsValid)
            {
                GO.transform.localPosition = data.pos.Value;

                if (data.layer == Layer.Back)
                    graphicUIEntity.transform.SetParent(Engine.stageScreen.HCBU.graphicUIBack.transform);
                else
                    graphicUIEntity.transform.SetParent(Engine.stageScreen.HCBU.graphicUIFront.transform);

                if (uniqueID.HasValue) data.UniqueID = uniqueID.Value;
                graphicUIEntity.UniqueID = uniqueID.HasValue ? uniqueID.Value : Engine.GenerateCharacterId();
                Engine.Entities.Add(graphicUIEntity);
            }
            else
                Destroy(graphicUIEntity);
        }

    }
}