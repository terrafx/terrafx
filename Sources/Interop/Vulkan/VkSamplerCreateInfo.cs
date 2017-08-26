// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkSamplerCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [ComAliasName("VkSamplerCreateFlags")]
        public uint flags;

        public VkFilter magFilter;

        public VkFilter minFilter;

        public VkSamplerMipmapMode mipmapMode;

        public VkSamplerAddressMode addressModeU;

        public VkSamplerAddressMode addressModeV;

        public VkSamplerAddressMode addressModeW;

        public float mipLodBias;

        [ComAliasName("VkBool32")]
        public uint anisotropyEnable;

        public float maxAnisotropy;

        [ComAliasName("VkBool32")]
        public uint compareEnable;

        public VkCompareOp compareOp;

        public float minLod;

        public float maxLod;

        public VkBorderColor borderColor;

        [ComAliasName("VkBool32")]
        public uint unnormalizedCoordinates;
        #endregion
    }
}
