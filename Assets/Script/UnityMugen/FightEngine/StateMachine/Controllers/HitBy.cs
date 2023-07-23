using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitBy")]
    public class HitBy : StateController
    {
        private HitAttribute m_hitAttr1;
        private HitAttribute m_hitAttr2;
        private Expression m_time;

        public HitBy(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_hitAttr1 = textsection.GetAttribute<HitAttribute>("value", null);
            m_hitAttr2 = textsection.GetAttribute<HitAttribute>("value2", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_time = textSection.GetAttribute<Expression>("time", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var time = EvaluationHelper.AsInt32(character, m_time, 1);

            if (m_hitAttr1 != null)
            {
                character.DefensiveInfo.HitBy1.Set(m_hitAttr1, time, false);
            }

            if (m_hitAttr2 != null)
            {
                character.DefensiveInfo.HitBy2.Set(m_hitAttr2, time, false);
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