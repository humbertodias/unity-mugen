using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PowerAdd")]
    public class PowerAdd : StateController
    {
        private Expression m_power;

        public PowerAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_power = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Run(Character character)
        {
            base.Load();

            var power = EvaluationHelper.AsInt32(character, m_power, null);
            if (power == null)
            {
                Debug.Log("PowerAdd : value Required");
                return;
            }

            character.BasePlayer.Power += power.Value;
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