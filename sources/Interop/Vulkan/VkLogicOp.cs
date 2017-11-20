// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkLogicOp
    {
        VK_LOGIC_OP_CLEAR = 0,

        VK_LOGIC_OP_AND = 1,

        VK_LOGIC_OP_AND_REVERSE = 2,

        VK_LOGIC_OP_COPY = 3,

        VK_LOGIC_OP_AND_INVERTED = 4,

        VK_LOGIC_OP_NO_OP = 5,

        VK_LOGIC_OP_XOR = 6,

        VK_LOGIC_OP_OR = 7,

        VK_LOGIC_OP_NOR = 8,

        VK_LOGIC_OP_EQUIVALENT = 9,

        VK_LOGIC_OP_INVERT = 10,

        VK_LOGIC_OP_OR_REVERSE = 11,

        VK_LOGIC_OP_COPY_INVERTED = 12,

        VK_LOGIC_OP_OR_INVERTED = 13,

        VK_LOGIC_OP_NAND = 14,

        VK_LOGIC_OP_SET = 15,

        VK_LOGIC_OP_BEGIN_RANGE = VK_LOGIC_OP_CLEAR,

        VK_LOGIC_OP_END_RANGE = VK_LOGIC_OP_SET,

        VK_LOGIC_OP_RANGE_SIZE = (VK_LOGIC_OP_SET - VK_LOGIC_OP_CLEAR + 1),

        VK_LOGIC_OP_MAX_ENUM = 0x7FFFFFFF
    }
}
