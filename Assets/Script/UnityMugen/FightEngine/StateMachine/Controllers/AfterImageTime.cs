using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AfterImageTime")]
    internal class AfterImageTime : StateController
    {
        private Expression m_time;

        public AfterImageTime(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                case "value":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, null);

            if (time != null)
            {
                character.AfterImages.ModifyDisplayTime(time.Value);
            }
            else
            {
                character.AfterImages.IsActive = false;
            }
        }
    }
}