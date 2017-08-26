// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPhysicalDeviceType
    {
        VK_PHYSICAL_DEVICE_TYPE_OTHER = 0,

        VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU = 1,

        VK_PHYSICAL_DEVICE_TYPE_DISCRETE_GPU = 2,

        VK_PHYSICAL_DEVICE_TYPE_VIRTUAL_GPU = 3,

        VK_PHYSICAL_DEVICE_TYPE_CPU = 4,

        VK_PHYSICAL_DEVICE_TYPE_BEGIN_RANGE = VK_PHYSICAL_DEVICE_TYPE_OTHER,

        VK_PHYSICAL_DEVICE_TYPE_END_RANGE = VK_PHYSICAL_DEVICE_TYPE_CPU,

        VK_PHYSICAL_DEVICE_TYPE_RANGE_SIZE = (VK_PHYSICAL_DEVICE_TYPE_CPU - VK_PHYSICAL_DEVICE_TYPE_OTHER + 1),

        VK_PHYSICAL_DEVICE_TYPE_MAX_ENUM = 0x7FFFFFFF
    }
}
