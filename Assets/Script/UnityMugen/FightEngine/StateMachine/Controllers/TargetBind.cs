using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetBind")]
    public class TargetBind : StateController
    {
        private Expression m_time;
        private Expression m_id;
        private Expression m_pos;

        public TargetBind(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_time = textSection.GetAttribute<Expression>("time", null);
                m_id = textSection.GetAttribute<Expression>("id", null);
                m_pos = textSection.GetAttribute<Expression>("pos", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var targetId = EvaluationHelper.AsInt32(character, m_id, int.MinValue);
            var position = EvaluationHelper.AsVector2(character, m_pos, Vector2.zero) * Constant.Scale;

            foreach (var target in character.GetTargets(targetId))
            {
                target.Bind.Set(character, position, time, 0, true);
            }
        }
    }
}