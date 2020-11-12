// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SmokeTypes.hlsl"

Texture3D textureInput : register(t0);
SamplerState samplerInput : register(s0);

float4 main(PSInput input) : SV_Target
{
    float r = 0;
    float g = 0;
    float b = 0;
    float a = 0;
    for (int i = 0; i < 100; i++)
    {
        float4 color = float4(1, 1, 1, 1);
        // get and apply the gray level intensitiy from the single value float texture
        float3 uvw = float3(input.uvw[0], input.uvw[1], (input.scale + i * 0.01) % 1.0);
        color = color * textureInput.Sample(samplerInput, uvw)[0];
        color = color * input.scale * input.scale * 0.5;
        r = r * a + color[0] * (1 - a);
        g = g * a + color[1] * (1 - a);
        b = b * a + color[2] * (1 - a);
        a = 1 - (1 - a) * (1 - color[3]);
    }
    float4 accumulatedColor = float4(r,g,b,a);
    return accumulatedColor;
}
