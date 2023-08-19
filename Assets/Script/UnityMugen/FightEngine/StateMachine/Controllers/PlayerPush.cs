using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PlayerPush")]
    public class PlayerPush : StateController
    {
        private Expression m_value;

        public PlayerPush(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_value = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var value = EvaluationHelper.AsBoolean(character, m_value, null);
            if (value == null)
            {
                Debug.Log("PlayerPush : value Required");
                return;
            }

            character.PushFlag = value.Value;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_value == null)
                return false;

            return true;
        }
    }
}