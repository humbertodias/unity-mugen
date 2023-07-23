Shader "Hidden/UnityMugen/TV_Vintage"
{
  HLSLINCLUDE


		#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
	//	#include "UnityCG.cginc"
      
		TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);


		float _Blend;
		float _TimeX;
		float _Distortion;
		float _Intensity;

		half4 _MainTex_ST;

		float4 Frag(VaryingsDefault i) : SV_Target
		{
			float2 uv = UnityStereoScreenSpaceUVAdjust(i.texcoord, _MainTex_ST);
			float3 col;

		//	col.r = tex2D(_MainTex,float2(uv.x+0.003*_Distortion,uv.y)).x;
		//	col.g = tex2D(_MainTex,float2(uv.x+0.000*_Distortion,uv.y)).y;
		//  col.b = tex2D(_MainTex,float2(uv.x-0.003*_Distortion,uv.y)).z;


			col.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x+0.003*_Distortion,uv.y)).x;
			col.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x+0.000*_Distortion,uv.y)).y;
			col.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x-0.003*_Distortion,uv.y)).z;


			//col = clamp(col*0.5+0.5*col*col*1.2,0.0,1.0) ;
			col *= 0.5 + 8.0*uv.x*uv.y*(1.0-uv.x)*(1.0-uv.y);
			//col *= float3(0.95,1.05,0.95);
			col *= 0.9+0.1*sin(10.0*_TimeX+uv.y*_Intensity);
			col *= 0.99+0.01*sin(110.0*_TimeX);

			return float4(col,1);
		}
  ENDHLSL


  SubShader
  {
      Cull Off ZWrite Off ZTest Always
      Pass
      {
          HLSLPROGRAM
        //    Cull Off ZWrite Off ZTest Always
		//	CGPROGRAM
			#pragma vertex VertDefault
			#pragma fragment Frag
		//	#pragma fragmentoption ARB_precision_hint_fastest
		//	#pragma target 3.0
		//	#include "UnityCG.cginc"
		//	ENDCG
          ENDHLSL
      }
  }
}