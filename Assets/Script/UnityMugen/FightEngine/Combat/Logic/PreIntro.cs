using System;
using UnityEngine;

namespace UnityMugen.Combat.Logic
{
    public class PreIntro : Base
    {
        public PreIntro() : base(RoundState.None) { }

        private void SetPlayer(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            Engine.Entities.Add(player);
            player.iniciado = true;

#warning nao sei se aqui é o melhor lugar para dar reset no player. nessessario pelo menos limpar o helpers do player.
            player.ResetFE();

            player.StateManager.ChangeState(0);
            player.SetLocalAnimation(0, 0);
            player.PlayerControl = PlayerControl.NoControl;
            player.Life = player.playerConstants.MaximumLife;
            player.SoundManager.Stop();
            player.JugglePoints = player.playerConstants.AirJuggle;
            //   player.StateManager.ChangeState(StateNumber.Initialize);
            player.OffensiveInfo.ResetFE();
            player.DefensiveInfo.ResetFE();

            if (player.Team.Side == TeamSide.Left)
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

        protected override void OnFirstTick()
        {
            base.OnFirstTick();

            Engine.Clock.Time = Launcher.initializationSettings.RoundLength;

            Engine.CameraFE.Location = new Vector2(0, 0);
            Engine.CameraFE.ResetScreenCamera(Engine.stageScreen.Stage.StartPositionCamera);

            Engine.Entities.Clear();

            Engine.Team1.ComboCounter.ResetFE();
            Engine.Team1.DoAction(SetPlayer);

            Engine.Team2.ComboCounter.ResetFE();
            Engine.Team2.DoAction(SetPlayer);

            if (Engine.RoundNumber == 1)
                Launcher.soundSystem.StopMusic();
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.None;
        }

        public override bool IsFinished()
        {
            return TickCount == Launcher.initializationSettings.IntroDelay;
        }

        public override void LateUpdate() { }

    }
}