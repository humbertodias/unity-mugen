namespace UnityMugen.Combat
{

    public class Clock
    {

        LauncherEngine Launcher => LauncherEngine.Inst;
        FightEngine Engine => Launcher.mugen.Engine;

        public Clock()
        {
            Time = -1;
        }

        public void Tick()
        {
            if (Launcher.engineInitialization.Mode != CombatMode.Training)
            {
                if (Engine.Assertions.TimerFreeze)
                    return;

                if (Time > 0f)
                    --Time;
                else if (Time < 0)
                    Time = 0;
            }
        }

        public int Time;
    }
}