using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{
    [StateControllerName("Explod")]
    public class Explod : StateController
    {
        private PrefixedExpression m_animationNumber;
        private Expression m_id;
        private Expression m_position;

        private Expression m_facing;
        private Expression m_verticalFacing;
        private Expression m_bindtime;
        private Expression m_velocity;
        private Expression m_acceleration;
        private Expression m_removeTime;
        private Expression m_superMoveTime;
        private Expression m_pauseMoveTime;
        private Expression m_scale;

        private Expression m_angle;
#warning Não aplicado ainda
        private Expression m_yAngle;
#warning Não aplicado ainda
        private Expression m_xAngle;

        private Expression m_spritePriority;
        private Expression m_drawOnTop;

#warning Não aplicado ainda
        private Expression m_ownPal;

#warning Não aplicado ainda
        private Expression m_remapPal;

        private Expression m_removeOnGetHit;
        private Expression m_explodIgnoreHitPause;
        private Blending m_blending;
        private Expression m_alpha;
        private Expression m_shadow;

#warning novo parametro criado por mim, Tiago -quando criar uma documentação ter que informar deste parametro
        public Expression m_reflection;

#warning não aplicado ainda
        //  public DeSpace space;

        [Obsolete("Deprecated parameters")]
        private PositionType? m_posType;

        [Obsolete("Deprecated parameters")]
        private Expression m_random;

        [Obsolete("Deprecated parameters")]
        private Expression m_superMove;

        public Explod(string label) : base(label)
        {
            m_posType = UnityMugen.PositionType.P1;
            m_blending = Misc.ToBlending(BlendType.None);
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "anim":
                    m_animationNumber = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "id":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
                case "pos":
                    m_position = GetAttribute<Expression>(expression, null);
                    break;
                case "postype":
                    {
                        //if (chara.BasePlayer.profile.mugenVersion == MugenVersion.V_1_1)
                        //     Debug.LogWarning("postype is deprecated. Read the documentation.");
                        //    m_posType = GetAttribute(expression, UnityMugen.PositionType.None);
                        //else
                        m_posType = GetAttribute(expression, UnityMugen.PositionType.P1);
                        break;
                    }
                case "facing":
                    m_facing = GetAttribute<Expression>(expression, null);
                    break;
                case "vfacing":
                    m_verticalFacing = GetAttribute<Expression>(expression, null);
                    break;
                case "bindtime":
                    m_bindtime = GetAttribute<Expression>(expression, null);
                    break;
                case "vel":
                case "velocity":
                    m_velocity = GetAttribute<Expression>(expression, null);
                    break;
                case "accel":
                    m_acceleration = GetAttribute<Expression>(expression, null);
                    break;
                case "random":
                    m_random = GetAttribute<Expression>(expression, null);
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
                case "angle":
                    m_angle = GetAttribute<Expression>(expression, null);
                    break;
                case "yangle":
                    m_yAngle = GetAttribute<Expression>(expression, null);
                    break;
                case "xangle":
                    m_xAngle = GetAttribute<Expression>(expression, null);
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
                case "remappal":
                    m_remapPal = GetAttribute<Expression>(expression, null);
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
                case "trans":
                    m_blending = GetAttribute<Blending>(expression, Misc.ToBlending(BlendType.None));
                    break;
            }
        }



        public override void Run(Character character)
        {
            var data = CreateExplodData(character);
            if (data == null)
            {
                Debug.Log("Explod : data Required");
                return;
            }

            data.Creator = character;
            data.Offseter = data.PositionType == UnityMugen.PositionType.P2 ? character.GetOpponent() : character;

            character.InstanceExplod(data);
        }

        private ExplodData CreateExplodData(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            var animationnumber = EvaluationHelper.AsInt32(character, m_animationNumber, null);
            if (animationnumber == null)
            {
                Debug.Log("Explod : anim Required");
                return null;
            }

            var id = EvaluationHelper.AsInt32(character, m_id, -1);
            var location = EvaluationHelper.AsVector2(character, m_position, Vector2.zero) * Constant.Scale;
            var horizfacing = EvaluationHelper.AsInt32(character, m_facing, 1);
            var vertfacing = EvaluationHelper.AsInt32(character, m_verticalFacing, 1);
            var bindtime = EvaluationHelper.AsInt32(character, m_bindtime, 0);
            var velocity = EvaluationHelper.AsVector2(character, m_velocity, Vector2.zero) * Constant.Scale;
            var acceleration = EvaluationHelper.AsVector2(character, m_acceleration, Vector2.zero) * Constant.Scale;
            var random = EvaluationHelper.AsVector2(character, m_random, Vector2.zero) * Constant.Scale;
            var removetime = EvaluationHelper.AsInt32(character, m_removeTime, -2);
            var supermove = EvaluationHelper.AsBoolean(character, m_superMove, false);
            var supermovetime = EvaluationHelper.AsInt32(character, m_superMoveTime, 0);
            var pausetime = EvaluationHelper.AsInt32(character, m_pauseMoveTime, 0);
            var scale = EvaluationHelper.AsVector2(character, m_scale, Vector2.one);

            var angle = EvaluationHelper.AsSingle(character, m_angle, 0);
            var yAngle = EvaluationHelper.AsSingle(character, m_yAngle, 0);
            var xAngle = EvaluationHelper.AsSingle(character, m_xAngle, 0);

            var spritepriority = EvaluationHelper.AsInt32(character, m_spritePriority, 0);
            var ontop = EvaluationHelper.AsBoolean(character, m_drawOnTop, false);
            var ownpalette = EvaluationHelper.AsBoolean(character, m_ownPal, false);
            var removeongethit = EvaluationHelper.AsBoolean(character, m_removeOnGetHit, false);
            var ignorehitpause = EvaluationHelper.AsBoolean(character, m_explodIgnoreHitPause, true);
            var shadow = EvaluationHelper.AsBoolean(character, m_shadow, false);
            var reflection = EvaluationHelper.AsBoolean(character, m_reflection, false);
            var alpha = EvaluationHelper.AsVector2(character, m_alpha, null);


            var data = new ExplodData();
            data.CommonAnimation = EvaluationHelper.IsCommon(m_animationNumber, false);
            data.AnimationNumber = animationnumber.Value;
            data.ExplodId = id;
            data.RemoveTime = removetime;
            data.Location = location;
            data.PositionType = m_posType.Value;
            data.Velocity = velocity;
            data.Acceleration = acceleration;
            data.BindTime = bindtime;
            data.Random = random / 2;
            data.SuperMove = supermove;
            data.SuperMoveTime = supermovetime;
            data.PauseTime = pausetime;
            data.Scale = scale;
            data.Angle = angle;
            data.SpritePriority = spritepriority;
            data.DrawOnTop = ontop;
            data.OwnPalFx = ownpalette;
            data.RemoveOnGetHit = removeongethit;
            data.IgnoreHitPause = ignorehitpause;
            data.Shadow = shadow;
            data.Reflection = reflection;

            var flip = SpriteEffects.None;
            if (horizfacing == -1) flip ^= SpriteEffects.FlipHorizontally;
            if (vertfacing == -1) flip ^= SpriteEffects.FlipVertically;
            data.Flip = flip;

            var transparency = m_blending;
            if (transparency.BlendType == BlendType.Add && transparency.SourceFactor == 0 && transparency.DestinationFactor == 0)
            {
                if (alpha != null)
                    data.Transparency = new Blending(BlendType.Add, alpha.Value.x, alpha.Value.y);
                else
                    data.Transparency = new Blending();
            }

            if (transparency.BlendType == BlendType.AddAlpha && alpha == Vector2.zero)
            {
                Debug.Log("Parametro Alpha requerido.");
            }

            return data;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_animationNumber == null)
                return false;

            return true;
        }

    }
}