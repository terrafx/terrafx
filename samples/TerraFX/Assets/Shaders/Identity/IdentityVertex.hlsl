// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "IdentityTypes.hlsl"

PSInput main(VSInput input)
{
    PSInput output;

    output.color = input.color;
    output.position = float4(input.position, 1.0f);

    return output;
}
