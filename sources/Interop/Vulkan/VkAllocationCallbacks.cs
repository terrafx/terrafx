// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkAllocationCallbacks
    {
        #region Fields
        public void* pUserData;

        [NativeTypeName("PFN_vkAllocationFunction")]
        public IntPtr pfnAllocation;

        [NativeTypeName("PFN_vkReallocationFunction")]
        public IntPtr pfnReallocation;

        [NativeTypeName("PFN_vkFreeFunction")]
        public IntPtr pfnFree;

        [NativeTypeName("PFN_vkInternalAllocationNotification")]
        public IntPtr pfnInternalAllocation;

        [NativeTypeName("PFN_vkInternalFreeNotification")]
        public IntPtr pfnInternalFree;
        #endregion
    }
}
