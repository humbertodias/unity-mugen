using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("SprPriority")]
    public class SprPriority : StateController
    {
        private Expression m_priority;

        public SprPriority(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_priority = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            character.DrawOrder = Misc.Clamp(EvaluationHelper.AsInt32(character, m_priority, 0), -5, 5);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_priority == null)
                return false;

            return true;
        }
    }
}