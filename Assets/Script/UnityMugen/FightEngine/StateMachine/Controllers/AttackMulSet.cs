using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AttackMulSet")]
    public class AttackMulSet : StateController
    {
        private Expression m_multiplier;

        public AttackMulSet(string label) : base(label) { }

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

            if (multiplier == null) return;

            character.OffensiveInfo.AttackMultiplier = multiplier.Value;
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