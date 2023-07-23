using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("SprPriority")]
    public class SprPriority : StateController
    {
        private Expression m_priority;

        public SprPriority(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_priority = textsection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            character.DrawOrder = Misc.Clamp(EvaluationHelper.AsInt32(character, m_priority, 0), -5, 5); ;
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