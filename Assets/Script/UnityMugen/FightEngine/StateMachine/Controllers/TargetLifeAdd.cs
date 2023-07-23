using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetLifeAdd")]
    public class TargetLifeAdd : StateController
    {
        private Expression m_life;
        private Expression m_targetId;
        private Expression m_kill;
        private Expression m_abs;

        public TargetLifeAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_life = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_targetId = textSection.GetAttribute<Expression>("ID", null);
                m_kill = textSection.GetAttribute<Expression>("kill", null);
                m_abs = textSection.GetAttribute<Expression>("absolute", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var amount = EvaluationHelper.AsInt32(character, m_life, null);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, int.MinValue);
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