using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ParentVarAdd")]
    public class ParentVarAdd : VarAdd
    {
        public ParentVarAdd(string label) : base(label) { }

        public override void Run(Character character)
        {
            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null) return;

            if (m_intNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_intNumber, null);
                var value = EvaluationHelper.AsInt32(character, m_value, null);

                if (index != null && value != null && helper.Creator.Variables.AddInteger(index.Value, false, value.Value) == false)
                {
                }
            }

            if (m_floatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_floatNumber, null);
                var value = EvaluationHelper.AsSingle(character, m_value, null);

                if (index != null && value != null && helper.Creator.Variables.AddFloat(index.Value, false, value.Value) == false)
                {
                }
            }

            if (m_systemIntNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_systemIntNumber, null);
                var value = EvaluationHelper.AsInt32(character, m_value, null);

                if (index != null && value != null && helper.Creator.Variables.AddInteger(index.Value, true, value.Value) == false)
                {
                }
            }

            if (m_systemFloatNumber != null)
            {
                var index = EvaluationHelper.AsInt32(character, m_systemFloatNumber, null);
                var value = EvaluationHelper.AsSingle(character, m_value, null);

                if (index != null && value != null && helper.Creator.Variables.AddFloat(index.Value, true, value.Value) == false)
                {
                }
            }
        }
    }
}