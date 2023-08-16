using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("StopSnd")]
    public class StopSnd : StateController
    {
        private Expression m_channel;

        public StopSnd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "channel":
                    m_channel = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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