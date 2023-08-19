using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PosSet")]
    public class PosSet : StateController
    {
        private Expression m_x;
        private Expression m_y;

        public PosSet(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "x":
                    m_x = GetAttribute<Expression>(expression, null);
                    break;
                case "y":
                    m_y = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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