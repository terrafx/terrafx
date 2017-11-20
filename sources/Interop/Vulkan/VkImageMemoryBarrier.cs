// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct VkImageMemoryBarrier
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkAccessFlags")]
        public uint srcAccessMask;

        [ComAliasName("VkAccessFlags")]
        public uint dstAccessMask;

        public VkImageLayout oldLayout;

        public VkImageLayout newLayout;

        public uint srcQueueFamilyIndex;

        public uint dstQueueFamilyIndex;

        [ComAliasName("VkImage")]
        public ulong image;

        public VkImageSubresourceRange subresourceRange;
        #endregion
    }
}
