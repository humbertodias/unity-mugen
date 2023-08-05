using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMugen.Drawing;

namespace UnityMugen.Video
{

    public class Renderer
    {

        private Material m_material;
        private readonly Texture2D m_nullpalette;

        public Renderer()
        {
            m_nullpalette = SpriteSystem.CreatePaletteTexture;

            var pixels = new byte[] { 1, 2, 1, 2 };

            var paldata = new Color[256];
            paldata[1] = Color.white;
            paldata[2] = Color.red;
            m_nullpalette.SetPixels(0, 0, 0, 0, paldata);
        }

        public void Draw(DrawState drawstate, Material material)
        {
            if (drawstate == null) throw new ArgumentNullException(nameof(drawstate));

            m_material = material;

            drawstate.Palette = drawstate.Palette ?? m_nullpalette;

            SetBlending(drawstate.Blending);
            NormalDraw(drawstate);
        }

        private void NormalDraw(DrawState drawstate)
        {
            if (drawstate == null) throw new ArgumentNullException(nameof(drawstate));

            SetShaderParameters(drawstate.ShaderParameters, drawstate.Palette);
        }

        private void SetShaderParameters(ShaderParameters parameters, Texture2D palette)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (palette == null) throw new ArgumentNullException(nameof(palette));

            m_material.SetTexture("xPalette", palette);

            OnDeviceReset();

            if (parameters.PaletteFxEnable)
            {
                m_material.SetInt("xPalFx_Use", 1);
                m_material.SetVector("xPalFx_Add", new Vector4(parameters.PaletteFxAdd.x, parameters.PaletteFxAdd.y, parameters.PaletteFxAdd.z, 0));
                m_material.SetVector("xPalFx_Mul", new Vector4(parameters.PaletteFxMultiply.x, parameters.PaletteFxMultiply.y, parameters.PaletteFxMultiply.z, 1));
                m_material.SetFloat("xPalFx_Invert", Convert.ToInt32(parameters.PaletteFxInvert));
                m_material.SetFloat("xPalFx_Color", parameters.PaletteFxColor);

                var sincolor = parameters.PaletteFxSinAdd * (float)Math.Sin(parameters.PaletteFxTime * (Math.PI * 2) / parameters.PaletteFxSinAdd.w);
                sincolor.w = 0;

                m_material.SetVector("xPalFx_SinMath", sincolor);
            }
            else
            {
                m_material.SetInt("xPalFx_Use", 0);
            }

            if (parameters.AfterImageEnable)
            {
                m_material.SetInt("xAI_Use", 1);
                m_material.SetInt("xAI_Invert", Convert.ToInt32(parameters.AfterImageInvert));
                m_material.SetFloat("xAI_color", parameters.AfterImageColor);
                m_material.SetVector("xAI_preadd", new Vector4(parameters.AfterImagePreAdd.x, parameters.AfterImagePreAdd.y, parameters.AfterImagePreAdd.z, 0));
                m_material.SetVector("xAI_contrast", new Vector4(parameters.AfterImageConstrast.x, parameters.AfterImageConstrast.y, parameters.AfterImageConstrast.z, 1));
                m_material.SetVector("xAI_postadd", new Vector4(parameters.AfterImagePostAdd.x, parameters.AfterImagePostAdd.y, parameters.AfterImagePostAdd.z, 0));
                m_material.SetVector("xAI_paladd", new Vector4(parameters.AfterImagePaletteAdd.x, parameters.AfterImagePaletteAdd.y, parameters.AfterImagePaletteAdd.z, 0));
                m_material.SetVector("xAI_palmul", new Vector4(parameters.AfterImagePaletteMultiply.x, parameters.AfterImagePaletteMultiply.y, parameters.AfterImagePaletteMultiply.z, 1));
                m_material.SetInt("xAI_number", parameters.AfterImageNumber);
            }
            else
            {
                m_material.SetInt("xAI_Use", 0);
            }
        }

        private void SetBlending(Blending blending)
        {
            switch (blending.BlendType)
            {
                case BlendType.AddAlpha:
                    m_material.SetFloat("Alpha", 1);
                    m_material.SetFloat("DstMode", 1);
                    m_material.SetFloat("Subtraction", 10);
                    break;

                case BlendType.Add:
                    m_material.SetFloat("Alpha", Alpha(blending));
                    m_material.SetFloat("DstMode", 1);
                    m_material.SetFloat("Subtraction", 0);
                    break;

                case BlendType.Subtract:
                    m_material.SetFloat("Alpha", 0.5f);
                    m_material.SetFloat("DstMode", 10);
                    m_material.SetFloat("Subtraction", 1);
                    break;

                case BlendType.None:
                default:
                    m_material.SetFloat("Alpha", 1);
                    m_material.SetFloat("DstMode", 10);
                    m_material.SetFloat("Subtraction", 0);
                    break;
            }
        }

        public void OnDeviceReset()
        {
            Matrix4x4 m = Matrix4x4.identity;
            m_material.SetMatrix("xMatrix", m);
            m_material.SetMatrix("xRotation", Matrix4x4.identity);
        }

        public float Alpha(Blending a)
        {
            byte sa = 0, da = 0;
            if (a.SourceFactor >= 0)
            {
                sa = (a.SourceFactor);
                if (a.DestinationFactor < 0)
                {
                    da = (byte)(a.DestinationFactor >> 1);
                    if (sa == 1 && da == 255)
                    {
                        sa = 0;
                    }
                }
                else
                {
                    da = (a.DestinationFactor);
                }
            }
            else
            {
                sa = (byte)(a.SourceFactor);
                da = (byte)(a.DestinationFactor);
            }
            if (sa == 1 && da == 255)
            {
                return -2;
            }

            sa = (byte)(((int)sa) * 256 >> 8);
            if (sa < 5 && da == 255)
            {
                return 0;
            }
            if (sa == 255 && da == 255) // Add
            {
                return 1/*-1*/;
            }
            int trans = (int)(sa);
            if (((int)sa) + ((int)da) < 254 || 256 < ((int)sa) + ((int)da))
            {
                trans |= ((int)da) << 10 | 1 << 9;
                int src = trans & 0xff;
                int dst = trans >> 10 & 0xff;
                if (dst < 255)
                {
                    return dst / 255f;
                }
                if (src > 0)
                {
                    return src / 255f;
                }
            }
            return trans / 255f;
        }
    }
}