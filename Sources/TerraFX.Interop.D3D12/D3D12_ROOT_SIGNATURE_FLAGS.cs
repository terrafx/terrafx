// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D12_ROOT_SIGNATURE_FLAGS
    {
        NONE = 0,

        ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT = 1,

        DENY_VERTEX_SHADER_ROOT_ACCESS = 2,

        DENY_HULL_SHADER_ROOT_ACCESS = 4,

        DENY_DOMAIN_SHADER_ROOT_ACCESS = 8,

        DENY_GEOMETRY_SHADER_ROOT_ACCESS = 16,

        DENY_PIXEL_SHADER_ROOT_ACCESS = 32,

        ALLOW_STREAM_OUTPUT = 64
    }
}
