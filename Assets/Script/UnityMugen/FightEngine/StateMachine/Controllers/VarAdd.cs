using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VarAdd")]
    public class VarAdd : StateController
    {
        public Expression IntNumber => m_intNumber;
        public Expression FloatNumber => m_floatNumber;
        public Expression SystemIntNumber => m_systemIntNumber;
        public Expression SystemFloatNumber => m_systemFloatNumber;
        public Expression Value => m_value;

        private static readonly Regex s_lineRegex;
        private readonly Expression m_index;

        private readonly Expression m_intNumber;
        private readonly Expression m_floatNumber;
        private readonly Expression m_systemIntNumber;
        private readonly Expression m_systemFloatNumber;
        private readonly Expression m_value;

        static VarAdd()
        {
            s_lineRegex = new Regex(@"(.*)?var\((.+)\)", RegexOptions.IgnoreCase);
        }

        public VarAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            base.Load();

            m_intNumber = textsection.GetAttribute<Expression>("v", null);
            m_floatNumber = textsection.GetAttribute<Expression>("fv", null);
            m_systemIntNumber = textsection.GetAttribute<Expression>("sysvar", null);
            m_systemFloatNumber = textsection.GetAttribute<Expression>("sysfvar", null);
            m_value = textsection.GetAttribute<Expression>("value", null);

            if (m_intNumber != null)
                m_index = m_intNumber;
            else if (m_floatNumber != null)
                m_index = m_floatNumber;
            else if (m_systemIntNumber != null)
                m_index = m_systemIntNumber;
            else if (m_systemFloatNumber != null)
                m_index = m_systemFloatNumber;


            foreach (var parsedline in textsection.ParsedLines)
            {
                var match = s_lineRegex.Match(parsedline.Key);
                if (match.Success == false) continue;

                var evalsystem = LauncherEngine.Inst.evaluationSystem;
                var sc = StringComparer.OrdinalIgnoreCase;
                var varType = match.Groups[1].Value;
                var varNumber = evalsystem.CreateExpression(match.Groups[2].Value);

                if (sc.Equals(varType, "")) m_intNumber = varNumber;
                if (sc.Equals(varType, "f")) m_floatNumber = varNumber;
                if (sc.Equals(varType, "sys")) m_systemIntNumber = varNumber;
                if (sc.Equals(varType, "sysf")) m_systemFloatNumber = varNumber;

                m_value = evalsystem.CreateExpression(parsedline.Value);
            }

        }

        public override void Run(Character character)
        {
            base.Load();

            if (IntNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, IntNumber, null);
                var value = EvaluationHelper.AsInt32(character, Value, null);

                if (index != null && value != null && character.Variables.AddInteger(index.Value, false, value.Value) == false)
                {
                }
            }

            if (FloatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, FloatNumber, null);
                var value = EvaluationHelper.AsSingle(character, Value, null);

                if (index != null && value != null && character.Variables.AddFloat(index.Value, false, value.Value) == false)
                {
                }
            }

            if (SystemIntNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, SystemIntNumber, null);
                var value = EvaluationHelper.AsInt32(character, Value, null);

                if (index != null && value != null && character.Variables.AddInteger(index.Value, true, value.Value) == false)
                {
                }
            }

            if (SystemFloatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, SystemFloatNumber, null);
                var value = EvaluationHelper.AsSingle(character, Value, null);

                if (index != null && value != null && character.Variables.AddFloat(index.Value, true, value.Value) == false)
                {
                }
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (Value == null)
                return false;

            var count = 0;
            if (IntNumber != null) ++count;
            if (FloatNumber != null) ++count;
            if (SystemIntNumber != null) ++count;
            if (SystemFloatNumber != null) ++count;
            if (count != 1)
                return false;

            return true;
        }
    }
}