using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ClearClipboard")]
    public class ClearClipboard : StateController
    {
        public ClearClipboard(string label) : base(label) { }

        public override void Run(Character character)
        {
            character.Clipboard.Length = 0;
        }
    }
}