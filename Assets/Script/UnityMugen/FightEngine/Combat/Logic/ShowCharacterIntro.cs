using System;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat.Logic
{
    public class ShowCharacterIntro : Base
    {
        public ShowCharacterIntro()
            : base(UnityMugen.RoundState.Intro)
        {
            m_finishearly = false;
        }

        public override void ResetFE()
        {
            base.ResetFE();

            m_finishearly = false;
        }

        public override void UpdateFE()
        {
            base.UpdateFE();

            CheckForEarlyEnd();
        }

        private void CheckForEarlyEnd()
        {
            if (IsFinished()) return;

            if (Engine.Team1.MainPlayer != null && PlayeInputSkip(Engine.Team1.MainPlayer)) m_finishearly = true;
            if (Engine.Team1.TeamMate != null && PlayeInputSkip(Engine.Team1.TeamMate)) m_finishearly = true;

            if (Engine.Team2.MainPlayer != null && PlayeInputSkip(Engine.Team2.MainPlayer)) m_finishearly = true;
            if (Engine.Team2.TeamMate != null && PlayeInputSkip(Engine.Team2.TeamMate)) m_finishearly = true;

            if (m_finishearly == false) return;

            Engine.CameraFE.Location = new Vector2(0, 0);

            Engine.Entities.Clear();
            Engine.Team1.DoAction(SetPlayer);
            Engine.Team2.DoAction(SetPlayer);
        }

        private void SetPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            Engine.Entities.Add(player);
            player.iniciado = true;

            player.StateManager.ChangeState(0);
            player.SetLocalAnimation(0, 0);
            player.PlayerControl = UnityMugen.PlayerControl.NoControl;
            player.Life = player.playerConstants.MaximumLife;
            player.SoundManager.Stop();
            player.JugglePoints = player.playerConstants.AirJuggle;
            player.StateManager.ChangeState(StateNumber.Standing);

            player.Explods.Clear();
            player.Helpers.Clear();

            if (player.Team.Side == UnityMugen.TeamSide.Left)
            {
                player.CurrentLocation = Engine.stageScreen.Stage.P1Start;
                player.CurrentFacing = Engine.stageScreen.Stage.P1Facing;
            }
            else
            {
                player.CurrentLocation = Engine.stageScreen.Stage.P2Start;
                player.CurrentFacing = Engine.stageScreen.Stage.P2Facing;
            }
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.None;
        }

        public override bool IsFinished()
        {
            return m_finishearly || Engine.Assertions.Intro == false;
        }

        private bool PlayeInputSkip(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            if (player.CommandManager.IsActive("x") ||
                player.CommandManager.IsActive("y") ||
                player.CommandManager.IsActive("z") ||
                player.CommandManager.IsActive("a") ||
                player.CommandManager.IsActive("a") ||
                player.CommandManager.IsActive("a") ||
                player.CommandManager.IsActive("taunt"))
            {
                Launcher.soundSystem.StopAllSounds();
                Engine.RoundInformation.StopAllSounds();
                return true;
            }
            return false;
        }

        public override void LateUpdate() { }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_finishearly;

    }
}