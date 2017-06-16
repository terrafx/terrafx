// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkSampleCountFlags : uint
    {
        NONE = 0x00000000,

        _1_BIT = 0x00000001,

        _2_BIT = 0x00000002,

        _4_BIT = 0x00000004,

        _8_BIT = 0x00000008,

        _16_BIT = 0x00000010,

        _32_BIT = 0x00000020,

        _64_BIT = 0x00000040
    }
}
