using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AllPalFx")]
    internal class AllPalFx : StateController
    {
        private Expression m_time;
        private Expression m_palAdd;
        private Expression m_palMul;
        private Expression m_sineAdd;
        private Expression m_palInvert;
        private Expression m_palColor;

        public AllPalFx(string label) : base(label) { }

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

            foreach (var entity in character.Engine.Entities)
            {
                entity.PaletteFx.Set(time, paladd, palmul, sinadd, invert, basecolor);
            }

            character.Engine.stageScreen.Stage.PaletteFx.Set(time, paladd, palmul, sinadd, invert, basecolor);
        }
    }
}