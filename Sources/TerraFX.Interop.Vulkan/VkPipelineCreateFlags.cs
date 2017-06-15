// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkPipelineCreateFlags : uint
    {
        NONE = 0x00000000,

        DISABLE_OPTIMIZATION_BIT = 0x00000001,

        ALLOW_DERIVATIVES_BIT = 0x00000002,

        DERIVATIVE_BIT = 0x00000004,

        VIEW_INDEX_FROM_DEVICE_INDEX_BIT_KHX = 0x00000008,

        DISPATCH_BASE_KHX = 0x00000010
    }
}
