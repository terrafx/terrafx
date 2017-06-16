// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkPipelineStageFlags : uint
    {
        NONE = 0x00000000,

        TOP_OF_PIPE_BIT = 0x00000001,

        DRAW_INDIRECT_BIT = 0x00000002,

        VERTEX_INPUT_BIT = 0x00000004,

        VERTEX_SHADER_BIT = 0x00000008,

        TESSELLATION_CONTROL_SHADER_BIT = 0x00000010,

        TESSELLATION_EVALUATION_SHADER_BIT = 0x00000020,

        GEOMETRY_SHADER_BIT = 0x00000040,

        FRAGMENT_SHADER_BIT = 0x00000080,

        EARLY_FRAGMENT_TESTS_BIT = 0x00000100,

        LATE_FRAGMENT_TESTS_BIT = 0x00000200,

        COLOR_ATTACHMENT_OUTPUT_BIT = 0x00000400,

        COMPUTE_SHADER_BIT = 0x00000800,

        TRANSFER_BIT = 0x00001000,

        BOTTOM_OF_PIPE_BIT = 0x00002000,

        HOST_BIT = 0x00004000,

        ALL_GRAPHICS_BIT = 0x00008000,

        ALL_COMMANDS_BIT = 0x00010000,

        COMMAND_PROCESS_BIT_NVX = 0x00020000
    }
}
