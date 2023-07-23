Shader "Hidden/UnityMugen/2xsal"
{
  HLSLINCLUDE


        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
      
        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 texture_size = float2(350, 200);
			float2 fp = (2.0 * frac(i.texcoord*texture_size));

			float2 texsize = texture_size;
			float dx = pow(texsize.x, -1.0) * 0.25;
			float dy = pow(texsize.y, -1.0) * 0.25;
			float3 dt = float3(1.0, 1.0, 1.0);

			float2 UL = i.texcoord + float2(-dx,    -dy);
			float2 UR = i.texcoord + float2( dx,     -dy);
			float2 DL = i.texcoord + float2(-dx,     dy);
			float2 DR = i.texcoord + float2( dx,     dy);

            //float4 c00 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            float3 c00 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UL).xyz;
            float3 c20 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UR).xyz;
            float3 c02 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, DL).xyz;
            float3 c22 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, DR).xyz;

			float m1=dot(abs(c00-c22),dt)+0.001;
			float m2=dot(abs(c02-c20),dt)+0.001;

			return float4((m1*(c02+c20)+m2*(c22+c00))/(2.0*(m1+m2)), 1.0);

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
