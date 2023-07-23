using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{
    [StateControllerName("VarRangeSet")]
    public class VarRangeSet : StateController
    {
        private Expression m_intNumber;
        private Expression m_floatNumber;
        private Expression m_startRange;
        private Expression m_endRange;

        public VarRangeSet(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_intNumber = textSection.GetAttribute<Expression>("value", null);
            m_floatNumber = textSection.GetAttribute<Expression>("fvalue", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_startRange = textSection.GetAttribute<Expression>("first", null);
                m_endRange = textSection.GetAttribute<Expression>("last", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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