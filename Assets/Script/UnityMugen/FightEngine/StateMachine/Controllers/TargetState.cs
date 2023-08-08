using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetState")]
    public class TargetState : StateController
    {
        private Expression m_stateNumber;
        private Expression m_targetId;

        public TargetState(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_stateNumber = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_targetId = textSection.GetAttribute<Expression>("ID", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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