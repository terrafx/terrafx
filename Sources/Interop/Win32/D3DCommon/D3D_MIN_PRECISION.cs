// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D_MIN_PRECISION
    {
        DEFAULT = 0,        // Default precision for the shader model

        FLOAT_16 = 1,       // Min 16 bit/component float

        FLOAT_2_8 = 2,      // Min 10(2.8)bit/comp. float

        RESERVED = 3,       // Reserved for future use

        SINT_16 = 4,        // Min 16 bit/comp. signed integer

        UINT_16 = 5,        // Min 16 bit/comp. unsigned integer

        // These values are abstractions of width only for use in situations
        // where a general width is needed instead of specific types. They
        // will never be used in shader operands.
        ANY_16 = 240,

        ANY_10 = 241
    }
}
