// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.D3D
{
    [Flags]
    public enum D3D_SHADER_REQUIRES_FLAGS
    {
        NONE = 0x00000000,

        DOUBLES = 0x00000001,

        EARLY_DEPTH_STENCIL = 0x00000002,

        UAVS_AT_EVERY_STAGE = 0x00000004,

        _64_UAVS = 0x00000008,

        MINIMUM_PRECISION = 0x00000010,

        _11_1_DOUBLE_EXTENSIONS = 0x00000020,

        _11_1_SHADER_EXTENSIONS = 0x00000040,

        LEVEL_9_COMPARISON_FILTERING = 0x00000080,

        TILED_RESOURCES = 0x00000100,

        STENCIL_REF = 0x00000200,

        INNER_COVERAGE = 0x00000400,

        TYPED_UAV_LOAD_ADDITIONAL_FORMATS = 0x00000800,

        ROVS = 0x00001000,

        VIEWPORT_AND_RT_ARRAY_INDEX_FROM_ANY_SHADER_FEEDING_RASTERIZER = 0x00002000
    }
}
