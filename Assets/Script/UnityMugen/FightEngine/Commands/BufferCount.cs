using System;
using System.Diagnostics;

namespace UnityMugen.Commands
{

    public class BufferCount
    {
        public BufferCount()
        {
            m_value = 0;
            m_isactive = false;
        }

        public BufferCount(int value)
        {
            m_value = value;
            m_isactive = m_value > 0;
        }

        public void ResetFE()
        {
            m_value = 0;
            m_isactive = false;
        }

        public void Set(int time)
        {
            m_value = Math.Max(m_value, time);
            m_isactive = m_value > 0;
        }

        public void Tick()
        {
            m_value = Math.Max(0, m_value - 1);
            m_isactive = m_value > 0;
        }

        public override string ToString()
        {
            return m_value.ToString();
        }

        public int Value => m_value;
        public bool IsActive => m_isactive;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public int m_value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool m_isactive;

        #endregion
    }
}