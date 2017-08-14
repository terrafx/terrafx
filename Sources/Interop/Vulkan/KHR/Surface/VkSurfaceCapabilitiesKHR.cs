// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct VkSurfaceCapabilitiesKHR
    {
        #region Fields
        public uint minImageCount;

        public uint maxImageCount;

        public VkExtent2D currentExtent;

        public VkExtent2D minImageExtent;

        public VkExtent2D maxImageExtent;

        public uint maxImageArrayLayers;

        [ComAliasName("VkSurfaceTransformFlagsKHR")]
        public uint supportedTransforms;

        public VkSurfaceTransformFlagBitsKHR currentTransform;

        [ComAliasName("VkCompositeAlphaFlagsKHR")]
        public uint supportedCompositeAlpha;

        [ComAliasName("VkImageUsageFlags")]
        public uint supportedUsageFlags;
        #endregion
    }
}
