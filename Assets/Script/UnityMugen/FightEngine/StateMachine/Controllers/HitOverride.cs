using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

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

        public HitOverride(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_hitAttr = textSection.GetAttribute<HitAttribute>("attr", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_slot = textSection.GetAttribute<Expression>("slot", null);
                m_stateNumber = textSection.GetAttribute<Expression>("stateno", null);
                m_time = textSection.GetAttribute<Expression>("time", null);
                m_forceAir = textSection.GetAttribute<Expression>("forceair", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

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