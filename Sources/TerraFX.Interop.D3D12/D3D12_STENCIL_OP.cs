// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.
namespace TerraFX.Interop.D3D12
{
    public enum D3D12_STENCIL_OP
    {
        UNDEFINED = 0,

        KEEP = 1,

        ZERO = 2,

        REPLACE = 3,

        INCR_SAT = 4,

        DECR_SAT = 5,

        INVERT = 6,

        INCR = 7,

        DECR = 8
    }
}
