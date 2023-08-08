using System;
using UnityEngine;

namespace UnityMugen.Drawing
{

    [Serializable]
    public class SpriteData
    {
        public Sprite sprite;
        public Vector2 offset;
        public int indexPalette;
        public bool isRGBA;
    }
}