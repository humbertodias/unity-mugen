using System;
using UnityEngine;

namespace UnityMugen.Drawing
{

    public class PaletteManager
    {
        [NonSerialized] public int currentPalette;
        [NonSerialized] public PaletteList palette;

        private Texture2D mColorSwapTex;
        private SpriteRenderer m_SpriteRenderer;
        private int oldPalette;

        public Texture2D CurrentPalette { get; set; }

        public PaletteManager() { }

        public PaletteManager(SpriteRenderer render)
        {
            m_SpriteRenderer = render;
        }

        public PaletteManager Clone()
        {
            return new PaletteManager();
        }

        void Update()
        {
            if (oldPalette != currentPalette)
            {
                if (currentPalette > palette.palettes.Count - 1)
                    currentPalette = palette.palettes.Count - 1;

                InitColorSwapTex(true);
                ApplyColors();
                oldPalette = currentPalette;
            }
        }

        public void SetExternalPalette(int _currentPalette, PaletteList _palette, SpriteRenderer _spriteRenderer)
        {
            this.currentPalette = _currentPalette;
            this.palette = _palette;
            this.m_SpriteRenderer = _spriteRenderer;

            if (currentPalette > palette.palettes.Count - 1)
                currentPalette = palette.palettes.Count - 1;

            InitColorSwapTex(false);
            ApplyColors();
            oldPalette = currentPalette;
        }

        public void ApplyColors()
        {
            for (int i = 0; i < palette.palettes[0].Length; i++)
            {
                // Color32 color32 = palette.palettes[0][i];
                mColorSwapTex.SetPixel(i, 0, palette.palettes[currentPalette][i]);
            }
            mColorSwapTex.Apply();
        }

        public void InitColorSwapTex(bool playing)
        {
            Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
            colorSwapTex.filterMode = FilterMode.Point;

            for (int i = 0; i < colorSwapTex.width; ++i)
                colorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            colorSwapTex.Apply();

            if (playing)
                m_SpriteRenderer.material.SetTexture("xPalette", colorSwapTex);
            else
                m_SpriteRenderer.sharedMaterial.SetTexture("xPalette", colorSwapTex);

            mColorSwapTex = colorSwapTex;
        }

        public void ClearAllSpritesColors()
        {
            for (int i = 0; i < mColorSwapTex.width; ++i)
            {
                mColorSwapTex.SetPixel(i, 0, new Color(0.0f, 0.0f, 0.0f, 0.0f));
            }
            mColorSwapTex.Apply();
        }
    }
}