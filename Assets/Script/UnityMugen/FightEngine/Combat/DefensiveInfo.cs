using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityMugen.Collections;
using UnityMugen.StateMachine;

namespace UnityMugen.Combat
{

    public class DefensiveInfo
    {
        public DefensiveInfo(Character character)
        {
            if (character == null) throw new ArgumentNullException(nameof(character));

            m_character = character;
            m_character.FallTime = 0;
            m_hitdef = new HitDefinition();
            m_blocked = false;
            m_killed = false;
            m_hitstatetype = StateType.None;
            m_hitshaketime = 0;
            m_defensemultiplier = 1;
            m_attacker = null;
            m_hittime = 0;
            m_hitby1 = new HitBy();
            m_hitby2 = new HitBy();
            m_isfalling = false;

            m_hitoverrides = new List<HitOverride>();
            for (var i = 0; i != 8; ++i) 
                m_hitoverrides.Add(new HitOverride());

            m_hitcount = 0;
        }

        public DefensiveInfo(DefensiveInfo defensiveInfo)
        {
            m_character = defensiveInfo.Character;
            m_hitdef = new HitDefinition(defensiveInfo.HitDef);
            m_blocked = defensiveInfo.Blocked;
            m_killed = defensiveInfo.Killed;
            m_hitstatetype = defensiveInfo.HitStateType;
            m_hitshaketime = defensiveInfo.HitShakeTime;
            m_defensemultiplier = defensiveInfo.DefenseMultiplier;
            m_attacker = defensiveInfo.Attacker;
            m_hittime = defensiveInfo.HitTime;
            m_hitby1 = new HitBy(defensiveInfo.HitBy1);
            m_hitby2 = new HitBy(defensiveInfo.HitBy2);
            m_isfalling = defensiveInfo.IsFalling;

            m_hitoverrides = new List<HitOverride>(defensiveInfo.HitOverrides);
            
            m_hitcount = defensiveInfo.HitCount;
        }

        public void ResetFE()
        {
            m_hitdef.ResetFE();
            m_blocked = false;
            m_killed = false;
            m_hitstatetype = StateType.None;
            m_hitshaketime = 0;
            m_defensemultiplier = 1;
            m_attacker = null;
            m_hittime = 0;
            m_hitby1.ResetFE();
            m_hitby2.ResetFE();
            m_isfalling = false;
            m_character.FallTime = 0;

            for (var i = 0; i != 8; ++i) m_hitoverrides[i].ResetFE();

            m_hitcount = 0;
        }

        public void UpdateFE()
        {
            HitBy1.UpdateFE();
            HitBy2.UpdateFE();

            if (HitShakeTime > 0) --HitShakeTime;
            else if (HitTime > -1) --HitTime;
            if (IsFalling) m_character.FallTime++;

            if (HitShakeTime < 0) HitShakeTime = 0;
            if (HitTime < 0) HitTime = 0;
            if (!IsFalling) m_character.FallTime = 0;

            if (m_character.StateManager.CurrentState.number == StateNumber.HitGetUp && m_character.StateManager.StateTime == 0) HitDef.Fall = false;

            foreach (var hitoverride in m_hitoverrides) hitoverride.UpdateFE();

            if (HitUsed)
            {
                HitUsed = false;
                HitDef.HitDamage = 0;
                HitDef.GuardDamage = 0;
            }
        }

        public void OnHit(HitDefinition hitdef, Character attacker, bool blocked, bool revertal = false, bool isProjectile = false)
        {
            if (hitdef == null) throw new ArgumentNullException(nameof(hitdef));
            if (attacker == null) throw new ArgumentNullException(nameof(attacker));

            HitUsed = false;

            var alreadyfalling = IsFalling;

            HitDef.Set(hitdef);
            Attacker = attacker;
            Blocked = blocked;

            if (alreadyfalling)
            {
                HitDef.Fall = true;
            }
            else
            {
                Juggle = m_character.BasePlayer.playerConstants.AirJuggle;
            }

            HitCount = m_character.MoveType == MoveType.BeingHit ? HitCount + 1 : 1;
            HitStateType = m_character.StateType;

            m_character.DrawOrder = HitDef.P2SpritePriority;
            m_character.PlayerControl = PlayerControl.NoControl;

            if (!revertal)
                m_character.MoveType = MoveType.BeingHit;

            if (blocked)
            {
                HitShakeTime = HitDef.GuardShakeTime;
                m_character.BasePlayer.Power += HitDef.P2GuardPowerAdjustment;
            }
            else
            {
                HitShakeTime = HitDef.ShakeTime;
                m_character.BasePlayer.Power += HitDef.P2HitPowerAdjustment;

                m_character.PaletteFx.Set(HitDef.PalFxTime, HitDef.PalFxAdd, HitDef.PalFxMul, HitDef.PalFxSinAdd, HitDef.PalFxInvert, HitDef.PalFxBaseColor);
                if (IsFalling)
                {
                    if(isProjectile)
                        Juggle -= HitDef.JugglePointsNeeded;
                    else
                        Juggle -= Attacker.JugglePoints;
                    
                    Attacker.JugglePoints = 0;
                }
            }
        }


