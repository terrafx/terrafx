// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

struct VSInput
{
    float4 color : COLOR;
    float3 position : POSITION;
};

struct PSInput
{
    float4 color : COLOR;
    float4 position : SV_Position;
};
