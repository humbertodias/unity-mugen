using System;

namespace UnityMugen.Combat.Logic
{
    public class Fighting : Base
    {
        public Fighting()
            : base(RoundState.Fight)
        {
        }

        public override void UpdateFE()
        {
            base.UpdateFE();

            if (TickCount != 0 && TickCount % 60 == 0)
                Engine.Clock.Tick();
        }

        private void GivePlayerControl(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            player.PlayerControl = PlayerControl.InControl;
        }

        protected override void OnFirstTick()
        {
            base.OnFirstTick();

            Engine.Team1.DoAction(GivePlayerControl);
            Engine.Team2.DoAction(GivePlayerControl);
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.None;
        }

        public override bool IsFinished()
        {
            return Engine.Clock.Time == 0 || Engine.Team1.VictoryStatus.Lose || Engine.Team2.VictoryStatus.Lose;
        }

        public override void LateUpdate() { }

    }
}