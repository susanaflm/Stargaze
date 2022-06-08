#ifndef TOON_LIGHTING_H
#define TOON_LIGHTING_H

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

#include "Assets/Shaders/Toon/LightingHelpers.hlsl"

#define COPY_TOON_PARAMETERS(output, toonOutput)	output.vertex = toonOutput.vertex; \
													output.viewDir = toonOutput.viewDir; \
													output.worldPosition = toonOutput.worldPosition; \
													output.shadowCoord = toonOutput.shadowCoord; \
													output.worldNormal = toonOutput.worldNormal; \
													output.worldTangent = toonOutput.worldTangent; \
													output.worldBinormal = toonOutput.worldBinormal;

#define TOON_PARAMETERS(input) input.worldTangent, input.worldBinormal, input.worldNormal, input.worldPosition, input.viewDir, input.shadowCoord

struct ToonAppData
{
	float4 vertex   : POSITION;
	float2 uv       : TEXCOORD0;
	float3 normal   : NORMAL;
	float4 tangent  : TANGENT;
};

struct ToonV2F
{
	float4 vertex           : SV_POSITION;

	float3 viewDir          : TEXCOORD0;

	float3 worldPosition    : POSITION1;
	
	float4 shadowCoord      : TEXCOORD1;
	float3 worldNormal      : TEXCOORD2;
	float4 worldTangent     : TEXCOORD3;
	float3 worldBinormal    : TEXCOORD4;
};

void ToonVertex(ToonAppData input, out ToonV2F output)
{
	output.vertex = TransformObjectToHClip(input.vertex.xyz);

	output.viewDir = GetWorldSpaceViewDir(input.vertex.xyz);
                
	output.worldPosition = TransformObjectToWorld(input.vertex.xyz);
	
	VertexPositionInputs vertexInputs = GetVertexPositionInputs(input.vertex.xyz);
	output.shadowCoord = GetShadowCoord(vertexInputs);
	
	output.worldNormal = TransformObjectToWorldNormal(input.normal);
	output.worldTangent = normalize(mul(input.tangent, GetWorldToObjectMatrix()));
	output.worldBinormal = normalize(cross(output.worldNormal, output.worldTangent.xyz) * input.tangent.w);
}

float3 CalculateDirectionLighting(float4 shadowCoord, float3 worldNormal, int banding)
{
	float3 mainLightDir;
	float3 mainLightColor;
	float mainLightDistanceAtten;
	float mainLightShadowAtten;

	MainLight(shadowCoord, mainLightDir, mainLightColor, mainLightDistanceAtten, mainLightShadowAtten);
                
	float NdotL = saturate(dot(worldNormal, mainLightDir));
	float power = NdotL * mainLightDistanceAtten * mainLightShadowAtten;
	
	return  mainLightColor * Banding(power, banding);
}

float3 CalculateAdditionalLighting(float3 specColor, float smoothness, float3 worldPosition, float3 worldNormal, float3 viewDir, int banding)
{
	float3 diff;
	float3 spec;

	AdditionalLights(specColor, 3 - smoothness, worldPosition, worldNormal, viewDir, diff, spec, banding);

	return diff + spec;
}

float4 ToonLighting(float4 baseColor, float4 bumpMapSample, float3 emissionColor, float3 specularColor, float smoothness, int banding, float4 worldTangent, float3 worldBinormal, float3 worldNormal, float3 worldPosition, float3 viewDir, float4 shadowCoord)
{
	float3 normalMap = UnpackNormal(bumpMapSample);

	float3x3 TBN_Matrix = float3x3
	(
		worldTangent.xyz,
		worldBinormal,
		worldNormal
	);

	float3 normal = normalize(mul(normalMap, TBN_Matrix));
                
	// Directional Lighting
	float3 directional = CalculateDirectionLighting(shadowCoord, normal, banding);

	// Additional Lighting
	float3 additional = CalculateAdditionalLighting(specularColor, smoothness, worldPosition, normal, viewDir, banding);

	// Final Calculations
	return baseColor * float4(directional + additional + _GlossyEnvironmentColor.rgb, 1) + float4(emissionColor, 0);
}

#endif