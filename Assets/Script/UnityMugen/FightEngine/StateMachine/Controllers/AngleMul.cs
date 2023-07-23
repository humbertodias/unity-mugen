using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleMul")]
    public class AngleMul : StateController
    {
        private Expression m_angle;

        public AngleMul(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_angle = textsection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

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