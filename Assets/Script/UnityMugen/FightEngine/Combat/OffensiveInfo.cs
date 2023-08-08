using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UnityMugen.Combat
{

    public class OffensiveInfo
    {
        public OffensiveInfo(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            m_character = character;
            m_hitdef = new HitDefinition();
            m_hitpausetime = 0;
            m_isactive = false;
            m_movecontact = 0;
            m_moveguarded = 0;
            m_movehit = 0;
            m_movereversed = 0;
            m_attackmultiplier = 1;
            m_hitcount = 0;
            m_uniquehitcount = 0;
            m_projectileinfo = new ProjectileInfo();
            m_targetlist = new List<Character>();
        }

        public OffensiveInfo(OffensiveInfo offensiveInfo)
        {
            m_character = offensiveInfo.m_character;
            m_hitdef = offensiveInfo.m_hitdef;
            m_hitpausetime = offensiveInfo.HitPauseTime;
            m_isactive = offensiveInfo.ActiveHitDef;
            m_movecontact = offensiveInfo.MoveContact;
            m_moveguarded = offensiveInfo.MoveGuarded;
            m_movehit = offensiveInfo.MoveHit;
            m_movereversed = offensiveInfo.MoveReversed;
            m_attackmultiplier = offensiveInfo.AttackMultiplier;
            m_hitcount = offensiveInfo.HitCount;
            m_uniquehitcount = offensiveInfo.UniqueHitCount;
            m_projectileinfo = new ProjectileInfo(offensiveInfo.ProjectileInfo);
            m_targetlist = new List<Character>(offensiveInfo.m_targetlist);
        }

        public void ResetFE()
        {
            m_hitdef.ResetFE();
            m_hitpausetime = 0;
            m_isactive = false;
            m_movecontact = 0;
            m_moveguarded = 0;
            m_movehit = 0;
            m_movereversed = 0;
            m_attackmultiplier = 1;
            m_hitcount = 0;
            m_uniquehitcount = 0;
            m_projectileinfo.ResetFE();
            m_targetlist.Clear();
        }

        public void UpdateFE()
        {
            ProjectileInfo.UpdateFE();

            if (MoveContact > 0) ++MoveContact;
            if (MoveHit > 0) ++MoveHit;
            if (MoveGuarded > 0) ++MoveGuarded;
            if (MoveReversed > 0) ++MoveReversed;
        }

        public void OnHit(HitDefinition hitdef, Character target, bool blocked)
        {
            if (hitdef == null) throw new ArgumentNullException(nameof(hitdef));
            if (target == null) throw new ArgumentNullException(nameof(target));

            m_character.DrawOrder = hitdef.P1SpritePriority;

            AddToTargetList(target);

            if (blocked)
            {
                m_character.BasePlayer.Power += hitdef.P1GuardPowerAdjustment;

                int MugenVersion = (int)m_character.BasePlayer.profile.mugenVersion;// IK
                HitPauseTime = hitdef.GuardPauseTime + (MugenVersion == 1 || MugenVersion == 2 ? 1 : 0);// IK

                MoveContact = 1;
                MoveGuarded = 1;
                MoveHit = 0;
                MoveReversed = 0;
            }
            else
            {
                P1NewState = hitdef.P1NewState;
                P2NewState = hitdef.P2NewState;

                if (MoveReversed > 0)
                {
                    target.BasePlayer.Power -= target.OffensiveInfo.HitDef.P1HitPowerAdjustment;
                    m_character.BasePlayer.Power += target.OffensiveInfo.HitDef.P1HitPowerAdjustment;

                    target.BasePlayer.Score -= Misc.AddScore(target.OffensiveInfo.HitDef);
                    m_character.BasePlayer.Score += Misc.AddScore(target.OffensiveInfo.HitDef);

                    target.OffensiveInfo.ResetFE();
                }

                m_character.BasePlayer.Power += hitdef.P1HitPowerAdjustment;
                m_character.BasePlayer.Score += Misc.AddScore(hitdef); // Tiago

                //if (/*m_character.Assertions.hit*/// IK acho que isso nao e necessario.
                //    m_character.StateType == StateType.Airborne && hitdef.AirAnimationType != HitAnimationType.None ||
                //    m_character.StateType != StateType.Airborne && hitdef.GroundAnimationType != HitAnimationType.None) {
                //    HitPauseTime += 1;
                //}
                int MugenVersion = (int)m_character.BasePlayer.profile.mugenVersion;// IK
                HitPauseTime = hitdef.PauseTime + (MugenVersion == 1 || MugenVersion == 2 ? 1 : 0);// IK

                MoveContact = 1;
                MoveGuarded = 0;
                MoveHit = 1;
                MoveReversed = 0;

                

            }
        }



        public void AddToTargetList(Character target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (target == m_character) throw new ArgumentOutOfRangeException(nameof(target));

            if (TargetList.Contains(target) == false) TargetList.Add(target);
        }

        public HitDefinition HitDef => m_hitdef;

        public int P1NewState { get; set; }
        public int P2NewState { get; set; }

        public bool ActiveHitDef
        {
            get { return m_isactive; }
            set { m_isactive = value; }
        }

        public int HitPauseTime
        {
            get { return m_hitpausetime; }
            set { m_hitpausetime = value; }
        }

        public int MoveContact
        {
            get { return m_movecontact; }
            set { m_movecontact = value; }
        }

        public int MoveHit
        {
            get { return m_movehit; }
            set { m_movehit = value; }
        }

        public int MoveGuarded
        {
            get { return m_moveguarded; }
            set { m_moveguarded = value; }
        }

        public int MoveReversed
        {
            get { return m_movereversed; }
            set { m_movereversed = value; }
        }

        public float AttackMultiplier
        {
            get { return m_attackmultiplier; }
            set { m_attackmultiplier = value; }
        }

        public int HitCount
        {
            get { return m_hitcount; }
            set { m_hitcount = value; }
        }

        public int UniqueHitCount
        {
            get { return m_uniquehitcount; }
            set { m_uniquehitcount = value; }
        }

        public ProjectileInfo ProjectileInfo => m_projectileinfo;

        public List<Character> TargetList => m_targetlist;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Character m_character;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HitDefinition m_hitdef;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isactive;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_hitpausetime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_movehit;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_moveguarded;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_movecontact;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_movereversed;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_attackmultiplier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_hitcount;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_uniquehitcount;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ProjectileInfo m_projectileinfo;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<Character> m_targetlist;

        #endregion
    }
}