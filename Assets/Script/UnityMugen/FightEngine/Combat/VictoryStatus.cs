using System;
using System.Diagnostics;

namespace UnityMugen.Combat
{
    public class VictoryStatus
    {
        static FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public VictoryStatus(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            m_team = team;
        }

        public bool Win => WinKO || WinTime;

        public bool WinKO => LoseKO == false && m_team.OtherTeam.VictoryStatus.LoseKO;

        public bool WinTime
        {
            get
            {
                if (WinKO) return false;
                if (Engine.Clock.Time != 0) return false;

                var mylife = GetLife(m_team);
                var otherlife = GetLife(m_team.OtherTeam);
                return otherlife < mylife;
            }
        }

        public bool WinPerfect
        {
            get
            {
                if (Win == false) return false;
                if (m_team.MainPlayer.Life != m_team.MainPlayer.playerConstants.MaximumLife) return false;
                if (m_team.TeamMate != null && m_team.TeamMate.Life != m_team.TeamMate.playerConstants.MaximumLife) return false;
                return true;
            }
        }

        public bool WinHyper
        {
            get
            {
                if (Win == false) return false;
                if (WinKO && m_team.Wins[m_team.Wins.Count - 1].Victory == Victory.Hyper)
                    return true;
                else
                    return false;
            }
        }

        public bool WinSpecial
        {
            get
            {
                if (Win == false) return false;
                if (WinKO && m_team.Wins[m_team.Wins.Count - 1].Victory == Victory.Special)
                    return true;
                else
                    return false;
            }
        }


        public bool Lose => LoseKO || LoseTime;

        public bool LoseKO => GetLife(m_team) == 0;

        public bool LoseTime
        {
            get
            {
                if (LoseKO) return false;
                if (Engine.Clock.Time != 0) return false;

                var mylife = GetLife(m_team);
                var otherlife = GetLife(m_team.OtherTeam);

                return otherlife > mylife;
            }
        }

        public bool TimeOver_DrawGame
        {
            get
            {
                var mylife = GetLife(m_team);
                var otherlife = GetLife(m_team.OtherTeam);
                if (Engine.Clock.Time == 0 && (mylife == otherlife))
                {
                    return true;
                }
                else
                    return false;
            }
        }

        private static float GetLife(Team team)
        {
            if (team.Mode != TeamMode.Turns) return team.MainPlayer.Life + (team.TeamMate?.Life ?? 0);

            if (team.OtherTeam.Wins.Count == 0)
            {
                return team.MainPlayer.Life;
            }
            return team.TeamMate.Life;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Team m_team;

    }
}