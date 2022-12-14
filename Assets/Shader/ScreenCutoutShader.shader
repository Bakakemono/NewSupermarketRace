// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ScreenCutoutShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Lighting Off
		Cull Back
		ZWrite On
		ZTest Less
		
		Fog{ Mode Off }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 center = float2(0.5, 0.5f);
				float speed = 1.0f;
				float amplitude = 0.05f;
				float time = _Time * 100.0f;
				// Calculate the distance from the current UV to the center of the image
				float dist = length(i.screenPos.xy - center);
				float ampModifier = 0.5f - dist;

				// Calculate the water distortion effect using a sine wave
				i.screenPos.x += cos(dist * speed + time) * amplitude * ampModifier;
				i.screenPos.y += sin(dist * speed + time) * amplitude * ampModifier;

				i.screenPos /= i.screenPos.w;
				fixed4 color = tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));
				//col.rgb = 1 - col.rgb;
				return color;
			}
			ENDCG
		}
	}
}
