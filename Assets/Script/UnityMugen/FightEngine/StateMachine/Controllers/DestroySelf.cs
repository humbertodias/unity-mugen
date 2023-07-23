using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("DestroySelf")]
    public class DestroySelf : StateController
    {
        private Expression m_recursive;
        private Expression m_removeExplods;

        public DestroySelf(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_recursive = textSection.GetAttribute<Expression>("recursive", null);
                m_removeExplods = textSection.GetAttribute<Expression>("removeexplods", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var recursive = EvaluationHelper.AsBoolean(character, m_recursive, false);
            var removeExplods = EvaluationHelper.AsBoolean(character, m_removeExplods, false);

            var helper = character as UnityMugen.Combat.Helper;
            if (helper != null) helper.Remove(recursive, removeExplods);
        }

    }
}