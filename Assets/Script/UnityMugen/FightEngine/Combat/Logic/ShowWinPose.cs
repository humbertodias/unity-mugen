using System;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat.Logic
{
    internal class ShowWinPose : Base
    {
        public bool isOver = false;

        public ShowWinPose()
            : base(RoundState.Over)
        {
            isOver = false;
        }

        private Team GetWinningTeam(out int playerNumber)
        {
            if (Engine.Team1.VictoryStatus.Win)
            {
                playerNumber = 1;
                return Engine.Team1;
            }
            if (Engine.Team2.VictoryStatus.Win)
            {
                playerNumber = 2;
                return Engine.Team2;
            }
            playerNumber = 0;
            return null;
        }

        private void EnterWinPose(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            player.StateManager.ChangeState(StateNumber.WinPose);
        }

        private void EnterTimeLosePose(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            player.StateManager.ChangeState(StateNumber.LoseTimeOverPose);
        }

        protected override void OnFirstTick()
        {
            base.OnFirstTick();
            int playerNumber;
            var winningteam = GetWinningTeam(out playerNumber);
            if (winningteam != null)
            {
                winningteam.DoAction(EnterWinPose);

                var win = new Win(Victory.Normal, winningteam.VictoryStatus.WinPerfect);
                winningteam.AddWin(win);

                if (winningteam.OtherTeam.VictoryStatus.LoseTime)
                {
                    winningteam.OtherTeam.DoAction(EnterTimeLosePose);
                }
            }
            else if(CurrentElement != RoundInformationType.DrawGame)
            {
                Engine.Team1.DoAction(EnterTimeLosePose);
                Engine.Team2.DoAction(EnterTimeLosePose);
            }
        }

        protected override RoundInformationType GetElement()
        {
            int playerNumber;
            var winningteam = GetWinningTeam(out playerNumber);

#warning Nao sei se isto esta certo, acredito que esteja correto.
            if (winningteam == null)
            {
                return RoundInformationType.DrawGame;
            }
            else if (winningteam.TeamMate != null)
            {
                return RoundInformationType.Win2;
            }

            if (playerNumber == 1) // Player 1
                return RoundInformationType.P1Win;
            else // Player 2
                return RoundInformationType.P2Win;
        }

        public override bool IsFinished()
        {
            // acho que esteticamente é melhor não pular o show win pose
            //if (Engine.Team1.MainPlayer != null && PlayeInputSkip(Engine.Team1.MainPlayer)) return true;
            //if (Engine.Team1.TeamMate != null && PlayeInputSkip(Engine.Team1.TeamMate)) return true;

            //if (Engine.Team2.MainPlayer != null && PlayeInputSkip(Engine.Team2.MainPlayer)) return true;
            //if (Engine.Team2.TeamMate != null && PlayeInputSkip(Engine.Team2.TeamMate)) return true;

            return Engine.Assertions.WinPose == false && (TickCount > Launcher.initializationSettings.OverTime && isOver /*|| CurrentElement == null*/);
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

    }
}