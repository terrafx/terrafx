// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkAllocationCallbacks
    {
        #region Fields
        public void* pUserData;

        public IntPtr /* PFN_vkAllocationFunction */ pfnAllocation;

        public IntPtr /* PFN_vkReallocationFunction */ pfnReallocation;

        public IntPtr /* PFN_vkFreeFunction */ pfnFree;

        public IntPtr /* PFN_vkInternalAllocationNotification */ pfnInternalAllocation;

        public IntPtr /* PFN_vkInternalFreeNotification */ pfnInternalFree;
        #endregion
    }
}
