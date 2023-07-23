using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ExplodBindTime")]
    public class ExplodBindTime : StateController
    {
        private Expression m_time;
        private Expression m_id;

        public ExplodBindTime(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_id = textSection.GetAttribute<Expression>("id", null);
                var expTime = textSection.GetAttribute<Expression>("time", null);
                var expValue = textSection.GetAttribute<Expression>("value", null);
                m_time = expTime ?? expValue;
            }
        }

        public override void Run(Character character)
        {
            Load();

            var explodId = EvaluationHelper.AsInt32(character, m_id, int.MinValue);
            var time = EvaluationHelper.AsInt32(character, m_time, 1);

            foreach (var explod in character.GetExplods(explodId))
                explod.Data.BindTime = time;
        }

    }
}