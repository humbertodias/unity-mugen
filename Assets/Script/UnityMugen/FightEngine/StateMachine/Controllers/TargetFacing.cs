using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetFacing")]
    public class TargetFacing : StateController
    {
        private Expression m_facing;
        private Expression m_targetId;

        public TargetFacing(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_facing = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var facing = EvaluationHelper.AsInt32(character, m_facing, 0);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);

            foreach (var target in character.GetTargets(targetId))
            {
                if (facing > 0)
                {
                    target.CurrentFacing = character.CurrentFacing;
                }
                else if (facing < 0)
                {
                    target.CurrentFacing = Misc.FlipFacing(character.CurrentFacing);
                }
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_facing == null)
                return false;

            return true;
        }
    }
}