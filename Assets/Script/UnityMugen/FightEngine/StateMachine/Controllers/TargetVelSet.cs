using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetVelSet")]
    public class TargetVelSet : StateController
    {
        private Expression m_targetId;
        private Expression m_x;
        private Expression m_y;

        public TargetVelSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
                case "x":
                    m_x = GetAttribute<Expression>(expression, null);
                    break;
                case "y":
                    m_y = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);
            var x = EvaluationHelper.AsSingle(character, m_x, null);
            var y = EvaluationHelper.AsSingle(character, m_y, null);

            foreach (var target in character.GetTargets(targetId))
            {
                var velocity = target.CurrentVelocity;

                if (x != null) velocity.x = x.Value;
                if (y != null) velocity.y = y.Value;

                target.CurrentVelocity = velocity;
            }
        }
    }
}