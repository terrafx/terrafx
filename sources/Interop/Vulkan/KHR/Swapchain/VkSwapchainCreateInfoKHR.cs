// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkSwapchainCreateInfoKHR
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkSwapchainCreateFlagsKHR")]
        public uint flags;

        [NativeTypeName("VkSurfaceKHR")]
        public IntPtr surface;

        public uint minImageCount;

        public VkFormat imageFormat;

        public VkColorSpaceKHR imageColorSpace;

        public VkExtent2D imageExtent;

        public uint imageArrayLayers;

        [NativeTypeName("VkImageUsageFlags")]
        public uint imageUsage;

        public VkSharingMode imageSharingMode;

        public uint queueFamilyIndexCount;

        [NativeTypeName("uint[]")]
        public uint* pQueueFamilyIndices;

        public VkSurfaceTransformFlagBitsKHR preTransform;

        public VkCompositeAlphaFlagBitsKHR compositeAlpha;

        public VkPresentModeKHR presentMode;

        [NativeTypeName("VkBool32")]
        public uint clipped;

        [NativeTypeName("VkSwapchainKHR")]
        public IntPtr oldSwapchain;
        #endregion
    }
}
