using System;
using UnityEngine;

namespace UnityMugen.Drawing
{

    public static class MiscSprites
    {

        public static SpriteUnpack SetPng(ref SpriteUnpack s, byte[] px)
        {
            Vector2Int size = new Vector2Int(s.Size[0], s.Size[1]);
            var texture = CreatePixelTexturePng(size);

            texture.SetPixelData(px, 0, 0);
            texture.Apply();

            s.Tex = texture;
            return s;
        }

        public static Texture2D FlipTexture(Texture2D orig)
        {
            Texture2D flip = CreatePixelTexture(new Vector2Int(orig.width, orig.height));
            int xN = orig.width;
            int yN = orig.height;

            for (int x = 0; x < xN; x++)
            {
                for (int y = 0; y < yN; y++)
                {
                    flip.SetPixel(x, yN - y - 1, orig.GetPixel(x, y));
                }
            }

            flip.Apply();
            return flip;
        }

        public static void Flip(UInt16[] size, ref byte[] p)
        {
            byte[] pixelsFlipped = new byte[size[0] * size[1]];

            for (int i = 0; i < size[1]; i++)
            {
                Array.Copy(p, i * size[0], pixelsFlipped, (size[1] - i - 1) * size[0], size[0]);
            }
            p = pixelsFlipped;
        }

        public static Texture2D CreatePixelTexture(Vector2Int size)
        {
            Texture2D texture = new Texture2D(size.x, size.y, TextureFormat.Alpha8, false, false);
            texture.filterMode = FilterMode.Point;
            return texture;
        }

        public static Texture2D CreatePixelTexturePng(Vector2Int size)
        {
            Texture2D texture = new Texture2D(size.x, size.y, TextureFormat.RGBA32, false, false);
            texture.filterMode = FilterMode.Point;
            return texture;
        }

        public static Texture2D CreatePaletteTexture()
        {
            Texture2D texture = new Texture2D(256, 1, TextureFormat.ARGB32, false, false);
            texture.filterMode = FilterMode.Point;
            return texture;
        }
    }
}