// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkImageCreateFlags : uint
    {
        NONE = 0x00000000,

        SPARSE_BINDING_BIT = 0x00000001,

        SPARSE_RESIDENCY_BIT = 0x00000002,

        SPARSE_ALIASED_BIT = 0x00000004,

        MUTABLE_FORMAT_BIT = 0x00000008,

        CUBE_COMPATIBLE_BIT = 0x00000010,

        BIND_SFR_BIT_KHX = 0x00000040,

        _2D_ARRAY_COMPATIBLE_BIT_KHR = 0x00000020
    }
}
