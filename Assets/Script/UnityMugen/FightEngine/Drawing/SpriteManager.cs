using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Video;

namespace UnityMugen.Drawing
{

    [DebuggerDisplay("{SpriteFile.Filepath}")]
    public class SpriteManager
    {

        Dictionary<SpriteId, SpriteData> m_sprites;
        public PaletteList Palettes { private set; get; }
        public int Version;
        public int PallSource;

        public SpriteManager((Dictionary<SpriteId, SpriteData>, PaletteList, int, int) data)
        {
            if (data.Item1 == null) throw new ArgumentNullException(nameof(data.Item1));
            if (data.Item2 == null) throw new ArgumentNullException(nameof(data.Item2));

            m_sprites = new Dictionary<SpriteId, SpriteData>(data.Item1);
            Palettes = data.Item2;
            Version = data.Item3;
            PallSource = data.Item4;

            DrawState = new DrawState();
        }

        public SpriteManager Clone()
        {
            var clone = new SpriteManager((m_sprites, Palettes, Version, PallSource));
            //clone.OverridePalette = OverridePalette;
            //clone.UseOverride = UseOverride;
            return clone;
        }


        public SpriteData GetSprite(SpriteId spriteid)
        {
            if (m_sprites.TryGetValue(spriteid, out SpriteData sprite))
                return sprite;

            return null;
        }

        public DrawState SetupDrawing(SpriteId id, PaletteList paletteList)
        {
            var sprite = GetSprite(id);

            DrawState.Reset();

            if (sprite != null)
            {
                try
                {
                    DrawState.Set(paletteList.PalTex[sprite.indexPalette], sprite.isRGBA);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("Palette de index: " + sprite.indexPalette);
                    DrawState.Set(paletteList.PalTex[0], false);
                }
            }

            return DrawState;
        }

        public DrawState DrawState;
    }

}