﻿using UnityEngine;
using UnityMugen.Audio;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("SuperPause")]
    public class SuperPause : StateController
    {
        private Expression m_time;
        private Expression m_cmdBufferTime;
        private Expression m_moveTime;
        private Expression m_pauseBackground;
        private PrefixedExpression m_sparkNumber;
        private PrefixedExpression m_soundId;
        private Expression m_animPosition;
        private Expression m_darken;
        private Expression m_p2DefMul;
        private Expression m_powerAdd;
        private Expression m_unhittable;

        public SuperPause(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "endcmdbuftime":
                    m_cmdBufferTime = GetAttribute<Expression>(expression, null);
                    break;
                case "movetime":
                    m_moveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "anim":
                    m_sparkNumber = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "sound":
                    m_soundId = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "pausebg":
                    m_pauseBackground = GetAttribute<Expression>(expression, null);
                    break;
                case "pos":
                    m_animPosition = GetAttribute<Expression>(expression, null);
                    break;
                case "darken":
                    m_darken = GetAttribute<Expression>(expression, null);
                    break;
                case "p2defmul":
                    m_p2DefMul = GetAttribute<Expression>(expression, null);
                    break;
                case "poweradd":
                    m_powerAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "unhittable":
                    m_unhittable = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            int? time = EvaluationHelper.AsInt32(character, m_time, 30);
            var buffertime = EvaluationHelper.AsInt32(character, m_cmdBufferTime, 0);
            var movetime = EvaluationHelper.AsInt32(character, m_moveTime, 0);
            var pausebg = EvaluationHelper.AsBoolean(character, m_pauseBackground, true);
            var power = EvaluationHelper.AsInt32(character, m_powerAdd, 0);

            var animationnumber = EvaluationHelper.AsInt32(character, m_sparkNumber, 30);

            var soundid = EvaluationHelper.AsSoundId(character, m_soundId, null);
            var animationposition = EvaluationHelper.AsVector2(character, m_animPosition, Vector2.zero) * Constant.Scale;
            var darkenscreen = EvaluationHelper.AsBoolean(character, m_darken, true);
            var p2defmul = EvaluationHelper.AsSingle(character, m_p2DefMul, null);
            var unhittable = EvaluationHelper.AsBoolean(character, m_unhittable, true);

            if (time == null) return;

            var pause = character.Engine.SuperPause;
            pause.Set(character, time.Value, buffertime, movetime, false, pausebg);

            character.BasePlayer.Power += power;

#warning adicionar isso futuramente
            //character.BasePlayer.Unhittable = Time + (Time > 0 ? 1 : 0);

            var data = new ExplodData();
            data.PositionType = PositionType.P1;
            data.Location = animationposition;
            data.RemoveTime = -2;
            data.CommonAnimation = EvaluationHelper.IsCommon(m_sparkNumber, true);
            data.AnimationNumber = animationnumber;
            data.Scale = Vector2.one;
            data.SuperMove = true;
            data.Creator = character;
            data.Offseter = character;
            data.DrawOnTop = true;

            character.InstanceExplod(data);

            if (soundid != null)
            {
                SoundManager soundmanager = m_soundId.IsCommon(true) ? character.Engine.CommonSounds : character.SoundManager;
                soundmanager.Play(soundid.Value);
            }

            // CanvasFull //
            GraphicUIData graphicUIData = new GraphicUIData();
            graphicUIData.removetime = time;
            graphicUIData.color = new Color(0, 0, 0, 0.9137255f);
            character.Engine.stageScreen.CanvasFull.Set(character, graphicUIData);
            ///////////////
        }

    }
}