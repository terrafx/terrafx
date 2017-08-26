// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkBufferUsageFlagBits : uint
    {
        VK_BUFFER_USAGE_TRANSFER_SRC_BIT = 0x00000001,

        VK_BUFFER_USAGE_TRANSFER_DST_BIT = 0x00000002,

        VK_BUFFER_USAGE_UNIFORM_TEXEL_BUFFER_BIT = 0x00000004,

        VK_BUFFER_USAGE_STORAGE_TEXEL_BUFFER_BIT = 0x00000008,

        VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT = 0x00000010,

        VK_BUFFER_USAGE_STORAGE_BUFFER_BIT = 0x00000020,

        VK_BUFFER_USAGE_INDEX_BUFFER_BIT = 0x00000040,

        VK_BUFFER_USAGE_VERTEX_BUFFER_BIT = 0x00000080,

        VK_BUFFER_USAGE_INDIRECT_BUFFER_BIT = 0x00000100,

        VK_BUFFER_USAGE_FLAG_BITS_MAX_ENUM = 0x7FFFFFFF
    }
}
