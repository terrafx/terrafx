// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SmokeTypes.hlsl"

Texture3D textureInput : register(t0);
SamplerState samplerInput : register(s0);

float4 main(PSInput input) : SV_Target
{
    float4 color = float4(1,1,1,1);
    // get and apply the gray level intensitiy from the single value float texture
    float3 uvw = float3(input.uvw[0], input.uvw[1], 0.5);
    color = color * textureInput.Sample(samplerInput, uvw)[0];
    color = color * input.scale * input.scale * 0.5;
    return color;
}
