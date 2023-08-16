using UnityEngine;
using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Gravity")]
    public class Gravity : StateController
    {
        public Gravity(string label) : base(label) { }

        public override void Run(Character character)
        {
            character.CurrentVelocity += new Vector2(0, character.BasePlayer.playerConstants.Vert_acceleration * Constant.Scale);
        }
    }
}