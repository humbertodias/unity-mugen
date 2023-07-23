using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Width")]
    public class Width : StateController
    {
        private Expression m_edge;
        private Expression m_player;
        private Expression m_expValue;

        public Width(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_edge = textSection.GetAttribute<Expression>("edge", null);
                m_player = textSection.GetAttribute<Expression>("player", null);

                m_expValue = textSection.GetAttribute<Expression>("value", null);

                if (m_expValue != null)
                {
                    m_edge = m_expValue;
                    m_player = m_expValue;
                }
            }
        }

        public override void Run(Character character)
        {
            Load();

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