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
            float pi = 3.141592;

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
                float relativePos = i.uv.xy - (_ScreenParams.xy / 2);
                float angle = acos(dot(float2(-1, 0), relativePos) / length(relativePos));
                float time = _Time * 4;
                //angle += time * 10 / 360 * pi;

                // sample the texture
                fixed4 col = tex2D(
                    _MainTex,
                    float2(
                        length(relativePos) * sin(angle),
                        length(relativePos) * cos(angle)
                        ) + (_ScreenParams.xy / 2)
                );
                
                //fixed4 col = tex2D(
                //    _MainTex,
                //    i.uv.xy
                //);
                
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
