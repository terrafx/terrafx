// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkImageCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkImageCreateFlags")]
        public uint flags;

        public VkImageType imageType;

        public VkFormat format;

        public VkExtent3D extent;

        public uint mipLevels;

        public uint arrayLayers;

        public VkSampleCountFlagBits samples;

        public VkImageTiling tiling;

        [NativeTypeName("VkImageUsageFlags")]
        public uint usage;

        public VkSharingMode sharingMode;

        public uint queueFamilyIndexCount;

        [NativeTypeName("uint[]")]
        public uint* pQueueFamilyIndices;

        public VkImageLayout initialLayout;
        #endregion
    }
}
