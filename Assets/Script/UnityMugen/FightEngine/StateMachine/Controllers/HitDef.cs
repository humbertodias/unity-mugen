using System;
using UnityEngine;
using UnityMugen.Audio;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitDef")]
    public class HitDef : StateController
    {

        protected HitAttribute m_hitAttr;
        private HitFlag m_hitFlag;
        private HitFlag m_guardFlag;
        private AffectTeam m_affectTeam;
        private HitAnimationType m_hitAnimType;
        private HitAnimationType m_airHitAnimType;
        private HitAnimationType m_fallHitAnimType;
        private HitPriority m_priority;
        private Expression m_damage;
        private Expression m_pauseTime;
        private Expression m_guardPauseTime;
        private PrefixedExpression m_sparkNumber;
        private PrefixedExpression m_guardSparkNumber;
        private Expression m_sparkPosition;
        private PrefixedExpression m_hitSound;
        private PrefixedExpression m_guardHitSound;
        private AttackEffect m_attackEffect;
        private AttackEffect m_airEffect;
        private Expression m_groundSlideTime;
        private Expression m_guardSlideTime;
        private Expression m_groundHitTime;
        private Expression m_guardHitTime;
        private Expression m_airHitTime;
        private Expression m_guardCtrlTime;
        private Expression m_guardDistance;
        private Expression m_yaccel;
        private Expression m_groundVelocity;
        private Expression m_guardVelocity;
        private Expression m_airVelocity;
        private Expression m_airGuardVelocity;
        private Expression m_groundCornerPushOff;
        private Expression m_airCornerPushOff;
        private Expression m_downCornerPushOff;
        private Expression m_guardCornerPushOff;
        private Expression m_airGuardCornerPushOff;
        private Expression m_airGuardCtrlTime;
        private Expression m_airJuggle;
        private Expression m_minDistance;
        private Expression m_maxDistance;
        private Expression m_snap;
        private Expression m_p1SpritePriority;
        private Expression m_p2SpritePriority;
        private Expression m_p1Facing;
        private Expression m_p1GetP2Facing;
        private Expression m_p2Facing;
        private Expression m_p1StateNumber;
        private Expression m_p2StateNumber;
        private Expression m_p2GetP1State;
        private Expression m_forceStand;
        private Expression m_fall;
        private Expression m_fallXVelocity;
        private Expression m_fallYVelocity;
        private Expression m_fallRecover;
        private Expression m_fallRecoverTime;
        private Expression m_fallDamage;
        private Expression m_airFall;

#warning ainda nao aplicado
        private Expression m_forceNoFall;

        private Expression m_downVelocity;
        private Expression m_downHitTime;
        private Expression m_downBounce;
        private Expression m_targetId;
        private Expression m_chainId;
        private Expression m_noChainId;
        private Expression m_hitOnce;
        private Expression m_kill;
        private Expression m_guardKill;
        private Expression m_fallKill;
        private Expression m_numberOfHits;
        private Expression m_p1PowerIncrease;
        private Expression m_p2PowerIncrease;
        private Expression m_palTime;
        private Expression m_palMul;
        private Expression m_palAdd;
        private Expression m_palSinAdd;
        private Expression m_palInvert;
        private Expression m_palColor;
        private Expression m_shakeTime;
        private Expression m_shakeFreq;
        private Expression m_shakeAmplitude;
        private Expression m_shakePhaseOffSet;
        private Expression m_fallShakeTime;
        private Expression m_fallShakeFreq;
        private Expression m_fallShakeAmplitude;
        private Expression m_fallShakePhaseOffSet;
        private Expression m_score;

        TempHitDef tempHitDef;

        public HitDef(string label) : base(label)
        {
            m_hitFlag = HitFlag.Default;
            m_guardFlag = HitFlag.NoGuard;
            m_affectTeam = AffectTeam.Enemy;
            m_hitAnimType = HitAnimationType.Light;
            m_priority = HitPriority.Default;
            m_attackEffect = AttackEffect.High;
            tempHitDef = new TempHitDef();
        }

        public struct TempHitDef
        {
            public string m_airHitAnimType;
            public string m_fallHitAnimType;
            public string m_airEffect;
        }

        public override void Complete()
        {
            m_airHitAnimType = GetAttribute(tempHitDef.m_airHitAnimType, m_hitAnimType);
            m_fallHitAnimType = GetAttribute(tempHitDef.m_fallHitAnimType, m_airHitAnimType == HitAnimationType.Up ? HitAnimationType.Up : HitAnimationType.Back);
            m_airEffect = GetAttribute(tempHitDef.m_airEffect, m_attackEffect);
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "attr":
                    m_hitAttr = GetAttribute<HitAttribute>(expression, null);
                    break;

                case "hitflag":
                    {
#warning hack because some animals kept the attribute no I don't put a value
                        if (string.IsNullOrEmpty(expression))
                            m_hitFlag = null;
                        else
                            m_hitFlag = GetAttribute(expression, HitFlag.Default);
                        break;
                    }
                case "guardflag":
                    m_guardFlag = GetAttribute(expression, HitFlag.NoGuard);
                    break;
                case "affectteam":
                    m_affectTeam = GetAttribute(expression, AffectTeam.Enemy);
                    break;
                case "animtype":
                    m_hitAnimType = GetAttribute(expression, HitAnimationType.Light);
                    break;
                case "air.animtype":
                    tempHitDef.m_airHitAnimType = expression;
                    break;
                case "fall.animtype":
                    tempHitDef.m_fallHitAnimType = expression;
                    break;
                case "priority":
                    m_priority = GetAttribute(expression, HitPriority.Default);
                    break;
                case "damage":
                    m_damage = GetAttribute<Expression>(expression, null);
                    break;
                case "pausetime":
                    m_pauseTime = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.pausetime":
                    m_guardPauseTime = GetAttribute<Expression>(expression, null);
                    break;
                case "sparkno":
                    m_sparkNumber = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "guard.sparkno":
                    m_guardSparkNumber = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "sparkxy":
                    m_sparkPosition = GetAttribute<Expression>(expression, null);
                    break;
                case "hitsound":
                    m_hitSound = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "guardsound":
                    m_guardHitSound = GetAttribute<PrefixedExpression>(expression, null);
                    break;
                case "ground.type":
                    m_attackEffect = GetAttribute(expression, AttackEffect.High);
                    break;
                case "air.type":
                    tempHitDef.m_airEffect = expression;
                    break;
                case "ground.slidetime":
                    m_groundSlideTime = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.slidetime":
                    m_guardSlideTime = GetAttribute<Expression>(expression, null);
                    break;
                case "ground.hittime":
                    m_groundHitTime = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.hittime":
                    m_guardHitTime = GetAttribute<Expression>(expression, null);
                    break;
                case "air.hittime":
                    m_airHitTime = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.ctrltime":
                    m_guardCtrlTime = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.dist":
                    m_guardDistance = GetAttribute<Expression>(expression, null);
                    break;
                case "yaccel":
                    m_yaccel = GetAttribute<Expression>(expression, null);
                    break;
                case "ground.velocity":
                    m_groundVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.velocity":
                    m_guardVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "air.velocity":
                    m_airVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "airguard.velocity":
                    m_airGuardVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "ground.cornerpush.veloff":
                    m_groundCornerPushOff = GetAttribute<Expression>(expression, null);
                    break;
                case "air.cornerpush.veloff":
                    m_airCornerPushOff = GetAttribute<Expression>(expression, null);
                    break;
                case "down.cornerpush.veloff":
                    m_downCornerPushOff = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.cornerpush.veloff":
                    m_guardCornerPushOff = GetAttribute<Expression>(expression, null);
                    break;
                case "airguard.cornerpush.veloff":
                    m_airGuardCornerPushOff = GetAttribute<Expression>(expression, null);
                    break;
                case "airguard.ctrltime":
                    m_airGuardCtrlTime = GetAttribute<Expression>(expression, null);
                    break;
                case "air.juggle":
                    m_airJuggle = GetAttribute<Expression>(expression, null);
                    break;
                case "mindist":
                    m_minDistance = GetAttribute<Expression>(expression, null);
                    break;
                case "maxdist":
                    m_maxDistance = GetAttribute<Expression>(expression, null);
                    break;
                case "snap":
                    m_snap = GetAttribute<Expression>(expression, null);
                    break;
                case "p1sprpriority":
                    m_p1SpritePriority = GetAttribute<Expression>(expression, null);
                    break;
                case "p2sprpriority":
                    m_p2SpritePriority = GetAttribute<Expression>(expression, null);
                    break;
                case "p1facing":
                    m_p1Facing = GetAttribute<Expression>(expression, null);
                    break;
                case "p1getp2facing":
                    m_p1GetP2Facing = GetAttribute<Expression>(expression, null);
                    break;
                case "p2facing":
                    m_p2Facing = GetAttribute<Expression>(expression, null);
                    break;
                case "p1stateno":
                    m_p1StateNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "p2stateno":
                    m_p2StateNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "p2getp1state":
                    m_p2GetP1State = GetAttribute<Expression>(expression, null);
                    break;
                case "forcestand":
                    m_forceStand = GetAttribute<Expression>(expression, null);
                    break;
                case "fall":
                    m_fall = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.xvelocity":
                    m_fallXVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.yvelocity":
                    m_fallYVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.recover":
                    m_fallRecover = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.recovertime":
                    m_fallRecoverTime = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.damage":
                    m_fallDamage = GetAttribute<Expression>(expression, null);
                    break;
                case "air.fall":
                    m_airFall = GetAttribute<Expression>(expression, null);
                    break;
                case "forcenofall ":
                    m_forceNoFall = GetAttribute<Expression>(expression, null);
                    break;
                case "down.velocity":
                    m_downVelocity = GetAttribute<Expression>(expression, null);
                    break;
                case "down.hittime":
                    m_downHitTime = GetAttribute<Expression>(expression, null);
                    break;
                case "down.bounce":
                    m_downBounce = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
                case "chainid":
                    m_chainId = GetAttribute<Expression>(expression, null);
                    break;
                case "nochainid":
                    m_noChainId = GetAttribute<Expression>(expression, null);
                    break;
                case "hitonce":
                    m_hitOnce = GetAttribute<Expression>(expression, null);
                    break;
                case "kill":
                    m_kill = GetAttribute<Expression>(expression, null);
                    break;
                case "guard.kill":
                    m_guardKill = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.kill":
                    m_fallKill = GetAttribute<Expression>(expression, null);
                    break;
                case "numhits":
                    m_numberOfHits = GetAttribute<Expression>(expression, null);
                    break;
                case "getpower":
                    m_p1PowerIncrease = GetAttribute<Expression>(expression, null);
                    break;
                case "givepower":
                    m_p2PowerIncrease = GetAttribute<Expression>(expression, null);
                    break;
                case "palfx.time":
                    m_palTime = GetAttribute<Expression>(expression, null);
                    break;
                case "palfx.mul":
                    m_palMul = GetAttribute<Expression>(expression, null);
                    break;
                case "palfx.add":
                    m_palAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "palfx.sinadd":
                    m_palSinAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "palfx.invertall":
                    m_palInvert = GetAttribute<Expression>(expression, null);
                    break;
                case "palfx.color":
                    m_palColor = GetAttribute<Expression>(expression, null);
                    break;
                case "envshake.time":
                    m_shakeTime = GetAttribute<Expression>(expression, null);
                    break;
                case "envshake.freq":
                    m_shakeFreq = GetAttribute<Expression>(expression, null);
                    break;
                case "envshake.ampl":
                    m_shakeAmplitude = GetAttribute<Expression>(expression, null);
                    break;
                case "envshake.phase":
                    m_shakePhaseOffSet = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.envshake.time":
                    m_fallShakeTime = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.envshake.freq":
                    m_fallShakeFreq = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.envshake.ampl":
                    m_fallShakeAmplitude = GetAttribute<Expression>(expression, null);
                    break;
                case "fall.envshake.phase":
                    m_fallShakePhaseOffSet = GetAttribute<Expression>(expression, null);
                    break;
                case "score":
                    m_score = GetAttribute<Expression>(expression, null);
                    break;

                    // Este parametro nem tem no mugen 1.0 e numca foi usado nas versoes anteriores
                    //case "attack.width":
                    //    m_attackwidth = GetAttribute<Expression>(expression, null);
                    //    break;
            }
        }

        public override void Run(Character character)
        {
            SetHitDefinition(character, character.OffensiveInfo.HitDef);

            character.OffensiveInfo.ActiveHitDef = true;
            character.OffensiveInfo.HitPauseTime = 0;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_hitAttr == null)
                return false;

            return true;
        }

        protected void SetHitDefinition(Character character, HitDefinition hitdef)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));
            if (hitdef == null) throw new ArgumentNullException(nameof(hitdef));

            var defaulthitspark = character.BasePlayer.playerConstants.DefaultSparkNumber;
            var defaultplayerhitspark = !character.BasePlayer.playerConstants.DefaultSparkNumberIsCommon;

            var defaultguardspark = character.BasePlayer.playerConstants.DefaultGuardSparkNumber;
            var defaultplayerguardspark = !character.BasePlayer.playerConstants.DefaultGuardSparkNumberIsCommon;

            hitdef.HitAttribute = m_hitAttr;
            hitdef.HitFlag = m_hitFlag;
            hitdef.GuardFlag = m_guardFlag;
            hitdef.Targeting = m_affectTeam;
            hitdef.GroundAnimationType = m_hitAnimType;
            hitdef.AirAnimationType = m_airHitAnimType;
            hitdef.FallAnimationType = m_fallHitAnimType;
            hitdef.HitPriority = m_priority;

            var damage = EvaluationHelper.AsVector2(character, m_damage, Vector2.zero);
            hitdef.HitDamage = damage.x;
            hitdef.GuardDamage = damage.y;

            var pauseshaketime = EvaluationHelper.AsVector2(character, m_pauseTime, Vector2.zero);
            hitdef.PauseTime = (int)pauseshaketime.x;
            hitdef.ShakeTime = (int)pauseshaketime.y;

            var guardpauseshaketime = EvaluationHelper.AsVector2(character, m_guardPauseTime, pauseshaketime);
            hitdef.GuardPauseTime = (int)guardpauseshaketime.x;
            hitdef.GuardShakeTime = (int)guardpauseshaketime.y;

            hitdef.PlayerSpark = !EvaluationHelper.IsCommon(m_sparkNumber, !defaultplayerhitspark);
            hitdef.SparkAnimation = EvaluationHelper.AsInt32(character, m_sparkNumber, defaulthitspark);

            hitdef.GuardPlayerSpark = !EvaluationHelper.IsCommon(m_guardSparkNumber, !defaultplayerguardspark);
            hitdef.GuardSparkAnimation = EvaluationHelper.AsInt32(character, m_guardSparkNumber, defaultguardspark);

            hitdef.SparkStartPosition = (Vector2)EvaluationHelper.AsVector2(character, m_sparkPosition, Vector2.zero) * Constant.Scale;

            hitdef.PlayerSound = !EvaluationHelper.IsCommon(m_hitSound, true);
            hitdef.HitSoundId = EvaluationHelper.AsSoundId(character, m_hitSound, SoundId.Invalid);

            hitdef.GuardPlayerSound = !EvaluationHelper.IsCommon(m_guardHitSound, true);
            hitdef.GuardSoundId = EvaluationHelper.AsSoundId(character, m_guardHitSound, SoundId.Invalid);

            hitdef.GroundAttackEffect = m_attackEffect;
            hitdef.AirAttackEffect = m_airEffect;

            hitdef.GroundHitTime = EvaluationHelper.AsInt32(character, m_groundHitTime, 0);
            hitdef.DownHitTime = EvaluationHelper.AsInt32(character, m_downHitTime, 0);
            hitdef.GuardHitTime = EvaluationHelper.AsInt32(character, m_guardHitTime, hitdef.GroundHitTime);
            hitdef.AirHitTime = EvaluationHelper.AsInt32(character, m_airHitTime, 20);
            hitdef.GroundSlideTime = EvaluationHelper.AsInt32(character, m_groundSlideTime, 0);
            hitdef.GuardSlideTime = EvaluationHelper.AsInt32(character, m_guardSlideTime, hitdef.GuardHitTime);
            hitdef.GuardControlTime = EvaluationHelper.AsInt32(character, m_guardCtrlTime, hitdef.GuardSlideTime);
            hitdef.AirGuardControlTime = EvaluationHelper.AsInt32(character, m_airGuardCtrlTime, hitdef.GuardControlTime);

            hitdef.GuardDistance = EvaluationHelper.AsSingle(character, m_guardDistance, character.BasePlayer.playerConstants.Attackdistance) * Constant.Scale;
            hitdef.YAcceleration = EvaluationHelper.AsSingle(character, m_yaccel, Constant.YaccelDefault) * Constant.Scale;
            hitdef.GroundVelocity = EvaluationHelper.AsVector2(character, m_groundVelocity, Vector2.zero) * Constant.Scale;
            hitdef.GroundGuardVelocity = new Vector2(EvaluationHelper.AsSingle(character, m_guardVelocity, hitdef.GroundVelocity.x * Constant.Scale2), 0) * Constant.Scale;
            hitdef.AirVelocity = EvaluationHelper.AsVector2(character, m_airVelocity, Vector2.zero) * Constant.Scale;
            hitdef.DownVelocity = EvaluationHelper.AsVector2(character, m_downVelocity, hitdef.AirVelocity * Constant.Scale2) * Constant.Scale;
            hitdef.AirGuardVelocity = EvaluationHelper.AsVector2(character, m_airGuardVelocity, (hitdef.AirVelocity * Constant.Scale2) * new Vector2(1.5f, 0.5f)) * Constant.Scale;
            hitdef.GroundCornerPush = EvaluationHelper.AsSingle(character, m_groundCornerPushOff, hitdef.HitAttribute.HasHeight(AttackStateType.Air) ? 0.0f : (hitdef.GroundGuardVelocity.x * Constant.Scale2) * 1.3f) * Constant.Scale;
            hitdef.AirCornerPush = EvaluationHelper.AsSingle(character, m_airCornerPushOff, hitdef.GroundCornerPush * Constant.Scale2) * Constant.Scale;
            hitdef.DownCornerPush = EvaluationHelper.AsSingle(character, m_downCornerPushOff, hitdef.GroundCornerPush * Constant.Scale2) * Constant.Scale;
            hitdef.GuardCornerPush = EvaluationHelper.AsSingle(character, m_guardCornerPushOff, hitdef.GroundCornerPush * Constant.Scale2) * Constant.Scale;
            hitdef.AirGuardCornerPush = EvaluationHelper.AsSingle(character, m_airGuardCornerPushOff, hitdef.GroundCornerPush * Constant.Scale2) * Constant.Scale;

            hitdef.JugglePointsNeeded = EvaluationHelper.AsInt32(character, m_airJuggle, 0);

            hitdef.MininumDistance = EvaluationHelper.AsVector2(character, m_minDistance, null) * Constant.Scale;
            hitdef.MaximumDistance = EvaluationHelper.AsVector2(character, m_maxDistance, null) * Constant.Scale;
            hitdef.SnapLocation = EvaluationHelper.AsVector2(character, m_snap, null) * Constant.Scale;
            hitdef.P1SpritePriority = EvaluationHelper.AsInt32(character, m_p1SpritePriority, 1);
            hitdef.P2SpritePriority = EvaluationHelper.AsInt32(character, m_p2SpritePriority, 0);
            hitdef.P1Facing = EvaluationHelper.AsInt32(character, m_p1Facing, 0);
            hitdef.P1GetP2Facing = EvaluationHelper.AsInt32(character, m_p1GetP2Facing, 0);
            hitdef.P2Facing = EvaluationHelper.AsInt32(character, m_p2Facing, 0);
            hitdef.P1NewState = EvaluationHelper.AsInt32(character, m_p1StateNumber, -1);
            hitdef.P2NewState = EvaluationHelper.AsInt32(character, m_p2StateNumber, -1);
            hitdef.P2UseP1State = EvaluationHelper.AsBoolean(character, m_p2GetP1State, true);
            hitdef.ForceStand = EvaluationHelper.AsBoolean(character, m_forceStand, hitdef.GroundVelocity.y != 0 ? true : false);
            hitdef.Fall = EvaluationHelper.AsBoolean(character, m_fall, false);
            hitdef.AirFall = EvaluationHelper.AsBoolean(character, m_airFall, hitdef.Fall);

            hitdef.FallVelocityX = EvaluationHelper.AsSingle(character, m_fallXVelocity, null) * Constant.Scale;
            hitdef.FallVelocityY = EvaluationHelper.AsSingle(character, m_fallYVelocity, -4.5f) * Constant.Scale;

            hitdef.FallCanRecover = EvaluationHelper.AsBoolean(character, m_fallRecover, true);
            hitdef.FallRecoverTime = EvaluationHelper.AsInt32(character, m_fallRecoverTime, 4);
            hitdef.FallDamage = EvaluationHelper.AsInt32(character, m_fallDamage, 0);
            hitdef.DownBounce = EvaluationHelper.AsBoolean(character, m_downBounce, false);
            hitdef.TargetId = EvaluationHelper.AsInt32(character, m_targetId, 0);
            hitdef.ChainId = EvaluationHelper.AsInt32(character, m_chainId, -1);

            var nochainid = EvaluationHelper.AsVector2(character, m_noChainId, new Vector2(-1, -1), -1);
            hitdef.NoChainId1 = nochainid.x;
            hitdef.NoChainId2 = nochainid.y;

            hitdef.HitOnce = EvaluationHelper.AsBoolean(character, m_hitOnce, hitdef.HitAttribute.HasData(new HitType(AttackClass.Throw, AttackPower.All)) ? true : false);
            hitdef.CanKill = EvaluationHelper.AsBoolean(character, m_kill, true);
            hitdef.CanGuardKill = EvaluationHelper.AsBoolean(character, m_guardKill, true);
            hitdef.CanFallKill = EvaluationHelper.AsBoolean(character, m_fallKill, true);
            hitdef.NumberOfHits = EvaluationHelper.AsInt32(character, m_numberOfHits, 1);

            if (m_p1PowerIncrease != null)
            {
                var statepower = m_p1PowerIncrease.Evaluate(character);

                if (statepower.Length > 0 && statepower[0].NumberType != NumberType.None)
                {
                    hitdef.P1HitPowerAdjustment = statepower[0].IntValue;
                }
                else
                {
                    float Default_Attack_LifeToPowerMul = character.Launcher.initializationSettings.Default_Attack_LifeToPowerMul;
                    hitdef.P1HitPowerAdjustment = (int)(hitdef.HitDamage * Default_Attack_LifeToPowerMul);
                }

                if (statepower.Length > 1 && statepower[1].NumberType != NumberType.None)
                {
                    hitdef.P1GuardPowerAdjustment = statepower[1].IntValue;
                }
                else
                {
                    hitdef.P1GuardPowerAdjustment = (int)(hitdef.P1HitPowerAdjustment * 0.5f);
                }
            }

            if (m_p2PowerIncrease != null)
            {
                var p2power = m_p2PowerIncrease.Evaluate(character);

                if (p2power.Length > 0 && p2power[0].NumberType != NumberType.None)
                {
                    hitdef.P2HitPowerAdjustment = p2power[0].IntValue;
                }
                else
                {
                    float Default_GetHit_LifeToPowerMul = character.Launcher.initializationSettings.Default_GetHit_LifeToPowerMul;
                    hitdef.P2HitPowerAdjustment = (int)(hitdef.HitDamage * Default_GetHit_LifeToPowerMul);
                }

                if (p2power.Length > 1 && p2power[1].NumberType != NumberType.None)
                {
                    hitdef.P2GuardPowerAdjustment = p2power[1].IntValue;
                }
                else
                {
                    hitdef.P2GuardPowerAdjustment = (int)(hitdef.P2HitPowerAdjustment * 0.5f);
                }
            }

            hitdef.PalFxTime = EvaluationHelper.AsInt32(character, m_palTime, 0);
            hitdef.PalFxAdd = EvaluationHelper.AsVector3(character, m_palAdd, Vector3.zero);
            hitdef.PalFxMul = EvaluationHelper.AsVector3(character, m_palMul, new Vector3(255, 255, 255));
            hitdef.PalFxBaseColor = EvaluationHelper.AsInt32(character, m_palColor, 255) / 255.0f;
            hitdef.PalFxInvert = EvaluationHelper.AsBoolean(character, m_palInvert, false);
            hitdef.PalFxSinAdd = EvaluationHelper.AsVector4(character, m_palSinAdd, new Vector4(0, 0, 0, 1), 1);

            hitdef.EnvShakeTime = EvaluationHelper.AsInt32(character, m_shakeTime, 0);
            hitdef.EnvShakeFrequency = Misc.Clamp(EvaluationHelper.AsSingle(character, m_shakeFreq, 60), 0, 180);
            hitdef.EnvShakeAmplitude = EvaluationHelper.AsInt32(character, m_shakeAmplitude, -4);
            hitdef.EnvShakePhase = EvaluationHelper.AsSingle(character, m_shakePhaseOffSet, hitdef.EnvShakeFrequency >= 90 ? 0 : 90);

            hitdef.EnvShakeFallTime = EvaluationHelper.AsInt32(character, m_fallShakeTime, 0);
            hitdef.EnvShakeFallFrequency = Misc.Clamp(EvaluationHelper.AsSingle(character, m_fallShakeFreq, 60), 0, 180);
            hitdef.EnvShakeFallAmplitude = EvaluationHelper.AsInt32(character, m_fallShakeAmplitude, -4);
            hitdef.EnvShakeFallPhase = EvaluationHelper.AsSingle(character, m_fallShakePhaseOffSet, hitdef.EnvShakeFallFrequency >= 90 ? 0 : 90);

            hitdef.Score = EvaluationHelper.AsInt32(character, m_score, 0);
            hitdef.AttackerID = character.Id;
        }
    }
}