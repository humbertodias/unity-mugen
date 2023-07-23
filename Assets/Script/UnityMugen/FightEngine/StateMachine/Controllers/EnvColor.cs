using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("EnvColor")]
    public class EnvColor : StateController
    {
        private Expression m_color;
        private Expression m_time;
        private Expression m_under;

        public EnvColor(StateSystem statesystem, string label, TextSection textsection)
            : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                var expValue = textSection.GetAttribute<Expression>("value", null);
                var expColor = textSection.GetAttribute<Expression>("color", null);

                m_color = expValue ?? expColor;
                m_time = textSection.GetAttribute<Expression>("time", null);
                m_under = textSection.GetAttribute<Expression>("under", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

#warning aqui é 1 ou 255
            var color = EvaluationHelper.AsVector3(character, m_color, Vector3.one);
            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var underflag = EvaluationHelper.AsBoolean(character, m_under, false);

            character.Engine.EnvironmentColor.Setup(new Color32((byte)color.x, (byte)color.y, (byte)color.z, 255), time, underflag);
        }

    }
}