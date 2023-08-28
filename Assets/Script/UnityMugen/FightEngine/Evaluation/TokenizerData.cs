using System;
using System.Diagnostics;
using System.Globalization;

namespace UnityMugen.Evaluation
{

    public abstract class TokenData
    {
        protected TokenData(Converter<string, bool> matcher)
        {
            if (matcher == null) throw new ArgumentNullException(nameof(matcher));

            m_matcher = matcher;
        }

        public bool Match(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            return m_matcher(input);
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Converter<string, bool> m_matcher;

        #endregion
    }

    public abstract class LiteralData : TokenData
    {
        protected LiteralData(string text)
            : base(x => IsTextMatch(x, text))
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            m_text = text;
        }

        private static bool IsTextMatch(string lhs, string rhs)
        {
            if (lhs == null) throw new ArgumentNullException(nameof(lhs));
            if (rhs == null) throw new ArgumentNullException(nameof(rhs));

            return string.Equals(lhs, rhs, StringComparison.OrdinalIgnoreCase);
        }

        public string Text => m_text;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_text;

        #endregion
    }

    public class UnknownData : TokenData
    {
        public UnknownData()
            : base(x => true)
        {
        }
    }

    public class TextData : TokenData
    {
        public TextData()
            : base(DoMatch)
        {
        }

        private static bool DoMatch(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            if (input.Length < 1) return false;

            return input[0] == '"' && input[input.Length - 1] == '"';
        }
    }

    public class SymbolData : LiteralData
    {
        public SymbolData(Symbol symbol, string text)
            : base(text)
        {
            if (symbol == Symbol.None) throw new ArgumentOutOfRangeException(nameof(symbol));

            m_symbol = symbol;
        }

        public Symbol Symbol => m_symbol;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Symbol m_symbol;

        #endregion
    }

    public abstract class NodeData : LiteralData
    {
        protected NodeData(string text, string functionname)
            : base(text)
        {
            if (functionname == null) throw new ArgumentNullException(nameof(functionname));

            m_name = functionname;
        }

        public string Name => m_name;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string m_name;

        #endregion
    }

    public abstract class NumberData : TokenData
    {
        protected NumberData(Converter<string, bool> matcher)
            : base(matcher)
        {
        }

        public abstract Number GetNumber(string text);
    }

    public class IntData : NumberData
    {
        public IntData()
            : base(IsIntNumber)
        {
        }

        public override Number GetNumber(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            int number;
            if (int.TryParse(text, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out number)) return new Number(number);

            return new Number();
        }

        private static bool IsIntNumber(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            int number;
            return int.TryParse(input, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out number);
        }
    }

    public class FloatData : NumberData
    {
        public FloatData()
            : base(IsFloatNumber)
        {
        }

        public override Number GetNumber(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            float number;
            if (float.TryParse(text, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out number)) return new Number(number);

            return new Number();
        }

        private static bool IsFloatNumber(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));

            float number;
            return float.TryParse(input, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out number);
        }
    }

    public abstract class OperatorData : NodeData
    {
        protected OperatorData(Operator @operator, string text, string functionname)
            : base(text, functionname)
        {
            if (@operator == Operator.None) throw new ArgumentOutOfRangeException("@operator");

            m_operator = @operator;
        }

        public Operator Operator => m_operator;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Operator m_operator;

        #endregion
    }

    public class UnaryOperatorData : OperatorData
    {
        public UnaryOperatorData(Operator @operator, string text, string functionname)
            : base(@operator, text, functionname)
        {
        }
    }

    public class BinaryOperatorData : OperatorData
    {
        public BinaryOperatorData(Operator @operator, string text, int precedence, string functionname)
            : base(@operator, text, functionname)
        {
            if (precedence < 0) throw new ArgumentOutOfRangeException(nameof(precedence));

            m_precedence = precedence;
        }

        public int Precedence => m_precedence;

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int m_precedence;

        #endregion
    }

    public class CustomFunctionData : NodeData
    {
        public CustomFunctionData(string text, string functionname)
            : base(text, functionname)
        {
            var methodinfo = Type.GetType(functionname).GetMethod("Parse");

            m_function = (NodeParse)Delegate.CreateDelegate(typeof(NodeParse), methodinfo);
        }

        public Node Parse(ParseState state)
        {
            if (state == null) throw new ArgumentNullException("state");

            return m_function(state);
        }

        #region Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly NodeParse m_function;

        #endregion
    }

    public class StateRedirectionData : CustomFunctionData
    {
        public StateRedirectionData(string text, string name)
            : base(text, name)
        {
        }
    }

    public class RangeData : TokenData
    {
        public RangeData()
            : base(x => false)
        {
        }
    }
}