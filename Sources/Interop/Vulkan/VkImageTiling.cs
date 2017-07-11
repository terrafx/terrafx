// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkImageTiling
    {
        VK_IMAGE_TILING_OPTIMAL = 0,

        VK_IMAGE_TILING_LINEAR = 1,

        VK_IMAGE_TILING_BEGIN_RANGE = VK_IMAGE_TILING_OPTIMAL,

        VK_IMAGE_TILING_END_RANGE = VK_IMAGE_TILING_LINEAR,

        VK_IMAGE_TILING_RANGE_SIZE = (VK_IMAGE_TILING_LINEAR - VK_IMAGE_TILING_OPTIMAL + 1),

        VK_IMAGE_TILING_MAX_ENUM = 0x7FFFFFFF
    }
}
