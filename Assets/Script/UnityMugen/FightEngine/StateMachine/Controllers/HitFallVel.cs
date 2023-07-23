using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitFallVel")]
    public class HitFallVel : StateController
    {
        public HitFallVel(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            var velocity = Vector2.zero;
            velocity.x = character.DefensiveInfo.HitDef.FallVelocityX ?? character.CurrentVelocity.x;
            velocity.y = character.DefensiveInfo.HitDef.FallVelocityY;
            character.CurrentVelocity = velocity;
        }
    }
}