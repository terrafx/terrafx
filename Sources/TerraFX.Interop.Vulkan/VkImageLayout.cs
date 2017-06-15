// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkImageLayout
    {
        UNDEFINED = 0,

        GENERAL = 1,

        COLOR_ATTACHMENT_OPTIMAL = 2,

        DEPTH_STENCIL_ATTACHMENT_OPTIMAL = 3,

        DEPTH_STENCIL_READ_ONLY_OPTIMAL = 4,

        SHADER_READ_ONLY_OPTIMAL = 5,

        TRANSFER_SRC_OPTIMAL = 6,

        TRANSFER_DST_OPTIMAL = 7,

        PREINITIALIZED = 8,

        PRESENT_SRC_KHR = 1000001002,

        SHARED_PRESENT_KHR = 1000111000
    }
}
