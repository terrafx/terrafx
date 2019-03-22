// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System;
using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkDisplayPropertiesKHR
    {
        #region Fields
        [ComAliasName("VkDisplayKHR")]
        public IntPtr display;

        [ComAliasName("string")]
        public sbyte* displayName;

        public VkExtent2D physicalDimensions;

        public VkExtent2D physicalResolution;

        [ComAliasName("VkSurfaceTransformFlagsKHR")]
        public uint supportedTransforms;

        [ComAliasName("VkBool32")]
        public uint planeReorderPossible;

        [ComAliasName("VkBool32")]
        public uint persistentContent;
        #endregion
    }
}
