// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from src\spec\vk.xml in the Vulkan-Docs repository for tag v1.0.51-core
// Original source is Copyright © 2015-2017 The Khronos Group Inc.

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct VkGraphicsPipelineCreateInfo
    {
        #region Fields
        public VkStructureType sType;

        public void* pNext;

        public VkPipelineCreateFlags flags;

        public uint stageCount;

        public VkPipelineShaderStageCreateInfo* pStages;

        public VkPipelineVertexInputStateCreateInfo* pVertexInputState;

        public VkPipelineInputAssemblyStateCreateInfo* pInputAssemblyState;

        public VkPipelineTessellationStateCreateInfo* pTessellationState;

        public VkPipelineViewportStateCreateInfo* pViewportState;

        public VkPipelineRasterizationStateCreateInfo* pRasterizationState;

        public VkPipelineMultisampleStateCreateInfo* pMultisampleState;

        public VkPipelineDepthStencilStateCreateInfo* pDepthStencilState;

        public VkPipelineColorBlendStateCreateInfo* pColorBlendState;

        public VkPipelineDynamicStateCreateInfo* pDynamicState;

        public VkPipelineLayout layout;

        public VkRenderPass renderPass;

        public uint subpass;

        public VkPipeline basePipelineHandle;

        public int basePipelineIndex;
        #endregion
    }
}
