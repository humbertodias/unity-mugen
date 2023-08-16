using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Offset")]
    public class Offset : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public Offset(string label) : base(label) { }

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
            var x = EvaluationHelper.AsSingle(character, m_x, null) * Constant.Scale;
            var y = EvaluationHelper.AsSingle(character, m_y, null) * Constant.Scale;

            var offset = character.DrawOffset;

            if (x != null) offset.x = x.Value;
            if (y != null) offset.y = y.Value;

            character.DrawOffset = offset;
        }
    }
}