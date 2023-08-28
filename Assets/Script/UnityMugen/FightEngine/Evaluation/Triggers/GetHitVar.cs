using System;
using System.Collections.Generic;
using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.StateMachine;

namespace UnityMugen.Evaluation.Triggers
{

    [CustomFunction("GetHitVar")]
    class GetHitVar : Function
    {
        public GetHitVar(List<IFunction> children, List<System.Object> arguments)
            : base(children, arguments)
        {
        }

        static GetHitVar()
        {
            HitVarMap = new Dictionary<string, Converter<Combat.Character, Number>>(StringComparer.OrdinalIgnoreCase);

            HitVarMap["xveladd"] = GetXVelAdd;
            HitVarMap["yveladd"] = GetYVelAdd;
            HitVarMap["type"] = GetHitType;
            HitVarMap["animtype"] = GetAnimType;
            HitVarMap["airtype"] = GetAirHitType;
            HitVarMap["groundtype"] = GetGroundHitType;
            HitVarMap["damage"] = GetDamage;
            HitVarMap["hitcount"] = GetHitCount;
            HitVarMap["fallcount"] = GetFallCount;
            HitVarMap["hitshaketime"] = GetHitShakeTime;
            HitVarMap["hittime"] = GetHitTime;
            HitVarMap["slidetime"] = GetSlideTime;
            HitVarMap["ctrltime"] = GetGuardControlTime;
            HitVarMap["recovertime"] = GetRecoverTime;
            HitVarMap["xoff"] = GetSnapX;
            HitVarMap["yoff"] = GetSnapY;
            HitVarMap["zoff"] = GetSnapZ;
            HitVarMap["xvel"] = x => GetHitVelocityX(x);
            HitVarMap["yvel"] = x => GetHitVelocityY(x);
            HitVarMap["yaccel"] = GetYAccleration;
            HitVarMap["chainid"] = x => new Number(x.DefensiveInfo.HitDef.ChainId);
            HitVarMap["hitid"] = x => new Number(x.DefensiveInfo.HitDef.ChainId);
            HitVarMap["guarded"] = x => new Number(x.DefensiveInfo.Blocked);
            HitVarMap["isbound"] = GetIsBound;
            HitVarMap["fall"] = x => new Number(x.DefensiveInfo.IsFalling);
            HitVarMap["fall.damage"] = x => new Number(x.DefensiveInfo.HitDef.FallDamage);
            HitVarMap["fall.xvel"] = x => new Number((x.DefensiveInfo.HitDef.FallVelocityX ?? x.CurrentVelocity.x) * Constant.Scale2);
            HitVarMap["fall.yvel"] = x => new Number(x.DefensiveInfo.HitDef.FallVelocityY * Constant.Scale2);
            HitVarMap["fall.time"] = x => new Number(x.FallTime);
            HitVarMap["fall.recover"] = x => new Number(x.DefensiveInfo.HitDef.FallCanRecover);
            HitVarMap["fall.recovertime"] = x => new Number(x.DefensiveInfo.HitDef.FallRecoverTime);
            HitVarMap["fall.kill"] = x => new Number(x.DefensiveInfo.HitDef.CanFallKill);
            HitVarMap["fall.envshake.time"] = x => new Number(x.DefensiveInfo.HitDef.EnvShakeFallTime);
            HitVarMap["fall.envshake.freq"] = x => new Number(x.DefensiveInfo.HitDef.EnvShakeFallFrequency);
            HitVarMap["fall.envshake.ampl"] = x => new Number(x.DefensiveInfo.HitDef.EnvShakeFallAmplitude);
            HitVarMap["fall.envshake.phase"] = x => new Number(x.DefensiveInfo.HitDef.EnvShakeFallPhase);


            HitVarMap["score"] = x => new Number(x.DefensiveInfo.HitDef.Score);
            HitVarMap["guardpower"] = x => new Number(x.DefensiveInfo.HitDef.P1GuardPowerAdjustment);
            HitVarMap["hitpower"] = x => new Number(x.DefensiveInfo.HitDef.P1HitPowerAdjustment);
            HitVarMap["id"] = x => new Number(x.DefensiveInfo.HitDef.AttackerID);
            HitVarMap["hitdamage"] = HitDamage;
            //HitVarMap["attr"] = Attr;
        }

#warning rever estes metodos abaixos pois eles so retornam 0 e error
        static Number GetXVelAdd(Character character)
        {
            Debug.Log("Error | GetVelocityAddX");

            //character.DefensiveInfo.GetHitVelocity().x * Constant.Scale2
            return new Number(0);
        }

