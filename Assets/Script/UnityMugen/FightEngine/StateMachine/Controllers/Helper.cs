﻿using System;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

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

        public Helper(string label) : base(label)
        {
            m_helperType = HelperType.Normal;
            m_posType = PositionType.P1;
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "helpertype":
                    m_helperType = GetAttribute(expression, HelperType.Normal);
                    break;
                case "name":
                    m_name = GetAttribute<string>(expression, null);
                    break;
                case "id":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
                case "pos":
                    m_position = GetAttribute<Expression>(expression, null);
                    break;
                case "postype":
                    m_posType = GetAttribute(expression, PositionType.P1);
                    break;
                case "facing":
                    m_facing = GetAttribute<Expression>(expression, null);
                    break;
                case "stateno":
                    m_stateNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "keyctrl":
                    m_keyCtrl = GetAttribute<Expression>(expression, null);
                    break;
                case "ownpal":
                    m_ownPal = GetAttribute<Expression>(expression, null);
                    break;
                case "remappal":
                    m_remapPal = GetAttribute<Expression>(expression, null);
                    break;
                case "supermovetime":
                    m_superMoveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "pausemovetime":
                    m_pauseMoveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "size.xscale":
                    m_xScale = GetAttribute<Expression>(expression, null);
                    break;
                case "size.yscale":
                    m_yScale = GetAttribute<Expression>(expression, null);
                    break;
                case "size.ground.back":
                    m_groundBack = GetAttribute<Expression>(expression, null);
                    break;
                case "size.ground.front":
                    m_groundFront = GetAttribute<Expression>(expression, null);
                    break;
                case "size.air.back":
                    m_airBack = GetAttribute<Expression>(expression, null);
                    break;
                case "size.air.front":
                    m_airFront = GetAttribute<Expression>(expression, null);
                    break;
                case "size.height":
                    m_height = GetAttribute<Expression>(expression, null);
                    break;
                case "size.proj.doscale":
                    m_projectScaling = GetAttribute<Expression>(expression, null);
                    break;
                case "size.head.pos":
                    m_headPos = GetAttribute<Expression>(expression, null);
                    break;
                case "size.mid.pos":
                    m_midPos = GetAttribute<Expression>(expression, null);
                    break;
                case "size.shadowoffset":
                    m_shadowOffSet = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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