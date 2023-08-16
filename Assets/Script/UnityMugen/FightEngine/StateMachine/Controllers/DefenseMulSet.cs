using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("DefenseMulSet", "DefenceMulSet")]
    public class DefenseMulSet : StateController
    {
        private Expression m_multiplier;

        public DefenseMulSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_multiplier = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var multiplier = EvaluationHelper.AsSingle(character, m_multiplier, null);

            if (multiplier == null)
            {
                Debug.Log("DefenseMulSet : value Required");
                return;
            }

            character.DefensiveInfo.DefenseMultiplier = multiplier.Value;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_multiplier == null)
                return false;

            return true;
        }

    }
}