#ifndef LIGHTING_HELPERS_H
#define LIGHTING_HELPERS_H

float Banding(float input, int banding)
{
	if (input == 0)
		return 0;
		
	float threshold = 1.0 / banding;
	int bandIndex = (input / threshold) + 1;

	return  threshold * bandIndex;
}

void MainLight(float4 shadowCoord, out float3 direction, out float3 color, out float distanceAtten, out float shadowAtten)
{
	Light mainLight = GetMainLight(shadowCoord);
	direction = mainLight.direction;
	color = mainLight.color;
	distanceAtten = mainLight.distanceAttenuation;

	ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();

	float shadowStrength = GetMainLightShadowStrength();

	shadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, shadowStrength, false);
}

void AdditionalLights(float3 specColor, float smoothness, float3 worldPosition, float3 worldNormal, float3 worldView, out float3 diffuse, out float3 specular, float banding)
{
	float3 diffuseColor = 0;
	float3 specularColor = 0;

	smoothness = exp2(10 * smoothness + 1);
	worldNormal = normalize(worldNormal);
	worldView = SafeNormalize(worldView);

	int lightCount = GetAdditionalLightsCount();

	for (int i = 0; i < lightCount; i++)
	{
		// Diffuse
		Light light = GetAdditionalLight(i, worldPosition);

		float NdotL = saturate(dot(worldNormal, light.direction));
		float attenuation = light.distanceAttenuation;

		diffuseColor += light.color * Banding(NdotL * attenuation, banding);

		// Specular
		float3 halfVec = SafeNormalize(float3(light.direction) + float3(worldView));
		half NdotH = half(saturate(dot(worldNormal, halfVec)));

		half modifier = Banding(pow(NdotH, smoothness), banding);
		
		half3 specularReflection = specColor * modifier;
		
		specularColor += light.color * specularReflection;
	}
	
	diffuse = diffuseColor;
	specular = specularColor;
}

#endif