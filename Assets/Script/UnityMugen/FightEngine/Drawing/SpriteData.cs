using System;
using UnityEngine;

namespace UnityMugen.Drawing
{

    [Serializable]
    public class SpriteData
    {
        public Sprite sprite;
        public Vector2 offset;
        public bool paletteOverride;
        public int indexPalette;
    }
}