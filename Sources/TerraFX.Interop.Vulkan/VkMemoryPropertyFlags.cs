// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkMemoryPropertyFlags : uint
    {
        NONE = 0x00000000,

        DEVICE_LOCAL_BIT = 0x00000001,

        HOST_VISIBLE_BIT = 0x00000002,

        HOST_COHERENT_BIT = 0x00000004,

        HOST_CACHED_BIT = 0x00000008,

        LAZILY_ALLOCATED_BIT = 0x00000010
    }
}
