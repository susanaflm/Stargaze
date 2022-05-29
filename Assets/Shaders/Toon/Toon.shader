Shader "Coolbeans Studio/Toon"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        [NoScaleOffset] _BaseMap ("Base Map", 2D) = "white" {}
        
        [Space(20)]
        
        _Smoothness ("Smoothness", Range(0, 3)) = 1
        _SpecularColor ("Specular Color", Color) = (1, 1, 1)
        
        [Space(20)]
        
        [NoScaleOffset] _BumpMap ("Normal Map", 2D) = "bump" {}
        
        [Space(20)]
        
        [NoScaleOffset] _EmissionMap ("Emission", 2D) = "black" {}
        
        [Space(20)]
        
        [IntRange] _Banding ("Band Count", Range(1, 10)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalRenderPipeline"
        }
        
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define _MAIN_LIGHT_SHADOWS
            #define _MAIN_LIGHT_SHADOWS_CASCADE
            #define _SHADOWS_SOFT
            #define _ADDITIONAL_LIGHT_SHADOWS
            #define DIRLIGHTMAP_COMBINED

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #include "Assets/Shaders/Toon/ToonLighting.hlsl"

            struct v2f : ToonV2F
            {
                float2 uvBaseMap        : TEXCOORD5;
                float2 uvNormalMap      : TEXCOORD6;
                float2 uvEmissionMap    : TEXCOORD7;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;

            float4 _BaseColor;

            float _Smoothness;
            float3 _SpecularColor;

            sampler2D _BumpMap;
            float4 _BumpMap_ST;

            sampler2D _EmissionMap;
            float4 _EmissionMap_ST;

            int _Banding;

            v2f vert (ToonAppData v)
            {
                v2f o;

                ToonV2F toonOutput;
                ToonVertex(v, toonOutput);
                
                COPY_TOON_PARAMETERS(o, toonOutput);

                // Shader specifics start here
                o.uvBaseMap = TRANSFORM_TEX(v.uv, _BaseMap);
                o.uvNormalMap = TRANSFORM_TEX(v.uv, _BumpMap);
                o.uvEmissionMap = TRANSFORM_TEX(v.uv, _EmissionMap);
                
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 baseColor = tex2D(_BaseMap, i.uvBaseMap) * _BaseColor;
                float4 bumpMapSample = tex2D(_BumpMap, i.uvNormalMap);
                float3 emissionColor = tex2D(_EmissionMap, i.uvEmissionMap).xyz;
                
                return ToonLighting(
                    baseColor, bumpMapSample, emissionColor,
                    _SpecularColor, _Smoothness,
                    _Banding,
                    TOON_PARAMETERS(i)
                );
            }

            ENDHLSL
        }
        
        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
}
