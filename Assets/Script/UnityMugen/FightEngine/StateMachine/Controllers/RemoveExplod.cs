using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("RemoveExplod")]
    public class RemoveExplod : StateController
    {
        private Expression m_id;

        public RemoveExplod(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "id":
                    m_id = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var explodId = EvaluationHelper.AsInt32(character, m_id, int.MinValue);

            var removelist = new List<UnityMugen.Combat.Explod>(character.GetExplods(explodId));

            foreach (var explod in removelist)
                explod.Kill();
        }
    }
}