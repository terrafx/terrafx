// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.
namespace TerraFX.Interop
{
    public enum D3D12_LOGIC_OP
    {
        CLEAR = 0,

        SET = 1,

        COPY = 2,

        COPY_INVERTED = 3,

        NOOP = 4,

        INVERT = 5,

        AND = 6,

        NAND = 7,

        OR = 8,

        NOR = 9,

        XOR = 10,

        EQUIV = 11,

        AND_REVERSE = 12,

        AND_INVERTED = 13,

        OR_REVERSE = 14,

        OR_INVERTED = 15
    }
}
