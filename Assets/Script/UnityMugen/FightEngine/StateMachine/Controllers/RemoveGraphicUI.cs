using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("RemoveGraphicUI")]
    public class RemoveGraphicUI : StateController
    {

        private Expression m_id;

        public RemoveGraphicUI(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_id = textSection.GetAttribute<Expression>("id", null);
        }

        public override void Run(Character character)
        {
            var ID = EvaluationHelper.AsInt32(character, m_id, int.MinValue);
            var removelist = new List<GraphicUIEntity>(character.GetGraphicUIs(ID));
            foreach (var graphicUIEntity in removelist)
                graphicUIEntity.Kill();
        }
    }
}