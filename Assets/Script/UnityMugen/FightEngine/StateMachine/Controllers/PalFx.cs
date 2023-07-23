using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.IO;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("PalFx")]
    public class PalFx : StateController
    {
        private Expression m_time;
        private Expression m_palAdd;
        private Expression m_palMul;
        private Expression m_sineAdd;
        private Expression m_palInvert;
        private Expression m_palColor;

        public PalFx(StateSystem statesystem, string label, TextSection textsection)
                : base(statesystem, label, textsection) { }

        public override void Load()
        {
            if (isLoaded == false)
            {
                base.Load();

                m_time = textSection.GetAttribute<Expression>("time", null);
                m_palAdd = textSection.GetAttribute<Expression>("add", null);
                m_palMul = textSection.GetAttribute<Expression>("mul", null);
                m_sineAdd = textSection.GetAttribute<Expression>("sinadd", null);
                m_palInvert = textSection.GetAttribute<Expression>("invertall", null);
                m_palColor = textSection.GetAttribute<Expression>("color", null);
            }
        }

        public override void Run(Character character)
        {
            Load();

            var time = EvaluationHelper.AsInt32(character, m_time, -2);
            var paladd = EvaluationHelper.AsVector3(character, m_palAdd, new Vector3(0, 0, 0));
            var palmul = EvaluationHelper.AsVector3(character, m_palMul, new Vector3(255, 255, 255));
            var sinadd = EvaluationHelper.AsVector4(character, m_sineAdd, new Vector4(0, 0, 0, 1));
            var invert = EvaluationHelper.AsBoolean(character, m_palInvert, false);
            var basecolor = EvaluationHelper.AsInt32(character, m_palColor, 255);

            character.PaletteFx.Set(time, paladd, palmul, sinadd, invert, basecolor / 255.0f);
        }

    }
}