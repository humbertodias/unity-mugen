using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PosSet")]
    public class PosSet : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public PosSet(StateSystem statesystem, string label, TextSection textsection)
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

            var x = EvaluationHelper.AsSingle(character, m_x, null);
            var y = EvaluationHelper.AsSingle(character, m_y, null);

            var cameralocation = character.Engine.CameraFE.Location;
            var location = character.CurrentLocation;
            if (m_x != null) location.x = cameralocation.x + (x.Value * Constant.Scale);
            if (m_y != null) location.y = y.Value * Constant.Scale;

            character.CurrentLocation = location;
        }
    }
}