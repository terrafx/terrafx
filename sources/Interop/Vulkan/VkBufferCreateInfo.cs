// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct VkBufferCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkBufferCreateFlags")]
        public uint flags;

        [ComAliasName("VkDeviceSize")]
        public ulong size;

        [ComAliasName("VkBufferUsageFlags")]
        public uint usage;

        public VkSharingMode sharingMode;

        public uint queueFamilyIndexCount;

        [ComAliasName("uint[]")]
        public uint* pQueueFamilyIndices;
        #endregion
    }
}
