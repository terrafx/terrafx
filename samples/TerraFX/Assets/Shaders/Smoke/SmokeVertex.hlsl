// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SmokeTypes.hlsl"
cbuffer PerFrameInput : register(b0)
{
    matrix frameTransform;
};

cbuffer PerPrimitiveInput : register(b1)
{
    matrix primitiveTransform;
};

PSInput main(VSInput input)
{
    PSInput output;

    float4 v4 = float4(input.position, 1.0f);
    // the quad position does not change
    output.position = v4;
    // the scale dops of as the smoke rises from bottom to top
    output.scale = (1.0f - (v4[1] + 0.5f));

    // the texture coordinates are animated to have the rising smoke visual effect
    v4 = mul(v4, primitiveTransform);
    v4 = mul(v4, frameTransform);
    output.uvw = v4;
 
    return output;
}
