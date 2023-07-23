namespace UnityMugen.Combat.Logic
{
    public class DisplayRoundNumber : Base
    {

        public bool isOver = false;

        public DisplayRoundNumber()
            : base(RoundState.Intro)
        {
            isOver = false;
        }

        protected override void OnFirstTick()
        {
            base.OnFirstTick();

            int musicId = Launcher.engineInitialization.musicID;
            if (musicId != -1 && Engine.RoundNumber == 1)
            {
                Launcher.soundSystem.PlayMusic(Launcher.profileLoader.musicProfiles[musicId].musicStart, true);
            }
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.Round;
        }

        public override bool IsFinished()
        {
            return isOver;
        }

        public override void LateUpdate() { }

    }
}