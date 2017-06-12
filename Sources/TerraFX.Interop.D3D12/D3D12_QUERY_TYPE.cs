// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D12
{
    public enum D3D12_QUERY_TYPE
    {
        OCCLUSION = 0,

        BINARY_OCCLUSION = 1,

        TIMESTAMP = 2,

        PIPELINE_STATISTICS = 3,

        SO_STATISTICS_STREAM0 = 4,

        SO_STATISTICS_STREAM1 = 5,

        SO_STATISTICS_STREAM2 = 6,

        SO_STATISTICS_STREAM3 = 7
    }
}
