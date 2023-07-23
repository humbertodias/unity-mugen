namespace UnityMugen.Combat.Logic
{
    public abstract class Base
    {
        public LauncherEngine Launcher => LauncherEngine.Inst;
        public FightEngine Engine => Launcher.mugen.Engine;

        protected Base(RoundState state)
        {
            TickCount = -1;
            State = state;
        }

        public virtual void ResetFE()
        {
            TickCount = -1;
        }

        public virtual void UpdateFE()
        {
            TickCount++;

            if (TickCount == 0)
            {
                OnFirstTick();
            }
        }

        protected virtual void OnFirstTick()
        {
            CurrentElement = GetElement();
        }

        protected abstract RoundInformationType GetElement();

        public abstract bool IsFinished();

        public abstract void LateUpdate();

        public int TickCount;

        public RoundState State;

        public RoundInformationType CurrentElement;

    }
}