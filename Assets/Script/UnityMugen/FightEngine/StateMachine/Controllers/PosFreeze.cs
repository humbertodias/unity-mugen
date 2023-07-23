using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PosFreeze")]
    public class PosFreeze : StateController
    {
        private Expression m_freeze;

        public PosFreeze(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_freeze = textSection.GetAttribute<Expression>("value", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var freeze = EvaluationHelper.AsBoolean(character, m_freeze, true);

            character.PositionFreeze = freeze;
        }
    }
}