using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleSet")]
    public class AngleSet : StateController
    {
        private Expression m_angle;

        public AngleSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_angle = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var angle = EvaluationHelper.AsSingle(character, m_angle, character.DrawingAngle);
            character.DrawingAngle = angle;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;
            if (m_angle == null)
                return false;
            return true;
        }

    }
}