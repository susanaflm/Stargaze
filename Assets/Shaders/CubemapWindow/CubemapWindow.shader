Shader "Unlit/CubemapWindow"
{
    Properties
    {
        [NoScaleOffset] _BaseMap ("Base Map", 2D) = "white" {}
        
        _BaseMapPower ("Base Map Power", Range(0, 1)) = 1
        
        [NoScaleOffset] _CubeMap ("Cube Map", Cube) = "white" {}
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;

                float2 baseMapUV    : TEXCOORD0;
                
                float3 viewDir      : TEXCOORD1;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            
            samplerCUBE _CubeMap;

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                output.positionHCS = TransformObjectToHClip(input.positionOS);

                output.baseMapUV = TRANSFORM_TEX(input.uv, _BaseMap);
                
                output.viewDir = GetWorldSpaceViewDir(mul(unity_ObjectToWorld, input.positionOS));

                return output;
            }

            float _BaseMapPower;

            float3 frag(Varyings input) : SV_Target
            {
                float4 baseMapSample = tex2D(_BaseMap, input.baseMapUV);
                float4 cubeSample = texCUBE(_CubeMap, input.viewDir);
                
                return (baseMapSample.rgb * baseMapSample.a * _BaseMapPower) + cubeSample.rgb;
            }
            ENDHLSL
        }
    }
}
