// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkBlendFactor
    {
        VK_BLEND_FACTOR_ZERO = 0,

        VK_BLEND_FACTOR_ONE = 1,

        VK_BLEND_FACTOR_SRC_COLOR = 2,

        VK_BLEND_FACTOR_ONE_MINUS_SRC_COLOR = 3,

        VK_BLEND_FACTOR_DST_COLOR = 4,

        VK_BLEND_FACTOR_ONE_MINUS_DST_COLOR = 5,

        VK_BLEND_FACTOR_SRC_ALPHA = 6,

        VK_BLEND_FACTOR_ONE_MINUS_SRC_ALPHA = 7,

        VK_BLEND_FACTOR_DST_ALPHA = 8,

        VK_BLEND_FACTOR_ONE_MINUS_DST_ALPHA = 9,

        VK_BLEND_FACTOR_CONSTANT_COLOR = 10,

        VK_BLEND_FACTOR_ONE_MINUS_CONSTANT_COLOR = 11,

        VK_BLEND_FACTOR_CONSTANT_ALPHA = 12,

        VK_BLEND_FACTOR_ONE_MINUS_CONSTANT_ALPHA = 13,

        VK_BLEND_FACTOR_SRC_ALPHA_SATURATE = 14,

        VK_BLEND_FACTOR_SRC1_COLOR = 15,

        VK_BLEND_FACTOR_ONE_MINUS_SRC1_COLOR = 16,

        VK_BLEND_FACTOR_SRC1_ALPHA = 17,

        VK_BLEND_FACTOR_ONE_MINUS_SRC1_ALPHA = 18,

        VK_BLEND_FACTOR_BEGIN_RANGE = VK_BLEND_FACTOR_ZERO,

        VK_BLEND_FACTOR_END_RANGE = VK_BLEND_FACTOR_ONE_MINUS_SRC1_ALPHA,

        VK_BLEND_FACTOR_RANGE_SIZE = VK_BLEND_FACTOR_ONE_MINUS_SRC1_ALPHA - VK_BLEND_FACTOR_ZERO + 1,

        VK_BLEND_FACTOR_MAX_ENUM = 0x7FFFFFFF
    }
}
