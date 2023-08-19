using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{
    [StateControllerName("VarRangeSet")]
    public class VarRangeSet : StateController
    {
        private Expression m_intNumber;
        private Expression m_floatNumber;
        private Expression m_startRange;
        private Expression m_endRange;

        public VarRangeSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_intNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "fvalue":
                    m_floatNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "first":
                    m_startRange = GetAttribute<Expression>(expression, null);
                    break;
                case "last":
                    m_endRange = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var start = EvaluationHelper.AsInt32(character, m_startRange, null);
            var end = EvaluationHelper.AsInt32(character, m_endRange, null);

            if (m_intNumber != null)
            {
                var value = EvaluationHelper.AsInt32(character, m_intNumber, null);
                if (value != null)
                {
                    for (var i = 0; i != character.Variables.IntegerVariables.Count; ++i)
                    {
                        if (i < start || i > end) continue;
                        character.Variables.SetInteger(i, false, value.Value);
                    }
                }
            }

            if (m_floatNumber != null)
            {
                var value = EvaluationHelper.AsSingle(character, m_floatNumber, null);
                if (value != null)
                {
                    for (var i = 0; i != character.Variables.FloatVariables.Count; ++i)
                    {
                        if (i < start || i > end) continue;
                        character.Variables.SetFloat(i, false, value.Value);
                    }
                }
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if ((m_intNumber != null ^ m_floatNumber != null) == false)
                return false;

            return true;
        }
    }
}