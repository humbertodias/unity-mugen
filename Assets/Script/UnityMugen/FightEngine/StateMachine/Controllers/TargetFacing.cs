using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetFacing")]
    public class TargetFacing : StateController
    {
        private Expression m_facing;
        private Expression m_targetId;

        public TargetFacing(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_facing = textsection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_targetId = textSection.GetAttribute<Expression>("ID", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var facing = EvaluationHelper.AsInt32(character, m_facing, 0);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, int.MinValue);

            foreach (var target in character.GetTargets(targetId))
            {
                if (facing > 0)
                {
                    target.CurrentFacing = character.CurrentFacing;
                }
                else if (facing < 0)
                {
                    target.CurrentFacing = Misc.FlipFacing(character.CurrentFacing);
                }
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_facing == null)
                return false;

            return true;
        }
    }
}