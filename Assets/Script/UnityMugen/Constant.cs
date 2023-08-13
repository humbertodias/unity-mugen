using UnityEngine;

namespace UnityMugen
{

    public class Constant
    {
        public const float Scale = 0.01f;
        public const float Scale2 = 100;

        public static Vector2 LocalCoord = new Vector2(320, 240);

        public const float VelAddKillX = .66f;
        public const float VelAddKillX2 = 2.5f;
        public const float VelAddKillY = -6f;
        public const float VelAddKillY2 = 2f;
        public const float VelAddKillY3 = -3f;

        public const float TimeFadeOutMusic = 0.5f;
        public const float YaccelDefault = 0.35f;

        public static int[] roundLengths = new int[] { 60, 99, 120, 180, 240, 300 };

        public static float[] Volume = { -80f, -39.6f, -35.2f, -30.8f, -26.4f, -22f, -17.6f, -13.2f, -8.8f, -4.4f, 0f };

    }
}