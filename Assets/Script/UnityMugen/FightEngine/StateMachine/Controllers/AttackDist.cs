using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AttackDist")]
    public class AttackDist : StateController
    {
        private Expression m_distance;

        public AttackDist(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_distance = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

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