using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ChangeAnim2")]
    public class ChangeAnim2 : StateController
    {
        private Expression m_animationNumber;
        private Expression m_elementNumber;

        public ChangeAnim2(string label) : base(label) { }

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
                Debug.Log("ChangeAnim2 : value Required");
                return;
            }

            if (character.StateManager.ForeignManager == null) return;

            --elementnumber;
            if (elementnumber < 0) elementnumber = 0;

            character.SetForeignAnimation(character.StateManager.ForeignManager.Character.AnimationManager, animationnumber.Value, elementnumber);
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