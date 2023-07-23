using System;
using UnityEngine;
using UnityMugen.Video;

namespace UnityMugen.Combat
{
    public class PaletteFx
    {
        public PaletteFx()
        {
            Reset();
        }

        public void Reset()
        {
            m_totaltime = 0;
            m_time = 0;
            m_add = Vector3.zero;
            m_mul = Vector3.zero;
            m_sinadd = new Vector4(0, 0, 0, 1);
            m_invert = false;
            m_basecolor = 1;
            m_isactive = false;
        }

        public void UpdateFE()
        {
            if (IsActive && (TotalTime == -1 || TotalTime > 0 && Time < TotalTime))
            {
                ++m_time;
            }
            else
            {
                Reset();
            }
        }

        public void SetShader(ShaderParameters shader)
        {
            if (shader == null) throw new ArgumentNullException(nameof(shader));


            if (IsActive)
            {
                shader.PaletteFxEnable = true;
                shader.PaletteFxAdd = Add;
                shader.PaletteFxColor = BaseColor;
                shader.PaletteFxInvert = Invert;
                shader.PaletteFxMultiply = Mul;
                shader.PaletteFxSinAdd = SinAdd;
                shader.PaletteFxTime = Time;
            }
            else
            {
                shader.PaletteFxEnable = false;
            }
        }

        public void Set(int totaltime, Vector3 add, Vector3 mul, Vector4 sinadd, bool invert, float basecolor)
        {
            m_totaltime = totaltime;
            m_time = 0;
            m_add = Clamp(add);
            m_mul = Clamp(mul);
            m_sinadd = Vector4Custom(Clamp(new Vector3(sinadd.x, sinadd.y, sinadd.z)), sinadd.w);
            m_invert = invert;
            m_basecolor = Misc.Clamp(basecolor, 0.0f, 1.0f);

            m_isactive = true;
        }

        private Vector4 Vector4Custom(Vector3 vector3, float w)
        {
            return new Vector4(vector3.x, vector3.y, vector3.z, w);
        }

        private static Vector3 Clamp(Vector3 input)
        {
            return input / 255.0f;
        }

        public bool IsActive => m_isactive;
        public int TotalTime => m_totaltime;
        public int Time => m_time;
        public Vector3 Add => m_add;
        public Vector3 Mul => m_mul;
        public Vector4 SinAdd => m_sinadd;
        public bool Invert => m_invert;
        public float BaseColor => m_basecolor;

        private bool m_isactive;
        private int m_totaltime;
        private int m_time;
        private Vector3 m_add;
        private Vector3 m_mul;
        private float m_basecolor;
        private Vector4 m_sinadd;
        private bool m_invert;

    }
}