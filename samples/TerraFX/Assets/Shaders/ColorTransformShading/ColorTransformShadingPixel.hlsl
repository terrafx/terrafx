// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "ColorTransformShadingTypes.hlsl"

Texture3D textureInput : register(t0);
SamplerState samplerInput : register(s0);

float4 main(PSInput input) : SV_Target
{
    float4 color = input.color;
    float3 normal = normalize(input.normal);
    float brightness = 0.2f + 0.8f * abs(normal[2]);
    return color * brightness;
}
