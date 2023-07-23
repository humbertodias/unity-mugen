using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleSet")]
    public class AngleSet : StateController
    {
        private Expression m_angle;

        public AngleSet(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_angle = textsection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

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