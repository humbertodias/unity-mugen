using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Projectile")]
    public class Projectile : HitDef
    {
        private Expression m_projPriority;
        private Expression m_pauseTime;
        private Expression m_pauseMoveTime;

#warning Não aplicado ainda
        private Expression m_ownPal;

#warning Não aplicado ainda
        private Expression m_remapPal;

        private Expression m_projectileId;
        private Expression m_animation;
        private Expression m_hitAnimation;
        private Expression m_removeAnimation;
        private Expression m_cancelAnimation;
        private Expression m_scale;
        private Expression m_removeOnHit;
        private Expression m_removeTime;
        private Expression m_velocity;
        private Expression m_removeVelocity;
        private Expression m_acceleration;
        private Expression m_velocityMultiplier;
        private Expression m_hits;
        private Expression m_missTime;
        private Expression m_spritePriority;
        private Expression m_edgeBound;
        private Expression m_stageBound;
        private Expression m_heightBound;
        private Expression m_offSet;
        private PositionType m_posType;
        private Expression m_shadow;
        private Expression m_superMoveTime;

        private AfterImage m_afterImage;


        public Projectile(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_projectileId = textSection.GetAttribute<Expression>("ProjID", null);
                m_animation = textSection.GetAttribute<Expression>("projanim", null);
                m_hitAnimation = textSection.GetAttribute<Expression>("projhitanim", null);
                m_removeAnimation = textSection.GetAttribute<Expression>("projremanim", null);
                m_cancelAnimation = textSection.GetAttribute<Expression>("projcancelanim", null);
                m_scale = textSection.GetAttribute<Expression>("projscale", null);
                m_removeOnHit = textSection.GetAttribute<Expression>("projremove", null);
                m_removeTime = textSection.GetAttribute<Expression>("projremovetime", null);
                m_velocity = textSection.GetAttribute<Expression>("velocity", null);
                m_removeVelocity = textSection.GetAttribute<Expression>("remvelocity", null);
                m_acceleration = textSection.GetAttribute<Expression>("accel", null);
                m_velocityMultiplier = textSection.GetAttribute<Expression>("velmul", null);
                m_hits = textSection.GetAttribute<Expression>("projhits", null);
                m_missTime = textSection.GetAttribute<Expression>("projmisstime", null);
                m_projPriority = textSection.GetAttribute<Expression>("projpriority", null);
                m_spritePriority = textSection.GetAttribute<Expression>("projsprpriority", null);
                m_edgeBound = textSection.GetAttribute<Expression>("projedgebound", null);
                m_stageBound = textSection.GetAttribute<Expression>("projstagebound", null);
                m_heightBound = textSection.GetAttribute<Expression>("projheightbound", null);
                m_offSet = textSection.GetAttribute<Expression>("offset", null);
                m_posType = textSection.GetAttribute("postype", PositionType.P1);
                m_shadow = textSection.GetAttribute<Expression>("projshadow", null);
                m_superMoveTime = textSection.GetAttribute<Expression>("supermovetime", null);

                m_pauseTime = textSection.GetAttribute<Expression>("pausetime", null);
                m_pauseMoveTime = textSection.GetAttribute<Expression>("pausemovetime", null);

                m_ownPal = textSection.GetAttribute<Expression>("ownpal", null);
                m_remapPal = textSection.GetAttribute<Expression>("remappal", null);

                AfterImageData afterImageData = new AfterImageData();
                afterImageData.m_time = textSection.GetAttribute<Expression>("afterimage.time", null);
                afterImageData.m_numberOfFrames = textSection.GetAttribute<Expression>("afterimage.length", null);
                afterImageData.m_paletteColor = textSection.GetAttribute<Expression>("afterimage.palcolor", null);
                afterImageData.m_paletteInversion = textSection.GetAttribute<Expression>("afterimage.palinvertall", null);
                afterImageData.m_paletteBrightness = textSection.GetAttribute<Expression>("afterimage.palbright", null);
                afterImageData.m_paletteContrast = textSection.GetAttribute<Expression>("afterimage.palcontrast", null);
                afterImageData.m_palettePostBrightness = textSection.GetAttribute<Expression>("afterimage.palpostbright", null);
                afterImageData.m_paletteAdd = textSection.GetAttribute<Expression>("afterimage.paladd", null);
                afterImageData.m_paletteMutliply = textSection.GetAttribute<Expression>("afterimage.palmul", null);
                afterImageData.m_timeGap = textSection.GetAttribute<Expression>("afterimage.timeGap", null);
                afterImageData.m_frameGap = textSection.GetAttribute<Expression>("afterimage.frameGap", null);
                afterImageData.m_trans = textSection.GetAttribute<Blending?>("afterimage.trans", Misc.ToBlending(BlendType.None));
                afterImageData.m_alpha = textSection.GetAttribute<Expression>("afterimage.alpha", null);
                m_afterImage = new AfterImage(afterImageData);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var data = new ProjectileData();

            if (m_hitAttr != null)
            {
                data.HitDef = new HitDefinition();
                SetHitDefinition(character, data.HitDef);
            }

            data.ProjectileId = EvaluationHelper.AsInt32(character, m_projectileId, 0);
            data.AnimationNumber = EvaluationHelper.AsInt32(character, m_animation, 0);
            data.HitAnimationNumber = EvaluationHelper.AsInt32(character, m_hitAnimation, -1);
            data.RemoveAnimationNumber = EvaluationHelper.AsInt32(character, m_removeAnimation, data.HitAnimationNumber);
            data.CancelAnimationNumber = EvaluationHelper.AsInt32(character, m_cancelAnimation, data.RemoveAnimationNumber);
            data.Scale = EvaluationHelper.AsVector2(character, m_scale, Vector2.one);
            data.RemoveOnHit = EvaluationHelper.AsBoolean(character, m_removeOnHit, true);
            data.RemoveTimeout = EvaluationHelper.AsInt32(character, m_removeTime, -1);
            data.InitialVelocity = EvaluationHelper.AsVector2(character, m_velocity, Vector2.zero) * Constant.Scale;
            data.RemoveVelocity = EvaluationHelper.AsVector2(character, m_removeVelocity, Vector2.zero) * Constant.Scale;
            data.Acceleration = EvaluationHelper.AsVector2(character, m_acceleration, Vector2.zero) * Constant.Scale;
            data.VelocityMultiplier = EvaluationHelper.AsVector2(character, m_velocityMultiplier, Vector2.one); // Scale
            data.HitsBeforeRemoval = EvaluationHelper.AsInt32(character, m_hits, 1);
            data.TimeBetweenHits = EvaluationHelper.AsInt32(character, m_missTime, 0);
            data.Priority = EvaluationHelper.AsInt32(character, m_projPriority, 1);
            data.SpritePriority = EvaluationHelper.AsInt32(character, m_spritePriority, 3);
            data.ScreenEdgeBound = EvaluationHelper.AsInt32(character, m_edgeBound, 40) * Constant.Scale;
            data.StageEdgeBound = EvaluationHelper.AsInt32(character, m_stageBound, 40) * Constant.Scale;

            var heightbounds = EvaluationHelper.AsVector2(character, m_heightBound, new Vector2(-240, 1)) * Constant.Scale;
            data.HeightLowerBound = heightbounds.x;
            data.HeightUpperBound = heightbounds.y;

            data.CreationOffset = (Vector2)EvaluationHelper.AsVector2(character, m_offSet, Vector2.zero) * Constant.Scale;
            data.PositionType = m_posType;

            data.SuperPauseMoveTime = EvaluationHelper.AsInt32(character, m_superMoveTime, 0);
            data.PauseMoveTime = EvaluationHelper.AsInt32(character, m_pauseMoveTime, 0);

#warning Os valores abaixo ainda não foram aplicados
            Vector3 shad = EvaluationHelper.AsVector3(character, m_shadow, Vector3.zero);
            data.ShadowColor = new Color(shad.x, shad.y, shad.z);

            m_afterImage.Run(character);

            character.InstanceProjectile(data);
        }


        public override bool IsValid()
        {
            return triggerMap.IsValid;
        }
    }

    public class AfterImageData
    {
        public Expression m_time;
        public Expression m_numberOfFrames;
        public Expression m_paletteColor;
        public Expression m_paletteInversion;
        public Expression m_paletteBrightness;
        public Expression m_paletteContrast;
        public Expression m_palettePostBrightness;
        public Expression m_paletteAdd;
        public Expression m_paletteMutliply;
        public Expression m_timeGap;
        public Expression m_frameGap;
        public Blending? m_trans;
        public Expression m_alpha;
    }

}