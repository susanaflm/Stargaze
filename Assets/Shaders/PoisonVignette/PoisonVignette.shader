Shader "Unlit/PoisonVignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _InnerRing ("Inner Ring", Range (0,1)) = 0.0
        _OuterRing ("Outer Ring", Range (0,1)) = 1.0
        _VignetteColor("Vignette Color", Color) = (0,1,0,1)
        _VignettePosition ("Center Position", Vector) = (0.5,0.5,1,1)
        _VignetteTexture ("Vignette Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalRenderPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 vignetteUV : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 vignetteUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _VignetteTexture;
            float4 _VignetteTexture_ST;
            float _InnerRing;
            float _OuterRing;
            float4 _VignetteColor;
            half4 _VignettePosition;


            ///Performs a linear interpolation between x and y using a weight between them
            float4 mix(float4 x, float4 y, float weight) //GLSL function mix()
            {
                return x *(1 - weight) + y * weight;
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.vignetteUV = TRANSFORM_TEX(v.vignetteUV, _VignetteTexture);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                half4 vignetteTexture = tex2D(_VignetteTexture, i.vignetteUV);
                
                //fixed4 col = _VignetteColor;
                half2 center = half2(_VignettePosition.x, _VignettePosition.y);
                float dist = distance(center, i.uv) * 1.414213;
                //float vig = clamp((_OuterRing - dist) / (_OuterRing - _InnerRing), 0.0, 1.0);
                float vig = smoothstep(_InnerRing, _OuterRing, dist);
                
                col = mix(col, _VignetteColor * vignetteTexture, vig);
                return col;
            }
            ENDHLSL
        }
    }
}
