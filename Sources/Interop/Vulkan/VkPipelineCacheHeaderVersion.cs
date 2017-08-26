// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkPipelineCacheHeaderVersion
    {
        VK_PIPELINE_CACHE_HEADER_VERSION_ONE = 1,

        VK_PIPELINE_CACHE_HEADER_VERSION_BEGIN_RANGE = VK_PIPELINE_CACHE_HEADER_VERSION_ONE,

        VK_PIPELINE_CACHE_HEADER_VERSION_END_RANGE = VK_PIPELINE_CACHE_HEADER_VERSION_ONE,

        VK_PIPELINE_CACHE_HEADER_VERSION_RANGE_SIZE = (VK_PIPELINE_CACHE_HEADER_VERSION_ONE - VK_PIPELINE_CACHE_HEADER_VERSION_ONE + 1),

        VK_PIPELINE_CACHE_HEADER_VERSION_MAX_ENUM = 0x7FFFFFFF
    }
}
