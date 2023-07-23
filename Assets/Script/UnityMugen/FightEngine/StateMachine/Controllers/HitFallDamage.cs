using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitFallDamage")]
    public class HitFallDamage : StateController
    {
        public HitFallDamage(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            character.Life -= character.DefensiveInfo.HitDef.FallDamage;
        }

    }
}