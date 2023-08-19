using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AttackDist")]
    public class AttackDist : StateController
    {
        private Expression m_distance;

        public AttackDist(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_distance = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var distance = EvaluationHelper.AsInt32(character, m_distance, null);

            if (distance != null && character.OffensiveInfo.ActiveHitDef)
            {
                character.OffensiveInfo.HitDef.GuardDistance = distance.Value;
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_distance == null)
                return false;

            return true;
        }

    }
}