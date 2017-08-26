// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPolygonMode
    {
        VK_POLYGON_MODE_FILL = 0,

        VK_POLYGON_MODE_LINE = 1,

        VK_POLYGON_MODE_POINT = 2,

        VK_POLYGON_MODE_BEGIN_RANGE = VK_POLYGON_MODE_FILL,

        VK_POLYGON_MODE_END_RANGE = VK_POLYGON_MODE_POINT,

        VK_POLYGON_MODE_RANGE_SIZE = (VK_POLYGON_MODE_POINT - VK_POLYGON_MODE_FILL + 1),

        VK_POLYGON_MODE_MAX_ENUM = 0x7FFFFFFF
    }
}
