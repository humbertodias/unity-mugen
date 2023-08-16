using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("HitAdd")]
    public class HitAdd : StateController
    {
        private Expression m_hits;

        public HitAdd(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                    m_hits = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
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