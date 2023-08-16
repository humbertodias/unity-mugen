using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("SelfState")]
    public class SelfState : StateController
    {
        private Expression m_stateNumber;
        private Expression m_control;
        private Expression m_animationNumber;

        public SelfState(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_stateNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "ctrl":
                    m_control = GetAttribute<Expression>(expression, null);
                    break;
                case "anim":
                    m_animationNumber = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var statenumber = EvaluationHelper.AsInt32(character, m_stateNumber, null);
            if (statenumber == null)
            {
                Debug.Log("SelfState : value Required");
                return;
            }

            var playercontrol = EvaluationHelper.AsBoolean(character, m_control, null);
            var animationnumber = EvaluationHelper.AsInt32(character, m_animationNumber, null);

            if (character.StateManager.States.Contains(statenumber.Value))
                character.StateManager.ForeignManager = null;
            else
                character.StateManager.ForeignManager = character.DefensiveInfo.Attacker.StateManager;

            character.StateManager.ChangeState(statenumber.Value);

            if (playercontrol != null)
            {
                if (playercontrol == true) character.PlayerControl = PlayerControl.InControl;
                if (playercontrol == false) character.PlayerControl = PlayerControl.NoControl;
            }

            if (animationnumber != null) character.SetLocalAnimation(animationnumber.Value, 0);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_stateNumber == null)
                return false;

            return true;
        }
    }
}