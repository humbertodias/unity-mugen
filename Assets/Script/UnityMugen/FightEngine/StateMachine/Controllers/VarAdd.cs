using System;
using System.Text.RegularExpressions;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VarAdd")]
    public class VarAdd : StateController
    {
        private static readonly Regex s_lineRegex;

        protected Expression m_intNumber;
        protected Expression m_floatNumber;
        protected Expression m_systemIntNumber;
        protected Expression m_systemFloatNumber;
        protected Expression m_value;

        static VarAdd()
        {
            s_lineRegex = new Regex(@"(.*)?var\((.+)\)", RegexOptions.IgnoreCase);
        }

        public VarAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "v":
                    m_intNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "fv":
                    m_floatNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "sysvar":
                    m_systemIntNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "sysfvar":
                    m_systemFloatNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "value":
                    m_value = GetAttribute<Expression>(expression, null);
                    break;
                default:
                    {
                        var match = s_lineRegex.Match(idAttribute);
                        if (match.Success == false) return;

                        var evalsystem = LauncherEngine.Inst.evaluationSystem;
                        var sc = StringComparer.OrdinalIgnoreCase;
                        var varType = match.Groups[1].Value;
                        var varNumber = evalsystem.CreateExpression(match.Groups[2].Value);

                        if (sc.Equals(varType, "")) m_intNumber = varNumber;
                        if (sc.Equals(varType, "f")) m_floatNumber = varNumber;
                        if (sc.Equals(varType, "sys")) m_systemIntNumber = varNumber;
                        if (sc.Equals(varType, "sysf")) m_systemFloatNumber = varNumber;

                        m_value = evalsystem.CreateExpression(expression);
                        break;
                    }
            }
        }

        public override void Run(Character character)
        {
            if (m_intNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_intNumber, null);
                var value = EvaluationHelper.AsInt32(character, m_value, null);

                if (index != null && value != null && character.Variables.AddInteger(index.Value, false, value.Value) == false)
                {
                }
            }

            if (m_floatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_floatNumber, null);
                var value = EvaluationHelper.AsSingle(character, m_value, null);

                if (index != null && value != null && character.Variables.AddFloat(index.Value, false, value.Value) == false)
                {
                }
            }

            if (m_systemIntNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_systemIntNumber, null);
                var value = EvaluationHelper.AsInt32(character, m_value, null);

                if (index != null && value != null && character.Variables.AddInteger(index.Value, true, value.Value) == false)
                {
                }
            }

            if (m_systemFloatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_systemFloatNumber, null);
                var value = EvaluationHelper.AsSingle(character, m_value, null);

                if (index != null && value != null && character.Variables.AddFloat(index.Value, true, value.Value) == false)
                {
                }
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_value == null)
                return false;

            var count = 0;
            if (m_intNumber != null) ++count;
            if (m_floatNumber != null) ++count;
            if (m_systemIntNumber != null) ++count;
            if (m_systemFloatNumber != null) ++count;
            if (count != 1)
                return false;

            return true;
        }
    }
}