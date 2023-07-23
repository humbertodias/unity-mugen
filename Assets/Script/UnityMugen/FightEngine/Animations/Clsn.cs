using System.Diagnostics;
using UnityEngine;


namespace UnityMugen.Animations
{
    public struct Clsn
    {

        public Clsn(/*ClsnType clsntype,*/ Rect rectangle)
        {
            //  m_clsntype = clsntype;
            m_rect = rectangle;
        }

        public Rect MakeRect(Vector2 location, Vector2 scale, Facing facing)
        {
            var x1 = location.x + (Rect.xMin * scale.x);
            var x2 = location.x + (Rect.xMax * scale.x);

            if (facing == Facing.Left)
            {
                x1 = location.x - (Rect.xMin * scale.x);
                x2 = location.x - (Rect.xMax * scale.x);
            }

            if (x1 > x2)
            {
                Misc.Swap(ref x1, ref x2);
            }

            var y1 = location.y + (Rect.yMin * scale.y);
            var y2 = location.y + (Rect.yMax * scale.y);

            var rectangle = new Rect(x1, y1, x2 - x1, y2 - y1);
            return rectangle;
        }

        public override string ToString()
        {
            return /*ClsnType.ToString() + " - " +*/ Rect.ToString();
        }


        // public ClsnType ClsnType => m_clsntype;
        public Rect Rect => m_rect;

        //   [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //   private readonly ClsnType m_clsntype;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Rect m_rect;

    }
}