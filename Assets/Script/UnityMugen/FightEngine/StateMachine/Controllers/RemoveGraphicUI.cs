using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("RemoveGraphicUI")]
    public class RemoveGraphicUI : StateController
    {

        private Expression m_id;

        public RemoveGraphicUI(string label) : base(label) { }

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
            var ID = EvaluationHelper.AsInt32(character, m_id, -1);
            var removelist = new List<GraphicUIEntity>(character.GetGraphicUIs(ID));
            foreach (var graphicUIEntity in removelist)
                graphicUIEntity.Kill();
        }
    }
}