using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("MoveHitReset")]
    public class MoveHitReset : StateController
    {
        public MoveHitReset(string label) : base(label) { }

        public override void Run(Character character)
        {
            character.OffensiveInfo.MoveContact = 0;
            character.OffensiveInfo.MoveGuarded = 0;
            character.OffensiveInfo.MoveHit = 0;
            character.OffensiveInfo.MoveReversed = 0;
        }
    }
}