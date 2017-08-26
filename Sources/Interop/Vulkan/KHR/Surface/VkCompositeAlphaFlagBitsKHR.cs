// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkCompositeAlphaFlagBitsKHR
    {
        VK_COMPOSITE_ALPHA_OPAQUE_BIT_KHR = 0x00000001,

        VK_COMPOSITE_ALPHA_PRE_MULTIPLIED_BIT_KHR = 0x00000002,

        VK_COMPOSITE_ALPHA_POST_MULTIPLIED_BIT_KHR = 0x00000004,

        VK_COMPOSITE_ALPHA_INHERIT_BIT_KHR = 0x00000008,

        VK_COMPOSITE_ALPHA_FLAG_BITS_MAX_ENUM_KHR = 0x7FFFFFFF
    }
}
