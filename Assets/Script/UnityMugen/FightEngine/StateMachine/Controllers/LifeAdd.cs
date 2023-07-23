using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("LifeAdd")]
    public class LifeAdd : StateController
    {

        private Expression m_life;
        private Expression m_canKill;
        private Expression m_abs;

        public LifeAdd(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection)
        {
            m_life = textSection.GetAttribute<Expression>("value", null);
        }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_canKill = textSection.GetAttribute<Expression>("kill", null);
                m_abs = textSection.GetAttribute<Expression>("absolute", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var amount = EvaluationHelper.AsInt32(character, m_life, null);
            if (amount == null)
            {
                Debug.Log("LifeAdd : value Required");
                return;
            }


            var cankill = EvaluationHelper.AsBoolean(character, m_canKill, true);
            var absolute = EvaluationHelper.AsBoolean(character, m_abs, false);

            var scaledamount = amount.Value;
            if (absolute == false)
                scaledamount = (int)(scaledamount / character.DefensiveInfo.DefenseMultiplier);

            character.Life += scaledamount;

            if (cankill == false && character.Life == 0)
                character.Life = 1;
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