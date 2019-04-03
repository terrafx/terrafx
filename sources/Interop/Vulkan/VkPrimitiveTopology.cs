// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPrimitiveTopology
    {
        VK_PRIMITIVE_TOPOLOGY_POINT_LIST = 0,

        VK_PRIMITIVE_TOPOLOGY_LINE_LIST = 1,

        VK_PRIMITIVE_TOPOLOGY_LINE_STRIP = 2,

        VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST = 3,

        VK_PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP = 4,

        VK_PRIMITIVE_TOPOLOGY_TRIANGLE_FAN = 5,

        VK_PRIMITIVE_TOPOLOGY_LINE_LIST_WITH_ADJACENCY = 6,

        VK_PRIMITIVE_TOPOLOGY_LINE_STRIP_WITH_ADJACENCY = 7,

        VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST_WITH_ADJACENCY = 8,

        VK_PRIMITIVE_TOPOLOGY_TRIANGLE_STRIP_WITH_ADJACENCY = 9,

        VK_PRIMITIVE_TOPOLOGY_PATCH_LIST = 10,

        VK_PRIMITIVE_TOPOLOGY_BEGIN_RANGE = VK_PRIMITIVE_TOPOLOGY_POINT_LIST,

        VK_PRIMITIVE_TOPOLOGY_END_RANGE = VK_PRIMITIVE_TOPOLOGY_PATCH_LIST,

        VK_PRIMITIVE_TOPOLOGY_RANGE_SIZE = VK_PRIMITIVE_TOPOLOGY_PATCH_LIST - VK_PRIMITIVE_TOPOLOGY_POINT_LIST + 1,

        VK_PRIMITIVE_TOPOLOGY_MAX_ENUM = 0x7FFFFFFF
    }
}
