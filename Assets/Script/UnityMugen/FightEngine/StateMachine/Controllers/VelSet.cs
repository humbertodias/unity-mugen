using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VelSet")]
    public class VelSet : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public VelSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
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
            float? x = EvaluationHelper.AsSingle(character, m_x, null) * Constant.Scale;
            float? y = EvaluationHelper.AsSingle(character, m_y, null) * Constant.Scale;

            x = x ?? character.CurrentVelocity.x;
            y = y ?? character.CurrentVelocity.y;

            character.CurrentVelocity = new Vector2(x.Value, y.Value);
        }
    }
}