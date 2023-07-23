Shader "Hidden/UnityMugen/Invert"
{
  HLSLINCLUDE


        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
      
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);


        float _Blend;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            color.rgb = lerp(color.rgb, 1 - color.rgb, _Blend.xxx);

            return color;
        }
  ENDHLSL


  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
              #pragma vertex VertDefault
              #pragma fragment Frag
          ENDHLSL
      }
  }
}
