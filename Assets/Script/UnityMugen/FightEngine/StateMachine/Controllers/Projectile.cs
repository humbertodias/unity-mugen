using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Projectile")]
    public class Projectile : HitDef
    {
        private Expression m_projPriority;
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

        AfterImageData afterImageData = new AfterImageData();

        public Projectile(string label) : base(label)
        {
            m_posType = PositionType.P1;
            afterImageData.m_trans = Misc.ToBlending(BlendType.None);
        }

        public override void Complete()
        {
            base.Complete();
            m_afterImage = new AfterImage(afterImageData);
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "projid":
                    m_projectileId = GetAttribute<Expression>(expression, null);
                    break;
                case "projanim":
                    m_animation = GetAttribute<Expression>(expression, null);
                    break;
                case "projhitanim":
                    m_hitAnimation = GetAttribute<Expression>(expression, null);
                    break;
                case "projremanim":
                    m_removeAnimation = GetAttribute<Expression>(expression, null);
                    break;
                case "projcancelanim":
                    m_cancelAnimation = GetAttribute<Expression>(expression, null);
                    break;
                case "projscale":
                    m_scale = GetAttribute<Expression>(expression, null);
                    break;
                case "projremove":
                    m_removeOnHit = GetAttribute<Expression>(expression, null);
                    break;
                case "projremovetime":
                    m_removeTime = GetAttribute<Expression>(expression, null);
                    break;
                case "velocity":
                    m_velocity = GetAttribute<Expression>(expression, null);
                    break;
                case "remvelocity":
                    m_removeVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "accel":
                    m_acceleration = GetAttribute<Expression>(expression, null);
                    break;
                case "velmul":
                    m_velocityMultiplier = GetAttribute<Expression>(expression, null);
                    break;
                case "projhits":
                    m_hits = GetAttribute<Expression>(expression, null);
                    break;
                case "projmisstime":
                    m_missTime = GetAttribute<Expression>(expression, null);
                    break;
                case "projpriority":
                    m_projPriority = GetAttribute<Expression>(expression, null);
                    break;
                case "projsprpriority":
                    m_spritePriority = GetAttribute<Expression>(expression, null);
                    break;
                case "projedgebound":
                    m_edgeBound = GetAttribute<Expression>(expression, null);
                    break;
                case "projstagebound":
                    m_stageBound = GetAttribute<Expression>(expression, null);
                    break;
                case "projheightbound":
                    m_heightBound = GetAttribute<Expression>(expression, null);
                    break;
                case "offset":
                    m_offSet = GetAttribute<Expression>(expression, null);
                    break;
                case "postype":
                    m_posType = GetAttribute(expression, PositionType.P1);
                    break;
                case "projshadow":
                    m_shadow = GetAttribute<Expression>(expression, null);
                    break;
                case "supermovetime":
                    m_superMoveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "pausemovetime":
                    m_pauseMoveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "ownpal":
                    m_ownPal = GetAttribute<Expression>(expression, null);
                    break;
                case "remappal":
                    m_remapPal = GetAttribute<Expression>(expression, null);
                    break;

                //AfterImage
                case "afterimage.time":
                    afterImageData.m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.length":
                    afterImageData.m_numberOfFrames = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.palcolor":
                    afterImageData.m_paletteColor = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.palinvertall":
                    afterImageData.m_paletteInversion = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.palbright":
                    afterImageData.m_paletteBrightness = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.palcontrast":
                    afterImageData.m_paletteContrast = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.palpostbright":
                    afterImageData.m_palettePostBrightness = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.paladd":
                    afterImageData.m_paletteAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.palmul":
                    afterImageData.m_paletteMutliply = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.timegap":
                    afterImageData.m_timeGap = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.framegap":
                    afterImageData.m_frameGap = GetAttribute<Expression>(expression, null);
                    break;
                case "afterimage.trans":
                    afterImageData.m_trans = GetAttribute<Blending?>(expression, Misc.ToBlending(BlendType.None));
                    break;
                case "afterimage.alpha":
                    afterImageData.m_alpha = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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

            var heightbounds = EvaluationHelper.AsVector2(character, m_heightBound, new Vector2(-240, 1));
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