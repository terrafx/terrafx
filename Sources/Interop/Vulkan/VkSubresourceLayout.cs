// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct VkSubresourceLayout
    {
        #region Fields
        [ComAliasName("VkDeviceSize")]
        public ulong offset;

        [ComAliasName("VkDeviceSize")]
        public ulong size;

        [ComAliasName("VkDeviceSize")]
        public ulong rowPitch;

        [ComAliasName("VkDeviceSize")]
        public ulong arrayPitch;

        [ComAliasName("VkDeviceSize")]
        public ulong depthPitch;
        #endregion
    }
}
