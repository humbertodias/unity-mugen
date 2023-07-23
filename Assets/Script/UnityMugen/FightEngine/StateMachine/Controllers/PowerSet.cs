using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PowerSet")]
    public class PowerSet : StateController
    {
        private Expression m_power;

        public PowerSet(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_power = textsection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var power = EvaluationHelper.AsInt32(character, m_power, null);
            if (power == null)
            {
                Debug.Log("PowerSet : value Required");
                return;
            }

            if (power != null) character.BasePlayer.Power = power.Value;
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