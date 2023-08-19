using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ModifyExplod")]
    public class ModifyExplod : StateController
    {

        private PrefixedExpression m_animationNumber;
        private Expression m_id;
        private Expression m_position;
        private PositionType? m_posType;
        private Expression m_facing;
        private Expression m_verticalFacing;
        private Expression m_bindTime;
        private Expression m_velocity;
        private Expression m_acceleration;
        private Expression m_randomDisplacement;
        private Expression m_removeTime;
        private Expression m_superMove;
        private Expression m_superMoveTime;
        private Expression m_pauseMoveTime;
        private Expression m_scale;
        private Expression m_spritePriority;
        private Expression m_drawOnTop;
        private Expression m_ownPal;
        private Expression m_removeOnGetHit;
        private Expression m_explodIgnoreHitPause;
        private Blending? m_blending;
        private Expression m_alpha;

#warning novo parametro criado por mim, Tiago -quando criar uma documentação ter que informar deste parametro
        public Expression m_reflection;

#warning não aplicado ainda
        //  public DeSpace space;

        /// <summary>
        /// nao esta sendo usado
        /// </summary>
#warning não aplicado ainda
        private Expression m_shadow;

        public ModifyExplod(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "id":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
                case "anim":
                    m_animationNumber = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "pos":
                    m_position = GetAttribute<Expression>(expression, null);
                    break;
                case "postype":
                    m_posType = GetAttribute<PositionType?>(expression, null);
                    break;
                case "facing":
                    m_facing = GetAttribute<Expression>(expression, null);
                    break;
                case "vfacing":
                    m_verticalFacing = GetAttribute<Expression>(expression, null);
                    break;
                case "bindtime":
                    m_bindTime = GetAttribute<Expression>(expression, null);
                    break;
                case "vel":
                case "velocity":
                    m_velocity = GetAttribute<Expression>(expression, null);
                    break;
                case "accel":
                    m_acceleration = GetAttribute<Expression>(expression, null);
                    break;
                case "random":
                    m_randomDisplacement = GetAttribute<Expression>(expression, null);
                    break;
                case "removetime":
                    m_removeTime = GetAttribute<Expression>(expression, null);
                    break;
                case "supermove":
                    m_superMove = GetAttribute<Expression>(expression, null);
                    break;
                case "supermovetime":
                    m_superMoveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "pausemovetime":
                    m_pauseMoveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "scale":
                    m_scale = GetAttribute<Expression>(expression, null);
                    break;
                case "sprpriority":
                    m_spritePriority = GetAttribute<Expression>(expression, null);
                    break;
                case "ontop":
                    m_drawOnTop = GetAttribute<Expression>(expression, null);
                    break;
                case "shadow":
                    m_shadow = GetAttribute<Expression>(expression, null);
                    break;
                case "ownpal":
                    m_ownPal = GetAttribute<Expression>(expression, null);
                    break;
                case "removeongethit":
                    m_removeOnGetHit = GetAttribute<Expression>(expression, null);
                    break;
                case "ignorehitpause":
                    m_explodIgnoreHitPause = GetAttribute<Expression>(expression, null);
                    break;
                case "alpha":
                    m_alpha = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var data = CreateModifyExplodData(character);
            if (data == null) return;

            foreach (var explod in character.GetExplods(data.Id)) explod.Modify(data);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_id == null)
                return false;

            return true;
        }

        private ModifyExplodData CreateModifyExplodData(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            int? ID = EvaluationHelper.AsInt32(character, m_id, null);
            if (ID == null)
            {
                Debug.Log("ModifyExplod : id Required");
                return null;
            }

            var data = new ModifyExplodData();
            data.CommonAnimation = EvaluationHelper.IsCommon(m_animationNumber, false);
            data.AnimationNumber = EvaluationHelper.AsInt32(character, m_animationNumber, null);
            data.Id = ID.Value;
            data.RemoveTime = EvaluationHelper.AsInt32(character, m_removeTime, null);
            data.Location = EvaluationHelper.AsVector2(character, m_position, null) * Constant.Scale;
            data.PositionType = m_posType;
            data.Velocity = EvaluationHelper.AsVector2(character, m_velocity, null) * Constant.Scale;
            data.Acceleration = EvaluationHelper.AsVector2(character, m_acceleration, null) * Constant.Scale;

            SpriteEffects? flip = null;
            var horizfacing = EvaluationHelper.AsInt32(character, m_facing, null);
            var vertfacing = EvaluationHelper.AsInt32(character, m_verticalFacing, null);
            if (horizfacing != null || vertfacing != null) flip = SpriteEffects.None;
            if (horizfacing == -1) flip ^= SpriteEffects.FlipHorizontally;
            if (vertfacing == -1) flip ^= SpriteEffects.FlipVertically;
            data.Flip = flip;

            data.BindTime = EvaluationHelper.AsInt32(character, m_bindTime, null);
            data.Random = EvaluationHelper.AsVector2(character, m_randomDisplacement, null) * Constant.Scale;
            data.SuperMove = EvaluationHelper.AsBoolean(character, m_superMove, null);
            data.SuperMoveTime = EvaluationHelper.AsInt32(character, m_superMoveTime, null);
            data.PauseTime = EvaluationHelper.AsInt32(character, m_pauseMoveTime, null);
            data.Scale = EvaluationHelper.AsVector2(character, m_scale, null, 1);
            data.SpritePriority = EvaluationHelper.AsInt32(character, m_spritePriority, null);
            data.DrawOnTop = EvaluationHelper.AsBoolean(character, m_drawOnTop, null);
            data.OwnPalFx = EvaluationHelper.AsBoolean(character, m_ownPal, null);
            data.RemoveOnGetHit = EvaluationHelper.AsBoolean(character, m_removeOnGetHit, null);
            data.IgnoreHitPause = EvaluationHelper.AsBoolean(character, m_explodIgnoreHitPause, null);

            var alpha = EvaluationHelper.AsVector2(character, m_alpha, null);
            var transparency = m_blending;
            if (transparency != null && transparency.Value.BlendType == BlendType.Add && transparency.Value.SourceFactor == 0 && transparency.Value.DestinationFactor == 0)
            {
                if (alpha != null)
                {
                    transparency = new Blending(BlendType.Add, alpha.Value.x, alpha.Value.y);
                }
                else
                {
                    transparency = new Blending();
                }
            }
            data.Transparency = transparency;

            return data;
        }

    }
}