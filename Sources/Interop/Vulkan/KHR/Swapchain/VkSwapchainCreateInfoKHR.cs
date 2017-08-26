// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ unsafe struct VkSwapchainCreateInfoKHR
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkSwapchainCreateFlagsKHR")]
        public uint flags;

        [ComAliasName("VkSurfaceKHR")]
        public IntPtr surface;

        public uint minImageCount;

        public VkFormat imageFormat;

        public VkColorSpaceKHR imageColorSpace;

        public VkExtent2D imageExtent;

        public uint imageArrayLayers;

        [ComAliasName("VkImageUsageFlags")]
        public uint imageUsage;

        public VkSharingMode imageSharingMode;

        public uint queueFamilyIndexCount;

        public uint* pQueueFamilyIndices;

        public VkSurfaceTransformFlagBitsKHR preTransform;

        public VkCompositeAlphaFlagBitsKHR compositeAlpha;

        public VkPresentModeKHR presentMode;

        [ComAliasName("VkBool32")]
        public uint clipped;

        [ComAliasName("VkSwapchainKHR")]
        public IntPtr oldSwapchain;
        #endregion
    }
}
