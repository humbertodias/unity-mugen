using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AppendToClipboard")]
    public class AppendToClipboard : DisplayToClipboard
    {
        private string m_formatString;
        private Expression m_params;

        public AppendToClipboard(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_formatString = textSection.GetAttribute<string>("text", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_params = textSection.GetAttribute<Expression>("params", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            if (m_params != null)
            {
                var result = m_params.Evaluate(character);
                if (result == null) return;

                character.Clipboard.Append(BuildString(result));
            }
            else
            {
                character.Clipboard.Append(m_formatString);
            }

        }

    }
}