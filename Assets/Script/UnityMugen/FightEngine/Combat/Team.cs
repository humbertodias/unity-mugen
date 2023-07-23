using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Collections;

namespace UnityMugen.Combat
{
    public class Team
    {
        static FightEngine Engine => LauncherEngine.Inst.mugen.Engine;

        public Team(TeamSide side)
        {
            m_side = side;
            m_victorystatus = new VictoryStatus(this);
            m_winhistory = new List<Win>(9);
            m_p1 = null;
            m_p2 = null;
        }

        public void Clear()
        {
            m_winhistory.Clear();
            m_p1 = null;
            m_p2 = null;
        }

        public void DoAction(Action<Player> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            if (Mode == TeamMode.Turns)
            {
                if (OtherTeam.Wins.Count == 0)
                {
                    action(MainPlayer);
                }
                else
                {
                    action(TeamMate);
                }
                return;
            }

            action(MainPlayer);
            if (TeamMate != null) action(TeamMate);
        }

        public void ResetPlayers()
        {
            m_combocounter = new ComboCounter(this);

            m_winhistory = new List<Win>(9);
            MainPlayer.StateManager.ChangeState(0);
            MainPlayer.SetLocalAnimation(0, 0);
            MainPlayer.PlayerControl = PlayerControl.NoControl;
            MainPlayer.Life = MainPlayer.playerConstants.MaximumLife;
            MainPlayer.Power = 0;
            MainPlayer.SoundManager.Stop();
            MainPlayer.JugglePoints = MainPlayer.playerConstants.AirJuggle;

            if (Side == TeamSide.Left)
            {
                MainPlayer.CurrentLocation = Engine.stageScreen.Stage.P1Start;
                MainPlayer.CurrentFacing = Engine.stageScreen.Stage.P1Facing;
                if (TeamMate != null)
                {
                    TeamMate.CurrentLocation = Engine.stageScreen.Stage.P3Start;
                    TeamMate.CurrentFacing = Engine.stageScreen.Stage.P3Facing;
                }
            }
            else
            {
                MainPlayer.CurrentLocation = Engine.stageScreen.Stage.P2Start;
                MainPlayer.CurrentFacing = Engine.stageScreen.Stage.P2Facing;
                if (TeamMate != null)
                {
                    TeamMate.CurrentLocation = Engine.stageScreen.Stage.P4Start;
                    TeamMate.CurrentFacing = Engine.stageScreen.Stage.P4Facing;
                }
            }

            if (TeamMate != null)
            {
                TeamMate.StateManager.ChangeState(0);
                TeamMate.SetLocalAnimation(0, 0);
                TeamMate.PlayerControl = PlayerControl.NoControl;
                TeamMate.Life = TeamMate.playerConstants.MaximumLife;
                TeamMate.Power = 0;
                TeamMate.SoundManager.Stop();
                TeamMate.JugglePoints = TeamMate.playerConstants.AirJuggle;

                //if (Side == TeamSide.Left)
                //{
                //	MainPlayer.CurrentLocation = Engine.Stage.P1Start;
                //	MainPlayer.CurrentFacing = Engine.Stage.P1Facing;
                //}
                //else
                //{
                //	MainPlayer.CurrentLocation = Engine.Stage.P2Start;
                //	MainPlayer.CurrentFacing = Engine.Stage.P2Facing;
                //}
            }

        }

        public void AddWin(Win win)
        {
            m_winhistory.Add(win);
        }

        public void CreatePlayers(TeamMode mode, List<PlayerCreation> PlayersCreation, PlayerID playerID, Transform entitiesTransform)
        {
            if (PlayersCreation == null) throw new ArgumentNullException(nameof(PlayersCreation));

            Clear();

            for (int i = 0; i < PlayersCreation.Count; i++)
            {
                PlayerProfileManager profile = PlayersCreation[i].profile;

                GameObject p = new GameObject(profile.name + playerID.ToString());
                Player GPlayer = p.AddComponent<Player>();
                //GPlayer = LauncherEngine._inst.Instance(GPlayer, Vector3.zero, Quaternion.identity);
                Player player = GPlayer.GetComponent<Player>();

                player.m_PlayerNumber = playerID;
                //GPlayer.name = PlayersCreation[i].profile.name ;
                player.transform.SetParent(entitiesTransform);

                Mode = mode;
                if (i == 0)
                {
                    m_p1 = player.Iniciar(PlayersCreation[i].mode, this, profile, PlayersCreation[i].paletteIndex);
                }
                else if (i == 1)
                {
                    m_p2 = player.Iniciar(PlayersCreation[i].mode, this, profile, PlayersCreation[i].paletteIndex);
                }
            }

            ResetPlayers();
        }

        public TeamSide Side => m_side;

        public TeamMode Mode { get; set; }

        public Player MainPlayer => m_p1;

        public Player TeamMate => m_p2;

        public Team OtherTeam => Side == TeamSide.Left ? Engine.Team2 : Engine.Team1;

        public VictoryStatus VictoryStatus => m_victorystatus;

        public ComboCounter ComboCounter => m_combocounter;

        public ListIterator<Win> Wins => new ListIterator<Win>(m_winhistory);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ComboCounter m_combocounter;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TeamSide m_side;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private VictoryStatus m_victorystatus;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Win> m_winhistory;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Player m_p1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Player m_p2;

    }
}