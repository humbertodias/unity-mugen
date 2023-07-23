using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AfterImageTime")]
    internal class AfterImageTime : StateController
    {
        private Expression m_time;

        public AfterImageTime(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                var expTime = textSection.GetAttribute<Expression>("time", null);
                var expValue = textSection.GetAttribute<Expression>("value", null);
                m_time = expTime ?? expValue;
            }
        }

        public override void Run(Character character)
        {
            Load();

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