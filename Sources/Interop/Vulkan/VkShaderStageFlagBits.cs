// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkShaderStageFlagBits : uint
    {
        VK_SHADER_STAGE_VERTEX_BIT = 0x00000001,

        VK_SHADER_STAGE_TESSELLATION_CONTROL_BIT = 0x00000002,

        VK_SHADER_STAGE_TESSELLATION_EVALUATION_BIT = 0x00000004,

        VK_SHADER_STAGE_GEOMETRY_BIT = 0x00000008,

        VK_SHADER_STAGE_FRAGMENT_BIT = 0x00000010,

        VK_SHADER_STAGE_COMPUTE_BIT = 0x00000020,

        VK_SHADER_STAGE_ALL_GRAPHICS = 0x0000001F,

        VK_SHADER_STAGE_ALL = 0x7FFFFFFF,

        VK_SHADER_STAGE_FLAG_BITS_MAX_ENUM = 0x7FFFFFFF
    }
}
