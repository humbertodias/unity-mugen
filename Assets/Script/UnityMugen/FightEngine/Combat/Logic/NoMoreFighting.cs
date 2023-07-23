namespace UnityMugen.Combat.Logic
{
    public class NoMoreFighting : Base
    {
        public NoMoreFighting()
            : base(RoundState.Over)
        {
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.None;
        }

        public override bool IsFinished()
        {
            return false;
        }

        public override void LateUpdate() { }

    }
}
