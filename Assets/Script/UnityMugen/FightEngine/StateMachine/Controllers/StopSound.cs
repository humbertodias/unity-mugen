using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("StopSnd")]
    public class StopSound : StateController
    {
        private Expression m_channel;

        public StopSound(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_channel = textSection.GetAttribute<Expression>("channel", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var channelnumber = EvaluationHelper.AsInt32(character, m_channel, null);
            if (channelnumber == null)
            {
                Debug.Log("StopSound : channel Required");
                return;
            }

            if (channelnumber == -1)
            {
                LauncherEngine.Inst.soundSystem.StopAllSounds();
            }
            else
            {
                character.SoundManager.Stop(channelnumber.Value);
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_channel == null)
                return false;

            return true;
        }
    }
}