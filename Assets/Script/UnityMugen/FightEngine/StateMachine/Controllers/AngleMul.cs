using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleMul")]
    public class AngleMul : StateController
    {
        private Expression m_angle;

        public AngleMul(string label) : base(label) { }

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
            character.DrawingAngle *= EvaluationHelper.AsSingle(character, m_angle, 1);
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