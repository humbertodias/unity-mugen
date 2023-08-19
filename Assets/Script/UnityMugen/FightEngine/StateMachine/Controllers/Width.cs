using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Width")]
    public class Width : StateController
    {
        private Expression m_edge;
        private Expression m_player;
        private Expression m_expValue;

        public Width(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "edge":
                    m_edge = GetAttribute<Expression>(expression, null);
                    break;
                case "player":
                    m_player = GetAttribute<Expression>(expression, null);
                    break;
                case "value":
                    m_edge = m_player = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var playerwidth = EvaluationHelper.AsVector2(character, m_player, null) * Constant.Scale;
            if (playerwidth != null)
            {
                character.Dimensions.SetOverride(playerwidth.Value.x, playerwidth.Value.y);
            }

            var edgewidth = EvaluationHelper.AsVector2(character, m_edge, null) * Constant.Scale;
            if (edgewidth != null)
            {
                character.Dimensions.SetEdgeOverride(edgewidth.Value.x, edgewidth.Value.y);
            }
        }
    }
}