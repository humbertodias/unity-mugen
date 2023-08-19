using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("LifeAdd")]
    public class LifeAdd : StateController
    {

        private Expression m_life;
        private Expression m_canKill;
        private Expression m_abs;

        public LifeAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_life = GetAttribute<Expression>(expression, null);
                    break;
                case "kill":
                    m_canKill = GetAttribute<Expression>(expression, null);
                    break;
                case "absolute":
                    m_abs = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var amount = EvaluationHelper.AsInt32(character, m_life, null);
            if (amount == null)
            {
                Debug.Log("LifeAdd : value Required");
                return;
            }

            var cankill = EvaluationHelper.AsBoolean(character, m_canKill, true);
            var absolute = EvaluationHelper.AsBoolean(character, m_abs, false);

            var scaledamount = amount.Value;
            if (absolute == false)
                scaledamount = (int)(scaledamount / character.DefensiveInfo.DefenseMultiplier);

            character.Life += scaledamount;

            if (cankill == false && character.Life == 0)
                character.Life = 1;
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