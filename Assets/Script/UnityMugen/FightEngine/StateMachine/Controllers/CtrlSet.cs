using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("CtrlSet")]
    public class CtrlSet : StateController
    {
        private Expression m_control;

        public CtrlSet(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_control = textsection.GetAttribute<Expression>("value", null);
        }


        public override void Run(Character character)
        {
            base.Load();

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