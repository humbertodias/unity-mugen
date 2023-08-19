using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("VelMul")]
    public class VelMul : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public VelMul(string label) : base(label) { }

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
            var x = EvaluationHelper.AsSingle(character, m_x, 1);
            var y = EvaluationHelper.AsSingle(character, m_y, 1);

            character.CurrentVelocity *= new Vector2(x, y);
        }
    }
}