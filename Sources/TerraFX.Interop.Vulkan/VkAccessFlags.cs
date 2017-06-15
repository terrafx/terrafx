// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkAccessFlags : uint
    {
        NONE = 0x00000000,

        INDIRECT_COMMAND_READ_BIT = 0x00000001,

        INDEX_READ_BIT = 0x00000002,

        VERTEX_ATTRIBUTE_READ_BIT = 0x00000004,

        UNIFORM_READ_BIT = 0x00000008,

        INPUT_ATTACHMENT_READ_BIT = 0x00000010,

        SHADER_READ_BIT = 0x00000020,

        SHADER_WRITE_BIT = 0x00000040,

        COLOR_ATTACHMENT_READ_BIT = 0x00000080,

        COLOR_ATTACHMENT_WRITE_BIT = 0x00000100,

        DEPTH_STENCIL_ATTACHMENT_READ_BIT = 0x00000200,

        DEPTH_STENCIL_ATTACHMENT_WRITE_BIT = 0x00000400,

        TRANSFER_READ_BIT = 0x00000800,

        TRANSFER_WRITE_BIT = 0x00001000,

        HOST_READ_BIT = 0x00002000,

        HOST_WRITE_BIT = 0x00004000,

        MEMORY_READ_BIT = 0x00008000,

        MEMORY_WRITE_BIT = 0x00010000,

        COMMAND_PROCESS_READ_BIT_NVX = 0x00020000,

        COMMAND_PROCESS_WRITE_BIT_NVX = 0x00040000
    }
}
