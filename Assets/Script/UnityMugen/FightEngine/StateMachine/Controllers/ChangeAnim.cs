using UnityMugen.Combat;
using UnityMugen.Evaluation;
using Debug = UnityEngine.Debug;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ChangeAnim")]
    public class ChangeAnim : StateController
    {

        private Expression m_animationNumber;
        private Expression m_elementNumber;

        public ChangeAnim(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_animationNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "elem":
                    m_elementNumber = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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