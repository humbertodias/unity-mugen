// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnityMugen/Sprites/ColorSwap"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		xPalette("xPalette", 2D) = "transparent" {}
		IsRGBA("IsRGBA", Float) = 0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 1

		Subtraction("Subtraction", Range(0,1)) = 0
		Alpha("Alpha", Range(0,1)) = 1
		BlendOPs("BlendOp",Range(0, 4)) = 0
		//SrcMode ("SrcMode",Range(0, 7)) = 1
		DstMode("DstMode", Range(1, 10)) = 10

		[Header(PalFx)]
		[MaterialToggle] xPalFx_Use("xPalFx_Use", Float) = 0
		xPalFx_Add("xPalFx_Add", Color) = (1,1,1,1)
		xPalFx_Mul("xPalFx_Mul", Color) = (1,1,1,1)
		[MaterialToggle] xPalFx_Invert("xPalFx_Invert", Float) = 0
		xPalFx_Color("xPalFx_Color", Float) = 0
		xPalFx_SinMath("xPalFx_SinMath", Color) = (1,1,1,1)

		[Header(After Image)]
		[MaterialToggle] xAI_Use("xAI_Use", Float) = 0
		[MaterialToggle] xAI_Invert("xAI_Invert", Float) = 0
		xAI_color("xAI_color", Float) = 0
		xAI_preadd("xAI_preadd", Color) = (1,1,1,1)
		xAI_contrast("xAI_contrast", Color) = (1,1,1,1)
		xAI_postadd("xAI_postadd", Color) = (1,1,1,1)
		xAI_paladd("xAI_paladd", Color) = (1,1,1,1)
		xAI_palmul("xAI_palmul", Color) = (1,1,1,1)
		xAI_number("xAI_number", Float) = 0

	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		//BlendOp [BlendOPs]
		Blend One [DstMode]
		
		//Blend One One
		//Blend One OneMinusSrcAlpha
		//Blend OneMinusDstColor One // Soft additive

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color : COLOR;
				half2 texcoord  : TEXCOORD0;
			};

			float4x4 xMatrix;
			float4x4 xRotation;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = mul(mul(IN.texcoord, xMatrix), xRotation); // Novo
				//OUT.texcoord = IN.texcoord;// Antigo
				//OUT.texcoord.y = 1.0 - OUT.texcoord.y;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D xPalette;

			float Subtraction;
			float Alpha;

			float xPalFx_Use;
			float xPalFx_Invert;
			float xPalFx_Color;
			fixed4 xPalFx_Add;
			fixed4 xPalFx_Mul;
			fixed4 xPalFx_SinMath;

			float xAI_Use;
			float xAI_Invert;
			float xAI_color;
			fixed4 xAI_preadd;
			fixed4 xAI_contrast;
			fixed4 xAI_postadd;
			fixed4 xAI_paladd;
			fixed4 xAI_palmul;
			float xAI_number;

			fixed4 BaseColor(fixed4 inputcolor, float base)
			{
				if (base == 0.0f)
				{
					float color = (0.299f * inputcolor.r) + (0.587f * inputcolor.g) + (0.114f * inputcolor.b);
					return float4(color, color, color, inputcolor.a);
				}
				return float4(inputcolor.rgb * base, inputcolor.a);
			}
			fixed4 PalFx(fixed4 inputcolor)
			{
				float4 output = BaseColor(inputcolor, xPalFx_Color);
				if (xPalFx_Invert == true) output = float4(1 - output.rgba);
				output = (output + xPalFx_Add + xPalFx_SinMath) * xPalFx_Mul;
				return float4(output.rgb, inputcolor.a);
			}
			fixed4 AfterImage(fixed4 inputcolor)
			{
				float4 output = BaseColor(inputcolor, xAI_color/*xPalFx_Color*/);
				if (xAI_Invert == true) output = float4(1 - output.rgba);

				output += xAI_preadd;
				output += xAI_postadd;
				output *= xAI_contrast;
				
				[unroll(25)]
				for (int i = 0; i < xAI_number; ++i)
					output = (output + xAI_paladd) * xAI_palmul;
					
				return float4(output.rgb, inputcolor.a);
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float4 textColor = tex2D(_MainTex, IN.texcoord);
				if (textColor.a == 0)
					return float4(0, 0, 0, 0);

				// number color (is 0 a 255) or (0 a 1)
				// indexAtual * maxNumberColor / numeroMaximo de Cores
				// textColor.a * 255 / 256
				float4 output_color = tex2D(xPalette, float2(textColor.a * 255 / 256, 0));

				output_color.rgb = lerp(output_color.rgb, 1 - output_color.rgb, Subtraction);
				output_color *= (Alpha);

				if (xPalFx_Use == true) output_color = PalFx(output_color);
				if (xAI_Use == true) output_color = AfterImage(output_color);

				output_color *= IN.color;
				
				return output_color;
			}
			ENDCG
		}

		//Pass
		//{
		//	//Cull Front
		//	Blend One OneMinusSrcAlpha
		//	CGPROGRAM
		//	#pragma vertex vert
		//	#pragma fragment frag
		//	#include "UnityCG.cginc"

		//	struct appdata_t2
		//	{
		//		float4 vertex   : POSITION;
		//		float4 color    : COLOR;
		//		float2 texcoord : TEXCOORD0;
		//	};

		//	struct v2f
		//	{
		//		float4 vertex   : SV_POSITION;
		//		fixed4 color : COLOR;
		//		half2 texcoord  : TEXCOORD0;
		//	};

		//	v2f vert(appdata_t2 IN)
		//	{
		//		v2f OUT;
		//		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		//		OUT.texcoord = IN.texcoord;
		//		OUT.color = IN.color;
		//		return OUT;
		//	}

		//	sampler2D _MainTex;
			
		//	fixed4 frag(v2f IN) : SV_Target
		//	{
		//		float4 textColor = tex2D(_MainTex, IN.texcoord);
		//		//if (textColor.a == 0)
		//		//	return float4(0, 0, 0, 0);

		//		//textColor *= IN.color;
		//		return textColor;
		//	}
		//	ENDCG
		//}
	}
}
