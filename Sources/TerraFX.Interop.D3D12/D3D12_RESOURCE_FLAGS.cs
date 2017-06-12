// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.D3D12
{
    [Flags]
    public enum D3D12_RESOURCE_FLAGS
    {
        NONE = 0,

        ALLOW_RENDER_TARGET = 1,

        ALLOW_DEPTH_STENCIL = 2,

        ALLOW_UNORDERED_ACCESS = 4,

        DENY_SHADER_RESOURCE = 8,

        ALLOW_CROSS_ADAPTER = 16,

        ALLOW_SIMULTANEOUS_ACCESS = 32
    }
}
