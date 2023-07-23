using System;
using UnityEngine;

namespace UnityMugen.Combat
{
    public class ComboCounter
    {
        public enum State { None, NotShown, MovingIn, MovingOut, Shown }

        LauncherEngine Launcher => LauncherEngine.Inst;


        public int HitCount;
        public string CounterText;
        public string DisplayText;
        public State StateCombo;
        public int DisplayTimeCount;
        public int HitBonus;

        bool resetMoveIn;
        readonly Team m_team;

        readonly string m_displayelement;
        readonly int m_displaytime;


        public ComboCounter(Team team)
        {
            if (team == null) throw new ArgumentNullException(nameof(team));

            m_team = team;

            var prefix = Misc.GetPrefix(m_team.Side);

            m_displayelement = Launcher.initializationSettings.comboText;
            m_displaytime = Launcher.initializationSettings.displaytimeCombo;

            StateCombo = State.NotShown;
            HitCount = 0;
            DisplayTimeCount = 0;
            CounterText = string.Empty;
            DisplayText = string.Empty;
            HitBonus = 0;
        }

        public void ResetFE()
        {
            StateCombo = State.NotShown;
            HitCount = 0;
            DisplayTimeCount = 0;
            CounterText = string.Empty;
            DisplayText = string.Empty;
            HitBonus = 0;
        }

        public void UpdateFE()
        {
            SetHitCount(GetNewHitCount());
        }

        public void AddHits(int hits)
        {
            if (hits < 0) throw new ArgumentOutOfRangeException(nameof(hits));

            HitBonus += hits;
        }

        private void SetHitCount(int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0)
                resetMoveIn = true;

            //if (count == 2 && HitCount > count && resetMoveIn)
            //{
            //    resetMoveIn = false;
            //    StateCombo = State.MovingIn;
            //}
            if (count < 2) return;

            if (resetMoveIn && StateCombo == State.Shown)
            {
                resetMoveIn = false;
                StateCombo = State.MovingIn;
            }

            if (count == HitCount)
            {
                DisplayTimeCount = m_displaytime;
            }
            else if (count > HitCount)
            {
                HitCount = count;
                CounterText = new StringFormatter().BuildString("%i", HitCount);

                DisplayText = m_displayelement;

                if (StateCombo == State.MovingOut || StateCombo == State.NotShown)
                    StateCombo = State.MovingIn;
            }
        }

        public int GetNewHitCount()
        {
            var otherteam = m_team.OtherTeam;
            var hitcount = 0;

            if (otherteam.MainPlayer != null && otherteam.MainPlayer.MoveType == MoveType.BeingHit &&
                otherteam.MainPlayer.DefensiveInfo.Blocked == false)
                hitcount += otherteam.MainPlayer.DefensiveInfo.HitCount;
            if (otherteam.TeamMate != null && otherteam.TeamMate.MoveType == MoveType.BeingHit &&
                otherteam.TeamMate.DefensiveInfo.Blocked == false)
                hitcount += otherteam.TeamMate.DefensiveInfo.HitCount;

            return hitcount + HitBonus;
        }

    }
}