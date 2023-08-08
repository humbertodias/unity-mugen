using UnityEngine;

namespace UnityMugen.Video
{

    public class DrawState
    {
        public ShaderParameters ShaderParameters { get; set; }
        public Texture2D Palette { get; set; }
        public bool IsRGBA { get; set; }
        public Blending Blending { get; set; }
        private readonly Renderer r_renderer;

        public DrawState()
        {
            Palette = null;
            Blending = new Blending();
            ShaderParameters = new ShaderParameters();
            r_renderer = new Renderer();
        }

        public void Use(Material material)
        {
            r_renderer.Draw(this, material);
        }

        public void Reset()
        {
            Palette = null;
            Blending = new Blending();
            ShaderParameters.Reset();
        }

        public void Set(Texture2D CurrentPalette, bool isRGBA)
        {
            IsRGBA = isRGBA;
            if (CurrentPalette != null)
            {
                Palette = CurrentPalette;
            }
            else
            {
                Palette = null;
            }
        }
    }
}