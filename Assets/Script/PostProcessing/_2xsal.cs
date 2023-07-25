//using System;
//using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

//[Serializable]
//[PostProcess(typeof(_2xsalRenderer), PostProcessEvent.AfterStack, "UnityMugen/2xsal")]
//public sealed class _2xsal : PostProcessEffectSettings { }

//public sealed class _2xsalRenderer : PostProcessEffectRenderer<_2xsal>
//{
//    public override void Render(PostProcessRenderContext context)
//    {
//        var sheet = context.propertySheets.Get(Shader.Find("Hidden/UnityMugen/2xsal"));
//        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
//    }
//}