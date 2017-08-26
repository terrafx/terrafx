// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkBorderColor
    {
        VK_BORDER_COLOR_FLOAT_TRANSPARENT_BLACK = 0,

        VK_BORDER_COLOR_INT_TRANSPARENT_BLACK = 1,

        VK_BORDER_COLOR_FLOAT_OPAQUE_BLACK = 2,

        VK_BORDER_COLOR_INT_OPAQUE_BLACK = 3,

        VK_BORDER_COLOR_FLOAT_OPAQUE_WHITE = 4,

        VK_BORDER_COLOR_INT_OPAQUE_WHITE = 5,

        VK_BORDER_COLOR_BEGIN_RANGE = VK_BORDER_COLOR_FLOAT_TRANSPARENT_BLACK,

        VK_BORDER_COLOR_END_RANGE = VK_BORDER_COLOR_INT_OPAQUE_WHITE,

        VK_BORDER_COLOR_RANGE_SIZE = (VK_BORDER_COLOR_INT_OPAQUE_WHITE - VK_BORDER_COLOR_FLOAT_TRANSPARENT_BLACK + 1),

        VK_BORDER_COLOR_MAX_ENUM = 0x7FFFFFFF
    }
}
