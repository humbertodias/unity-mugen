using UnityEngine;
using UnityMugen.Video;

namespace UnityMugen.Combat
{
    public class ProjectileData
    {
        public HitDefinition HitDef { get; set; }
        public long ProjectileId { get; set; }
        public int AnimationNumber { get; set; }
        public int HitAnimationNumber { get; set; }
        public int RemoveAnimationNumber { get; set; }
        public int CancelAnimationNumber { get; set; }
        public Vector2 Scale { get; set; }
        public bool RemoveOnHit { get; set; }
        public int RemoveTimeout { get; set; }
        public Vector2 InitialVelocity { get; set; }
        public Vector2 RemoveVelocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 VelocityMultiplier { get; set; }
        public int HitsBeforeRemoval { get; set; }
        public int TimeBetweenHits { get; set; }
        public int Priority { get; set; }
        public int SpritePriority { get; set; }
        public float ScreenEdgeBound { get; set; }
        public float StageEdgeBound { get; set; }
        public float HeightLowerBound { get; set; }
        public float HeightUpperBound { get; set; }
        public Vector2 CreationOffset { get; set; }
        public PositionType PositionType { get; set; }
        public Color ShadowColor { get; set; }
        public int SuperPauseMoveTime { get; set; }
        public int PauseMoveTime { get; set; }
        public int AfterImageTime { get; set; }
        public int AfterImageNumberOfFrames { get; set; }
        public Vector3 AfterImageBaseColor { get; set; }
        public bool AfterImageInvertColor { get; set; }
        public Vector3 AfterImagePreAddColor { get; set; }
        public Vector3 AfterImageConstrast { get; set; }
        public Vector3 AfterImagePostAddColor { get; set; }
        public Vector3 AfterImagePaletteColorAdd { get; set; }
        public Vector3 AfterImagePaletteColorMul { get; set; }
        public int AfterImageTimeGap { get; set; }
        public int AfterImageFrameGap { get; set; }
        public Blending AfterImageTransparency { get; set; }

        public long UniqueID { get; set; }
    }
}