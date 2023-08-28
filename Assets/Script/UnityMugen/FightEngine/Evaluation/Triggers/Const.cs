using System;
using System.Collections.Generic;
using UnityMugen.Combat;

namespace UnityMugen.Evaluation.Triggers
{

    [CustomFunction("Const")]
    internal class Const : Function
    {
        //playerConstants == Constants

        static Const()
        {
            s_playermap = new Dictionary<string, Converter<Combat.Player, Number>>(StringComparer.OrdinalIgnoreCase);
            s_helpermap = new Dictionary<string, Converter<Combat.Helper, Number>>(StringComparer.OrdinalIgnoreCase);


            s_playermap["data.life"] = x => new Number(x.playerConstants.MaximumLife);
            s_helpermap["data.life"] = x => new Number(x.BasePlayer.playerConstants.MaximumLife);

            s_playermap["data.power"] = x => new Number(x.playerConstants.MaximumPower);
            s_helpermap["data.power"] = x => new Number(x.BasePlayer.playerConstants.MaximumPower);

            s_playermap["data.attack"] = x => new Number(x.playerConstants.AttackPower);
            s_helpermap["data.attack"] = x => new Number(x.BasePlayer.playerConstants.AttackPower);

            s_playermap["data.defence"] = x => new Number(x.playerConstants.DefensivePower);
            s_helpermap["data.defence"] = x => new Number(x.BasePlayer.playerConstants.DefensivePower);

            s_playermap["data.fall.defence_mul"] = x => new Number(100.0f / (100.0f + x.playerConstants.FallDefenceMul));
            s_helpermap["data.fall.defence_mul"] = x => new Number(100.0f / (100.0f + x.BasePlayer.playerConstants.FallDefenceMul));

            s_playermap["data.fall.defence_up"] = x => new Number(x.playerConstants.FallDefenceMul);
            s_helpermap["data.fall.defence_up"] = x => new Number(x.BasePlayer.playerConstants.FallDefenseUp);

            s_playermap["data.liedown.time"] = x => new Number(x.playerConstants.LieDownTime);
            s_helpermap["data.liedown.time"] = x => new Number(x.BasePlayer.playerConstants.LieDownTime);

            s_playermap["data.airjuggle"] = x => new Number(x.playerConstants.AirJuggle);
            s_helpermap["data.airjuggle"] = x => new Number(x.BasePlayer.playerConstants.AirJuggle);

            s_playermap["data.sparkno"] = x => new Number(x.playerConstants.DefaultSparkNumber);
            s_helpermap["data.sparkno"] = x => new Number(x.BasePlayer.playerConstants.DefaultSparkNumber);

            s_playermap["data.guard.sparkno"] = x => new Number(x.playerConstants.DefaultGuardSparkNumber);
            s_helpermap["data.guard.sparkno"] = x => new Number(x.BasePlayer.playerConstants.DefaultGuardSparkNumber);

            s_playermap["data.KO.echo"] = x => new Number(x.playerConstants.KOEcho);
            s_helpermap["data.KO.echo"] = x => new Number(x.BasePlayer.playerConstants.KOEcho);

            s_playermap["data.IntPersistIndex"] = x => new Number(x.playerConstants.PersistanceIntIndex);
            s_helpermap["data.IntPersistIndex"] = x => new Number(x.BasePlayer.playerConstants.PersistanceIntIndex);

            s_playermap["data.FloatPersistIndex"] = x => new Number(x.playerConstants.PersistanceFloatIndex);
            s_helpermap["data.FloatPersistIndex"] = x => new Number(x.BasePlayer.playerConstants.PersistanceFloatIndex);

            s_playermap["size.draw.offset.x"] = x => new Number(x.playerConstants.Drawoffset.x);
            s_helpermap["size.draw.offset.x"] = x => new Number(x.BasePlayer.playerConstants.Drawoffset.x);

            s_playermap["size.draw.offset.y"] = x => new Number(x.playerConstants.Drawoffset.y);
            s_helpermap["size.draw.offset.y"] = x => new Number(x.BasePlayer.playerConstants.Drawoffset.y);

            s_playermap["size.xscale"] = x => new Number(x.playerConstants.Scale.x);
            s_helpermap["size.xscale"] = x => new Number(x.Data.Scale.x);

            s_playermap["size.yscale"] = x => new Number(x.playerConstants.Scale.y);
            s_helpermap["size.yscale"] = x => new Number(x.Data.Scale.y);

            s_playermap["size.ground.back"] = x => new Number(x.playerConstants.GroundBack);
            s_helpermap["size.ground.back"] = x => new Number(x.BasePlayer.playerConstants.GroundBack);

            s_playermap["size.ground.front"] = x => new Number(x.playerConstants.GroundFront);
            s_helpermap["size.ground.front"] = x => new Number(x.BasePlayer.playerConstants.GroundFront);

            s_playermap["size.air.back"] = x => new Number(x.playerConstants.Airback);
            s_helpermap["size.air.back"] = x => new Number(x.BasePlayer.playerConstants.Airback);

            s_playermap["size.air.front"] = x => new Number(x.playerConstants.Airfront);
            s_helpermap["size.air.front"] = x => new Number(x.BasePlayer.playerConstants.Airfront);

            s_playermap["size.height"] = x => new Number(x.playerConstants.Height);
            s_helpermap["size.height"] = x => new Number(x.BasePlayer.playerConstants.Height);

            s_playermap["size.attack.dist"] = x => new Number(x.playerConstants.Attackdistance);
            s_helpermap["size.attack.dist"] = x => new Number(x.BasePlayer.playerConstants.Attackdistance);

            s_playermap["size.proj.attack.dist"] = x => new Number(x.playerConstants.Projectileattackdist);
            s_helpermap["size.proj.attack.dist"] = x => new Number(x.BasePlayer.playerConstants.Projectileattackdist);

            s_playermap["size.proj.doscale"] = x => new Number(x.playerConstants.ProjectileScaling);
            s_helpermap["size.proj.doscale"] = x => new Number(x.Data.ProjectileScaling);

            s_playermap["size.head.pos.x"] = x => new Number((Int32)x.playerConstants.Headposition.x);
            s_helpermap["size.head.pos.x"] = x => new Number((Int32)x.Data.HeadPosition.x);

            s_playermap["size.head.pos.y"] = x => new Number((Int32)x.playerConstants.Headposition.y);
            s_helpermap["size.head.pos.y"] = x => new Number((Int32)x.Data.HeadPosition.y);

            s_playermap["size.mid.pos.x"] = x => new Number((Int32)x.playerConstants.Midposition.x);
            s_helpermap["size.mid.pos.x"] = x => new Number((Int32)x.Data.MidPosition.x);

            s_playermap["size.mid.pos.y"] = x => new Number((Int32)x.playerConstants.Midposition.y);
            s_helpermap["size.mid.pos.y"] = x => new Number((Int32)x.Data.MidPosition.y);

            s_playermap["size.shadowoffset"] = x => new Number(x.playerConstants.Shadowoffset);
            s_helpermap["size.shadowoffset"] = x => new Number(x.Data.ShadowOffset);

            s_playermap["velocity.walk.fwd.x"] = x => new Number(x.playerConstants.Walk_forward);
            s_helpermap["velocity.walk.fwd.x"] = x => new Number(x.BasePlayer.playerConstants.Walk_forward);

            s_playermap["velocity.walk.back.x"] = x => new Number(x.playerConstants.Walk_back);
            s_helpermap["velocity.walk.back.x"] = x => new Number(x.BasePlayer.playerConstants.Walk_back);

            s_playermap["velocity.run.fwd.x"] = x => new Number(x.playerConstants.Run_fwd.x);
            s_helpermap["velocity.run.fwd.x"] = x => new Number(x.BasePlayer.playerConstants.Run_fwd.x);

            s_playermap["velocity.run.fwd.y"] = x => new Number(x.playerConstants.Run_fwd.y);
            s_helpermap["velocity.run.fwd.y"] = x => new Number(x.BasePlayer.playerConstants.Run_fwd.y);

            s_playermap["velocity.run.back.x"] = x => new Number(x.playerConstants.Run_back.x);
            s_helpermap["velocity.run.back.x"] = x => new Number(x.BasePlayer.playerConstants.Run_back.x);

            s_playermap["velocity.run.back.y"] = x => new Number(x.playerConstants.Run_back.y);
            s_helpermap["velocity.run.back.y"] = x => new Number(x.BasePlayer.playerConstants.Run_back.y);

            s_playermap["velocity.jump.y"] = x => new Number(x.playerConstants.Jump_neutral.y);
            s_helpermap["velocity.jump.y"] = x => new Number(x.BasePlayer.playerConstants.Jump_neutral.y);

            s_playermap["velocity.jump.neu.x"] = x => new Number(x.playerConstants.Jump_neutral.x);
            s_helpermap["velocity.jump.neu.x"] = x => new Number(x.BasePlayer.playerConstants.Jump_neutral.x);

            s_playermap["velocity.jump.neu.y"] = x => new Number(x.playerConstants.Jump_neutral.y);
            s_helpermap["velocity.jump.neu.y"] = x => new Number(x.BasePlayer.playerConstants.Jump_neutral.y);

            s_playermap["velocity.runjump.y"] = x => new Number(x.playerConstants.Jump_neutral.y);
            s_helpermap["velocity.runjump.y"] = x => new Number(x.BasePlayer.playerConstants.Jump_neutral.y);

            s_playermap["velocity.jump.back.x"] = x => new Number(x.playerConstants.Jump_back.x);
            s_helpermap["velocity.jump.back.x"] = x => new Number(x.BasePlayer.playerConstants.Jump_back.x);

            s_playermap["velocity.jump.fwd.x"] = x => new Number(x.playerConstants.Jump_forward.x);
            s_helpermap["velocity.jump.fwd.x"] = x => new Number(x.BasePlayer.playerConstants.Jump_forward.x);

            s_playermap["velocity.runjump.back.x"] = x => new Number(x.playerConstants.Runjump_back.x);
            s_helpermap["velocity.runjump.back.x"] = x => new Number(x.BasePlayer.playerConstants.Runjump_back.x);

            s_playermap["velocity.runjump.fwd.x"] = x => new Number(x.playerConstants.Runjump_fwd.x);
            s_helpermap["velocity.runjump.fwd.x"] = x => new Number(x.BasePlayer.playerConstants.Runjump_fwd.x);

            s_playermap["velocity.airjump.y"] = x => new Number(x.playerConstants.Airjump_neutral.y);
            s_helpermap["velocity.airjump.y"] = x => new Number(x.BasePlayer.playerConstants.Airjump_neutral.y);

            s_playermap["velocity.airjump.neu.x"] = x => new Number(x.playerConstants.Airjump_neutral.x);
            s_helpermap["velocity.airjump.neu.x"] = x => new Number(x.BasePlayer.playerConstants.Airjump_neutral.x);

            s_playermap["velocity.airjump.back.x"] = x => new Number(x.playerConstants.Airjump_back.x);
            s_helpermap["velocity.airjump.back.x"] = x => new Number(x.BasePlayer.playerConstants.Airjump_back.x);

            s_playermap["velocity.airjump.fwd.x"] = x => new Number(x.playerConstants.Airjump_forward.x);
            s_helpermap["velocity.airjump.fwd.x"] = x => new Number(x.BasePlayer.playerConstants.Airjump_forward.x);

            s_playermap["velocity.air.gethit.groundrecover.x"] = x => new Number(x.playerConstants.AirGethitGroundrecover.x);
            s_helpermap["velocity.air.gethit.groundrecover.x"] = x => new Number(x.BasePlayer.playerConstants.AirGethitGroundrecover.x);

            s_playermap["velocity.air.gethit.groundrecover.y"] = x => new Number(x.playerConstants.AirGethitGroundrecover.y);
            s_helpermap["velocity.air.gethit.groundrecover.y"] = x => new Number(x.BasePlayer.playerConstants.AirGethitGroundrecover.y);

            s_playermap["velocity.air.gethit.airrecover.mul.x"] = x => new Number(x.playerConstants.AirGethitAirrecoverMul.x);
            s_helpermap["velocity.air.gethit.airrecover.mul.x"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverMul.x);

            s_playermap["velocity.air.gethit.airrecover.mul.y"] = x => new Number(x.playerConstants.AirGethitAirrecoverMul.y);
            s_helpermap["velocity.air.gethit.airrecover.mul.y"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverMul.y);

            s_playermap["velocity.air.gethit.airrecover.add.x"] = x => new Number(x.playerConstants.AirGethitAirrecoverAdd.x);
            s_helpermap["velocity.air.gethit.airrecover.add.x"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverAdd.x);

            s_playermap["velocity.air.gethit.airrecover.add.y"] = x => new Number(x.playerConstants.AirGethitAirrecoverAdd.y);
            s_helpermap["velocity.air.gethit.airrecover.add.y"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverAdd.y);

            s_playermap["velocity.air.gethit.airrecover.back"] = x => new Number(x.playerConstants.AirGethitAirrecoverBack);
            s_helpermap["velocity.air.gethit.airrecover.back"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverBack);

            s_playermap["velocity.air.gethit.airrecover.fwd"] = x => new Number(x.playerConstants.AirGethitAirrecoverFwd);
            s_helpermap["velocity.air.gethit.airrecover.fwd"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverFwd);

            s_playermap["velocity.air.gethit.airrecover.up"] = x => new Number(x.playerConstants.AirGethitAirrecoverUp);
            s_helpermap["velocity.air.gethit.airrecover.up"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverUp);

            s_playermap["velocity.air.gethit.airrecover.down"] = x => new Number(x.playerConstants.AirGethitAirrecoverDown);
            s_helpermap["velocity.air.gethit.airrecover.down"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverDown);



            s_playermap["movement.airjump.num"] = x => new Number(x.playerConstants.Airjumps);
            s_helpermap["movement.airjump.num"] = x => new Number(x.BasePlayer.playerConstants.Airjumps);

            s_playermap["movement.airjump.height"] = x => new Number(x.playerConstants.Airjumpheight);
            s_helpermap["movement.airjump.height"] = x => new Number(x.BasePlayer.playerConstants.Airjumpheight);

            s_playermap["movement.yaccel"] = x => new Number(x.playerConstants.Vert_acceleration);
            s_helpermap["movement.yaccel"] = x => new Number(x.BasePlayer.playerConstants.Vert_acceleration);

            s_playermap["movement.stand.friction"] = x => new Number(x.playerConstants.Standfriction);
            s_helpermap["movement.stand.friction"] = x => new Number(x.BasePlayer.playerConstants.Standfriction);

            s_playermap["movement.crouch.friction"] = x => new Number(x.playerConstants.Crouchfriction);
            s_helpermap["movement.crouch.friction"] = x => new Number(x.BasePlayer.playerConstants.Crouchfriction);

            s_playermap["movement.stand.friction.threshold"] = x => new Number(x.playerConstants.StandFrictionThreshold);
            s_helpermap["movement.stand.friction.threshold"] = x => new Number(x.BasePlayer.playerConstants.StandFrictionThreshold);

            s_playermap["movement.crouch.friction.threshold"] = x => new Number(x.playerConstants.CrouchFrictionThreshold);
            s_helpermap["movement.crouch.friction.threshold"] = x => new Number(x.BasePlayer.playerConstants.CrouchFrictionThreshold);

            s_playermap["movement.jump.changeanim.threshold"] = x => new Number(x.playerConstants.JumpChangeanimThreshold);
            s_helpermap["movement.jump.changeanim.threshold"] = x => new Number(x.BasePlayer.playerConstants.JumpChangeanimThreshold);

            s_playermap["movement.air.gethit.groundlevel"] = x => new Number(x.playerConstants.AirGethitGroundlevel);
            s_helpermap["movement.air.gethit.groundlevel"] = x => new Number(x.BasePlayer.playerConstants.AirGethitGroundlevel);

            s_playermap["movement.air.gethit.groundrecover.ground.threshold"] = x => new Number(x.playerConstants.AirGethitGroundrecoverGroundThreshold);
            s_helpermap["movement.air.gethit.groundrecover.ground.threshold"] = x => new Number(x.BasePlayer.playerConstants.AirGethitGroundrecoverGroundThreshold);

            s_playermap["movement.air.gethit.groundrecover.groundlevel"] = x => new Number(x.playerConstants.AirGethitGroundrecoverGroundlevel);
            s_helpermap["movement.air.gethit.groundrecover.groundlevel"] = x => new Number(x.BasePlayer.playerConstants.AirGethitGroundrecoverGroundlevel);

            s_playermap["movement.air.gethit.airrecover.threshold"] = x => new Number(x.playerConstants.AirGethitAirrecoverThreshold);
            s_helpermap["movement.air.gethit.airrecover.threshold"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverThreshold);

            s_playermap["movement.air.gethit.airrecover.yaccel"] = x => new Number(x.playerConstants.AirGethitAirrecoverYaccel);
            s_helpermap["movement.air.gethit.airrecover.yaccel"] = x => new Number(x.BasePlayer.playerConstants.AirGethitAirrecoverYaccel);

            s_playermap["movement.air.gethit.trip.groundlevel"] = x => new Number(x.playerConstants.AirGethitTripGroundlevel);
            s_helpermap["movement.air.gethit.trip.groundlevel"] = x => new Number(x.BasePlayer.playerConstants.AirGethitTripGroundlevel);

            s_playermap["movement.down.bounce.offset.x"] = x => new Number(x.playerConstants.DownBounceOffset.x);
            s_helpermap["movement.down.bounce.offset.x"] = x => new Number(x.BasePlayer.playerConstants.DownBounceOffset.x);

            s_playermap["movement.down.bounce.offset.y"] = x => new Number(x.playerConstants.DownBounceOffset.y);
            s_helpermap["movement.down.bounce.offset.y"] = x => new Number(x.BasePlayer.playerConstants.DownBounceOffset.y);

            s_playermap["movement.down.bounce.yaccel"] = x => new Number(x.playerConstants.DownBounceYaccel);
            s_helpermap["movement.down.bounce.yaccel"] = x => new Number(x.BasePlayer.playerConstants.DownBounceYaccel);

            s_playermap["movement.down.bounce.groundlevel"] = x => new Number(x.playerConstants.DownBounceGroundlevel);
            s_helpermap["movement.down.bounce.groundlevel"] = x => new Number(x.BasePlayer.playerConstants.DownBounceGroundlevel);

            s_playermap["movement.down.friction.threshold"] = x => new Number(x.playerConstants.DownFrictionThreshold);
            s_helpermap["movement.down.friction.threshold"] = x => new Number(x.BasePlayer.playerConstants.DownFrictionThreshold);

        }

        public Const(List<IFunction> children, List<Object> arguments)
            : base(children, arguments)
        {
        }

        public override Number Evaluate(Character state)
        {
            if (Arguments.Count != 1) return new Number();
            string consttype = (string)Arguments[0];

            Combat.Player player = state as Combat.Player;
            if (player != null && s_playermap.ContainsKey(consttype) == true) return s_playermap[consttype](player);

            Combat.Helper helper = state as Combat.Helper;
            if (helper != null && s_helpermap.ContainsKey(consttype) == true) return s_helpermap[consttype](helper);

            return new Number();
        }

        public static Node Parse(ParseState state)
        {
            if (state.CurrentSymbol != Symbol.LeftParen) return null;
            ++state.TokenIndex;

            var constant = state.CurrentUnknown;
            if (constant == null) return null;

            state.BaseNode.Arguments.Add(constant);
            ++state.TokenIndex;

            if (state.CurrentSymbol != Symbol.RightParen) return null;
            ++state.TokenIndex;

            return state.BaseNode;
        }

        #region Fields

        readonly static Dictionary<string, Converter<Combat.Player, Number>> s_playermap;
        readonly static Dictionary<string, Converter<Combat.Helper, Number>> s_helpermap;

        #endregion

    }
}