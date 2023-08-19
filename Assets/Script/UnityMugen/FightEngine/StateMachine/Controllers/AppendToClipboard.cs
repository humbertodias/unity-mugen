using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AppendToClipboard")]
    public class AppendToClipboard : DisplayToClipboard
    {

        public AppendToClipboard(string label) : base(label) { }

        public override void Run(Character character)
        {
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