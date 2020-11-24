// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

struct VSInput
{
    float3 position : POSITION;
    float3 normal : NORMAL;
    float3 uvw : TEXCOORD;
};

struct PSInput
{
    float4 position : SV_Position;
    float3 normal : NORMAL;
    float3 uvw : TEXCOORD;
};
