using System;
using UnityEngine;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat.Logic
{
    public class CombatOver : Base
    {
        public bool isOver = false;
        public TypeCombatOver m_typeCombatOver;

        public CombatOver() : base(RoundState.PreOver)
        {
            isOver = false;
            m_typeCombatOver = TypeCombatOver.None;
        }

        private void RemovePlayerControl(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            player.PlayerControl = PlayerControl.NoControl;
        }

        protected override void OnFirstTick()
        {
            if (Engine.Team1.VictoryStatus.LoseTime)
                m_typeCombatOver = TypeCombatOver.TimeOver_P2WIN;

            if (Engine.Team2.VictoryStatus.LoseTime)
                m_typeCombatOver = TypeCombatOver.TimeOver_P1WIN;

            if (Engine.Team1.VictoryStatus.LoseKO)
                m_typeCombatOver = TypeCombatOver.KO_WIN_P2;

            if (Engine.Team2.VictoryStatus.LoseKO)
                m_typeCombatOver = TypeCombatOver.KO_WIN_P1;


            if (Engine.Team1.VictoryStatus.WinPerfect && Engine.Team2.VictoryStatus.Lose)
                m_typeCombatOver = TypeCombatOver.KO_WIN_P1_PERFECT;

            if (Engine.Team1.VictoryStatus.WinPerfect && Engine.Team2.VictoryStatus.LoseTime)
                m_typeCombatOver = TypeCombatOver.TimeOver_P1WIN_PERFECT;

            if (Engine.Team2.VictoryStatus.WinPerfect && Engine.Team1.VictoryStatus.Lose)
                m_typeCombatOver = TypeCombatOver.KO_WIN_P2_PERFECT;

            if (Engine.Team2.VictoryStatus.WinPerfect && Engine.Team1.VictoryStatus.LoseTime)
                m_typeCombatOver = TypeCombatOver.TimeOver_P2WIN_PERFECT;

            if (Engine.Team1.VictoryStatus.WinKO && !Engine.Team1.VictoryStatus.WinPerfect)
                m_typeCombatOver = TypeCombatOver.KO_WIN_P1;

            if (Engine.Team2.VictoryStatus.WinKO && !Engine.Team2.VictoryStatus.WinPerfect)
                m_typeCombatOver = TypeCombatOver.KO_WIN_P2;

            if (Engine.Team1.VictoryStatus.LoseKO && Engine.Team2.VictoryStatus.LoseKO)
                m_typeCombatOver = TypeCombatOver.Draw_Game;

            if (Engine.Team1.VictoryStatus.TimeOver_DrawGame)
            {
                Engine.DrawGames++;
                m_typeCombatOver = TypeCombatOver.TimeOver_DrawGame;
            }
            //TimeOut_Draw, Draw_Game

            base.OnFirstTick();
        }

        public override void UpdateFE()
        {
            base.UpdateFE();

            if (TickCount < Launcher.initializationSettings.KOSlowTime && Engine.Assertions.NoKOSlow == false &&
                (m_typeCombatOver == TypeCombatOver.KO_WIN_P1 ||
                    m_typeCombatOver == TypeCombatOver.KO_WIN_P2 ||
                    m_typeCombatOver == TypeCombatOver.KO_WIN_P1_PERFECT ||
                    m_typeCombatOver == TypeCombatOver.KO_WIN_P2_PERFECT ||
                    m_typeCombatOver == TypeCombatOver.Draw_Game))
            {
                Engine.Speed = GameSpeed.Slow;
            }
            else
            {
                Engine.Speed = GameSpeed.Normal;
            }

            if (TickCount == Launcher.initializationSettings.OverWaitTime)
            {
                Engine.Team1.DoAction(RemovePlayerControl);
                Engine.Team2.DoAction(RemovePlayerControl);
            }
        }

        protected override RoundInformationType GetElement()
        {
            switch (m_typeCombatOver)
            {
#warning Nao terminado
                case TypeCombatOver.Draw_Game:
                    return RoundInformationType.DrawGame;

                case TypeCombatOver.KO_WIN_P1:
                case TypeCombatOver.KO_WIN_P2:
                case TypeCombatOver.KO_WIN_P1_PERFECT:
                case TypeCombatOver.KO_WIN_P2_PERFECT:
                    return RoundInformationType.KO;

                case TypeCombatOver.TimeOver_DrawGame:
                case TypeCombatOver.TimeOver_P1WIN:
                case TypeCombatOver.TimeOver_P2WIN:
                case TypeCombatOver.TimeOver_P1WIN_PERFECT:
                case TypeCombatOver.TimeOver_P2WIN_PERFECT:
                    return RoundInformationType.TimeOver;

                default:
                    throw new ArgumentOutOfRangeException("m_wintype");
            }
        }

        public override bool IsFinished()
        {
            if (TickCount < Launcher.initializationSettings.OverWaitTime) return false;

            bool p1IsReady = IsReady(Engine.Team1);
            bool p2IsReady = IsReady(Engine.Team2);
            if (p1IsReady || p2IsReady)
                return isOver && (TickCount - Launcher.initializationSettings.OverWaitTime > Launcher.initializationSettings.WinTime);

            return false;
        }

        private bool IsReady(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            if (IsDone(team.MainPlayer) == false) return false;
            if (team.TeamMate != null && IsDone(team.TeamMate) == false) return false;

            return true;
        }

        private bool IsDone(Player player)
        {
            if (player == null) throw new ArgumentNullException(nameof(player));

            //if (player.Life > 0)
            //{
            //    return player.StateManager.CurrentState.Number == StateNumber.Standing;
            //}
            if (player.CurrentVelocity == Vector2.zero) {
                switch (m_typeCombatOver)
                {
                    case TypeCombatOver.Draw_Game:
                        return true;

                    case TypeCombatOver.KO_WIN_P1:
                    case TypeCombatOver.KO_WIN_P2:
                    case TypeCombatOver.KO_WIN_P1_PERFECT:
                    case TypeCombatOver.KO_WIN_P2_PERFECT:
                        if (player.StateManager.CurrentState.number == StateNumber.HitLieDead)
                        {
                            return true;
                        }
                        break;

                    case TypeCombatOver.TimeOver_DrawGame:
                    case TypeCombatOver.TimeOver_P1WIN:
                    case TypeCombatOver.TimeOver_P2WIN:
                    case TypeCombatOver.TimeOver_P1WIN_PERFECT:
                    case TypeCombatOver.TimeOver_P2WIN_PERFECT:
                        if (player.StateManager.CurrentState.number == StateNumber.Standing)
                        {
                            return true;
                        }
                        break;

                    default:
                        return false;
                }
            }
            return false;
        }

        public override void LateUpdate() { }

    }
}