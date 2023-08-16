using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetLifeAdd")]
    public class TargetLifeAdd : StateController
    {
        private Expression m_life;
        private Expression m_targetId;
        private Expression m_kill;
        private Expression m_abs;

        public TargetLifeAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_life = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_targetId = GetAttribute<Expression>(expression, null);
                    break;
                case "kill":
                    m_kill = GetAttribute<Expression>(expression, null);
                    break;
                case "absolute":
                    m_abs = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var amount = EvaluationHelper.AsInt32(character, m_life, null);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);
            var cankill = EvaluationHelper.AsBoolean(character, m_kill, true);
            var absolute = EvaluationHelper.AsBoolean(character, m_abs, false);

            if (amount == null) return;

            foreach (var target in character.GetTargets(targetId))
            {
                var newamount = amount.Value;

                if (absolute == false && newamount < 0)
                {
                    newamount = (int)(newamount * character.OffensiveInfo.AttackMultiplier);
                    newamount = (int)(newamount / target.DefensiveInfo.DefenseMultiplier);
                }

                target.Life += newamount;
                if (target.Life == 0 && cankill == false) target.Life = 1;
            }
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