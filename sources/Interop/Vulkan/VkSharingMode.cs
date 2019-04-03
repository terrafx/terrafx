// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkSharingMode
    {
        VK_SHARING_MODE_EXCLUSIVE = 0,

        VK_SHARING_MODE_CONCURRENT = 1,

        VK_SHARING_MODE_BEGIN_RANGE = VK_SHARING_MODE_EXCLUSIVE,

        VK_SHARING_MODE_END_RANGE = VK_SHARING_MODE_CONCURRENT,

        VK_SHARING_MODE_RANGE_SIZE = VK_SHARING_MODE_CONCURRENT - VK_SHARING_MODE_EXCLUSIVE + 1,

        VK_SHARING_MODE_MAX_ENUM = 0x7FFFFFFF
    }
}
