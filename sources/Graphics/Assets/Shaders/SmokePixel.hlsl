// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SmokeTypes.hlsl"

Texture3D textureInput : register(t0);
SamplerState samplerInput : register(s0);

float4 main(PSInput input) : SV_Target
{
    float4 color = float4(1,1,1,1);
    // get and apply the gray level intensitiy from the single value float texture 
    color = color * textureInput.Sample(samplerInput, input.uvw);
    return color;
}
