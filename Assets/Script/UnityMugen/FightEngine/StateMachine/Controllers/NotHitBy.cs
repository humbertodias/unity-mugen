using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("NotHitBy")]
    public class NotHitBy : StateController
    {
        private HitAttribute m_hitAttr1;
        private HitAttribute m_hitAttr2;
        private Expression m_time;

        public NotHitBy(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_hitAttr1 = GetAttribute<HitAttribute>(expression, null);
                    break;
                case "value2":
                    m_hitAttr2 = GetAttribute<HitAttribute>(expression, null);
                    break;
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, 1);

            if (m_hitAttr1 != null)
            {
                character.DefensiveInfo.HitBy1.Set(m_hitAttr1, time, true);
            }

            if (m_hitAttr2 != null)
            {
                character.DefensiveInfo.HitBy2.Set(m_hitAttr2, time, true);
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_hitAttr1 != null == (m_hitAttr2 != null))
                return false;

            return true;
        }

    }
}