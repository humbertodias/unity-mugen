namespace UnityMugen.Combat.Logic
{
    public class ShowFight : Base
    {

        public bool isOver = false;

        public ShowFight()
            : base(UnityMugen.RoundState.Intro)
        {
            isOver = false;
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.Fight;
        }

        public override bool IsFinished()
        {
            return isOver;// CurrentElement == null;
        }

        public override void LateUpdate() { }

    }
}