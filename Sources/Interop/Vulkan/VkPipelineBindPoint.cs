// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPipelineBindPoint
    {
        VK_PIPELINE_BIND_POINT_GRAPHICS = 0,

        VK_PIPELINE_BIND_POINT_COMPUTE = 1,

        VK_PIPELINE_BIND_POINT_BEGIN_RANGE = VK_PIPELINE_BIND_POINT_GRAPHICS,

        VK_PIPELINE_BIND_POINT_END_RANGE = VK_PIPELINE_BIND_POINT_COMPUTE,

        VK_PIPELINE_BIND_POINT_RANGE_SIZE = (VK_PIPELINE_BIND_POINT_COMPUTE - VK_PIPELINE_BIND_POINT_GRAPHICS + 1),

        VK_PIPELINE_BIND_POINT_MAX_ENUM = 0x7FFFFFFF
    }
}
