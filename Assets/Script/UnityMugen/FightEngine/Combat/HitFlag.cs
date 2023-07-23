using System.Diagnostics;

namespace UnityMugen.Combat
{

    public class HitFlag
    {
        static HitFlag()
        {
            s_default = new HitFlag(HitFlagCombo.DontCare, true, true, true, true, false);
            s_noGuard = new HitFlag(HitFlagCombo.No, false, false, false, false, false);
        }

        public HitFlag(HitFlagCombo combo, bool high, bool low, bool air, bool falling, bool down)
        {
            m_high = high;
            m_low = low;
            m_air = air;
            m_falling = falling;
            m_down = down;
            m_combo = combo;
        }

        public static HitFlag Default => s_default;
        public static HitFlag NoGuard => s_noGuard;

        public bool HitHigh => m_high;

        public bool HitLow => m_low;

        public bool HitFalling => m_falling;

        public bool HitAir => m_air;

        public bool HitDown => m_down;

        public HitFlagCombo ComboFlag => m_combo;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly HitFlag s_default;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly HitFlag s_noGuard;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool m_high;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool m_low;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool m_air;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool m_falling;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly bool m_down;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly HitFlagCombo m_combo;

        #endregion
    }
}