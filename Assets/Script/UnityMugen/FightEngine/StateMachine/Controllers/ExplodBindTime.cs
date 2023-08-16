using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ExplodBindTime")]
    public class ExplodBindTime : StateController
    {
        private Expression m_time;
        private Expression m_id;

        public ExplodBindTime(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "id":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
                case "time":
                case "value":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var explodId = EvaluationHelper.AsInt32(character, m_id, int.MinValue);
            var time = EvaluationHelper.AsInt32(character, m_time, 1);

            foreach (var explod in character.GetExplods(explodId))
                explod.Data.BindTime = time;
        }

    }
}