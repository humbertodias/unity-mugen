using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AngleDraw")]
    public class AngleDraw : StateController
    {
        private Expression m_angle;
        private Expression m_scale;

        public AngleDraw(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_angle = textSection.GetAttribute<Expression>("value", null);
                m_scale = textSection.GetAttribute<Expression>("scale", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var angle = EvaluationHelper.AsSingle(character, m_angle, character.DrawingAngle);

            character.DrawingAngle = angle;
            character.AngleDraw = true;

            var scale = EvaluationHelper.AsVector2(character, m_scale, null, 1);
            if (scale != null) character.DrawScale = scale.Value;
        }

    }
}