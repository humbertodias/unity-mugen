using System.Collections.Generic;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("RemoveExplod")]
    public class RemoveExplod : StateController
    {
        private Expression m_id;

        public RemoveExplod(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_id = textSection.GetAttribute<Expression>("id", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var explodId = EvaluationHelper.AsInt32(character, m_id, int.MinValue);

            var removelist = new List<UnityMugen.Combat.Explod>(character.GetExplods(explodId));

            foreach (var explod in removelist)
                explod.Kill();
        }
    }
}