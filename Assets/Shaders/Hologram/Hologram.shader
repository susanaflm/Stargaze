Shader "Unlit/Hologram"
{
    Properties
    {
        [NoScaleOffset] _BaseMap ("Base Map", 2D) = "white" { }
        
        [Space(20)]
        
        _SlideSpeed ("Slide Speed", float) = 1
        
        [Space(20)]
        
        [Toggle] _HardTransition ("Hard Transition", int) = 0
        _HardTransitionThreshold ("Hard Transition Threshold", Range(0, 1)) = 0.5
        
        [Space(20)]
        
        _Bands ("Bands", float) = 20
        
        [Space(20)]
        
        _MinOpacity ("Minimum Opacity", Range(0, 1)) = 0.5
        _MaxOpacity ("Maximum Opacity", Range(0, 1)) = 1
        
        [Space(20)]
        
        _Tint ("Tint Color", Color) = (1, 1, 1, 1)
        
        [Space(20)]
        
        _BorderSize ("Boder Size", float) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderQueue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            
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

                float4 positionOS   : TEXCOORD0;

                float2 baseMapUV    : TEXCOORD1;
            };

            sampler2D _BaseMap;
            float4 _BaseMap_ST;
            float4 _BaseMap_TexelSize;

            float _SlideSpeed;
            float _Bands;

            float _MinOpacity;
            float _MaxOpacity;

            float4 _Tint;

            float _BorderSize;

            int _HardTransition;
            float _HardTransitionThreshold;

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);

                output.positionOS = input.positionOS;

                output.baseMapUV = TRANSFORM_TEX(input.uv, _BaseMap);
                
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 sample = tex2D(_BaseMap, input.baseMapUV);

                float verticalSliderOpacity = 1 - frac((input.positionOS.y - (_Time.x * _SlideSpeed)) * _Bands);

                if (_HardTransition)
                {
                    verticalSliderOpacity = verticalSliderOpacity > _HardTransitionThreshold ? _MaxOpacity : _MinOpacity;
                }
                else
                {
                    verticalSliderOpacity = _MinOpacity + (_MaxOpacity - _MinOpacity) * verticalSliderOpacity;
                }

                float2 texelPos = _BaseMap_TexelSize.zw * input.baseMapUV;

                float verticalBorderOpacity = 1;
                float horizontalBorderOpacity = 1;
                
                if (texelPos.x < _BorderSize)
                {
                    verticalBorderOpacity = texelPos.x / _BorderSize;
                }
                else if (texelPos.x > _BaseMap_TexelSize.z - _BorderSize)
                {
                    verticalBorderOpacity = 1 - (texelPos.x - (_BaseMap_TexelSize.z - _BorderSize)) / _BorderSize;
                }

                if (texelPos.y < _BorderSize)
                {
                    horizontalBorderOpacity = texelPos.y / _BorderSize;
                }
                else if (texelPos.y > _BaseMap_TexelSize.w - _BorderSize)
                {
                    horizontalBorderOpacity = 1 - (texelPos.y - (_BaseMap_TexelSize.w - _BorderSize)) / _BorderSize;
                }

                sample.a = verticalSliderOpacity * verticalBorderOpacity * horizontalBorderOpacity;
                
                return sample * _Tint;
            }
            ENDHLSL
        }
    }
}
