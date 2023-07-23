using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityMugen.Combat
{
    public class EngineAssertions
    {
        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;


        public EngineAssertions()
        {
            ResetFE();
        }


        public EngineAssertions(EngineAssertions engineAssertions)
        {
            m_intro = engineAssertions.Intro;
            m_winpose = engineAssertions.WinPose;
            m_nobardisplay = engineAssertions.NoBarDisplay;
            m_nobacklayer = engineAssertions.NoBackLayer;
            m_nofrontlayer = engineAssertions.NoFrontLayer;
            m_nokosound = engineAssertions.NoKOSound;
            m_nokoslow = engineAssertions.NoKOSlow;
            m_noglobalshadow = engineAssertions.GlobalNoShadow;
            m_nomusic = engineAssertions.NoMusic;
            m_timerfreeze = engineAssertions.TimerFreeze;
        }

        public void BackMemory(EngineAssertions engineAssertions)
        {
            m_intro = engineAssertions.Intro;
            m_winpose = engineAssertions.WinPose;
            m_nobardisplay = engineAssertions.NoBarDisplay;
            m_nobacklayer = engineAssertions.NoBackLayer;
            m_nofrontlayer = engineAssertions.NoFrontLayer;
            m_nokosound = engineAssertions.NoKOSound;
            m_nokoslow = engineAssertions.NoKOSlow;
            m_noglobalshadow = engineAssertions.GlobalNoShadow;
            m_nomusic = engineAssertions.NoMusic;
            m_timerfreeze = engineAssertions.TimerFreeze;
        }

        public void UpdateFE()
        {
            if (NoFrontLayer)
            {
                if (Engine.stageScreen.Foreground)
                    Engine.stageScreen.Foreground.SetActive(false);
            }
            else
            {
                if (Engine.stageScreen.Foreground)
                    Engine.stageScreen.Foreground.SetActive(true);
            }
            if (NoBackLayer)
            {
                if (Engine.stageScreen.Background)
                {
                    Engine.stageScreen.Background.SetActive(false);
                    Engine.CameraFE.camera.clearFlags = CameraClearFlags.SolidColor;
                }
            }
            else
            {
                if (Engine.stageScreen.Background)
                {
                    Engine.stageScreen.Background.SetActive(true);
                    Engine.CameraFE.camera.clearFlags = CameraClearFlags.Skybox;
                }
            }

            if (NoMusic)
                Launcher.soundSystem.audioSourceMusic.mute = true;
            else
                Launcher.soundSystem.audioSourceMusic.mute = false;



            bool isPause = false;
            if (Engine != null && Engine.Entities != null)
                foreach (var entity in Engine.Entities)
                {
                    if (Engine.SuperPause.IsPaused(entity) || Engine.Pause.IsPaused(entity))
                    {
                        isPause = true;
                        break;
                    }
                }

            if (isPause == false)
                ResetFE();
        }

        public void ResetFE()
        {
            m_intro = false;
            m_winpose = false;
            m_nobardisplay = false;
            m_nobacklayer = false;
            m_nofrontlayer = false;
            m_nokosound = false;
            m_nokoslow = false;
            m_noglobalshadow = false;
            m_nomusic = false;
            m_timerfreeze = false;
        }

        public bool Intro
        {
            get { return m_intro; }
            set { m_intro = value; }
        }

        public bool WinPose
        {
            get { return m_winpose; }
            set { m_winpose = value; }
        }

        public bool NoBarDisplay
        {
            get { return m_nobardisplay; }
            set { m_nobardisplay = value; }
        }

        public bool NoBackLayer
        {
            get { return m_nobacklayer; }
            set { m_nobacklayer = value; }
        }

        public bool NoFrontLayer
        {
            get { return m_nofrontlayer; }
            set { m_nofrontlayer = value; }
        }

        public bool NoKOSound
        {
            get { return m_nokosound; }
            set { m_nokosound = value; }
        }

        public bool NoKOSlow
        {
            get { return m_nokoslow; }
            set { m_nokoslow = value; }
        }

        public bool GlobalNoShadow
        {
            get { return m_noglobalshadow; }
            set { m_noglobalshadow = value; }
        }

        public bool NoMusic
        {
            get { return m_nomusic; }
            set { m_nomusic = value; }
        }

        public bool TimerFreeze
        {
            get { return m_timerfreeze; }
            set { m_timerfreeze = value; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_intro;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_winpose;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nobardisplay;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nobacklayer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nofrontlayer;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nokosound;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nokoslow;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_noglobalshadow;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nomusic;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_timerfreeze;

        #endregion
    }

}