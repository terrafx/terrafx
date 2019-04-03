// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPresentModeKHR
    {
        VK_PRESENT_MODE_IMMEDIATE_KHR = 0,

        VK_PRESENT_MODE_MAILBOX_KHR = 1,

        VK_PRESENT_MODE_FIFO_KHR = 2,

        VK_PRESENT_MODE_FIFO_RELAXED_KHR = 3,

        VK_PRESENT_MODE_SHARED_DEMAND_REFRESH_KHR = 1000111000,

        VK_PRESENT_MODE_SHARED_CONTINUOUS_REFRESH_KHR = 1000111001,

        VK_PRESENT_MODE_BEGIN_RANGE_KHR = VK_PRESENT_MODE_IMMEDIATE_KHR,

        VK_PRESENT_MODE_END_RANGE_KHR = VK_PRESENT_MODE_FIFO_RELAXED_KHR,

        VK_PRESENT_MODE_RANGE_SIZE_KHR = VK_PRESENT_MODE_FIFO_RELAXED_KHR - VK_PRESENT_MODE_IMMEDIATE_KHR + 1,

        VK_PRESENT_MODE_MAX_ENUM_KHR = 0x7FFFFFFF
    }
}
