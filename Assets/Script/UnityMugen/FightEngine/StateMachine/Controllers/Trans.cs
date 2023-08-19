using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Trans")]
    public class Trans : StateController
    {
        private Blending m_blending;
        private Expression m_alpha;

        public Trans(string label) : base(label)
        {
            m_blending = new Blending();
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "trans":
                    m_blending = GetAttribute(expression, new Blending());
                    break;
                case "alpha":
                    m_alpha = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var alpha = EvaluationHelper.AsVector2(character, m_alpha, new Vector2(255, 0));

            if (m_blending.BlendType == BlendType.Add && m_blending.SourceFactor == 0 && m_blending.DestinationFactor == 0)
            {
                character.Transparency = new Blending(BlendType.Add, alpha.x, alpha.y);
            }
            else
            {
                character.Transparency = m_blending;
            }
        }
    }
}