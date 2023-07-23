using UnityMugen.Combat;

namespace UnityMugen.Evaluation.Triggers
{

    [CustomFunction("Const")]
    internal static class Const
    {
        [Tag("data.life")]
        public static float Data_Life(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.MaximumLife;
        }
        [Tag("data.power")]
        public static float Data_Power(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.MaximumPower;
        }
        [Tag("data.attack")]
        public static float Data_Attack(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AttackPower;
        }
        [Tag("data.defence")]
        public static int Data_Defence(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DefensivePower;
        }
        [Tag("data.fall.defence_mul")]
        public static float Data_Fall_Defence_Mul(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.FallDefenceMul;
        }

        [Tag("data.fall.defence_up")]
        public static float Data_Fall_Defence_Up(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.FallDefenseUp;
        }

        [Tag("data.liedown.time")]
        public static int Data_Liedown_Time(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.LieDownTime;
        }
        [Tag("data.airjuggle")]
        public static int Data_Airjuggle(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirJuggle;
        }

        [Tag("data.sparkno")]
        public static int Data_Sparkno(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DefaultSparkNumber;
        }
        [Tag("data.guard.sparkno")]
        public static int Data_Guard_Sparkno(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DefaultGuardSparkNumber;
        }
        [Tag("data.KO.echo")]
        public static bool Data_Ko_Echo(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return false;
            }

            return character.BasePlayer.playerConstants.KOEcho;
        }
        [Tag("data.IntPersistIndex")]
        public static int Data_IntPersistIndex(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.PersistanceIntIndex;
        }
        [Tag("data.FloatPersistIndex")]
        public static int Data_FloatPersistIndex(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.PersistanceFloatIndex;
        }

