using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetVelAdd")]
    public class TargetVelAdd : StateController
    {
        private Expression m_targetId;
        private Expression m_x;
        private Expression m_y;

        public TargetVelAdd(StateSystem statesystem, string label, TextSection textsection)
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

            var x = EvaluationHelper.AsSingle(character, m_x, null);
            var y = EvaluationHelper.AsSingle(character, m_y, null);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, int.MinValue);

            foreach (var target in character.GetTargets(targetId))
            {
                var velocity = new Vector2(0, 0);

                if (x != null) velocity.x = x.Value;
                if (y != null) velocity.y = y.Value;

                target.CurrentVelocity += velocity;
            }
        }
    }
}