        public HitOverride GetOverride(HitDefinition hitdef)
        {
            if (hitdef == null) throw new ArgumentNullException(nameof(hitdef));

            foreach (var hitoverride in HitOverrides)
            {
                if (hitoverride.IsActive == false) continue;

                if (hitoverride.Attribute.HasHeight(hitdef.HitAttribute.AttackHeight) == false) continue;
                foreach (var hittype in hitdef.HitAttribute.AttackData)
                    if (hitoverride.Attribute.HasData(hittype) == false)
                        continue;

                return hitoverride;
            }

            return null;
        }

        public Vector2 GetHitVelocity()
        {
            Vector2 velocity;

            if (Blocked)
            {
                if (HitStateType == StateType.Airborne)
                    velocity = HitDef.AirGuardVelocity;
                else
                    velocity = HitDef.GroundGuardVelocity;
            }
            else
            {
                if (HitStateType == StateType.Airborne)
                    velocity = HitDef.AirVelocity;
                else if (HitStateType == StateType.Liedown)
                    velocity = HitDef.DownVelocity;
                else
                    velocity = HitDef.GroundVelocity;


                if (Killed)
                {
                    if (HitStateType == StateType.Airborne)
                    {
                        if (velocity.x < 0f)
                            velocity.x -= (Constant.VelAddKillX2 * Constant.Scale);

                        if (velocity.y <= 0f)
                        {
                            velocity.y -= (Constant.VelAddKillY2 * Constant.Scale);
                            if (velocity.y > Constant.VelAddKillY3 * Constant.Scale)
                                velocity.y = (Constant.VelAddKillY3 * Constant.Scale);
                        }
                    }
                    else
                    {
                        if (velocity.y == 0f)
                            velocity.x *= (Constant.VelAddKillX);

                        if (velocity.x < 0f)
                            velocity.x -= (Constant.VelAddKillX2 * Constant.Scale);

                        if (velocity.y <= 0f)
                        {
                            velocity.y -= (Constant.VelAddKillY2 * Constant.Scale);
                            if (velocity.y > Constant.VelAddKillY * Constant.Scale)
                                velocity.y = (Constant.VelAddKillY * Constant.Scale);
                        }
                    }
                }
            }

            return velocity;
        }

        public HitDefinition HitDef => m_hitdef;

        public bool Blocked
        {
            get { return m_blocked; }
            set { m_blocked = value; }
        }

        public bool Killed
        {
            get { return m_killed; }
            set { m_killed = value; }
        }

        public StateType HitStateType
        {
            get { return m_hitstatetype; }
            set { m_hitstatetype = value; }
        }

        public float HitShakeTime
        {
            get { return m_hitshaketime; }
            set { m_hitshaketime = value; }
        }

        public float DefenseMultiplier
        {
            get { return m_defensemultiplier; }
            set { m_defensemultiplier = value; }
        }

        public Character Attacker
        {
            get { return m_attacker; }
            set { m_attacker = value; }
        }

        public int HitTime
        {
            get { return m_hittime; }
            set { m_hittime = value; }
        }

        public bool HitUsed;
        public int Juggle;

        public HitBy HitBy1 => m_hitby1;

        public HitBy HitBy2 => m_hitby2;

        public bool IsFalling => m_character.MoveType == MoveType.BeingHit ? HitDef.Fall : false;

        public ListIterator<HitOverride> HitOverrides => new ListIterator<HitOverride>(m_hitoverrides);

        public int HitCount
        {
            get { return m_hitcount; }
            set { m_hitcount = value; }
        }

        public Character Character
        {
            get { return m_character; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Character m_character;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HitDefinition m_hitdef;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_blocked;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_killed;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private StateType m_hitstatetype;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_hitshaketime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_defensemultiplier;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Character m_attacker;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_hittime;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HitBy m_hitby1;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HitBy m_hitby2;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isfalling;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<HitOverride> m_hitoverrides;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int m_hitcount;

        #endregion
    }
}