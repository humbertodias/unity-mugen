using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetState")]
    public class TargetState : StateController
    {
        private Expression m_stateNumber;
        private Expression m_targetId;

        public TargetState(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_stateNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var statenumber = EvaluationHelper.AsInt32(character, m_stateNumber, null);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);

            if (statenumber == null) return;

            foreach (var target in character.GetTargets(targetId))
            {
                target.StateManager.ForeignManager = character.StateManager;
                target.StateManager.ChangeState(statenumber.Value);
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_stateNumber == null)
                return false;

            return true;
        }
    }
}