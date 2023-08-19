using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("SndPan")]
    public class SndPan : StateController
    {
        private Expression m_channel;
        private Expression m_pan;
        private Expression m_absPan;

        public SndPan(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "channel":
                    m_channel = GetAttribute<Expression>(expression, null);
                    break;
                case "pan":
                    m_pan = GetAttribute<Expression>(expression, null);
                    break;
                case "abspan":
                    m_absPan = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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