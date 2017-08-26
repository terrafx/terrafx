// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkComponentSwizzle
    {
        VK_COMPONENT_SWIZZLE_IDENTITY = 0,

        VK_COMPONENT_SWIZZLE_ZERO = 1,

        VK_COMPONENT_SWIZZLE_ONE = 2,

        VK_COMPONENT_SWIZZLE_R = 3,

        VK_COMPONENT_SWIZZLE_G = 4,

        VK_COMPONENT_SWIZZLE_B = 5,

        VK_COMPONENT_SWIZZLE_A = 6,

        VK_COMPONENT_SWIZZLE_BEGIN_RANGE = VK_COMPONENT_SWIZZLE_IDENTITY,

        VK_COMPONENT_SWIZZLE_END_RANGE = VK_COMPONENT_SWIZZLE_A,

        VK_COMPONENT_SWIZZLE_RANGE_SIZE = (VK_COMPONENT_SWIZZLE_A - VK_COMPONENT_SWIZZLE_IDENTITY + 1),

        VK_COMPONENT_SWIZZLE_MAX_ENUM = 0x7FFFFFFF
    }
}
