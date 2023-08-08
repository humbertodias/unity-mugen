using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetVelSet")]
    public class TargetVelSet : StateController
    {
        private Expression m_targetId;
        private Expression m_x;
        private Expression m_y;

        public TargetVelSet(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_targetId = textSection.GetAttribute<Expression>("id", null);
                m_x = textSection.GetAttribute<Expression>("x", null);
                m_y = textSection.GetAttribute<Expression>("y", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);
            var x = EvaluationHelper.AsSingle(character, m_x, null);
            var y = EvaluationHelper.AsSingle(character, m_y, null);

            foreach (var target in character.GetTargets(targetId))
            {
                var velocity = target.CurrentVelocity;

                if (x != null) velocity.x = x.Value;
                if (y != null) velocity.y = y.Value;

                target.CurrentVelocity = velocity;
            }
        }
    }
}