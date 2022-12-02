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
				//float2 uv : TEXCOORD0;
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
				i.screenPos /= i.screenPos.w;
				fixed4 col = tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));
				//col.rgb = 1 - col.rgb;
				return col;

				//float t = _Time * 100.0;
				//float2 uv = i.vertex / _ScreenParams.xy;
				//float2 p0 = (uv - 0.5) * _ScreenParams.xy;
				//float2 hvp = _ScreenParams.xy * 0.5;
				//float2 p1d = float2(cos(t / 98.0), sin(t / 178.0)) * hvp - p0;
				//float2 p2d = float2(sin(-t / 124.0), cos(-t / 104.0)) * hvp - p0;
				//float2 p3d = float2(cos(-t / 165.0), cos(t / 45.0)) * hvp - p0;
				//float sum = 0.5 + 0.5 * (
				//	cos(length(p1d) / 30.0) +
				//	cos(length(p2d) / 20.0) +
				//	sin(length(p3d) / 25.0) * sin(p3d.x / 20.0) * sin(p3d.y / 15.0));
				//fixed4 col = tex2D(_MainTex, float2(frac(sum), 0));
				//return col;
			}
			ENDCG
		}
	}
}
