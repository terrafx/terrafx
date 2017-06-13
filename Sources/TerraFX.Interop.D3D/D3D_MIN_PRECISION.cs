// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D_MIN_PRECISION
    {
        DEFAULT = 0,

        FLOAT_16 = 1,

        FLOAT_2_8 = 2,

        RESERVED = 3,

        SINT_16 = 4,

        UINT_16 = 5,

        ANY_16 = 240,

        ANY_10 = 241
    }
}
