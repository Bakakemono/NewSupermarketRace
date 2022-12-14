Shader "Custom/FunnySpiral"
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
            static const float maxDist = 0.707106f;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5f, 0.5f);
                float speed = 10.0f;
                float amplitude = 0.05f;
                float time = _Time * 100.0f;
                // Calculate the distance from the current UV to the center of the image
                float dist = length(i.uv.xy - center);
                float ampModifier = (maxDist - dist) / maxDist;

                // Calculate the water distortion effect using a sine wave
                float2 displacement =
                    float2(
                        cos(dist * speed + time) * amplitude * ampModifier,
                        sin(dist * speed + time) * amplitude * ampModifier
                        );

                fixed4 col = tex2D(
                    _MainTex,
                    i.uv.xy + displacement
                );
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
