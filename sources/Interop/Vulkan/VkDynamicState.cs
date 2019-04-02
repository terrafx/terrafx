// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkDynamicState
    {
        VK_DYNAMIC_STATE_VIEWPORT = 0,

        VK_DYNAMIC_STATE_SCISSOR = 1,

        VK_DYNAMIC_STATE_LINE_WIDTH = 2,

        VK_DYNAMIC_STATE_DEPTH_BIAS = 3,

        VK_DYNAMIC_STATE_BLEND_CONSTANTS = 4,

        VK_DYNAMIC_STATE_DEPTH_BOUNDS = 5,

        VK_DYNAMIC_STATE_STENCIL_COMPARE_MASK = 6,

        VK_DYNAMIC_STATE_STENCIL_WRITE_MASK = 7,

        VK_DYNAMIC_STATE_STENCIL_REFERENCE = 8,

        VK_DYNAMIC_STATE_VIEWPORT_W_SCALING_NV = 1000087000,

        VK_DYNAMIC_STATE_DISCARD_RECTANGLE_EXT = 1000099000,

        VK_DYNAMIC_STATE_BEGIN_RANGE = VK_DYNAMIC_STATE_VIEWPORT,

        VK_DYNAMIC_STATE_END_RANGE = VK_DYNAMIC_STATE_STENCIL_REFERENCE,

        VK_DYNAMIC_STATE_RANGE_SIZE = VK_DYNAMIC_STATE_STENCIL_REFERENCE - VK_DYNAMIC_STATE_VIEWPORT + 1,

        VK_DYNAMIC_STATE_MAX_ENUM = 0x7FFFFFFF
    }
}
