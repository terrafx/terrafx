// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkQueryPipelineStatisticFlags : uint
    {
        NONE = 0x00000000,

        INPUT_ASSEMBLY_VERTICES_BIT = 0x00000001,

        INPUT_ASSEMBLY_PRIMITIVES_BIT = 0x00000002,

        VERTEX_SHADER_INVOCATIONS_BIT = 0x00000004,

        GEOMETRY_SHADER_INVOCATIONS_BIT = 0x00000008,

        GEOMETRY_SHADER_PRIMITIVES_BIT = 0x00000010,

        CLIPPING_INVOCATIONS_BIT = 0x00000020,

        CLIPPING_PRIMITIVES_BIT = 0x00000040,

        FRAGMENT_SHADER_INVOCATIONS_BIT = 0x00000080,

        TESSELLATION_CONTROL_SHADER_PATCHES_BIT = 0x00000100,

        TESSELLATION_EVALUATION_SHADER_INVOCATIONS_BIT = 0x00000200,

        COMPUTE_SHADER_INVOCATIONS_BIT = 0x00000400
    }
}
