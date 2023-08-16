using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PowerAdd")]
    public class PowerAdd : StateController
    {
        private Expression m_power;

        public PowerAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_power = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var power = EvaluationHelper.AsInt32(character, m_power, null);
            if (power == null)
            {
                Debug.Log("PowerAdd : value Required");
                return;
            }

            character.BasePlayer.Power += power.Value;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_power == null)
                return false;

            return true;
        }
    }
}