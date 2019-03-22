// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkBufferCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkBufferCreateFlags")]
        public uint flags;

        [NativeTypeName("VkDeviceSize")]
        public ulong size;

        [NativeTypeName("VkBufferUsageFlags")]
        public uint usage;

        public VkSharingMode sharingMode;

        public uint queueFamilyIndexCount;

        [NativeTypeName("uint[]")]
        public uint* pQueueFamilyIndices;
        #endregion
    }
}
