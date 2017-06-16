// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPrimitiveTopology
    {
        POINT_LIST = 0,

        LINE_LIST = 1,

        LINE_STRIP = 2,

        TRIANGLE_LIST = 3,

        TRIANGLE_STRIP = 4,

        TRIANGLE_FAN = 5,

        LINE_LIST_WITH_ADJACENCY = 6,

        LINE_STRIP_WITH_ADJACENCY = 7,

        TRIANGLE_LIST_WITH_ADJACENCY = 8,

        TRIANGLE_STRIP_WITH_ADJACENCY = 9,

        PATCH_LIST = 10
    }
}
