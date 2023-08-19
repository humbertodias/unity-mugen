using UnityMugen.Combat;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("FallEnvShake")]
    public class FallEnvShake : StateController
    {
        public FallEnvShake(string label) : base(label) { }

        public override void Run(Character character)
        {
            var hitdef = character.DefensiveInfo.HitDef;

            if (hitdef.EnvShakeFallTime == 0) return;

            var envshake = character.Engine.EnvironmentShake;
            envshake.Set(hitdef.EnvShakeFallTime, hitdef.EnvShakeFallFrequency, hitdef.EnvShakeAmplitude, hitdef.EnvShakeFallPhase);

            hitdef.EnvShakeFallTime = 0;
        }
    }
}