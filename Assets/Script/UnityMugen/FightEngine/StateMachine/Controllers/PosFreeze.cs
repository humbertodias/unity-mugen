using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PosFreeze")]
    public class PosFreeze : StateController
    {
        private Expression m_freeze;

        public PosFreeze(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_freeze = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var freeze = EvaluationHelper.AsBoolean(character, m_freeze, true);

            character.PositionFreeze = freeze;
        }
    }
}