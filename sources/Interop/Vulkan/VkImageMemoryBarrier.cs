// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkImageMemoryBarrier
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkAccessFlags")]
        public uint srcAccessMask;

        [NativeTypeName("VkAccessFlags")]
        public uint dstAccessMask;

        public VkImageLayout oldLayout;

        public VkImageLayout newLayout;

        public uint srcQueueFamilyIndex;

        public uint dstQueueFamilyIndex;

        [NativeTypeName("VkImage")]
        public ulong image;

        public VkImageSubresourceRange subresourceRange;
        #endregion
    }
}
