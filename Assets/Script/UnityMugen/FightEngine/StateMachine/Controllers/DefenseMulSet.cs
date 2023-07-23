using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("DefenseMulSet", "DefenceMulSet")]
    public class DefenseMulSet : StateController
    {
        private Expression m_multiplier;

        public DefenseMulSet(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection)
        {
            m_multiplier = textsection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var multiplier = EvaluationHelper.AsSingle(character, m_multiplier, null);

            if (multiplier == null)
            {
                Debug.Log("DefenseMulSet : value Required");
                return;
            }

            character.DefensiveInfo.DefenseMultiplier = multiplier.Value;
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_multiplier == null)
                return false;

            return true;
        }

    }
}