        static Number GetYVelAdd(Character character)
        {
            Debug.Log("Error | GetVelocityAddY");
            return new Number(0);
        }

        static Number GetFallCount(Character character)// Novo Tiago - nao tenho recerteza nada para comparar ///
        {
            var time = character.StateManager.StateTime;
            if (time < 0) time = 0;

            if ((character.StateManager.StateNumber == StateNumber.HitBounce ||
                character.StateManager.StateNumber == StateNumber.HitTrip)
                    && time == 0)
                ;

            if (character.DefensiveInfo.IsFalling || character.DefensiveInfo.IsFalling)
                ;

            Debug.Log("Error | GetFallCount");
            return new Number(0);
        }

        static Number GetRecoverTime(Character character) // Novo Tiago
        {
            // Debug.Log("Error | GetRecoverTime");
            if (character.StateManager.StateNumber == StateNumber.HitLieDown)
            {
                var time = character.StateManager.StateTime;
                if (time < 0) time = 0;
                return new Number(character.BasePlayer.playerConstants.LieDownTime - time);
            }
            return new Number(0);
        }

        static Number GetHitCount(Character character)
        {
            return new Number(character.DefensiveInfo.HitCount);
        }

        static Number GetHitVelocityX(Character character)
        {
            if (character.DefensiveInfo.Attacker.typeEntity != TypeEntity.Projectile &&
                character.DefensiveInfo.Attacker.CurrentFacing == character.CurrentFacing)
            { // Novo Em analize de acordo com IK

                int facing = character.DefensiveInfo.Attacker.BasePlayer.CurrentFacing == UnityMugen.Facing.Left ? -1 : 1;
                return new Number((character.DefensiveInfo.GetHitVelocity().x * -facing) * Constant.Scale2);
            }
            else {
                int facing = character.CurrentFacing == UnityMugen.Facing.Left ? -1 : 1;
                return new Number((character.DefensiveInfo.GetHitVelocity().x * facing) * Constant.Scale2);
            } 
        }

        static Number GetHitVelocityY(Character character)
        {
            return new Number(character.DefensiveInfo.GetHitVelocity().y * Constant.Scale2);
        }

        static Number GetHitType(Character character)
        {
            if (character.Life == 0) return new Number(3);
            return character.DefensiveInfo.HitStateType == UnityMugen.StateType.Airborne ? GetAirHitType(character) : GetGroundHitType(character);
        }

        static Number GetAnimType(Character character)
        {
            var hat = HitAnimationType.Light;

            if (character.DefensiveInfo.IsFalling) hat = character.DefensiveInfo.HitDef.FallAnimationType;
            else if (character.DefensiveInfo.HitStateType == UnityMugen.StateType.Airborne) hat = character.DefensiveInfo.HitDef.AirAnimationType;
            else hat = character.DefensiveInfo.HitDef.GroundAnimationType;

            switch (hat)
            {
                case HitAnimationType.None:
                    return new Number();

                case HitAnimationType.Light:
                    return new Number(0);

                case HitAnimationType.Medium:
                    return new Number(1);

                case HitAnimationType.Hard:
                    return new Number(2);

                case HitAnimationType.Back:
                    return new Number(3);

                case HitAnimationType.Up:
                    return new Number(4);

                case HitAnimationType.DiagUp:
                    return new Number(5);

                default:
                    return new Number(0);
            }
        }

        static Number GetAirHitType(Character character)
        {
            switch (character.DefensiveInfo.HitDef.AirAttackEffect)
            {
                case AttackEffect.High:
                    return new Number(1);

                case AttackEffect.Low:
                    return new Number(2);

                case AttackEffect.Trip:
                    return new Number(3);

                case AttackEffect.None:
                default:
                    return new Number(0);
            }
        }

