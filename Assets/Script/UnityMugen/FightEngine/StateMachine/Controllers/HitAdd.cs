using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitAdd")]
    public class HitAdd : StateController
    {
        private Expression m_hits;

        public HitAdd(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_hits = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var hits = EvaluationHelper.AsInt32(character, m_hits, null);

            if (hits == null || hits <= 0)
            {
                Debug.Log("HitAdd : value Required");
                return;
            }

            character.Team.ComboCounter.AddHits(hits.Value);
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_hits == null)
                return false;

            return true;
        }
    }
}