using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("CtrlSet")]
    public class CtrlSet : StateController
    {
        private Expression m_control;

        public CtrlSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_control = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var control = EvaluationHelper.AsBoolean(character, m_control, null);
            if (control == null)
            {
                Debug.Log("CtrlSet : value Required");
                return;
            }

            if (control != null)
            {
                if (control == true) character.PlayerControl = PlayerControl.InControl;
                if (control == false) character.PlayerControl = PlayerControl.NoControl;
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_control == null)
                return false;

            return true;
        }

    }
}