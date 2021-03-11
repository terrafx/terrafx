// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "ColorTransformShadingTypes.hlsl"

cbuffer PerPrimitiveInput : register(b0)
{
    matrix primitiveTransform;
};

cbuffer PerFrameInput : register(b1)
{
    matrix frameTransform;
};

PSInput main(VSInput input)
{
    PSInput output;

    float4 p4 = float4(input.position, 1.0f);
    p4 = mul(p4, primitiveTransform);
    p4 = mul(p4, frameTransform);
    output.position = p4;

    float4 n4 = float4(input.normal, 0.0f);
    n4 = mul(n4, primitiveTransform);
    output.normal = float3(n4[0], n4[1], n4[2]);

    output.color = input.color;
    return output;
}
