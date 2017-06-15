// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkDependencyFlags : uint
    {
        NONE = 0x00000000,

        BY_REGION_BIT = 0x00000001,

        VIEW_LOCAL_BIT_KHX = 0x00000002,

        DEVICE_GROUP_BIT_KHX = 0x00000004
    }
}
