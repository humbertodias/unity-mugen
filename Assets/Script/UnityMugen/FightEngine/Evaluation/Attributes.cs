using System;
using System.Diagnostics;

namespace UnityMugen.Evaluation
{

    public class TagAttribute : Attribute
    {
        public TagAttribute(string text)
        {
            m_text = text;
        }

        public override string ToString()
        {
            return m_text;
        }

        public string Value => m_text;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_text;

        #endregion
    }

    public class TokenMappingAttribute : TagAttribute
    {
        public TokenMappingAttribute(string text)
            : base(text)
        {
        }
    }

    public abstract class FunctionMappingAttribute : TokenMappingAttribute
    {
        protected FunctionMappingAttribute(string text, string name)
            : base(text)
        {
            m_name = name;
        }

        public string Name => m_name;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_name;

        #endregion
    }

    public class UnaryOperatorMappingAttribute : FunctionMappingAttribute
    {
        public UnaryOperatorMappingAttribute(string text, string name)
            : base(text, name)
        {
        }
    }

    public class BinaryOperatorMappingAttribute : FunctionMappingAttribute
    {
        public BinaryOperatorMappingAttribute(string text, string name, int precedence)
            : base(text, name)
        {
            m_precedence = precedence;
        }

        public int Precedence => m_precedence;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int m_precedence;

        #endregion
    }

    public class CustomFunctionAttribute : FunctionMappingAttribute
    {
        public CustomFunctionAttribute(string text)
            : base(text, text)
        {
        }
    }

    public class StateRedirectionAttribute : FunctionMappingAttribute
    {
        public StateRedirectionAttribute(string text)
            : base(text, text)
        {
        }
    }
}