//using System;
//using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

//[Serializable]
//[PostProcess(typeof(TV_VintageRenderer), PostProcessEvent.AfterStack, "UnityMugen/TV_Vintage")]
//public sealed class TV_Vintage : PostProcessEffectSettings
//{
//    [Range(0f, 10f), Tooltip("Grayscale effect intensity.")]
//    public FloatParameter distortion = new FloatParameter { value = 0.38f };

//    [Range(0f, 1000f), Tooltip("Grayscale effect intensity.")]
//    public FloatParameter intensity = new FloatParameter { value = 1000f };

//}

//public sealed class TV_VintageRenderer : PostProcessEffectRenderer<TV_Vintage>
//{
//    public override void Render(PostProcessRenderContext context)
//    {
//        var sheet = context.propertySheets.Get(Shader.Find("Hidden/UnityMugen/TV_Vintage"));
//        sheet.properties.SetFloat("_Distortion", settings.distortion);
//        sheet.properties.SetFloat("_Intensity", settings.intensity);
//        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
//    }
//}