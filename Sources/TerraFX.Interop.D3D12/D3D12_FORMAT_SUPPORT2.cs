// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D12_FORMAT_SUPPORT2
    {
        NONE = 0,

        UAV_ATOMIC_ADD = 1,

        UAV_ATOMIC_BITWISE_OPS = 2,

        UAV_ATOMIC_COMPARE_STORE_OR_COMPARE_EXCHANGE = 4,

        UAV_ATOMIC_EXCHANGE = 8,

        UAV_ATOMIC_SIGNED_MIN_OR_MAX = 16,

        UAV_ATOMIC_UNSIGNED_MIN_OR_MAX = 32,

        UAV_TYPED_LOAD = 64,

        UAV_TYPED_STORE = 128,

        OUTPUT_MERGER_LOGIC_OP = 256,

        TILED = 512,

        MULTIPLANE_OVERLAY = 16384
    }
}
