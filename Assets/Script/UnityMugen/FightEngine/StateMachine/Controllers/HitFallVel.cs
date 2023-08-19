using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitFallVel")]
    public class HitFallVel : StateController
    {
        public HitFallVel(string label) : base(label) { }

        public override void Run(Character character)
        {
            var velocity = Vector2.zero;
            velocity.x = character.DefensiveInfo.HitDef.FallVelocityX ?? character.CurrentVelocity.x;
            velocity.y = character.DefensiveInfo.HitDef.FallVelocityY;
            character.CurrentVelocity = velocity;
        }
    }
}