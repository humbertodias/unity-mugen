using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;
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
        private Expression m_randomDisplacement;

        [Obsolete("Deprecated parameters")]
        private Expression m_superMove;

        public Explod(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_animationNumber = textSection.GetAttribute<PrefixedExpression>("anim", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_id = textSection.GetAttribute<Expression>("id", null);
                m_position = textSection.GetAttribute<Expression>("pos", null);

                if (chara.BasePlayer.profile.mugenVersion == MugenVersion.V_1_1)
                    Debug.LogError("postype is deprecated. Read the documentation.");
                //    m_posType = textSection.GetAttribute("postype", UnityMugen.PositionType.None);
                //else
                    m_posType = textSection.GetAttribute("postype", UnityMugen.PositionType.P1);
    
                m_facing = textSection.GetAttribute<Expression>("facing", null);
                m_verticalFacing = textSection.GetAttribute<Expression>("vfacing", null);
                m_bindtime = textSection.GetAttribute<Expression>("BindTime", null);

                var expVel = textSection.GetAttribute<Expression>("vel", null);
                var expVelocity = textSection.GetAttribute<Expression>("velocity", null);
                m_velocity = expVel ?? expVelocity;

                m_acceleration = textSection.GetAttribute<Expression>("accel", null);
                m_randomDisplacement = textSection.GetAttribute<Expression>("random", null);
                m_removeTime = textSection.GetAttribute<Expression>("removetime", null);
                m_superMove = textSection.GetAttribute<Expression>("supermove", null);
                m_superMoveTime = textSection.GetAttribute<Expression>("supermovetime", null);
                m_pauseMoveTime = textSection.GetAttribute<Expression>("pausemovetime", null);
                m_scale = textSection.GetAttribute<Expression>("scale", null);
                m_spritePriority = textSection.GetAttribute<Expression>("sprpriority", null);
                m_drawOnTop = textSection.GetAttribute<Expression>("ontop", null);
                m_shadow = textSection.GetAttribute<Expression>("shadow", null);
                m_ownPal = textSection.GetAttribute<Expression>("ownpal", null);
                m_remapPal = textSection.GetAttribute<Expression>("remappal", null);
                m_removeOnGetHit = textSection.GetAttribute<Expression>("removeongethit", null);
                m_explodIgnoreHitPause = textSection.GetAttribute<Expression>("ignorehitpause", null);

                m_alpha = textSection.GetAttribute<Expression>("alpha", null);
                m_blending = textSection.GetAttribute<Blending>("trans", Misc.ToBlending(BlendType.None));
            }
        }

        Character chara;
        public override void Run(Character character)
        {
            chara = character;
            Load();

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
            var randomdisplacement = EvaluationHelper.AsVector2(character, m_randomDisplacement, Vector2.zero) * Constant.Scale;
            var removetime = EvaluationHelper.AsInt32(character, m_removeTime, -2);
            var supermove = EvaluationHelper.AsBoolean(character, m_superMove, false);
            var supermovetime = EvaluationHelper.AsInt32(character, m_superMoveTime, 0);
            var pausetime = EvaluationHelper.AsInt32(character, m_pauseMoveTime, 0);
            var scale = EvaluationHelper.AsVector2(character, m_scale, Vector2.one);
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
            data.Random = randomdisplacement;
            data.SuperMove = supermove;
            data.SuperMoveTime = supermovetime;
            data.PauseTime = pausetime;
            data.Scale = scale;
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