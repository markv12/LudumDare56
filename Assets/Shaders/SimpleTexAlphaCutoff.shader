Shader "Custom/SimpleTexAlphaCutoff" {
    Properties {
        _MainTex ("BaseMap", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader {
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fog

            sampler2D _MainTex;
            float4 _Color;

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 texcoord : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                half4 col = tex2D(_MainTex, i.texcoord) * _Color;
                clip(col.a - 0.5);
                return col;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
