using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("BGPalFx")]
    public class BGPalFx : StateController
    {
        private Expression m_time;
        private Expression m_palAdd;
        private Expression m_palMul;
        private Expression m_sineAdd;
        private Expression m_palInvert;
        private Expression m_palColor;

        public BGPalFx(string label) : base(label) { }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "add":
                    m_palAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "mul":
                    m_palMul = GetAttribute<Expression>(expression, null);
                    break;
                case "sinadd":
                    m_sineAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "invertall":
                    m_palInvert = GetAttribute<Expression>(expression, null);
                    break;
                case "color":
                    m_palColor = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, -2);
            var paladd = EvaluationHelper.AsVector3(character, m_palAdd, Vector3.zero);
            var palmul = EvaluationHelper.AsVector3(character, m_palMul, new Vector3(255, 255, 255));
            var sinadd = EvaluationHelper.AsVector4(character, m_sineAdd, new Vector4(0, 0, 0, 1), 1);
            var invert = EvaluationHelper.AsBoolean(character, m_palInvert, false);
            var basecolor = EvaluationHelper.AsInt32(character, m_palColor, 255);

            if (time < -1) return;

            var palfx = character.Engine.stageScreen.Stage.PaletteFx;
            palfx.Set(time, paladd, palmul, sinadd, invert, basecolor / 255.0f);
        }

    }
}