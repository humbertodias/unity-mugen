// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "UnityMugen/Sprites/Shadow" {
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("_Color", Color) = (1,1,1,1)
		_Pos("_Pos", Float) = 0
	}

		SubShader{
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
			Blend One OneMinusSrcAlpha

		   Pass {
			   Stencil {
				   Ref 2
				   Comp NotEqual
				   Pass Replace
			   }

				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;

				struct v2f {
					half4 pos : POSITION;
					fixed4 color : COLOR;
					half2 uv : TEXCOORD0;
				};

				fixed4 _Color;

				v2f vert(appdata_img v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					half2 uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
					o.uv = uv;
					return o;
				}

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

					#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
					if (_AlphaSplitEnabled)
						color.a = tex2D(_AlphaTex, uv).r;
					#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

					return color;
				}

				half4 frag(v2f i) : COLOR {
					half4 color = tex2D(_MainTex, i.uv);
					if (color.a == 0.0)
						discard;
					else {
						color.rgb = 0;
						color.rgba = _Color.rgba;
					}

					return color;
				}
				ENDCG
		   }

	}

		Fallback off
}