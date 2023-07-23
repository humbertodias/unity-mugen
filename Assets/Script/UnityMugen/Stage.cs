using System;
using UnityEngine;
using UnityMugen;
using UnityMugen.Combat;

namespace UnityMugen
{

    [Serializable]
    public class Stage
    {
        public int stageID;
        public int musicID = -1;

        [Header("Camera")]
        [Tooltip("Camera starting position: Usually 0 for both")]
        public Vector3 StartPositionCamera = new Vector3(0, 0.925878f, -3);
        public float boundLeft = -4.55f;
        public float boundRight = 4.55f;
        public float boundHigh = -2.85f;
        public float boundLow = 0.271f;
        public float VerticalFollow = 1f;
        public float FloorTension;
        public float Tension = 2;

        [Header("Shadow")]
        public byte ShadowIntensity = 128;
        public Color ShadowColor = new Color(0, 0, 0, 0.3333333f);
        public float ShadowScale = -0.5f;
        public float ShadowFade = 2.2f;
        public bool ShadowReflection;


        public int ZOffset = 210;
        public int ZOffsetLink;
        public bool AutoTurn = true;
        public bool ResetBackgrounds;
        public float LeftEdgeDistance = 0.3f;
        public float RightEdgeDistance = 0.3f;

        [Header("Zoom")]
        public float minZoom = -2.21f;
        public float maxZoom = -2.98f;
        //public float zoomLimiter = -2.21f;

        [Header("PlayerInfo")]
        public Vector2 P1Start = new Vector2(-0.9f, 0);
        public Facing P1Facing = Facing.Right;
        public Vector2 P2Start = new Vector2(0.9f, 0);
        public Facing P2Facing = Facing.Left;
        public Vector2 P3Start = new Vector2(-1.3f, 0);
        public Facing P3Facing = Facing.Right;
        public Vector2 P4Start = new Vector2(1.3f, 0);
        public Facing P4Facing = Facing.Left;
        //   public Rect PlayerBounds;

        [Header("Debug")]
        public bool DebugBackgrounds;

        public PaletteFx PaletteFx = new PaletteFx();
        public GameObject[] environmentColor;

        public void Reset()
        {
            //Backgrounds.Reset();
            PaletteFx.Reset();
        }

        public void Draw(Layer backgroundLayer)
        {

        }
    }
}