// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.D3D12
{
    [Flags]
    public enum D3D12_HEAP_FLAGS
    {
        ALLOW_ALL_BUFFERS_AND_TEXTURES = 0,

        NONE = 0,

        SHARED = 1,

        DENY_BUFFERS = 4,

        ALLOW_DISPLAY = 8,

        SHARED_CROSS_ADAPTER = 32,

        DENY_RT_DS_TEXTURES = 64,

        ALLOW_ONLY_NON_RT_DS_TEXTURES = 68,

        DENY_NON_RT_DS_TEXTURES = 128,

        ALLOW_ONLY_RT_DS_TEXTURES = 132,

        ALLOW_ONLY_BUFFERS = 192,

        HARDWARE_PROTECTED = 256,

        ALLOW_WRITE_WATCH = 512
    }
}
