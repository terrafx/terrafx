// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum VkPipelineCreateFlagBits : uint
    {
        VK_PIPELINE_CREATE_DISABLE_OPTIMIZATION_BIT = 0x00000001,

        VK_PIPELINE_CREATE_ALLOW_DERIVATIVES_BIT = 0x00000002,

        VK_PIPELINE_CREATE_DERIVATIVE_BIT = 0x00000004,

        VK_PIPELINE_CREATE_VIEW_INDEX_FROM_DEVICE_INDEX_BIT_KHX = 0x00000008,

        VK_PIPELINE_CREATE_DISPATCH_BASE_KHX = 0x00000010,

        VK_PIPELINE_CREATE_FLAG_BITS_MAX_ENUM = 0x7FFFFFFF
    }
}
