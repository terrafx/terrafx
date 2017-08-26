// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public enum VkSystemAllocationScope
    {
        VK_SYSTEM_ALLOCATION_SCOPE_COMMAND = 0,

        VK_SYSTEM_ALLOCATION_SCOPE_OBJECT = 1,

        VK_SYSTEM_ALLOCATION_SCOPE_CACHE = 2,

        VK_SYSTEM_ALLOCATION_SCOPE_DEVICE = 3,

        VK_SYSTEM_ALLOCATION_SCOPE_INSTANCE = 4,

        VK_SYSTEM_ALLOCATION_SCOPE_BEGIN_RANGE = VK_SYSTEM_ALLOCATION_SCOPE_COMMAND,

        VK_SYSTEM_ALLOCATION_SCOPE_END_RANGE = VK_SYSTEM_ALLOCATION_SCOPE_INSTANCE,

        VK_SYSTEM_ALLOCATION_SCOPE_RANGE_SIZE = (VK_SYSTEM_ALLOCATION_SCOPE_INSTANCE - VK_SYSTEM_ALLOCATION_SCOPE_COMMAND + 1),

        VK_SYSTEM_ALLOCATION_SCOPE_MAX_ENUM = 0x7FFFFFFF
    }
}
