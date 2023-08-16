using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{
#warning falta aplicar
    [StateControllerName("VictoryQuote")]
    public class VictoryQuote : StateController
    {

        private Expression m_value;
        private string m_language;

        public VictoryQuote(string label) : base(label)
        {
            m_language = "Def";
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value ":
                    m_value = GetAttribute<Expression>(expression, null);
                    break;
                case "language ":
                    m_language = GetAttribute<string>(expression, "Def");
                    break;
            }
        }

        public override void Run(Character character)
        {
            var value = EvaluationHelper.AsInt32(character, m_value, -1);
        }

    }
}