using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("SndPan")]
    public class SndPan : StateController
    {
        private Expression m_channel;
        private Expression m_pan;
        private Expression m_absPan;

        public SndPan(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_channel = textSection.GetAttribute<Expression>("channel", null);
            m_pan = textSection.GetAttribute<Expression>("pan", null);
            m_absPan = textSection.GetAttribute<Expression>("abspan", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var channel = EvaluationHelper.AsInt32(character, m_channel, null);
            var pan = EvaluationHelper.AsInt32(character, m_pan, null);
            var abspan = EvaluationHelper.AsInt32(character, m_absPan, null);

            if (channel == null) return;

            if (pan != null)
                character.SoundManager.RelativePan(channel.Value, pan.Value);
            if (abspan != null)
                character.SoundManager.AbsolutePan(channel.Value, abspan.Value);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_channel == null)
                return false;
            if (m_pan != null && m_absPan != null)
                return false;

            return true;
        }
    }
}