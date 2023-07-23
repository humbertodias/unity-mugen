using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Gravity")]
    public class Gravity : StateController
    {
        public Gravity(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            character.CurrentVelocity += new Vector2(0, character.BasePlayer.playerConstants.Vert_acceleration * Constant.Scale);
        }

    }
}