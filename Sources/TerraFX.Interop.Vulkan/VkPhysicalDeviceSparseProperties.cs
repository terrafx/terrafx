// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    public struct VkPhysicalDeviceSparseProperties
    {
        #region Fields
        public VkBool32 residencyStandard2DBlockShape;

        public VkBool32 residencyStandard2DMultisampleBlockShape;

        public VkBool32 residencyStandard3DBlockShape;

        public VkBool32 residencyAlignedMipSize;

        public VkBool32 residencyNonResidentStrict;
        #endregion
    }
}
