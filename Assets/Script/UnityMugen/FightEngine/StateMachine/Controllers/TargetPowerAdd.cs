using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("TargetPowerAdd")]
    public class TargetPowerAdd : StateController
    {
        private Expression m_power;
        private Expression m_targetId;

        public TargetPowerAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_power = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_targetId = textSection.GetAttribute<Expression>("ID", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var amount = EvaluationHelper.AsInt32(character, m_power, null);
            var targetId = EvaluationHelper.AsInt32(character, m_targetId, -1);

            if (amount == null)
            {
                Debug.Log("TargetPowerAdd : value Required");
                return;
            }

            foreach (var target in character.GetTargets(targetId))
            {
                target.BasePlayer.Power += amount.Value;
            }
        }

        public override bool IsValid()
        {
            if (base.IsValid() == false)
                return false;

            if (m_power == null)
                return false;

            return true;
        }
    }
}