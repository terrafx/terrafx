// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkBufferUsageFlags : uint
    {
        NONE = 0x00000000,

        TRANSFER_SRC_BIT = 0x00000001,

        TRANSFER_DST_BIT = 0x00000002,

        UNIFORM_TEXEL_BUFFER_BIT = 0x00000004,

        STORAGE_TEXEL_BUFFER_BIT = 0x00000008,

        UNIFORM_BUFFER_BIT = 0x00000010,

        STORAGE_BUFFER_BIT = 0x00000020,

        INDEX_BUFFER_BIT = 0x00000040,

        VERTEX_BUFFER_BIT = 0x00000080,

        INDIRECT_BUFFER_BIT = 0x00000100
    }
}
