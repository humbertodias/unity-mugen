using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleAdd")]
    public class AngleAdd : StateController
    {
        private Expression m_angle;

        public AngleAdd(string label) : base(label) { }

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
            character.DrawingAngle += EvaluationHelper.AsSingle(character, m_angle, 0);
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