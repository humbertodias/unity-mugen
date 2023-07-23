using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Offset")]
    public class Offset : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public Offset(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_x = textSection.GetAttribute<Expression>("x", null);
                m_y = textSection.GetAttribute<Expression>("y", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var x = EvaluationHelper.AsSingle(character, m_x, null) * Constant.Scale;
            var y = EvaluationHelper.AsSingle(character, m_y, null) * Constant.Scale;

            var offset = character.DrawOffset;

            if (x != null) offset.x = x.Value;
            if (y != null) offset.y = y.Value;

            character.DrawOffset = offset;
        }
    }
}