        static Number GetGroundHitType(Character character)
        {
            switch (character.DefensiveInfo.HitDef.GroundAttackEffect)
            {
                case AttackEffect.High:
                    return new Number(1);

                case AttackEffect.Low:
                    return new Number(2);

                case AttackEffect.Trip:
                    return new Number(3);

                case AttackEffect.None:
                default:
                    return new Number(0);
            }
        }

        static Number GetDamage(Character character)
        {
            return character.DefensiveInfo.Blocked ? new Number(character.DefensiveInfo.HitDef.GuardDamage) : new Number(character.DefensiveInfo.HitDef.HitDamage);
        }

        static Number GetHitShakeTime(Character p)
        {
            return new Number(p.DefensiveInfo.HitShakeTime);
        }

        static Number GetHitTime(Combat.Character p)
        {
            return new Number(p.DefensiveInfo.HitTime);
        }

        static Number GetSlideTime(Combat.Character p)
        {
            return (p.DefensiveInfo.Blocked == true) ? new Number(p.DefensiveInfo.HitDef.GuardSlideTime) : new Number(p.DefensiveInfo.HitDef.GroundSlideTime);
        }

        static Number GetGuardControlTime(Character character)
        {
            switch (character.DefensiveInfo.HitStateType)
            {
                case UnityMugen.StateType.Airborne:
                    return new Number(character.DefensiveInfo.HitDef.AirGuardControlTime);

                default:
                    return new Number(character.DefensiveInfo.HitDef.GuardControlTime);
            }
        }

        static Number GetSnapX(Character character)
        {
            var location = character.DefensiveInfo.HitDef.SnapLocation;
            if (location.HasValue == false)
                return new Number();

            return new Number(location.Value.x * Constant.Scale2);
        }

        static Number GetSnapY(Character character)
        {
            var location = character.DefensiveInfo.HitDef.SnapLocation;
            if (location.HasValue == false)
                return new Number();

            return new Number(location.Value.y * Constant.Scale2);
        }

        static Number GetSnapZ(Character character)
        {
            return new Number();
        }

        static Number GetYAccleration(Character character)
        {
            if (character.DefensiveInfo.HitDef.YAcceleration != 0)
                return new Number(character.DefensiveInfo.HitDef.YAcceleration * Constant.Scale2);
            else
                return new Number(Constant.YaccelDefault);
        }

        static Number GetIsBound(Character character)
        {
            var bind = character.Bind;
            return new Number(bind.IsActive && bind.IsTargetBind);

            //Teste
            //return bind.IsActive && character.Id == character.Enemy().Bind.BindTo.Id;
        }





        // Abaixo Novos Atibutos
#warning testar e adicionar a documentacao
        //[Tag("attr")]
        public static HitAttribute Attr(Character character)
        {
            //if (character == null)
            //    return new Number();

            if (character.DefensiveInfo.HitDef.HitAttribute.AttackData.Count > 0)
                return character.DefensiveInfo.HitDef.HitAttribute;
            else
                return character.OffensiveInfo.HitDef.HitAttribute;
        }

        static Number HitDamage(Character character)
        {
            if (character == null)
                return new Number();

            if (character.DefensiveInfo.HitDef.HitDamage != 0)
                return new Number(character.DefensiveInfo.HitDef.HitDamage);
            else
                return new Number(character.OffensiveInfo.HitDef.HitDamage);
        }



        public override Number Evaluate(Character state)
        {
            if (state == null) return new Number();

            string consttype = (string)Arguments[0];

            if (HitVarMap.ContainsKey(consttype) == true) return HitVarMap[consttype](state);
            return new Number();
        }

        public static Node Parse(ParseState parsestate)
        {
            if (parsestate.CurrentSymbol != Symbol.LeftParen) return null;
            ++parsestate.TokenIndex;

            if (parsestate.CurrentToken == null) return null;
            var constant = parsestate.CurrentToken.ToString();

            parsestate.BaseNode.Arguments.Add(constant);
            ++parsestate.TokenIndex;

            if (parsestate.CurrentSymbol != Symbol.RightParen) return null;
            ++parsestate.TokenIndex;

            return parsestate.BaseNode;
        }

        static Dictionary<string, Converter<Combat.Character, Number>> HitVarMap { get; set; }
    }

}