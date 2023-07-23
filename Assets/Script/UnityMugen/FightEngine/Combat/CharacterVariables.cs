using System.Collections.Generic;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.Combat
{

    public class CharacterVariables
    {
        public CharacterVariables()
        {
            m_int = new List<int>(60);
            m_sysint = new List<int>(5);
            m_float = new List<float>(40);
            m_sysfloat = new List<float>(5);

            for (var i = 0; i != m_int.Capacity; ++i) m_int.Add(0);
            for (var i = 0; i != m_sysint.Capacity; ++i) m_sysint.Add(0);
            for (var i = 0; i != m_float.Capacity; ++i) m_float.Add(0);
            for (var i = 0; i != m_sysfloat.Capacity; ++i) m_sysfloat.Add(0);
        }

        public void ResetFE()
        {
            for (var i = 0; i != m_int.Count; ++i) m_int[i] = 0;
            for (var i = 0; i != m_sysint.Count; ++i) m_sysint[i] = 0;
            for (var i = 0; i != m_float.Count; ++i) m_float[i] = 0;
            for (var i = 0; i != m_sysfloat.Count; ++i) m_sysfloat[i] = 0;
        }

        public bool GetInteger(int index, bool system, out int value)
        {
            var variables = system ? m_sysint : m_int;

            if (index < 0 || index > variables.Count)
            {
                value = int.MinValue;
                return false;
            }

            value = variables[index];
            return true;
        }

        public bool SetInteger(int index, bool system, int value)
        {
            var variables = system ? m_sysint : m_int;

            if (index < 0 || index > variables.Count)
            {
                return false;
            }

            variables[index] = value;
            return true;
        }

        public bool AddInteger(int index, bool system, int value)
        {
            var variables = system ? m_sysint : m_int;

            if (index < 0 || index > variables.Count)
            {
                return false;
            }

            variables[index] += value;
            return true;
        }

        public bool GetFloat(int index, bool system, out float value)
        {
            var variables = system ? m_sysfloat : m_float;

            if (index < 0 || index > variables.Count)
            {
                value = float.MinValue;
                return false;
            }

            value = variables[index];
            return true;
        }

        public bool SetFloat(int index, bool system, float value)
        {
            var variables = system ? m_sysfloat : m_float;

            if (index < 0 || index > variables.Count)
            {
                return false;
            }

            variables[index] = value;
            return true;
        }

        public bool AddFloat(int index, bool system, float value)
        {
            var variables = system ? m_sysfloat : m_float;

            if (index < 0 || index > variables.Count)
            {
                return false;
            }

            variables[index] += value;
            return true;
        }

        public ListIterator<int> IntegerVariables => new ListIterator<int>(m_int);

        public ListIterator<int> SystemIntegerVariables => new ListIterator<int>(m_sysint);

        public ListIterator<float> FloatVariables => new ListIterator<float>(m_float);

        public ListIterator<float> SystemFloatVariables => new ListIterator<float>(m_sysfloat);

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<int> m_int;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<int> m_sysint;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<float> m_float;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<float> m_sysfloat;

        #endregion
    }
}