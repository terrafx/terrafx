// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkImageCreateFlagBits : uint
    {
        VK_IMAGE_CREATE_SPARSE_BINDING_BIT = 0x00000001,

        VK_IMAGE_CREATE_SPARSE_RESIDENCY_BIT = 0x00000002,

        VK_IMAGE_CREATE_SPARSE_ALIASED_BIT = 0x00000004,

        VK_IMAGE_CREATE_MUTABLE_FORMAT_BIT = 0x00000008,

        VK_IMAGE_CREATE_CUBE_COMPATIBLE_BIT = 0x00000010,

        VK_IMAGE_CREATE_BIND_SFR_BIT_KHX = 0x00000040,

        VK_IMAGE_CREATE_2D_ARRAY_COMPATIBLE_BIT_KHR = 0x00000020,

        VK_IMAGE_CREATE_FLAG_BITS_MAX_ENUM = 0x7FFFFFFF
    }
}
