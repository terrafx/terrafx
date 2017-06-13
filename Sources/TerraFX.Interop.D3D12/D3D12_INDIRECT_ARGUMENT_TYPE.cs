// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D12_INDIRECT_ARGUMENT_TYPE
    {
        DRAW = 0,

        DRAW_INDEXED = 1,

        DISPATCH = 2,

        VERTEX_BUFFER_VIEW = 3,

        INDEX_BUFFER_VIEW = 4,

        CONSTANT = 5,

        CONSTANT_BUFFER_VIEW = 6,

        SHADER_RESOURCE_VIEW = 7,

        UNORDERED_ACCESS_VIEW = 8
    }
}
