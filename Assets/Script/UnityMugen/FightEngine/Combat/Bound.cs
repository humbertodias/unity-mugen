using System;
using UnityEngine;

namespace UnityMugen.Combat
{
    [Serializable]
    public class Bound
    {

        public float up;
        public float down;
        public float left;
        public float right;

        public Bound(float up, float down, float left, float right)
        {
            this.up = up;
            this.down = down;
            this.left = left;
            this.right = right;
        }

        public Vector2 bound(Vector2 location)
        {
            var output = location;

            if (output.x < left) output.x = left;
            if (output.x > right) output.x = right;

            if (output.y < up) output.y = up;
            if (output.y > down) output.y = down;

            return output;
        }

        public override string ToString()
        {
            return string.Format("UP: {0} ,DOWN: {1} ,LEFT: {2}, RIGHT: {3}", up, down, left, right);
        }


        public Rect ConverterBound(float up, float down, float left, float right)
        {
            Rect rect = new Rect();
            rect.xMax = right;
            rect.xMin = left;
            rect.yMax = up;
            rect.yMin = down;
            return rect;
        }

        public Rect ConverterBound(Bound bound)
        {
            return ConverterBound(bound.up, bound.down, bound.left, bound.right);
        }

    }

}