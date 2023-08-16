using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetPowerAdd")]
    public class TargetPowerAdd : StateController
    {
        private Expression m_power;
        private Expression m_targetId;

        public TargetPowerAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_power = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var amount = EvaluationHelper.AsInt32(character, m_power, null);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);

            if (amount == null)
            {
                Debug.Log("TargetPowerAdd : value Required");
                return;
            }

            foreach (var target in character.GetTargets(targetId))
            {
                target.BasePlayer.Power += amount.Value;
            }
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