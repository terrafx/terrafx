// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    unsafe public struct VkPipelineRasterizationStateCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        public VkPipelineRasterizationStateCreateFlags flags;

        public VkBool32 depthClampEnable;

        public VkBool32 rasterizerDiscardEnable;

        public VkPolygonMode polygonMode;

        public VkCullModeFlags cullMode;

        public VkFrontFace frontFace;

        public VkBool32 depthBiasEnable;

        public float depthBiasConstantFactor;

        public float depthBiasClamp;

        public float depthBiasSlopeFactor;

        public float lineWidth;
        #endregion
    }
}
