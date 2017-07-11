// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkIndexType
    {
        VK_INDEX_TYPE_UINT16 = 0,

        VK_INDEX_TYPE_UINT32 = 1,

        VK_INDEX_TYPE_BEGIN_RANGE = VK_INDEX_TYPE_UINT16,

        VK_INDEX_TYPE_END_RANGE = VK_INDEX_TYPE_UINT32,

        VK_INDEX_TYPE_RANGE_SIZE = (VK_INDEX_TYPE_UINT32 - VK_INDEX_TYPE_UINT16 + 1),

        VK_INDEX_TYPE_MAX_ENUM = 0x7FFFFFFF
    }
}
