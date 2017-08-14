// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkSurfaceTransformFlagBitsKHR
    {
        VK_SURFACE_TRANSFORM_IDENTITY_BIT_KHR = 0x00000001,

        VK_SURFACE_TRANSFORM_ROTATE_90_BIT_KHR = 0x00000002,

        VK_SURFACE_TRANSFORM_ROTATE_180_BIT_KHR = 0x00000004,

        VK_SURFACE_TRANSFORM_ROTATE_270_BIT_KHR = 0x00000008,

        VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_BIT_KHR = 0x00000010,

        VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_90_BIT_KHR = 0x00000020,

        VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_180_BIT_KHR = 0x00000040,

        VK_SURFACE_TRANSFORM_HORIZONTAL_MIRROR_ROTATE_270_BIT_KHR = 0x00000080,

        VK_SURFACE_TRANSFORM_INHERIT_BIT_KHR = 0x00000100,

        VK_SURFACE_TRANSFORM_FLAG_BITS_MAX_ENUM_KHR = 0x7FFFFFFF
    }
}
