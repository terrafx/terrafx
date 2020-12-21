// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "Texture3DTypes.hlsl"

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
    output.position = v4;

    v4 = mul(v4, primitiveTransform);
    v4 = mul(v4, frameTransform);

    output.uvw = v4;

    return output;
}
