// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkQueryType
    {
        VK_QUERY_TYPE_OCCLUSION = 0,

        VK_QUERY_TYPE_PIPELINE_STATISTICS = 1,

        VK_QUERY_TYPE_TIMESTAMP = 2,

        VK_QUERY_TYPE_BEGIN_RANGE = VK_QUERY_TYPE_OCCLUSION,

        VK_QUERY_TYPE_END_RANGE = VK_QUERY_TYPE_TIMESTAMP,

        VK_QUERY_TYPE_RANGE_SIZE = (VK_QUERY_TYPE_TIMESTAMP - VK_QUERY_TYPE_OCCLUSION + 1),

        VK_QUERY_TYPE_MAX_ENUM = 0x7FFFFFFF
    }
}
