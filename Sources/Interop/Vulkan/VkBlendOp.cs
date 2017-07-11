// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkBlendOp
    {
        VK_BLEND_OP_ADD = 0,

        VK_BLEND_OP_SUBTRACT = 1,

        VK_BLEND_OP_REVERSE_SUBTRACT = 2,

        VK_BLEND_OP_MIN = 3,

        VK_BLEND_OP_MAX = 4,

        VK_BLEND_OP_BEGIN_RANGE = VK_BLEND_OP_ADD,

        VK_BLEND_OP_END_RANGE = VK_BLEND_OP_MAX,

        VK_BLEND_OP_RANGE_SIZE = (VK_BLEND_OP_MAX - VK_BLEND_OP_ADD + 1),

        VK_BLEND_OP_MAX_ENUM = 0x7FFFFFFF
    }
}
