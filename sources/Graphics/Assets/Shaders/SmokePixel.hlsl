// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SmokeTypes.hlsl"

Texture3D textureInput : register(t0);
SamplerState samplerInput : register(s0);

float4 main(PSInput input) : SV_Target
{
    float r = 0.0f;
    float g = 0.0f;
    float b = 0.0f;
    float a = 0.0f;
    for (int i = 0; i < 100; i++)
    {
        // get and apply the gray level intensitiy from the single value float texture
        float3 uvw = float3(input.uvw[0], input.uvw[1], (input.uvw[2] + i * 0.01f) % 1.0f);
        float4 texel = textureInput.Sample(samplerInput, uvw);
        float4 color = texel[0] * float4(1.0f, 1.0f, 1.0f, 1.0f);
        float4 scale = input.scale;
        color = color * scale * scale * 0.5f;
        r = r * a + color[0] * (1.0f - a);
        g = g * a + color[1] * (1.0f - a);
        b = b * a + color[2] * (1.0f - a);
        a = 1.0f - (1.0f - a) * (1.0f - color[3]);
    }
    float4 accumulatedColor = float4(r,g,b,a);
    return accumulatedColor;
}
