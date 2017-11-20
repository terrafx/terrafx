// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkAccessFlagBits : uint
    {
        VK_ACCESS_INDIRECT_COMMAND_READ_BIT = 0x00000001,

        VK_ACCESS_INDEX_READ_BIT = 0x00000002,

        VK_ACCESS_VERTEX_ATTRIBUTE_READ_BIT = 0x00000004,

        VK_ACCESS_UNIFORM_READ_BIT = 0x00000008,

        VK_ACCESS_INPUT_ATTACHMENT_READ_BIT = 0x00000010,

        VK_ACCESS_SHADER_READ_BIT = 0x00000020,

        VK_ACCESS_SHADER_WRITE_BIT = 0x00000040,

        VK_ACCESS_COLOR_ATTACHMENT_READ_BIT = 0x00000080,

        VK_ACCESS_COLOR_ATTACHMENT_WRITE_BIT = 0x00000100,

        VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_READ_BIT = 0x00000200,

        VK_ACCESS_DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 0x00000400,

        VK_ACCESS_TRANSFER_READ_BIT = 0x00000800,

        VK_ACCESS_TRANSFER_WRITE_BIT = 0x00001000,

        VK_ACCESS_HOST_READ_BIT = 0x00002000,

        VK_ACCESS_HOST_WRITE_BIT = 0x00004000,

        VK_ACCESS_MEMORY_READ_BIT = 0x00008000,

        VK_ACCESS_MEMORY_WRITE_BIT = 0x00010000,

        VK_ACCESS_COMMAND_PROCESS_READ_BIT_NVX = 0x00020000,

        VK_ACCESS_COMMAND_PROCESS_WRITE_BIT_NVX = 0x00040000,

        VK_ACCESS_FLAG_BITS_MAX_ENUM = 0x7FFFFFFF
    }
}
