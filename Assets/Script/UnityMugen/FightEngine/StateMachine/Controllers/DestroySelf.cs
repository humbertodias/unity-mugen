using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("DestroySelf")]
    public class DestroySelf : StateController
    {
        private Expression m_recursive;
        private Expression m_removeExplods;

        public DestroySelf(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "recursive":
                    m_recursive = GetAttribute<Expression>(expression, null);
                    break;
                case "removeexplods":
                    m_removeExplods = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var recursive = EvaluationHelper.AsBoolean(character, m_recursive, false);
            var removeExplods = EvaluationHelper.AsBoolean(character, m_removeExplods, false);

            var helper = character as UnityMugen.Combat.Helper;
            if (helper != null) helper.Remove(recursive, removeExplods);
        }

    }
}