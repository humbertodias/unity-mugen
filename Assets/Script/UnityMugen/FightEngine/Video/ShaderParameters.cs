using UnityEngine;

namespace UnityMugen.Video
{

    public class ShaderParameters
    {

        private int m_fontcolorindex;
        private int m_fonttotalcolors;
        private bool m_afterimageuse;
        private bool m_afterimageinvert;
        private float m_afterimagecolor;
        private Vector3 m_afterimagepreadd;
        private Vector3 m_afterimagecontrast;
        private Vector3 m_afterimagepostadd;
        private Vector3 m_afterimagepaladd;
        private Vector3 m_afterimagepalmul;
        private int m_afterimagenumber;
        private bool m_usepalfx;
        private int m_palfxtime;
        private Vector3 m_palfxadd;
        private Vector3 m_palfxmul;
        private Vector4 m_palfxsinadd;
        private bool m_palfxinvert;
        private float m_palfxcolor;
        private Vector4 m_shadowcolor;

        public ShaderParameters()
        {
            Reset();
        }

        public void Reset()
        {
            m_fontcolorindex = 0;
            m_fonttotalcolors = 0;

            m_afterimageuse = false;
            m_afterimageinvert = false;
            m_afterimagecolor = 0;
            m_afterimagepreadd = Vector3.zero;
            m_afterimagecontrast = Vector3.zero;
            m_afterimagepostadd = Vector3.zero;
            m_afterimagepaladd = Vector3.zero;
            m_afterimagepalmul = Vector3.zero;
            m_afterimagenumber = 0;

            m_usepalfx = false;
            m_palfxadd = Vector3.zero;
            m_palfxcolor = 0;
            m_palfxinvert = false;
            m_palfxmul = Vector3.zero;
            m_palfxsinadd = Vector4.zero;
            m_palfxtime = 0;

            m_shadowcolor = Vector4.zero;
        }

        public int FontColorIndex
        {
            get => m_fontcolorindex;
            set { m_fontcolorindex = value; }
        }

        public int FontTotalColors
        {
            get => m_fonttotalcolors;
            set { m_fonttotalcolors = value; }
        }

        public bool AfterImageEnable
        {
            get => m_afterimageuse;
            set { m_afterimageuse = value; }
        }

        public bool AfterImageInvert
        {
            get => m_afterimageinvert;
            set { m_afterimageinvert = value; }
        }

        public float AfterImageColor
        {
            get => m_afterimagecolor;
            set { m_afterimagecolor = value; }
        }

        public Vector3 AfterImagePreAdd
        {
            get => m_afterimagepreadd;
            set { m_afterimagepreadd = value; }
        }

        public Vector3 AfterImageConstrast
        {
            get => m_afterimagecontrast;
            set { m_afterimagecontrast = value; }
        }

        public Vector3 AfterImagePostAdd
        {
            get => m_afterimagepostadd;
            set { m_afterimagepostadd = value; }
        }

        public Vector3 AfterImagePaletteAdd
        {
            get => m_afterimagepaladd;
            set { m_afterimagepaladd = value; }
        }

        public Vector3 AfterImagePaletteMultiply
        {
            get => m_afterimagepalmul;
            set { m_afterimagepalmul = value; }
        }

        public int AfterImageNumber
        {
            get => m_afterimagenumber;
            set { m_afterimagenumber = value; }
        }

        public bool PaletteFxEnable
        {
            get => m_usepalfx;
            set { m_usepalfx = value; }
        }

        public int PaletteFxTime
        {
            get => m_palfxtime;
            set { m_palfxtime = value; }
        }

        public bool PaletteFxInvert
        {
            get => m_palfxinvert;
            set { m_palfxinvert = value; }
        }

        public Vector3 PaletteFxAdd
        {
            get => m_palfxadd;
            set { m_palfxadd = value; }
        }

        public Vector3 PaletteFxMultiply
        {
            get => m_palfxmul;
            set { m_palfxmul = value; }
        }

        public Vector4 PaletteFxSinAdd
        {
            get => m_palfxsinadd;
            set { m_palfxsinadd = value; }
        }

        public float PaletteFxColor
        {
            get => m_palfxcolor;
            set { m_palfxcolor = value; }
        }

        public Vector4 ShadowColor
        {
            get => m_shadowcolor;
            set { m_shadowcolor = value; }
        }
    }
}