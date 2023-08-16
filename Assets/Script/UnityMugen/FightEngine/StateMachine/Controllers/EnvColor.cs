using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("EnvColor")]
    public class EnvColor : StateController
    {
        private Expression m_color;
        private Expression m_time;
        private Expression m_under;

        public EnvColor(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "value":
                case "color":
                    m_color = GetAttribute<Expression>(expression, null);
                    break;
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "under":
                    m_under = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
#warning aqui é 1 ou 255
            var color = EvaluationHelper.AsVector3(character, m_color, Vector3.one);
            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var underflag = EvaluationHelper.AsBoolean(character, m_under, false);

            character.Engine.EnvironmentColor.Setup(new Color32((byte)color.x, (byte)color.y, (byte)color.z, 255), time, underflag);
        }

    }
}