// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D12_RESOURCE_STATES
    {
        COMMON = 0,

        PRESENT = 0,

        VERTEX_AND_CONSTANT_BUFFER = 1,

        INDEX_BUFFER = 2,

        RENDER_TARGET = 4,

        UNORDERED_ACCESS = 8,

        DEPTH_WRITE = 16,

        DEPTH_READ = 32,

        NON_PIXEL_SHADER_RESOURCE = 64,

        PIXEL_SHADER_RESOURCE = 128,

        STREAM_OUT = 256,

        INDIRECT_ARGUMENT = 512,

        PREDICATION = 512,

        COPY_DEST = 1024,

        COPY_SOURCE = 2048,

        GENERIC_READ = 2755,

        RESOLVE_DEST = 4096,

        RESOLVE_SOURCE = 8192
    }
}
