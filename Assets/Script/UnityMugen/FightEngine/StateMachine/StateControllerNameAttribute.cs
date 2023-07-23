using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityMugen.Collections;

namespace UnityMugen.StateMachine
{

    [AttributeUsage(AttributeTargets.Class)]
    public class StateControllerNameAttribute : Attribute
    {
        private readonly List<string> m_names;
        public ListIterator<string> Names => new ListIterator<string>(m_names);

        public StateControllerNameAttribute(params string[] names)
        {
            if (names == null) throw new ArgumentNullException(nameof(names));
            if (names.Length == 0) throw new ArgumentException("names");

            m_names = new List<string>(names);
        }
    }
}