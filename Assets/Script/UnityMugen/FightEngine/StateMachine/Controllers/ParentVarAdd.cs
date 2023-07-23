using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ParentVarAdd")]
    public class ParentVarAdd : StateController
    {
        private static readonly Regex s_lineRegex;
        private readonly Expression m_index;

        public Expression IntNumber => m_intNumber;
        public Expression FloatNumber => m_floatNumber;
        public Expression SystemIntNumber => m_systemIntNumber;
        public Expression SystemFloatNumber => m_systemFloatNumber;
        public Expression Value => m_value;

        private readonly Expression m_intNumber;
        private readonly Expression m_floatNumber;
        private readonly Expression m_systemIntNumber;
        private readonly Expression m_systemFloatNumber;
        private readonly Expression m_value;

        static ParentVarAdd()
        {
            s_lineRegex = new Regex(@"(.*)?var\((.+)\)", RegexOptions.IgnoreCase);
        }

        public ParentVarAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            base.Load();

            m_intNumber = textsection.GetAttribute<Expression>("v", null);
            m_floatNumber = textsection.GetAttribute<Expression>("fv", null);
            m_systemIntNumber = textsection.GetAttribute<Expression>("sysvar", null);
            m_systemFloatNumber = textsection.GetAttribute<Expression>("sysfvar", null);
            m_value = textsection.GetAttribute<Expression>("value", null);

        }

        public override void Run(Character character)
        {
            base.Load();

            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null) return;

            if (IntNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, IntNumber, null);
                var value = EvaluationHelper.AsInt32(character, Value, null);

                if (index != null && value != null && helper.Creator.Variables.AddInteger(index.Value, false, value.Value) == false)
                {
                }
            }

            if (FloatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, FloatNumber, null);
                var value = EvaluationHelper.AsSingle(character, Value, null);

                if (index != null && value != null && helper.Creator.Variables.AddFloat(index.Value, false, value.Value) == false)
                {
                }
            }

            if (SystemIntNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, SystemIntNumber, null);
                var value = EvaluationHelper.AsInt32(character, Value, null);

                if (index != null && value != null && helper.Creator.Variables.AddInteger(index.Value, true, value.Value) == false)
                {
                }
            }

            if (SystemFloatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, SystemFloatNumber, null);
                var value = EvaluationHelper.AsSingle(character, Value, null);

                if (index != null && value != null && helper.Creator.Variables.AddFloat(index.Value, true, value.Value) == false)
                {
                }
            }
        }
    }
}