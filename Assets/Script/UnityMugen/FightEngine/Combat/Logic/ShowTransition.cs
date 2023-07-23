using UnityMugen.StateMachine;

namespace UnityMugen.Combat.Logic
{
    public enum TypeShowTransition
    {
        IN,
        OUT
    }

    public class ShowTransition : Base
    {

        public bool isOver = false;
        public TypeShowTransition typeShowTransition;
        private bool trainnerMode;

        public ShowTransition(TypeShowTransition _typeShowTransition, bool _trainnerMode = false)
            : base(RoundState.PreIntro)
        {
            isOver = false;
            typeShowTransition = _typeShowTransition;
            trainnerMode = _trainnerMode;
        }

        public override bool IsFinished()
        {
            //if (TickCount == 300)
            //{
            //    isOver = true;
            //}
            if(isOver && 
                typeShowTransition == TypeShowTransition.IN && 
                Engine.Initialization.Mode == CombatMode.Training)
                Engine.ResetFE(false);

            return isOver;
        }

        protected override RoundInformationType GetElement()
        {
            return RoundInformationType.None;
        }

        protected override void OnFirstTick()
        {
            base.OnFirstTick();
            //if (Engine.RoundNumber == 1) // Se eu deixar isso abilitado o personagem não voltara a cor de Palette escolhida no inicio
            if (typeShowTransition == TypeShowTransition.OUT)
            {
                if (!trainnerMode)
                {
                    Engine.Team1.MainPlayer.StateManager.ChangeState(StateNumber.Initialize);
                    Engine.Team2.MainPlayer.StateManager.ChangeState(StateNumber.Initialize);
                }
                else
                {
                    int musicId = Launcher.engineInitialization.musicID;
                    if (musicId != -1 && Engine.RoundNumber == 1)
                    {
                        Launcher.soundSystem.PlayMusic(Launcher.profileLoader.musicProfiles[musicId].musicStart, true);
                    }
                }
            }
        }

        public override void UpdateFE()
        {
            base.UpdateFE();
        }

        private void SetPlayer(Player player)
        {
            //player.StateManager.ChangeState(StateNumber.Initialize);
        }

        public override void LateUpdate()
        {
            if (Engine.RoundNumber == 1)
            {
                Engine.Team1.DoAction(SetPlayer);
                Engine.Team2.DoAction(SetPlayer);
            }
        }

    }
}