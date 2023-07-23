using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("ClearClipboard")]
    public class ClearClipboard : StateController
    {
        public ClearClipboard(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            character.Clipboard.Length = 0;
        }
    }
}