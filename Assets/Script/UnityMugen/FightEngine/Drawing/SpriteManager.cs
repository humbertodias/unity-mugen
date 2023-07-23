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

        public SpriteManager(Dictionary<SpriteId, SpriteData> spriteDatas)
        {
            if (spriteDatas == null) throw new ArgumentNullException(nameof(spriteDatas));

            m_sprites = new Dictionary<SpriteId, SpriteData>(spriteDatas);
            DrawState = new DrawState();
        }

        public SpriteManager Clone()
        {
            var clone = new SpriteManager(m_sprites);
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

        public DrawState SetupDrawing(SpriteId id, PaletteList paletteList, Texture2D CurrentPalette)
        {
            var sprite = GetSprite(id);

            DrawState.Reset();

            //if (sprite != null && sprite.paletteOverride)
            //{ 
            //    DrawState.Set(CurrentPalette);
            //}
            //else
            if (sprite != null)
            {
                try
                {
                    DrawState.Set(paletteList.PalTex[sprite.indexPalette]);
                }
                catch
                {
                    UnityEngine.Debug.LogWarning("Palette de index: " + sprite.indexPalette);
                    DrawState.Set(paletteList.PalTex[0]);
                }
            }

            return DrawState;
        }

        public DrawState DrawState;
    }

}