// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "InstancingTypes.hlsl"

cbuffer PerFrameInput : register(b0)
{
    matrix frameTransform;
};

cbuffer PerPrimitiveInput : register(b1)
{
    matrix primitiveTransform[128];
};

PSInput main(VSInput input)
{
    PSInput output;

    output.color = input.color;

    output.position = float4(input.position, 1.0f);
    output.position = mul(output.position, primitiveTransform[input.instance]);
    output.position = mul(output.position, frameTransform);

    return output;
}
