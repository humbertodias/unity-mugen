using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

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

        public ForceFeedback(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                ForceFeedbackType m_rumbletype = textSection.GetAttribute("waveform", ForceFeedbackType.Sine);
                m_time = textSection.GetAttribute<Expression>("time", null);
                m_self = textSection.GetAttribute<Expression>("self", null);

#warning variaveis nao aplicaveis no projeto freq, ampl
                m_freq = textSection.GetAttribute<Expression>("freq", null);
                m_ampl = textSection.GetAttribute<Expression>("ampl", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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