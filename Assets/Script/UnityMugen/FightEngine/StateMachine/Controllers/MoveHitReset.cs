using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("MoveHitReset")]
    public class MoveHitReset : StateController
    {
        public MoveHitReset(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            character.OffensiveInfo.MoveContact = 0;
            character.OffensiveInfo.MoveGuarded = 0;
            character.OffensiveInfo.MoveHit = 0;
            character.OffensiveInfo.MoveReversed = 0;
        }
    }
}