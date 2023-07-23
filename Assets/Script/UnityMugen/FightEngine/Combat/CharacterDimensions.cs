using System;
using System.Diagnostics;

namespace UnityMugen.Combat
{
    public class CharacterDimensions
    {
        public CharacterDimensions(PlayerConstants constants)
        {
            if (constants == null) throw new ArgumentNullException(nameof(constants));

            m_groundfrontwidth = constants.GroundFront * Constant.Scale;
            m_groundbackwidth = constants.GroundBack * Constant.Scale;
            m_airfrontwidth = constants.Airfront * Constant.Scale;
            m_airbackwidth = constants.Airback * Constant.Scale;
            m_height = constants.Height * Constant.Scale;

            m_frontwidthoverride = 0;
            m_backwidthoverride = 0;
            m_frontedgewidthoverride = 0;
            m_backedgewidthoverride = 0;
        }

        public CharacterDimensions(float groundfront, float groundback, float airfront, float airback, float height)
        {
            m_groundfrontwidth = groundfront;
            m_groundbackwidth = groundback;
            m_airfrontwidth = airfront;
            m_airbackwidth = airback;
            m_height = height;

            m_frontwidthoverride = 0;
            m_backwidthoverride = 0;
            m_frontedgewidthoverride = 0;
            m_backedgewidthoverride = 0;
        }

        public void SetOverride(float frontwidth, float backwidth)
        {
            m_frontwidthoverride = frontwidth;
            m_backwidthoverride = backwidth;
        }

        public void SetEdgeOverride(float frontwidth, float backwidth)
        {
            m_frontedgewidthoverride = frontwidth;
            m_backedgewidthoverride = backwidth;
        }

        public void ClearOverride()
        {
            m_frontwidthoverride = 0;
            m_backwidthoverride = 0;
            m_frontedgewidthoverride = 0;
            m_backedgewidthoverride = 0;
        }

        public float GetFrontWidth(StateType statetype)
        {
            var width = m_frontwidthoverride;

            switch (statetype)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(statetype), "Statetype is not valid");

                case StateType.Liedown:
                case StateType.Standing:
                case StateType.Crouching:
                    width += m_groundfrontwidth;
                    break;

                case StateType.Airborne:
                    width += m_airfrontwidth;
                    break;
            }

            return width;
        }

        public float GetBackWidth(StateType statetype)
        {
            var width = m_backwidthoverride;

            switch (statetype)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(statetype), "Statetype is not valid");

                case StateType.Liedown:
                case StateType.Standing:
                case StateType.Crouching:
                    width += m_groundbackwidth;
                    break;

                case StateType.Airborne:
                    width += m_airbackwidth;
                    break;
            }

            return width;
        }

        public float Height => m_height;

        public float FrontEdgeWidth => m_frontedgewidthoverride;

        public float BackEdgeWidth => m_backedgewidthoverride;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly float m_groundfrontwidth;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly float m_groundbackwidth;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly float m_airfrontwidth;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly float m_airbackwidth;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly float m_height;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_frontwidthoverride;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_backwidthoverride;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_frontedgewidthoverride;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private float m_backedgewidthoverride;

        #endregion
    }
}