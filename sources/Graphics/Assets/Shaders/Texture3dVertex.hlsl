// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

#include "Texture3dTypes.hlsl"

PSInput main(VSInput input)
{
    PSInput output;

    output.position = float4(input.position, 1.0f);
    output.uvw = input.uvw;

    return output;
}
