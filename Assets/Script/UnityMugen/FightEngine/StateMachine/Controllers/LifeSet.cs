using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("LifeSet")]
    public class LifeSet : StateController
    {
        private Expression m_life;

        public LifeSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_life = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var amount = EvaluationHelper.AsInt32(character, m_life, null);
            if (amount == null)
            {
                Debug.Log("LifeSet : value Required");
                return;
            }

            character.Life = amount.Value;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_life == null)
                return false;

            return true;
        }

    }
}