        [Tag("size.draw.offset.x")]
        public static float Size_Draw_Offset_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Drawoffset.x;
        }
        [Tag("size.draw.offset.y")]
        public static float Size_Draw_Offset_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Drawoffset.y;
        }

        [Tag("size.xscale")]
        public static float Size_Xscale(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Scale.x;

            var helper = character as Helper;
            if (helper != null) return helper.Data.Scale.x;

            error = true;
            return 0;
        }
        [Tag("size.yscale")]
        public static float Size_Yscale(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Scale.y;

            var helper = character as Helper;
            if (helper != null) return helper.Data.Scale.y;

            error = true;
            return 0;
        }
        [Tag("size.ground.back")]
        public static float Size_Ground_Back(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.GroundBack;
        }
        [Tag("size.ground.front")]
        public static float Size_Ground_Front(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.GroundFront;
        }
        [Tag("Size.Air.Back")]
        public static float Size_Air_Back(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airback;
        }
        [Tag("Size.Air.Front")]
        public static float Size_Air_Front(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airfront;
        }
        [Tag("Size.Height")]
        public static float Size_Height(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Height;
        }
        [Tag("Size.Attack.Dist")]
        public static float Size_Attack_Dist(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Attackdistance;
        }
        [Tag("Size.Proj.Attack.Dist")]
        public static float Size_Proj_Attack_Dist(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Projectileattackdist;
        }
        [Tag("Size.Proj.Doscale")]
        public static bool Size_Proj_Doscale(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.ProjectileScaling;

            var helper = character as Helper;
            if (helper != null) return helper.Data.ProjectileScaling;

            error = true;
            return false;
        }
        [Tag("Size.Head.Pos.X")]
        public static float Size_Head_Pos_X(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Headposition.x;

            var helper = character as Helper;
            if (helper != null) return helper.Data.HeadPosition.x;

            error = true;
            return 0;
        }
        [Tag("Size.Head.Pos.Y")]
        public static float Size_Head_Pos_Y(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Headposition.y;

            var helper = character as Helper;
            if (helper != null) return helper.Data.HeadPosition.y * Constant.Scale2;

            error = true;
            return 0;
        }
        [Tag("size.mid.pos.x")]
        public static float Size_Mid_Pos_X(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Midposition.x;

            var helper = character as Helper;
            if (helper != null) return helper.Data.MidPosition.x * Constant.Scale2;

            error = true;
            return 0;
        }
        [Tag("size.mid.pos.y")]
        public static float Size_Mid_Pos_Y(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Midposition.y;

            var helper = character as Helper;
            if (helper != null) return helper.Data.MidPosition.y * Constant.Scale2;

            error = true;
            return 0;
        }
        [Tag("Size.Shadowoffset")]
        public static float Size_Shadowoffset(Character character, ref bool error)
        {
            var player = character as Player;
            if (player != null) return player.playerConstants.Shadowoffset;

            var helper = character as Helper;
            if (helper != null) return helper.Data.ShadowOffset * Constant.Scale2;

            error = true;
            return 0;
        }







        [Tag("Velocity.Walk.Fwd.X")]
        public static float Velocity_Walk_Fwd_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Walk_forward;
        }
        [Tag("Velocity.Walk.Back.X")]
        public static float Velocity_Walk_Back_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Walk_back;
        }
        [Tag("Velocity.Run.Fwd.X")]
        public static float Velocity_Run_Fwd_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Run_fwd.x;
        }
        [Tag("Velocity.Run.Fwd.Y")]
        public static float Velocity_Run_Fwd_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Run_fwd.y;
        }
        [Tag("Velocity.Run.Back.X")]
        public static float Velocity_Run_Back_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Run_back.x;
        }
        [Tag("Velocity.Run.Back.Y")]
        public static float Velocity_Run_Back_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Run_back.y;
        }
        [Tag("Velocity.Jump.Y")]
        public static float Velocity_Jump_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Jump_neutral.y;
        }
        [Tag("Velocity.Jump.Neu.X")]
        public static float Velocity_Jump_Neu_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Jump_neutral.x;
        }
        [Tag("Velocity.Jump.Neu.Y")]
        public static float Velocity_Jump_Neu_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Jump_neutral.y;
        }
        [Tag("Velocity.Runjump.Y")]
        public static float Velocity_Runjump_Y(Character character, ref bool error)
        {
            return Velocity_Jump_Neu_Y(character, ref error);
        }
        [Tag("Velocity.Jump.Back.X")]
        public static float Velocity_Jump_Back_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Jump_back.x;
        }
        [Tag("Velocity.Jump.Fwd.X")]
        public static float Velocity_Jump_Fwd_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Jump_forward.x;
        }
        [Tag("Velocity.Runjump.Back.X")]
        public static float Velocity_Runjump_Back_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Runjump_back.x;
        }
        [Tag("Velocity.Runjump.Fwd.X")]
        public static float Velocity_Runjump_Fwd_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Runjump_fwd.x;
        }
        [Tag("Velocity.Airjump.Y")]
        public static float Velocity_Airjump_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airjump_neutral.y;
        }
        [Tag("Velocity.Airjump.Neu.X")]
        public static float Velocity_Airjump_Neu_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airjump_neutral.x;
        }
        [Tag("Velocity.Airjump.Back.X")]
        public static float Velocity_Airjump_Back_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airjump_back.x;
        }
        [Tag("Velocity.Airjump.Fwd.X")]
        public static float Velocity_Airjump_Fwd_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airjump_forward.x;
        }
        [Tag("Velocity.air.gethit.groundrecover.x")]
        public static float Velocity_air_gethit_groundrecover_x(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitGroundrecover.x;
        }
        [Tag("Velocity.air.gethit.groundrecover.y")]
        public static float Velocity_air_gethit_groundrecover_y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitGroundrecover.y;
        }
        [Tag("Velocity.air.gethit.airrecover.mul.x")]
        public static float Velocity_air_gethit_airrecover_mul_x(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverMul.x;
        }
        [Tag("Velocity.air.gethit.airrecover.mul.y")]
        public static float Velocity_air_gethit_airrecover_mul_y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverMul.y;
        }
        [Tag("Velocity.air.gethit.airrecover.add.x")]
        public static float Velocity_air_gethit_airrecover_add_X(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverAdd.x;
        }
        [Tag("Velocity.air.gethit.airrecover.add.y")]
        public static float Velocity_air_gethit_airrecover_add_Y(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverAdd.y;
        }
        [Tag("Velocity.air.gethit.airrecover.back")]
        public static float Velocity_air_gethit_airrecover_back(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverBack;
        }
        [Tag("Velocity.air.gethit.airrecover.fwd")]
        public static float Velocity_air_gethit_airrecover_fwd(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverFwd;
        }
        [Tag("Velocity.air.gethit.airrecover.up")]
        public static float Velocity_air_gethit_airrecover_up(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverUp;
        }
        [Tag("Velocity.air.gethit.airrecover.down")]
        public static float Velocity_air_gethit_airrecover_down(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }
            return character.BasePlayer.playerConstants.AirGethitAirrecoverDown;
        }





        [Tag("Movement.Airjump.Num")]
        public static int Movement_Airjump_Num(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airjumps;
        }
        [Tag("Movement.Airjump.Height")]
        public static float Movement_Airjump_Height(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Airjumpheight;
        }
        [Tag("Movement.Yaccel")]
        public static float Movement_Yaccel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Vert_acceleration;
        }
        [Tag("Movement.Stand.Friction")]
        public static float Movement_Stand_Friction(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Standfriction;
        }
        [Tag("Movement.Crouch.Friction")]
        public static float Movement_Crouch_Friction(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.Crouchfriction;
        }



        [Tag("Movement.stand.friction.threshold")]
        public static float MovementStandFrictionThreshold(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.StandFrictionThreshold;
        }
        [Tag("Movement.crouch.friction.threshold")]
        public static float MovementCrouchFrictionThreshold(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.CrouchFrictionThreshold;
        }
        [Tag("Movement.jump.changeanim.threshold")]
        public static float MovementJumpChangeanimThreshold(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.JumpChangeanimThreshold;
        }
        [Tag("Movement.air.gethit.groundlevel")]
        public static float MovementAirGethitGroundlevel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirGethitGroundlevel;
        }
        [Tag("Movement.air.gethit.groundrecover.ground.threshold")]
        public static float MovementAirGethitGroundrecoverGroundThreshold(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirGethitGroundrecoverGroundThreshold;
        }
        [Tag("Movement.air.gethit.groundrecover.groundlevel")]
        public static float Movement_Air_Gethit_Groundrecover_Groundlevel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirGethitGroundrecoverGroundlevel;
        }
        [Tag("Movement.air.gethit.airrecover.threshold")]
        public static float Movement_AirGethitAirrecoverThreshold(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirGethitAirrecoverThreshold;
        }
        [Tag("Movement.air.gethit.airrecover.yaccel")]
        public static float Movement_AirGethitAirrecoverYaccel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirGethitAirrecoverYaccel;
        }
        [Tag("Movement.air.gethit.trip.groundlevel")]
        public static float Movement_AirGethitTripGroundlevel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.AirGethitTripGroundlevel;
        }
        [Tag("Movement.down.bounce.offset.x")]
        public static float Movement_DownBounceOffsetX(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DownBounceOffset.x;
        }
        [Tag("Movement.down.bounce.offset.y")]
        public static float Movement_DownBounceOffsetY(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DownBounceOffset.y;
        }
        [Tag("Movement.down.bounce.yaccel")]
        public static float Movement_DownBounceYaccel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DownBounceYaccel;
        }
        [Tag("Movement.down.bounce.groundlevel")]
        public static float Movement_DownBounceGroundlevel(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DownBounceGroundlevel;
        }
        [Tag("Movement.down.friction.threshold")]
        public static float Movement_DownFrictionThreshold(Character character, ref bool error)
        {
            if (character == null)
            {
                error = true;
                return 0;
            }

            return character.BasePlayer.playerConstants.DownFrictionThreshold;
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
    }
}