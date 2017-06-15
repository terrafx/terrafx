// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkFormatFeatureFlags : uint
    {
        NONE = 0x00000000,

        SAMPLED_IMAGE_BIT = 0x00000001,

        STORAGE_IMAGE_BIT = 0x00000002,

        STORAGE_IMAGE_ATOMIC_BIT = 0x00000004,

        UNIFORM_TEXEL_BUFFER_BIT = 0x00000008,

        STORAGE_TEXEL_BUFFER_BIT = 0x00000010,

        STORAGE_TEXEL_BUFFER_ATOMIC_BIT = 0x00000020,

        VERTEX_BUFFER_BIT = 0x00000040,

        COLOR_ATTACHMENT_BIT = 0x00000080,

        COLOR_ATTACHMENT_BLEND_BIT = 0x00000100,

        DEPTH_STENCIL_ATTACHMENT_BIT = 0x00000200,

        BLIT_SRC_BIT = 0x00000400,

        BLIT_DST_BIT = 0x00000800,

        SAMPLED_IMAGE_FILTER_LINEAR_BIT = 0x00001000,

        SAMPLED_IMAGE_FILTER_CUBIC_BIT_IMG = 0x00002000,

        TRANSFER_SRC_BIT_KHR = 0x00004000,

        TRANSFER_DST_BIT_KHR = 0x00008000
    }
}
