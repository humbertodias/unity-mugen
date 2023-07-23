using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Turn")]
    public class Turn : StateController
    {
        public Turn(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            character.CurrentFacing = Misc.FlipFacing(character.CurrentFacing);
        }
    }
}