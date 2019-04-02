// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkImageLayout
    {
        VK_IMAGE_LAYOUT_UNDEFINED = 0,

        VK_IMAGE_LAYOUT_GENERAL = 1,

        VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL = 2,

        VK_IMAGE_LAYOUT_DEPTH_STENCIL_ATTACHMENT_OPTIMAL = 3,

        VK_IMAGE_LAYOUT_DEPTH_STENCIL_READ_ONLY_OPTIMAL = 4,

        VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL = 5,

        VK_IMAGE_LAYOUT_TRANSFER_SRC_OPTIMAL = 6,

        VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL = 7,

        VK_IMAGE_LAYOUT_PREINITIALIZED = 8,

        VK_IMAGE_LAYOUT_PRESENT_SRC_KHR = 1000001002,

        VK_IMAGE_LAYOUT_SHARED_PRESENT_KHR = 1000111000,

        VK_IMAGE_LAYOUT_BEGIN_RANGE = VK_IMAGE_LAYOUT_UNDEFINED,

        VK_IMAGE_LAYOUT_END_RANGE = VK_IMAGE_LAYOUT_PREINITIALIZED,

        VK_IMAGE_LAYOUT_RANGE_SIZE = VK_IMAGE_LAYOUT_PREINITIALIZED - VK_IMAGE_LAYOUT_UNDEFINED + 1,

        VK_IMAGE_LAYOUT_MAX_ENUM = 0x7FFFFFFF
    }
}
