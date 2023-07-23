using UnityMugen.Combat;
using Debug = UnityEngine.Debug;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ChangeAnim")]
    public class ChangeAnim : StateController
    {

        private Expression m_animationNumber;
        private Expression m_elementNumber;

        public ChangeAnim(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_animationNumber = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_elementNumber = textSection.GetAttribute<Expression>("elem", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var animationnumber = EvaluationHelper.AsInt32(character, m_animationNumber, null);
            var elementnumber = EvaluationHelper.AsInt32(character, m_elementNumber, 0);

            if (animationnumber == null)
            {
                Debug.Log("ChangeAnim : value Required");
                return;
            }

            --elementnumber;
            if (elementnumber < 0) elementnumber = 0;

            character.SetLocalAnimation(animationnumber.Value, elementnumber);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_animationNumber == null)
                return false;

            return true;
        }

    }
}