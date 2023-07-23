using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Pause")]
    public class Pause : StateController
    {
        private Expression m_time;
        private Expression m_cmdBufferTime;
        private Expression m_moveTime;
        private Expression m_pauseBackground;

        public Pause(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_time = textSection.GetAttribute<Expression>("time", null);
        }


        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_cmdBufferTime = textSection.GetAttribute<Expression>("endcmdbuftime", null);
                m_moveTime = textSection.GetAttribute<Expression>("movetime", null);
                m_pauseBackground = textSection.GetAttribute<Expression>("pausebg", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var time = EvaluationHelper.AsInt32(character, m_time, null);
            var buffertime = EvaluationHelper.AsInt32(character, m_cmdBufferTime, 0);
            var movetime = EvaluationHelper.AsInt32(character, m_moveTime, 0);
            var pausebg = EvaluationHelper.AsBoolean(character, m_pauseBackground, true);

#warning adicionar isso futuramente ainda não esta em funcionalidade
            //Endcmdbuftime

            character.Engine.Pause.Set(character, time.Value, buffertime, movetime, false, pausebg);
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