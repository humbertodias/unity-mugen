using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Turn")]
    public class Turn : StateController
    {
        public Turn(string label) : base(label) { }

        public override void Run(Character character)
        {
            character.CurrentFacing = Misc.FlipFacing(character.CurrentFacing);
        }
    }
}