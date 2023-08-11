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
            TimeElasped = 0;
            Time = 0;
            Frequency = 0;
            Amplitude = 0;
            Phase = 0;
        }

        public void Set(int time, float frequency, float amplitude, float phase)
        {
            TimeElasped = 0;
            Time = time;
            Frequency = frequency;
            Amplitude = amplitude;
            Phase = phase;
        }

        public void UpdateFE()
        {
            if (IsActive == false) return;

            Vector3 PosCam = Engine.CameraFE.gameObject.transform.position;
            Engine.CameraFE.gameObject.transform.position =
                new Vector3(PosCam.x + DrawOffset.x, PosCam.y + DrawOffset.y, PosCam.z);

            if (TimeElasped == 0)
            {
                foreach (Animation anim in Engine.stageScreen.StageActions.envShakeAnimations)
                {
                    anim.Play();
                }
            }
            ++TimeElasped;
        }

        public bool IsActive => Time > 0 && TimeElasped < Time;

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

        public int TimeElasped;
        public int Time;
        public float Frequency;
        public float Amplitude;
        public float Phase;

    }
}