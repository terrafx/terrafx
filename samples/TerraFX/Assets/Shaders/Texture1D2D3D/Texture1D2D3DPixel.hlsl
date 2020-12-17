// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "Texture1D2D3DTypes.hlsl"

Texture1D texture1DInput : register(t0);
Texture2D texture2DInput : register(t1);
Texture3D texture3DInput : register(t2);
SamplerState sampler1DInput : register(s0);
SamplerState sampler2DInput : register(s1);
SamplerState sampler3DInput : register(s2);

float4 main(PSInput input) : SV_Target
{
    float4 texel1D = texture1DInput.Sample(sampler1DInput, input.uvw[0]);
    float4 texel2D = texture2DInput.Sample(sampler2DInput, float2(input.uvw[0], input.uvw[1]));
    float4 texel3D = texture3DInput.Sample(sampler3DInput, input.uvw);

    float4 texel = texel3D * (1-texel2D[3]) + texel2D * texel2D[3];
    float d = abs(input.uvw[0] - input.uvw[1]);

    if (d < 0.1) {
        texel = texel * (10 * d) + texel1D * (1 - 10 * d);
    }

    return texel;
}
