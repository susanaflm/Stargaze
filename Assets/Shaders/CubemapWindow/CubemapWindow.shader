Shader "Unlit/CubemapWindow"
{
    Properties
    {
        _CubeMap ("Cube Map", Cube) = "white" {}
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
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float3 viewDir      : TEXCOORD0;
            };

            samplerCUBE _CubeMap;

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.viewDir = GetWorldSpaceViewDir(mul(unity_ObjectToWorld, input.positionOS));

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 sample = texCUBE(_CubeMap, input.viewDir);
                return sample;
            }
            ENDHLSL
        }
    }
}
