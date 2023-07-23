using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PlayerPush")]
    public class PlayerPush : StateController
    {
        private Expression m_value;

        public PlayerPush(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_value = textsection.GetAttribute<Expression>("value", null);
        }


        public override void Run(Character character)
        {
            base.Load();

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