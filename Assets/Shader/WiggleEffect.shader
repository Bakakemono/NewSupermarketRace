Shader "Custom/WiggleEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            static const float MAX_DIST = 0.707106f;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

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

                fixed4 color =
                    tex2D(
                        _MainTex,
                        i.uv.xy + displacement
                    );
                return color;
            }
            ENDCG
        }
    }
}
