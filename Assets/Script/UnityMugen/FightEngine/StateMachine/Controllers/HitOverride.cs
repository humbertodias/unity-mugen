using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitOverride")]
    public class HitOverride : StateController
    {
        private HitAttribute m_hitAttr;
        private Expression m_slot;
        private Expression m_stateNumber;
        private Expression m_time;
        private Expression m_forceAir;

        public HitOverride(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "attr":
                    m_hitAttr = GetAttribute<HitAttribute>(expression, null);
                    break;
                case "slot":
                    m_slot = GetAttribute<Expression>(expression, null);
                    break;
                case "stateno":
                    m_stateNumber = GetAttribute<Expression>(expression, null);
                    break;
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "forceair":
                    m_forceAir = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var slotnumber = EvaluationHelper.AsInt32(character, m_slot, 0);
            var statenumber = EvaluationHelper.AsInt32(character, m_stateNumber, int.MinValue);
            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var forceair = EvaluationHelper.AsBoolean(character, m_forceAir, false);

            if (slotnumber < 0 || slotnumber > 7)
            {
                Debug.Log("HitOverride : slot error");
                return;
            }

            character.DefensiveInfo.HitOverrides[slotnumber].Set(m_hitAttr, statenumber, time, forceair);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_hitAttr == null)
                return false;

            return true;
        }

    }
}