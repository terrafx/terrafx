// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct VkPipelineMultisampleStateCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        [NativeTypeName("VkPipelineMultisampleStateCreateFlags")]
        public uint flags;

        public VkSampleCountFlagBits rasterizationSamples;

        [NativeTypeName("VkBool32")]
        public uint sampleShadingEnable;

        public float minSampleShading;

        [NativeTypeName("VkSampleMask")]
        public uint* pSampleMask;

        [NativeTypeName("VkBool32")]
        public uint alphaToCoverageEnable;

        [NativeTypeName("VkBool32")]
        public uint alphaToOneEnable;
        #endregion
    }
}
