// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkCompareOp
    {
        VK_COMPARE_OP_NEVER = 0,

        VK_COMPARE_OP_LESS = 1,

        VK_COMPARE_OP_EQUAL = 2,

        VK_COMPARE_OP_LESS_OR_EQUAL = 3,

        VK_COMPARE_OP_GREATER = 4,

        VK_COMPARE_OP_NOT_EQUAL = 5,

        VK_COMPARE_OP_GREATER_OR_EQUAL = 6,

        VK_COMPARE_OP_ALWAYS = 7,

        VK_COMPARE_OP_BEGIN_RANGE = VK_COMPARE_OP_NEVER,

        VK_COMPARE_OP_END_RANGE = VK_COMPARE_OP_ALWAYS,

        VK_COMPARE_OP_RANGE_SIZE = VK_COMPARE_OP_ALWAYS - VK_COMPARE_OP_NEVER + 1,

        VK_COMPARE_OP_MAX_ENUM = 0x7FFFFFFF
    }
}
