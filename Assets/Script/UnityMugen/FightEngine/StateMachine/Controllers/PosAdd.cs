using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PosAdd")]
    public class PosAdd : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public PosAdd(string label) : base(label) { }

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
            var x = EvaluationHelper.AsSingle(character, m_x, 0);
            var y = EvaluationHelper.AsSingle(character, m_y, 0);

            character.Move(new Vector2(x, y) * Constant.Scale);
        }
    }
}