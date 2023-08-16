using UnityEngine;
using UnityMugen.Combat;
using UnityMugen.Evaluation;
using UnityMugen.Video;

namespace UnityMugen.StateMachine.Controllers
{

    [StateControllerName("AfterImage")]
    public class AfterImage : StateController
    {
        private Expression m_time;
        private Expression m_numberOfFrames;
        private Expression m_paletteColor;
        private Expression m_paletteInversion;
        private Expression m_paletteBrightness;
        private Expression m_paletteContrast;
        private Expression m_palettePostBrightness;
        private Expression m_paletteAdd;
        private Expression m_paletteMutliply;
        private Expression m_timeGap;
        private Expression m_frameGap;
        private Blending? m_trans;
        private Expression m_alpha;

        public AfterImage(string label) : base(label)
        {
            m_trans = Misc.ToBlending(BlendType.None);
        }

        public override void SetAttributes(string idAttribute, string expression)
        {
            base.SetAttributes(idAttribute, expression);
            switch (idAttribute)
            {
                case "time":
                    m_time = GetAttribute<Expression>(expression, null);
                    break;
                case "length":
                    m_numberOfFrames = GetAttribute<Expression>(expression, null);
                    break;
                case "palcolor":
                    m_paletteColor = GetAttribute<Expression>(expression, null);
                    break;
                case "palinvertall":
                    m_paletteInversion = GetAttribute<Expression>(expression, null);
                    break;
                case "palbright":
                    m_paletteBrightness = GetAttribute<Expression>(expression, null);
                    break;
                case "palcontrast":
                    m_paletteContrast = GetAttribute<Expression>(expression, null);
                    break;
                case "palpostbright":
                    m_palettePostBrightness = GetAttribute<Expression>(expression, null);
                    break;
                case "paladd":
                    m_paletteAdd = GetAttribute<Expression>(expression, null);
                    break;
                case "palmul":
                    m_paletteMutliply = GetAttribute<Expression>(expression, null);
                    break;
                case "timegap":
                    m_timeGap = GetAttribute<Expression>(expression, null);
                    break;
                case "framegap":
                    m_frameGap = GetAttribute<Expression>(expression, null);
                    break;
                case "trans":
                    m_trans = GetAttribute<Blending?>(expression, Misc.ToBlending(BlendType.None));
                    break;
                case "alpha":
                    m_alpha = GetAttribute<Expression>(expression, null);
                    break;
            }
        }

        public AfterImage(AfterImageData afterImage)
        {
            m_time = afterImage.m_time;
            m_numberOfFrames = afterImage.m_numberOfFrames;
            m_paletteColor = afterImage.m_paletteColor;
            m_paletteInversion = afterImage.m_paletteInversion;
            m_paletteBrightness = afterImage.m_paletteBrightness;
            m_paletteContrast = afterImage.m_paletteContrast;
            m_palettePostBrightness = afterImage.m_palettePostBrightness;
            m_paletteAdd = afterImage.m_paletteAdd;
            m_paletteMutliply = afterImage.m_paletteMutliply;
            m_timeGap = afterImage.m_timeGap;
            m_frameGap = afterImage.m_frameGap;
            m_trans = afterImage.m_trans;
            m_alpha = afterImage.m_alpha;
        }

        public override void Run(Character character)
        {
            var time = EvaluationHelper.AsInt32(character, m_time, 1);
            var numberofframes = EvaluationHelper.AsInt32(character, m_numberOfFrames, 20);
            var basecolor = EvaluationHelper.AsInt32(character, m_paletteColor, 255);
            var invert = EvaluationHelper.AsBoolean(character, m_paletteInversion, false);
            var palpreadd = EvaluationHelper.AsVector3(character, m_paletteBrightness, new Vector3(30, 30, 30));
            var palcontrast = EvaluationHelper.AsVector3(character, m_paletteContrast, new Vector3(120, 120, 220));
            var palpostadd = EvaluationHelper.AsVector3(character, m_palettePostBrightness, Vector3.zero);
            var paladd = EvaluationHelper.AsVector3(character, m_paletteAdd, new Vector3(10, 10, 25));
            var palmul = EvaluationHelper.AsVector3(character, m_paletteMutliply, new Vector3(.65f, .65f, .75f));
            var timegap = EvaluationHelper.AsInt32(character, m_timeGap, 1);
            var framegap = EvaluationHelper.AsInt32(character, m_frameGap, 4);


            var afterimages = character.AfterImages;
            afterimages.ResetFE();
            afterimages.Time = time;
            afterimages.Length = Misc.Clamp(numberofframes, 0, 60);
            afterimages.BaseColor = basecolor / 255.0f;
            afterimages.InvertColor = invert;
            afterimages.ColorPreAdd = Misc.ClampVector3(palpreadd / 255.0f, Vector3.zero, Vector3.one);
            afterimages.ColorContrast = Misc.ClampVector3(palcontrast / 255.0f, Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
            afterimages.ColorPostAdd = Misc.ClampVector3(palpostadd / 255.0f, Vector3.zero, Vector3.one);
            afterimages.ColorPaletteAdd = Misc.ClampVector3(paladd / 255.0f, Vector3.zero, Vector3.one);
            afterimages.ColorPaletteMultiply = Misc.ClampVector3(palmul, Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
            afterimages.TimeGap = timegap;
            afterimages.FrameGap = framegap;
            afterimages.IsActive = true;

            var alpha = EvaluationHelper.AsVector2(character, m_alpha, new Vector2(255, 0));
            if (m_trans.Value.BlendType == BlendType.AddAlpha)
                afterimages.Transparency = new Blending(m_trans.Value.BlendType, alpha.x, alpha.y);
            else
                afterimages.Transparency = Misc.ToBlending(m_trans.Value.BlendType);
        }
    }
}