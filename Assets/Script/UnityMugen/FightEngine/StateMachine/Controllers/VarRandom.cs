using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VarRandom")]
    public class VarRandom : StateController
    {
        private Expression m_intNumber;
        private Expression m_range;

        public VarRandom(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_intNumber = textSection.GetAttribute<Expression>("v", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_range = textSection.GetAttribute<Expression>("range", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var varindex = EvaluationHelper.AsInt32(character, m_intNumber, null);
            if (varindex == null) return;

            int min;
            int max;
            if (GetRange(character, out min, out max) == false) return;

            if (min > max) Misc.Swap(ref min, ref max);

            var randomvalue = LauncherEngine.Inst.random.NewInt(min, max);

            if (character.Variables.SetInteger(varindex.Value, false, randomvalue) == false)
            {
            }
        }

        private bool GetRange(Character character, out int min, out int max)
        {
            if (m_range == null)
            {
                min = 0;
                max = 1000;
                return true;
            }

            var result = m_range.Evaluate(character);

            if (result.Length > 0 && result[0].NumberType != NumberType.None)
            {
                if (result.Length > 1 && result[1].NumberType != NumberType.None)
                {
                    min = result[0].IntValue;
                    max = result[1].IntValue;
                    return true;
                }

                min = 0;
                max = result[0].IntValue;
                return true;
            }

            min = 0;
            max = 1;
            return false;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_intNumber == null)
                return false;

            return true;
        }
    }
}