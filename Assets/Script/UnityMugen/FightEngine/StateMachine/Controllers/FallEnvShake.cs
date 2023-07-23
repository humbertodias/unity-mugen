using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("FallEnvShake")]
    public class FallEnvShake : StateController
    {
        public FallEnvShake(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Run(Character character)
        {
            base.Load();

            var hitdef = character.DefensiveInfo.HitDef;

            if (hitdef.EnvShakeFallTime == 0) return;

            var envshake = character.Engine.EnvironmentShake;
            envshake.Set(hitdef.EnvShakeFallTime, hitdef.EnvShakeFallFrequency, hitdef.EnvShakeAmplitude, hitdef.EnvShakeFallPhase);

            hitdef.EnvShakeFallTime = 0;
        }
    }
}