// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "SierpinskiTypes.hlsl"
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

    float4 p4 = float4(input.position, 1.0f);
    p4 = mul(p4, primitiveTransform);
    p4 = mul(p4, frameTransform);
    output.position = p4;

    float4 n4 = float4(input.normal, 1.0f);
    n4 = mul(n4, primitiveTransform);
    n4 = mul(n4, frameTransform);
    output.normal = float3(n4[0], n4[1], n4[2]);

    output.uvw = input.uvw;
    return output;
}
