using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityMugen.Combat
{
    public class Pause
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;

        public Pause(bool superpause)
        {
            m_issuperpause = superpause;
            m_creator = null;
            m_totaltime = 0;
            m_elapsedtime = -1;
            Commandbuffertime = 0;
            m_movetime = 0;
            Hitpause = false;
            Pausebackgrounds = true;
            m_pausedentities = new List<Entity>();
        }

        public Pause(Pause pause)
        {
            Commandbuffertime = pause.Commandbuffertime;
            m_issuperpause = pause.IsSuperPause;
            m_creator = pause.Creator;
            m_movetime = pause.MoveTime;
            m_elapsedtime = pause.ElapsedTime;
            m_totaltime = pause.Totaltime;
            Hitpause = pause.Hitpause;
            Pausebackgrounds = pause.Pausebackgrounds;
            m_pausedentities = pause.m_pausedentities;
        }


        public void BackMemory(Pause pause)
        {
            Commandbuffertime = pause.Commandbuffertime;
            m_issuperpause = pause.IsSuperPause;
            m_creator = pause.Creator;
            m_movetime = pause.MoveTime;
            m_elapsedtime = pause.ElapsedTime;
            m_totaltime = pause.Totaltime;
            Hitpause = pause.Hitpause;
            Pausebackgrounds = pause.Pausebackgrounds;
        }

        public void ResetFE()
        {
            m_creator = null;
            m_totaltime = 0;
            m_elapsedtime = -1;
            Commandbuffertime = 0;
            m_movetime = 0;
            Hitpause = false;
            Pausebackgrounds = true;
            m_pausedentities.Clear();
        }

        public void UpdateFE()
        {
            if (IsActive)
            {
                if (m_elapsedtime == 0)
                {
                    foreach (var entity in Engine.Entities)
                        m_pausedentities.Add(entity);
                }
                ++m_elapsedtime;
            }
            else
            {
                ResetFE();
            }
        }

        public void Set(Character creator, int time, int buffertime, int movetime, bool hitpause, bool pausebackgrounds)
        {
            if (creator == null) throw new ArgumentNullException(nameof(creator));

            ResetFE();

            m_creator = creator;
            m_totaltime = time;
            m_elapsedtime = 0;
            Commandbuffertime = buffertime;
            m_movetime = movetime;
            Hitpause = hitpause;
            Pausebackgrounds = pausebackgrounds;

            //foreach (var entity in Engine.Entities)
            //    m_pausedentities.Add(entity);
        }

        public bool IsPaused(Entity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (m_pausedentities.Contains(entity) == false)
                return false;

            return entity.IsPaused(this);
        }

        public bool IsPaused(Stage stage)
        {
            if (stage == null) throw new ArgumentNullException(nameof(stage));

            if (IsActive == false) return false;

            return Pausebackgrounds;
        }

        public bool IsActive => m_elapsedtime >= 0 && m_elapsedtime <= m_totaltime;

        public int Commandbuffertime { get; set; }
        public bool IsSuperPause => m_issuperpause;
        public Character Creator => m_creator;
        public int MoveTime => m_movetime;
        public int ElapsedTime => m_elapsedtime;
        public int Totaltime => m_totaltime;
        public bool Hitpause { get; set; }
        public bool Pausebackgrounds;
        
        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_issuperpause;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Character m_creator;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_totaltime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_elapsedtime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_movetime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Entity> m_pausedentities;

        #endregion
    }
}