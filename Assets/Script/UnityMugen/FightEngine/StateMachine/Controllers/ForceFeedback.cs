using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{
    [StateControllerName("ForceFeedback")]
    public class ForceFeedback : StateController
    {
        private ForceFeedbackType m_rumbleType;
        private Expression m_time;
        private Expression m_freq;
        private Expression m_ampl;
        private Expression m_self;

        public ForceFeedback(string label) : base(label)
        {
            m_rumbleType = ForceFeedbackType.Sine;
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "waveform":
                    m_rumbleType = GetAttribute(expression, ForceFeedbackType.Sine);
                    break;
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "self":
                    m_self = GetAttribute<Expression>(expression, null);
                    break;

#warning variaveis nao aplicaveis no projeto freq, ampl
                case "freq":
                    m_freq = GetAttribute<Expression>(expression, null);
                    break;
                case "ampl":
                    m_ampl = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, 60);
            var Self = EvaluationHelper.AsBoolean(character, m_self, true);

            GamepadIndex gamepad;
            switch (character.m_PlayerNumber)
            {
                case PlayerID.One:
                default:
                    gamepad = GamepadIndex.GamepadOne;
                    break;
                case PlayerID.Two:
                    gamepad = GamepadIndex.GamepadTwo;
                    break;
                case PlayerID.Three:
                    gamepad = GamepadIndex.GamepadThree;
                    break;
                case PlayerID.Four:
                    gamepad = GamepadIndex.GamepadFour;
                    break;
            }

            if (Self)
            {
                character.ForceFeedbackJoy.Set(time * 2, gamepad);
            }
            else
            {
                foreach (var target in character.GetTargets(int.MinValue))
                {
                    target.ForceFeedbackJoy.Set(time * 2, gamepad);
                    break;
                }
            }
        }
    }
}