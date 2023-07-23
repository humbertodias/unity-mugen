using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Helper")]
    public class Helper : StateController
    {

        [Obsolete("helpertype is deprecated.")]
        private HelperType m_helperType;

        private string m_name;
        private Expression m_id;
        private Expression m_position;
        private PositionType m_posType;
        private Expression m_facing;
        private Expression m_stateNumber;
        private Expression m_keyCtrl;

#warning Não aplicado ainda
        private Expression m_ownPal;

#warning Não aplicado ainda
        private Expression m_remapPal;

        private Expression m_superMoveTime;
        private Expression m_pauseMoveTime;
        private Expression m_xScale;
        private Expression m_yScale;
        private Expression m_groundBack;
        private Expression m_groundFront;
        private Expression m_airBack;
        private Expression m_airFront;
        private Expression m_height;
        private Expression m_projectScaling;
        private Expression m_headPos;
        private Expression m_midPos;
        private Expression m_shadowOffSet;

        public Helper(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_helperType = textSection.GetAttribute("helpertype", HelperType.Normal);
                m_name = textSection.GetAttribute<string>("name", null);
                m_id = textSection.GetAttribute<Expression>("id", null);
                m_position = textSection.GetAttribute<Expression>("pos", null);
                m_posType = textSection.GetAttribute("postype", PositionType.P1);
                m_facing = textSection.GetAttribute<Expression>("facing", null);
                m_stateNumber = textSection.GetAttribute<Expression>("stateno", null);
                m_keyCtrl = textSection.GetAttribute<Expression>("keyctrl", null);
                m_ownPal = textSection.GetAttribute<Expression>("ownpal", null);
                m_remapPal = textSection.GetAttribute<Expression>("remappal", null);
                m_superMoveTime = textSection.GetAttribute<Expression>("SuperMoveTime", null);
                m_pauseMoveTime = textSection.GetAttribute<Expression>("PauseMoveTime", null);
                m_xScale = textSection.GetAttribute<Expression>("size.xscale", null);
                m_yScale = textSection.GetAttribute<Expression>("size.yscale", null);
                m_groundBack = textSection.GetAttribute<Expression>("size.ground.back", null);
                m_groundFront = textSection.GetAttribute<Expression>("size.ground.front", null);
                m_airBack = textSection.GetAttribute<Expression>("size.air.back", null);
                m_airFront = textSection.GetAttribute<Expression>("size.air.front", null);
                m_height = textSection.GetAttribute<Expression>("size.height", null);
                m_projectScaling = textSection.GetAttribute<Expression>("size.proj.doscale", null);
                m_headPos = textSection.GetAttribute<Expression>("size.head.pos", null);
                m_midPos = textSection.GetAttribute<Expression>("size.mid.pos", null);
                m_shadowOffSet = textSection.GetAttribute<Expression>("size.shadowoffset", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var helperName = m_name ?? character.BasePlayer.profile.displayName + "'s Helper";
            var helperId = EvaluationHelper.AsInt32(character, m_id, 0);
            var positionOffset = (Vector2)EvaluationHelper.AsVector2(character, m_position, Vector2.zero) * Constant.Scale;
            var facingflag = EvaluationHelper.AsInt32(character, m_facing, 1);
            var statenumber = EvaluationHelper.AsInt32(character, m_stateNumber, 0);
            var keycontrol = EvaluationHelper.AsBoolean(character, m_keyCtrl, false);
            var ownpalette = EvaluationHelper.AsBoolean(character, m_ownPal, false);
            var supermovetime = EvaluationHelper.AsInt32(character, m_superMoveTime, 0);
            var pausemovetime = EvaluationHelper.AsInt32(character, m_pauseMoveTime, 0);
            var scalex = EvaluationHelper.AsSingle(character, m_xScale, character.BasePlayer.playerConstants.Scale.x);
            var scaley = EvaluationHelper.AsSingle(character, m_yScale, character.BasePlayer.playerConstants.Scale.y);
            var groundfront = EvaluationHelper.AsSingle(character, m_groundFront, character.BasePlayer.playerConstants.GroundFront) * Constant.Scale;
            var groundback = EvaluationHelper.AsSingle(character, m_groundBack, character.BasePlayer.playerConstants.GroundBack) * Constant.Scale;
            var airfront = EvaluationHelper.AsSingle(character, m_airFront, character.BasePlayer.playerConstants.Airfront) * Constant.Scale;
            var airback = EvaluationHelper.AsSingle(character, m_airBack, character.BasePlayer.playerConstants.Airback) * Constant.Scale;
            var height = EvaluationHelper.AsSingle(character, m_height, character.BasePlayer.playerConstants.Height) * Constant.Scale;
            var projectilescaling = EvaluationHelper.AsBoolean(character, m_projectScaling, character.BasePlayer.playerConstants.ProjectileScaling);
            var headposition = (Vector2)EvaluationHelper.AsVector2(character, m_headPos, character.BasePlayer.playerConstants.Headposition) * Constant.Scale;
            var midposition = (Vector2)EvaluationHelper.AsVector2(character, m_midPos, character.BasePlayer.playerConstants.Midposition) * Constant.Scale;
            var shadowoffset = EvaluationHelper.AsSingle(character, m_shadowOffSet, character.BasePlayer.playerConstants.Shadowoffset);

            var data = new HelperData();
            data.Name = helperName;
            data.HelperId = helperId;
            data.Type = m_helperType;
            data.FacingFlag = facingflag;
            data.PositionType = m_posType;
            data.CreationOffset = positionOffset;
            data.KeyControl = keycontrol;
            data.OwnPaletteFx = ownpalette;
            data.InitialStateNumber = statenumber;
            data.Scale = new Vector2(scalex, scaley);
            data.GroundFront = groundfront;
            data.GroundBack = groundback;
            data.AirFront = airfront;
            data.AirBack = airback;
            data.Height = height;
            data.SuperPauseTime = supermovetime;
            data.PauseTime = pausemovetime;
            data.ProjectileScaling = projectilescaling;
            data.HeadPosition = headposition;
            data.MidPosition = midposition;
            data.ShadowOffset = shadowoffset;

            character.InstanceHelper(data);
        }

    }
}