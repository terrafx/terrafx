// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkPipelineVertexInputStateCreateFlags : uint
    {
        NONE = 0x00000000,

        VERTEX_BIT = 0x00000001,

        TESSELLATION_CONTROL_BIT = 0x00000002,

        TESSELLATION_EVALUATION_BIT = 0x00000004,

        GEOMETRY_BIT = 0x00000008,

        FRAGMENT_BIT = 0x00000010,

        COMPUTE_BIT = 0x00000020,

        ALL_GRAPHICS = 0x0000001F,

        ALL = 0x7FFFFFFF,
    }
}
