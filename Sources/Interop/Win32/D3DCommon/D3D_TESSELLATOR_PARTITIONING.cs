// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D_TESSELLATOR_PARTITIONING
    {
        UNDEFINED = 0,

        INTEGER = 1,

        POW2 = 2,

        FRACTIONAL_ODD = 3,

        FRACTIONAL_EVEN = 4
    }
}
