using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("EnvShake")]
    public class EnvShake : StateController
    {
        private Expression m_time;
        private Expression m_freq;
        private Expression m_amplitude;
        private Expression m_phaseOffSet;

        public EnvShake(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "freq":
                    m_freq = GetAttribute<Expression>(expression, null);
                    break;
                case "ampl":
                    m_amplitude = GetAttribute<Expression>(expression, null);
                    break;
                case "phase":
                    m_phaseOffSet = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, null);
            var frequency = Misc.Clamp(EvaluationHelper.AsSingle(character, m_freq, 60), 0, 180);
            var amplitude = EvaluationHelper.AsInt32(character, m_amplitude, -4);
            var phase = EvaluationHelper.AsSingle(character, m_phaseOffSet, frequency >= 90 ? 0 : 90);

            if (time == null) return;

            var envshake = character.Engine.EnvironmentShake;
            envshake.Set(time.Value, frequency, amplitude, phase);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_time == null)
                return false;

            return true;
        }

    }
}