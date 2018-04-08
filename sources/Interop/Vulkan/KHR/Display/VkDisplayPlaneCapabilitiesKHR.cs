// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct VkDisplayPlaneCapabilitiesKHR
    {
        #region Fields
        [ComAliasName("VkDisplayPlaneAlphaFlagsKHR")]
        public uint supportedAlpha;

        public VkOffset2D minSrcPosition;

        public VkOffset2D maxSrcPosition;

        public VkExtent2D minSrcExtent;

        public VkExtent2D maxSrcExtent;

        public VkOffset2D minDstPosition;

        public VkOffset2D maxDstPosition;

        public VkExtent2D minDstExtent;

        public VkExtent2D maxDstExtent;
        #endregion
    }
}
