using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Trans")]
    public class Trans : StateController
    {
        private Blending m_blending;
        private Expression m_alpha;

        public Trans(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_blending = textSection.GetAttribute("trans", new Blending());
                m_alpha = textSection.GetAttribute<Expression>("alpha", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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