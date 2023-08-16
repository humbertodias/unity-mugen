using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("BindToParent")]
    public class BindToParent : StateController
    {
        private Expression m_time;
        private Expression m_facing;
        private Expression m_position;

        public BindToParent(string label) : base(label) { }


        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "facing":
                    m_facing = GetAttribute<Expression>(expression, null);
                    break;
                case "pos":
                    m_position = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var helper = character as UnityMugen.Combat.Helper;
            if (helper == null)
            {
                Debug.Log("BindToParent : helper Required");
                return;
            }

            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var facing = EvaluationHelper.AsInt32(character, m_facing, 0);
            var offset = EvaluationHelper.AsVector2(character, m_position, Vector2.zero) * Constant.Scale;

            helper.Bind.Set(helper.Creator, offset, time, facing, false);
        }

    }
}