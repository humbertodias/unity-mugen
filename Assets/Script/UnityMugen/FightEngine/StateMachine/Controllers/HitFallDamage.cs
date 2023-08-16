using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitFallDamage")]
    public class HitFallDamage : StateController
    {
        public HitFallDamage(string label) : base(label) { }

        public override void Run(Character character)
        {
            character.Life -= character.DefensiveInfo.HitDef.FallDamage;
        }
    }
}