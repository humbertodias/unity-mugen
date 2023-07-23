using System;
using UnityEngine;
using UnityMugen.Audio;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

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

        public HitDef(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_hitAttr = textSection.GetAttribute<HitAttribute>("attr", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

#warning hack because some animals kept the attribute no I don't put a value
                if (!textSection.ContainsError("hitflag", out string error) && error != null)
                /*    Debug.LogError("never create an empty attribute: "+ error)*/;
                else
                    m_hitFlag = textSection.GetAttribute("hitflag", HitFlag.Default);
    /////////////////////////////


                m_guardFlag = textSection.GetAttribute("guardflag", HitFlag.NoGuard);
                m_affectTeam = textSection.GetAttribute("affectteam", AffectTeam.Enemy);
                m_hitAnimType = textSection.GetAttribute("animtype", HitAnimationType.Light);
                m_airHitAnimType = textSection.GetAttribute("air.animtype", m_hitAnimType);
                m_fallHitAnimType = textSection.GetAttribute("fall.animtype", m_airHitAnimType == HitAnimationType.Up ? HitAnimationType.Up : HitAnimationType.Back);
                m_priority = textSection.GetAttribute("priority", HitPriority.Default);
                m_damage = textSection.GetAttribute<Expression>("damage", null);
                m_pauseTime = textSection.GetAttribute<Expression>("pausetime", null);
                m_guardPauseTime = textSection.GetAttribute<Expression>("guard.pausetime", null);
                m_sparkNumber = textSection.GetAttribute<PrefixedExpression>("sparkno", null);
                m_guardSparkNumber = textSection.GetAttribute<PrefixedExpression>("guard.sparkno", null);
                m_sparkPosition = textSection.GetAttribute<Expression>("sparkxy", null);
                m_hitSound = textSection.GetAttribute<PrefixedExpression>("hitsound", null);
                m_guardHitSound = textSection.GetAttribute<PrefixedExpression>("guardsound", null);
                m_attackEffect = textSection.GetAttribute("ground.type", AttackEffect.High);
                m_airEffect = textSection.GetAttribute("air.type", m_attackEffect);
                m_groundSlideTime = textSection.GetAttribute<Expression>("ground.slidetime", null);
                m_guardSlideTime = textSection.GetAttribute<Expression>("guard.slidetime", null);
                m_groundHitTime = textSection.GetAttribute<Expression>("ground.hittime", null);
                m_guardHitTime = textSection.GetAttribute<Expression>("guard.hittime", null);
                m_airHitTime = textSection.GetAttribute<Expression>("air.hittime", null);
                m_guardCtrlTime = textSection.GetAttribute<Expression>("guard.ctrltime", null);
                m_guardDistance = textSection.GetAttribute<Expression>("guard.dist", null);
                m_yaccel = textSection.GetAttribute<Expression>("yaccel", null);
                m_groundVelocity = textSection.GetAttribute<Expression>("ground.velocity", null);
                m_guardVelocity = textSection.GetAttribute<Expression>("guard.velocity", null);
                m_airVelocity = textSection.GetAttribute<Expression>("air.velocity", null);
                m_airGuardVelocity = textSection.GetAttribute<Expression>("airguard.velocity", null);
                m_groundCornerPushOff = textSection.GetAttribute<Expression>("ground.cornerpush.veloff", null);
                m_airCornerPushOff = textSection.GetAttribute<Expression>("air.cornerpush.veloff", null);
                m_downCornerPushOff = textSection.GetAttribute<Expression>("down.cornerpush.veloff", null);
                m_guardCornerPushOff = textSection.GetAttribute<Expression>("guard.cornerpush.veloff", null);
                m_airGuardCornerPushOff = textSection.GetAttribute<Expression>("airguard.cornerpush.veloff", null);
                m_airGuardCtrlTime = textSection.GetAttribute<Expression>("airguard.ctrltime", null);
                m_airJuggle = textSection.GetAttribute<Expression>("air.juggle", null);
                m_minDistance = textSection.GetAttribute<Expression>("mindist", null);
                m_maxDistance = textSection.GetAttribute<Expression>("maxdist", null);
                m_snap = textSection.GetAttribute<Expression>("snap", null);
                m_p1SpritePriority = textSection.GetAttribute<Expression>("p1sprpriority", null);
                m_p2SpritePriority = textSection.GetAttribute<Expression>("p2sprpriority", null);
                m_p1Facing = textSection.GetAttribute<Expression>("p1facing", null);
                m_p1GetP2Facing = textSection.GetAttribute<Expression>("p1getp2facing", null);
                m_p2Facing = textSection.GetAttribute<Expression>("p2facing", null);
                m_p1StateNumber = textSection.GetAttribute<Expression>("p1stateno", null);
                m_p2StateNumber = textSection.GetAttribute<Expression>("p2stateno", null);
                m_p2GetP1State = textSection.GetAttribute<Expression>("p2getp1state", null);
                m_forceStand = textSection.GetAttribute<Expression>("forcestand", null);
                m_fall = textSection.GetAttribute<Expression>("fall", null);
                m_fallXVelocity = textSection.GetAttribute<Expression>("fall.xvelocity", null);
                m_fallYVelocity = textSection.GetAttribute<Expression>("fall.yvelocity", null);
                m_fallRecover = textSection.GetAttribute<Expression>("fall.recover", null);
                m_fallRecoverTime = textSection.GetAttribute<Expression>("fall.recovertime", null);
                m_fallDamage = textSection.GetAttribute<Expression>("fall.damage", null);
                m_airFall = textSection.GetAttribute<Expression>("air.fall", null);
                m_forceNoFall = textSection.GetAttribute<Expression>("forcenofall ", null);
                m_downVelocity = textSection.GetAttribute<Expression>("down.velocity", null);
                m_downHitTime = textSection.GetAttribute<Expression>("down.hittime", null);
                m_downBounce = textSection.GetAttribute<Expression>("down.bounce", null);
                m_targetId = textSection.GetAttribute<Expression>("id", null);
                m_chainId = textSection.GetAttribute<Expression>("chainid", null);
                m_noChainId = textSection.GetAttribute<Expression>("nochainid", null);
                m_hitOnce = textSection.GetAttribute<Expression>("hitonce", null);
                m_kill = textSection.GetAttribute<Expression>("kill", null);
                m_guardKill = textSection.GetAttribute<Expression>("guard.kill", null);
                m_fallKill = textSection.GetAttribute<Expression>("fall.kill", null);
                m_numberOfHits = textSection.GetAttribute<Expression>("numhits", null);
                m_p1PowerIncrease = textSection.GetAttribute<Expression>("getpower", null);
                m_p2PowerIncrease = textSection.GetAttribute<Expression>("givepower", null);
                m_palTime = textSection.GetAttribute<Expression>("palfx.time", null);
                m_palMul = textSection.GetAttribute<Expression>("palfx.mul", null);
                m_palAdd = textSection.GetAttribute<Expression>("palfx.add", null);
                m_palSinAdd = textSection.GetAttribute<Expression>("palfx.sinadd", null);
                m_palInvert = textSection.GetAttribute<Expression>("palfx.invertall", null);
                m_palColor = textSection.GetAttribute<Expression>("palfx.color", null);
                m_shakeTime = textSection.GetAttribute<Expression>("envshake.time", null);
                m_shakeFreq = textSection.GetAttribute<Expression>("envshake.freq", null);
                m_shakeAmplitude = textSection.GetAttribute<Expression>("envshake.ampl", null);
                m_shakePhaseOffSet = textSection.GetAttribute<Expression>("envshake.phase", null);
                m_fallShakeTime = textSection.GetAttribute<Expression>("fall.envshake.time", null);
                m_fallShakeFreq = textSection.GetAttribute<Expression>("fall.envshake.freq", null);
                m_fallShakeAmplitude = textSection.GetAttribute<Expression>("fall.envshake.ampl", null);
                m_fallShakePhaseOffSet = textSection.GetAttribute<Expression>("fall.envshake.phase", null);
                m_score = textSection.GetAttribute<Expression>("score", null);
                // Este parametro nem tem no mugen 1.0 e numca foi usado nas versoes anteriores
                //m_attackwidth = textsection.GetAttribute<Expression>("attack.width", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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
            hitdef.GuardSlideTime = EvaluationHelper.AsInt32(character, m_groundSlideTime, hitdef.GuardHitTime);
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

#warning  Tiago - possivel valor nao aplicado
            hitdef.JugglePointsNeeded = EvaluationHelper.AsInt32(character, m_airJuggle, 0);

            hitdef.MininumDistance = EvaluationHelper.AsVector2(character, m_minDistance, null) * Constant.Scale;
            hitdef.MaximumDistance = EvaluationHelper.AsVector2(character, m_maxDistance, null) * Constant.Scale;
            hitdef.SnapLocation = EvaluationHelper.AsVector2(character, m_snap, null) * Constant.Scale;
            hitdef.P1SpritePriority = EvaluationHelper.AsInt32(character, m_p1SpritePriority, 1);
            hitdef.P2SpritePriority = EvaluationHelper.AsInt32(character, m_p2SpritePriority, 0);
            hitdef.P1Facing = EvaluationHelper.AsInt32(character, m_p1Facing, 0);
            hitdef.P1GetP2Facing = EvaluationHelper.AsInt32(character, m_p1GetP2Facing, 0);
            hitdef.P2Facing = EvaluationHelper.AsInt32(character, m_p2Facing, 0);
            hitdef.P1NewState = EvaluationHelper.AsInt32(character, m_p1StateNumber, null);
            hitdef.P2NewState = EvaluationHelper.AsInt32(character, m_p2StateNumber, null);
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
            hitdef.PalFxSinAdd = EvaluationHelper.AsVector4(character, m_palSinAdd, new Vector4(0, 0, 0, 1));

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