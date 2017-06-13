// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D_SHADER_INPUT_TYPE
    {
        CBUFFER = 0,

        TBUFFER = 1,

        TEXTURE = 2,

        SAMPLER = 3,

        UAV_RWTYPED = 4,

        STRUCTURED = 5,

        UAV_RWSTRUCTURED = 6,

        BYTEADDRESS = 7,

        UAV_RWBYTEADDRESS = 8,

        UAV_APPEND_STRUCTURED = 9,

        UAV_CONSUME_STRUCTURED = 10,

        UAV_RWSTRUCTURED_WITH_COUNTER = 11
    }
}
