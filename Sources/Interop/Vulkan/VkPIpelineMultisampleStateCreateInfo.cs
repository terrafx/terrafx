// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    unsafe public struct VkPipelineMultisampleStateCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        public VkPipelineMultisampleStateCreateFlags flags;

        public VkSampleCountFlags rasterizationSamples;

        public VkBool32 sampleShadingEnable;

        public float minSampleShading;

        public VkSampleMask* pSampleMask;

        public VkBool32 alphaToCoverageEnable;

        public VkBool32 alphaToOneEnable;
        #endregion
    }
}
