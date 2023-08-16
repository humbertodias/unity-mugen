using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleDraw")]
    public class AngleDraw : StateController
    {
        private Expression m_angle;
        private Expression m_scale;

        public AngleDraw(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_angle = GetAttribute<Expression>(expression, null);
                    break;
                case "scale":
                    m_scale = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var angle = EvaluationHelper.AsSingle(character, m_angle, character.DrawingAngle);

            character.DrawingAngle = angle;
            character.AngleDraw = true;

            var scale = EvaluationHelper.AsVector2(character, m_scale, null, 1);
            if (scale != null) character.DrawScale = scale.Value;
        }

    }
}