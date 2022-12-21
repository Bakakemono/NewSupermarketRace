// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ScreenCutoutShader"
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
			static const float MAX_DIST = 0.707106f;

			fixed4 frag (v2f i) : SV_Target
			{
				float2 center = float2(0.5f, 0.5f);
				float speed = 10.0f;
				float amplitude = 0.05f;
				float time = _Time * 10.0f;
				// Calculate the distance from the current UV to the center of the image
				//float dist = length(i.screenPos.xy - center);
				float dist = length(i.uv.xy - center);
				float ampModifier = (MAX_DIST - dist) / MAX_DIST;

				// Calculate the water distortion effect using a sine wave
				float2 displacement = 
					float2(
						sin(dist * speed + time) * amplitude * ampModifier,
						cos(dist * speed + time) * amplitude * ampModifier
						);

				i.screenPos /= i.screenPos.w;
				//fixed4 color = 
				//	tex2D(
				//		_MainTex,
				//		float2(i.screenPos.x, i.screenPos.y) + displacement
				//	);
				fixed4 color =
					tex2D(
						_MainTex,
						float2(i.screenPos.x, i.screenPos.y) + displacement
					);
				//fixed4 color = fixed4(ampModifier, 0, 0, 0);
				//fixed4 color = fixed4(ampModifier, 0, 0, 0);
				//if (dist < 0.7071) {
				//	color = fixed4(1, 0, 0, 0);
				//}
				//else {
				//	color = fixed4(0, 0, 0, 0);
				//}
				
				//col.rgb = 1 - col.rgb;
				return color;
			}
			ENDCG
		}
	}
}
