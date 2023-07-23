using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("LifeSet")]
    public class LifeSet : StateController
    {
        private Expression m_life;

        public LifeSet(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_life = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var amount = EvaluationHelper.AsInt32(character, m_life, null);
            if (amount == null)
            {
                Debug.Log("LifeSet : value Required");
                return;
            }

            character.Life = amount.Value;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_life == null)
                return false;

            return true;
        }

    }
}