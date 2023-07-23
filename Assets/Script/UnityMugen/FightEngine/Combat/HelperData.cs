using UnityEngine;

namespace UnityMugen.Combat
{
    public class HelperData
    {
        public string Name { get; set; }
        public long HelperId { get; set; }
        public HelperType Type { get; set; }
        public bool KeyControl { get; set; }
        public int FacingFlag { get; set; }
        public PositionType PositionType { get; set; }
        public Vector2 CreationOffset { get; set; }
        public int InitialStateNumber { get; set; }
        public Vector2 Scale { get; set; }
        public float GroundFront { get; set; }
        public float GroundBack { get; set; }
        public float AirFront { get; set; }
        public float AirBack { get; set; }
        public float Height { get; set; }
        public bool OwnPaletteFx { get; set; }
        public int SuperPauseTime { get; set; }
        public int PauseTime { get; set; }
        public bool ProjectileScaling { get; set; }
        public Vector2 HeadPosition { get; set; }
        public Vector2 MidPosition { get; set; }
        public float ShadowOffset { get; set; }

        public long UniqueID { get; set; }
    }
}