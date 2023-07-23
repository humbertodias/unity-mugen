using System;
using System.Diagnostics;
using UnityEngine;

namespace UnityMugen.Combat
{
    public class EnvironmentShake
    {

        public FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public void ResetFE()
        {
            m_timeticks = 0;
            m_time = 0;
            m_frequency = 0;
            m_amplitude = 0;
            m_phase = 0;
        }

        public void Set(int time, float frequency, float amplitude, float phase)
        {
            m_timeticks = 0;
            m_time = time;
            m_frequency = frequency;
            m_amplitude = amplitude;
            m_phase = phase;
        }

        public void UpdateFE()
        {
            if (IsActive == false) return;

            Vector3 PosCam = Engine.CameraFE.gameObject.transform.position;
            Engine.CameraFE.gameObject.transform.position =
                new Vector3(PosCam.x + DrawOffset.x, PosCam.y + DrawOffset.y, PosCam.z);

            if (m_timeticks == 0)
            {
                foreach (Animation anim in Engine.stageScreen.StageActions.envShakeAnimations)
                {
                    anim.Play();
                }
            }
            ++m_timeticks;
        }

        public bool IsActive => m_time > 0 && m_timeticks < m_time;

        public Vector2 DrawOffset
        {
            get
            {
                if (IsActive == false)
                    return Vector2.zero;

                var movement = Amplitude * (float)Math.Sin(TimeElasped * Frequency + Phase);
                movement = movement * Constant.Scale;
                return new Vector2(movement, movement) / 1.5f;
            }
        }

        public int TimeElasped => m_timeticks;

        public int Time => m_time;

        public float Frequency => m_frequency;

        public float Amplitude => m_amplitude;

        public float Phase => m_phase;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_timeticks;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_time;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_frequency;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_amplitude;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_phase;

        #endregion
    }
}