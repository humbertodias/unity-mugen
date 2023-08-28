using System;
using System.Diagnostics;

namespace UnityMugen.Evaluation
{

    class TokenMappingAttribute : Attribute
    {
        public TokenMappingAttribute(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            m_text = text;
        }

        public string Text
        {
            get { return m_text; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly string m_text;

        #endregion
    }

    class FunctionMappingAttribute : TokenMappingAttribute
    {
        protected FunctionMappingAttribute(string text, Type type)
            : base(text)
        {
            if (type == null) throw new ArgumentNullException("type");

            m_type = type;
        }

        public Type Type
        {
            get { return m_type; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly Type m_type;

        #endregion
    }

    class UnaryOperatorMappingAttribute : FunctionMappingAttribute
    {
        public UnaryOperatorMappingAttribute(string text, Type type)
            : base(text, type)
        {
        }

    }

    class BinaryOperatorMappingAttribute : FunctionMappingAttribute
    {
        public BinaryOperatorMappingAttribute(string text, Type type, int precedence)
            : base(text, type)
        {
            if (precedence < 0) throw new ArgumentOutOfRangeException("precedence");

            m_precedence = precedence;
        }

        public int Precedence => m_precedence;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int m_precedence;

        #endregion
    }

    class CustomFunctionAttribute : Attribute
    {
        public CustomFunctionAttribute(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            m_text = text;
        }

        public String Text
        {
            get { return m_text; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly string m_text;

        #endregion
    }

    class StateRedirectionAttribute : Attribute
    {
        public StateRedirectionAttribute(string text)
        {
            if (text == null) throw new ArgumentNullException("text");

            m_text = text;
        }

        public string Text
        {
            get { return m_text; }
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly string m_text;

        #endregion
    }
}