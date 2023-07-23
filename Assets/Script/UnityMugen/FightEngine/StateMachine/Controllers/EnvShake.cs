using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("EnvShake")]
    public class EnvShake : StateController
    {
        private Expression m_time;
        private Expression m_freq;
        private Expression m_amplitude;
        private Expression m_phaseOffSet;

        public EnvShake(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_time = textSection.GetAttribute<Expression>("time", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_freq = textSection.GetAttribute<Expression>("freq", null);
                m_amplitude = textSection.GetAttribute<Expression>("ampl", null);
                m_phaseOffSet = textSection.GetAttribute<Expression>("phase", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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