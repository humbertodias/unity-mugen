using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Drawing;
using UnityMugen.Video;

namespace UnityMugen.Animations
{
    public class AnimationElement
    {

        public AnimationElement(int id, List<Rect> clsnsAttack, List<Rect> clsnnNormal, int ticks, int starttick, SpriteId spriteid, Vector2 offset, SpriteEffects flip, Blending _blending, Vector2 _scale, float _rotate, Vector2 scale)
        {
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id));
            if (clsnsAttack == null) throw new ArgumentNullException(nameof(clsnsAttack));
            if (ticks < -1) throw new ArgumentOutOfRangeException(nameof(ticks));
            if (starttick < 0) throw new ArgumentOutOfRangeException(nameof(starttick));

            Id = id;
            Gameticks = ticks;
            SpriteId = spriteid;

            Offset = offset * Constant.Scale;
            Flip = flip;
            StartTick = starttick;
            Blending = _blending;
            Scale = _scale;
            Rotate = _rotate;


            //ClsnsAttack = clsnsAttack;
            //ClsnsNormal = clsnnNormal;

            ClsnsTipe1Attack = ScaleRect(clsnsAttack, scale);
            ClsnsTipe2Normal = ScaleRect(clsnnNormal, scale);
        }

        //public List<Clsn>.Enumerator GetEnumerator()
        //{
        //    return m_clsnsAttack.GetEnumerator();
        //}
        private List<Rect> ScaleRect(List<Rect> rects, Vector2 scale)
        {
            List<Rect> newRects = new List<Rect>();

            // Escalar Offset para o Rect do collision esta em analize
            // para saber se não vai gerar algum erro.
            // Isto conserto o especial do Ryu fazendo dar os 5 hits
            // 27/09/2022 
            //Vector2 newOffset = Offset * Constant.Scale;

            for (int i = 0; i < rects.Count; i++)
            {
                var x1 = Offset.x + (rects[i].xMin * scale.x);
                var x2 = Offset.x + (rects[i].xMax * scale.x);

                //if (facing == Facing.Left)
                //{
                //    x1 = location.x - (line.clnsNormal[i].xMin * scale.x);
                //    x2 = location.x - (line.clnsNormal[i].xMax * scale.x);
                //}

                if (x1 > x2)
                {
                    Misc.Swap(ref x1, ref x2);
                }

                var y1 = Offset.y + (rects[i].yMin * scale.y);
                var y2 = Offset.y + (rects[i].yMax * scale.y);

                var rectangle = new Rect(x1, y1, x2 - x1, y2 - y1);
                newRects.Add(rectangle);
            }

            return newRects;
        }

        public override string ToString()
        {
            return "Element #" + Id.ToString();
        }

        public List<Rect> ClsnsTipe1Attack;
        public List<Rect> ClsnsTipe2Normal;

        public int Id;
        public int Gameticks;
        public SpriteId SpriteId;
        public Vector2 Offset;
        public SpriteEffects Flip;
        public int StartTick;
        public Blending Blending;
        public Vector2 Scale;
        public float Rotate;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private readonly List<Rect> m_clsnsAttack;

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //private readonly List<Rect> m_clsnsNormal;

    }
}