// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkStencilOp
    {
        VK_STENCIL_OP_KEEP = 0,

        VK_STENCIL_OP_ZERO = 1,

        VK_STENCIL_OP_REPLACE = 2,

        VK_STENCIL_OP_INCREMENT_AND_CLAMP = 3,

        VK_STENCIL_OP_DECREMENT_AND_CLAMP = 4,

        VK_STENCIL_OP_INVERT = 5,

        VK_STENCIL_OP_INCREMENT_AND_WRAP = 6,

        VK_STENCIL_OP_DECREMENT_AND_WRAP = 7,

        VK_STENCIL_OP_BEGIN_RANGE = VK_STENCIL_OP_KEEP,

        VK_STENCIL_OP_END_RANGE = VK_STENCIL_OP_DECREMENT_AND_WRAP,

        VK_STENCIL_OP_RANGE_SIZE = (VK_STENCIL_OP_DECREMENT_AND_WRAP - VK_STENCIL_OP_KEEP + 1),

        VK_STENCIL_OP_MAX_ENUM = 0x7FFFFFFF
    }
}
