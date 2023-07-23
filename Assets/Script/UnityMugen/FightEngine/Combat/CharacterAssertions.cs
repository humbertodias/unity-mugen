using System.Diagnostics;

namespace UnityMugen.Combat
{

    public class CharacterAssertions
    {
        public CharacterAssertions()
        {
            ResetFE();
        }

        public void ResetFE()
        {
            m_invisible = false;
            m_nostandingguard = false;
            m_nocrouchingguard = false;
            m_noairguard = false;
            m_nowalk = false;
            m_noautoturn = false;
            m_nojugglecheck = false;
            m_noshadow = false;
            m_unguardable = false;
            m_noko = false;
        }

        public bool Invisible
        {
            get { return m_invisible; }
            set { m_invisible = value; }
        }

        public bool NoStandingGuard
        {
            get { return m_nostandingguard; }
            set { m_nostandingguard = value; }
        }

        public bool NoCrouchingGuard
        {
            get { return m_nocrouchingguard; }
            set { m_nocrouchingguard = value; }
        }

        public bool NoAirGuard
        {
            get { return m_noairguard; }
            set { m_noairguard = value; }
        }

        public bool NoWalk
        {
            get { return m_nowalk; }
            set { m_nowalk = value; }
        }

        public bool NoAutoTurn
        {
            get { return m_noautoturn; }
            set { m_noautoturn = value; }
        }

        public bool NoJuggleCheck
        {
            get { return m_nojugglecheck; }
            set { m_nojugglecheck = value; }
        }

        public bool NoShadow
        {
            get { return m_noshadow; }
            set { m_noshadow = value; }
        }

        public bool UnGuardable
        {
            get { return m_unguardable; }
            set { m_unguardable = value; }
        }

        public bool NoKO
        {
            get { return m_noko; }
            set { m_noko = value; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_invisible;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nostandingguard;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nocrouchingguard;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_noairguard;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nowalk;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_noautoturn;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_nojugglecheck;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_noshadow;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_unguardable;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_noko;

        #endregion
    }
}