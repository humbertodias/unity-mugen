using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("Pause")]
    public class Pause : StateController
    {
        private Expression m_time;
        private Expression m_cmdBufferTime;
        private Expression m_moveTime;
        private Expression m_pauseBackground;

        public Pause(string label) : base(label) { }


        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "endcmdbuftime":
                    m_cmdBufferTime = GetAttribute<Expression>(expression, null);
                    break;
                case "movetime":
                    m_moveTime = GetAttribute<Expression>(expression, null);
                    break;
                case "pausebg":
                    m_pauseBackground = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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