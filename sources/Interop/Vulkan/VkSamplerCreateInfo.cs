// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkSamplerCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkSamplerCreateFlags")]
        public uint flags;

        public VkFilter magFilter;

        public VkFilter minFilter;

        public VkSamplerMipmapMode mipmapMode;

        public VkSamplerAddressMode addressModeU;

        public VkSamplerAddressMode addressModeV;

        public VkSamplerAddressMode addressModeW;

        public float mipLodBias;

        [NativeTypeName("VkBool32")]
        public uint anisotropyEnable;

        public float maxAnisotropy;

        [NativeTypeName("VkBool32")]
        public uint compareEnable;

        public VkCompareOp compareOp;

        public float minLod;

        public float maxLod;

        public VkBorderColor borderColor;

        [NativeTypeName("VkBool32")]
        public uint unnormalizedCoordinates;
        #endregion
    }
}
