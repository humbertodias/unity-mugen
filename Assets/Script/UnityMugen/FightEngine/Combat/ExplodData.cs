using System.Diagnostics;
using UnityEngine;
using UnityMugen.Video;

namespace UnityMugen.Combat
{

    [DebuggerDisplay("Id #{ExplodId} - {CommonAnimation}, {AnimationNumber}")]
    public class ExplodData
    {
        public bool IsHitSpark { get; set; }

        public bool CommonAnimation { get; set; }
        public int AnimationNumber { get; set; }
        public long ExplodId { get; set; }
        public PositionType PositionType { get; set; }
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public SpriteEffects Flip { get; set; }
        public int BindTime { get; set; }
        public int RemoveTime { get; set; }
        public Vector2 Random { get; set; }
        public bool SuperMove { get; set; }
        public int SuperMoveTime { get; set; }
        public int PauseTime { get; set; }
        public Vector2 Scale { get; set; }
        public float Angle { get; set; }//Novo
        public int SpritePriority { get; set; }
        public bool DrawOnTop { get; set; }
        public bool OwnPalFx { get; set; }
        public bool RemoveOnGetHit { get; set; }
        public bool IgnoreHitPause { get; set; }
        public bool Shadow { get; set; }
        public bool Reflection { get; set; }
        public Blending Transparency { get; set; }
        public int Facing { get; set; }
        public int vFacing { get; set; }

        public Character Creator { get; set; }
        public Entity Offseter { get; set; }

        public long UniqueID { get; set; }
    }

    [DebuggerDisplay("Id #{Id} - {CommonAnimation}, {AnimationNumber}")]
    public class ModifyExplodData
    {
        public bool? CommonAnimation { get; set; }
        public int? AnimationNumber { get; set; }
        public int Id { get; set; }
        public PositionType? PositionType { get; set; }
        public Vector2? Location { get; set; }
        public Vector2? Velocity { get; set; }
        public Vector2? Acceleration { get; set; }
        public SpriteEffects? Flip { get; set; }
        public int? BindTime { get; set; }
        public int? RemoveTime { get; set; }
        public Vector2? Random { get; set; }
        public bool? SuperMove { get; set; }
        public int? SuperMoveTime { get; set; }
        public int? PauseTime { get; set; }
        public Vector2? Scale { get; set; }
        public int? SpritePriority { get; set; }
        public bool? DrawOnTop { get; set; }
        public bool? OwnPalFx { get; set; }
        public bool? RemoveOnGetHit { get; set; }
        public bool? IgnoreHitPause { get; set; }
        public Blending? Transparency { get; set; }
        public SpaceType SpaceType { get; set; }
        public bool Reflection { get; set; }
    }
}