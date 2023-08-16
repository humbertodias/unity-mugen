using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetBind")]
    public class TargetBind : StateController
    {
        private Expression m_time;
        private Expression m_id;
        private Expression m_pos;

        public TargetBind(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "id":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
                case "pos":
                    m_pos = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var targetId = EvaluationHelper.AsInt32(character, m_id, -1);
            var position = EvaluationHelper.AsVector2(character, m_pos, Vector2.zero) * Constant.Scale;

            foreach (var target in character.GetTargets(targetId))
            {
                target.Bind.Set(character, position, time, 0, true);
            }
        }
    }
}