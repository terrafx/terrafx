// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkImageCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkImageCreateFlags")]
        public uint flags;

        public VkImageType imageType;

        public VkFormat format;

        public VkExtent3D extent;

        public uint mipLevels;

        public uint arrayLayers;

        public VkSampleCountFlagBits samples;

        public VkImageTiling tiling;

        [ComAliasName("VkImageUsageFlags")]
        public uint usage;

        public VkSharingMode sharingMode;

        public uint queueFamilyIndexCount;

        public uint* pQueueFamilyIndices;

        public VkImageLayout initialLayout;
        #endregion
    }
}
