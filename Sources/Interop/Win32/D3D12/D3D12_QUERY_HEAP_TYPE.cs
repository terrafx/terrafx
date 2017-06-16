// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public enum D3D12_QUERY_HEAP_TYPE
    {
        OCCLUSION = 0,

        TIMESTAMP = 1,

        PIPELINE_STATISTICS = 2,

        SO_STATISTICS = 3
    }
}
