// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkCompareOp;
using static TerraFX.Interop.VkFrontFace;
using static TerraFX.Interop.VkFormat;
using static TerraFX.Interop.VkPrimitiveTopology;
using static TerraFX.Interop.VkSampleCountFlagBits;
using static TerraFX.Interop.VkShaderStageFlagBits;
using static TerraFX.Interop.VkStructureType;
using static TerraFX.Interop.VkVertexInputRate;
using static TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.DisposeUtilities;
using static TerraFX.Utilities.InteropUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsPipeline : GraphicsPipeline
    {
        private ValueLazy<VkPipeline> _vulkanPipeline;
        private ValueLazy<VkPipelineLayout> _vulkanPipelineLayout;

        private State _state;

        internal VulkanGraphicsPipeline(VulkanGraphicsDevice graphicsDevice, VulkanGraphicsShader? vertexShader, VulkanGraphicsShader? pixelShader)
            : base(graphicsDevice, vertexShader, pixelShader)
        {
            _vulkanPipeline = new ValueLazy<VkPipeline>(CreateVulkanGraphicsPipeline);
            _vulkanPipelineLayout = new ValueLazy<VkPipelineLayout>(CreateVulkanPipelineLayout);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipeline" /> class.</summary>
        ~VulkanGraphicsPipeline()
        {
            Dispose(isDisposing: true);
        }

        /// <inheritdoc cref="GraphicsPipeline.GraphicsDevice" />
        public VulkanGraphicsDevice VulkanGraphicsDevice => (VulkanGraphicsDevice)GraphicsDevice;

        /// <summary>Gets the underlying <see cref="VkPipeline" /> for the pipeline.</summary>
        public VkPipeline VulkanPipeline => _vulkanPipeline.Value;

        /// <summary>Gets the underlying <see cref="VkPipelineLayout" /> for the pipeline.</summary>
        public VkPipelineLayout VulkanPipelineLayout => _vulkanPipelineLayout.Value;

        /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
        public VulkanGraphicsShader? VulkanPixelShader => (VulkanGraphicsShader?)PixelShader;

        /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
        public VulkanGraphicsShader? VulkanVertexShader => (VulkanGraphicsShader?)VertexShader;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanPipeline.Dispose(DisposeVulkanPipeline);
                _vulkanPipelineLayout.Dispose(DisposeVulkanPipelineLayout);

                DisposeIfNotNull(PixelShader);
                DisposeIfNotNull(VertexShader);
            }

            _state.EndDispose();
        }

        private VkPipeline CreateVulkanGraphicsPipeline()
        {
            var pipelineShaderStageCreateInfos = stackalloc VkPipelineShaderStageCreateInfo[2];
            uint pipelineShaderStageCreateInfosCount = 0;

            try
            {
                VkPipeline vulkanPipeline;

                var graphicsDevice = VulkanGraphicsDevice;
                var graphicsSurface = graphicsDevice.GraphicsSurface;

                var vertexInputBindingDescription = new VkVertexInputBindingDescription {
                    inputRate = VK_VERTEX_INPUT_RATE_VERTEX,
                };

                var vertexInputAttributeDescriptions = Array.Empty<VkVertexInputAttributeDescription>();

                var pipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
                    vertexBindingDescriptionCount = 1,
                    pVertexBindingDescriptions = &vertexInputBindingDescription,
                };

                var pipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                    topology = VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
                };

                var viewport = new VkViewport {
                    width = graphicsSurface.Width,
                    height = graphicsSurface.Height,
                    minDepth = 0.0f,
                    maxDepth = 1.0f,
                };

                var scissorRect2D = new VkRect2D {
                    extent = new VkExtent2D {
                        width = (uint)viewport.width,
                        height = (uint)viewport.height,
                    },
                };

                var pipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                    viewportCount = 1,
                    pViewports = &viewport,
                    scissorCount = 1,
                    pScissors = &scissorRect2D,
                };

                var pipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                    frontFace = VK_FRONT_FACE_CLOCKWISE,
                    lineWidth = 1.0f,
                };

                var pipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                    rasterizationSamples = VK_SAMPLE_COUNT_1_BIT,
                };

                var pipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                    depthCompareOp = VK_COMPARE_OP_ALWAYS,
                    front = new VkStencilOpState {
                        compareOp = VK_COMPARE_OP_ALWAYS,
                    },
                    back = new VkStencilOpState {
                        compareOp = VK_COMPARE_OP_ALWAYS,
                    },
                };

                var pipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState {
                    colorWriteMask = 0xF,
                };

                var pipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                    attachmentCount = 1,
                    pAttachments = &pipelineColorBlendAttachmentState,
                };

                var graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo {
                    sType = VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                    pViewportState = &pipelineViewportStateCreateInfo,
                    pRasterizationState = &pipelineRasterizationStateCreateInfo,
                    pMultisampleState = &pipelineMultisampleStateCreateInfo,
                    pDepthStencilState = &pipelineDepthStencilStateCreateInfo,
                    pColorBlendState = &pipelineColorBlendStateCreateInfo,
                    layout = VulkanPipelineLayout,
                    renderPass = graphicsDevice.VulkanRenderPass,
                };

                var vertexShader = VulkanVertexShader;

                if (vertexShader != null)
                {
                    var index = pipelineShaderStageCreateInfosCount++;

                    var entryPointName = MarshalStringToUtf8(vertexShader.EntryPointName);
                    var entryPointNameLength = entryPointName.Length + 1;

                    pipelineShaderStageCreateInfos[index] = new VkPipelineShaderStageCreateInfo {
                        sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                        stage = VK_SHADER_STAGE_VERTEX_BIT,
                        module = vertexShader.VulkanShaderModule,
                        pName = (sbyte*)Allocate(entryPointNameLength),
                    };

                    var destination = new Span<sbyte>(pipelineShaderStageCreateInfos[index].pName, entryPointNameLength);
                    entryPointName.CopyTo(destination);
                    destination[entryPointName.Length] = 0x00;

                    vertexInputBindingDescription.stride = sizeof(float) * 7;
                    vertexInputAttributeDescriptions = new VkVertexInputAttributeDescription[2] {
                        new VkVertexInputAttributeDescription {
                            format = VK_FORMAT_R32G32B32_SFLOAT,
                        },
                        new VkVertexInputAttributeDescription {
                            location = 1,
                            format = VK_FORMAT_R32G32B32A32_SFLOAT,
                            offset = sizeof(float) * 3
                        },
                    };

                    graphicsPipelineCreateInfo.pVertexInputState = &pipelineVertexInputStateCreateInfo;
                    graphicsPipelineCreateInfo.pInputAssemblyState = &pipelineInputAssemblyStateCreateInfo;
                }

                var pixelShader = VulkanPixelShader;

                if (pixelShader != null)
                {
                    var index = pipelineShaderStageCreateInfosCount++;

                    var entryPointName = MarshalStringToUtf8(pixelShader.EntryPointName);
                    var entryPointNameLength = entryPointName.Length + 1;

                    pipelineShaderStageCreateInfos[index] = new VkPipelineShaderStageCreateInfo {
                        sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                        stage = VK_SHADER_STAGE_FRAGMENT_BIT,
                        module = pixelShader.VulkanShaderModule,
                        pName = (sbyte*)Allocate(entryPointNameLength),
                    };

                    var destination = new Span<sbyte>(pipelineShaderStageCreateInfos[index].pName, entryPointNameLength);
                    entryPointName.CopyTo(destination);
                    destination[entryPointName.Length] = 0x00;
                }

                if (pipelineShaderStageCreateInfosCount != 0)
                {
                    graphicsPipelineCreateInfo.stageCount = pipelineShaderStageCreateInfosCount;
                    graphicsPipelineCreateInfo.pStages = pipelineShaderStageCreateInfos;
                }

                fixed (VkVertexInputAttributeDescription* pVertexInputAttributeDescriptions = vertexInputAttributeDescriptions)
                {
                    pipelineVertexInputStateCreateInfo.vertexAttributeDescriptionCount = unchecked((uint)vertexInputAttributeDescriptions.Length);
                    pipelineVertexInputStateCreateInfo.pVertexAttributeDescriptions = pVertexInputAttributeDescriptions;

                    ThrowExternalExceptionIfNotSuccess(nameof(vkCreateGraphicsPipelines), vkCreateGraphicsPipelines(graphicsDevice.VulkanDevice, pipelineCache: VK_NULL_HANDLE, 1, &graphicsPipelineCreateInfo, pAllocator: null, (ulong*)&vulkanPipeline));
                }

                return vulkanPipeline;
            }
            finally
            {
                for (uint index = 0; index < pipelineShaderStageCreateInfosCount; index++)
                {
                    var entryPointName = pipelineShaderStageCreateInfos[index].pName;

                    if (entryPointName != null)
                    {
                        Free(entryPointName);
                    }
                }
            }
        }

        private VkPipelineLayout CreateVulkanPipelineLayout()
        {
            VkPipelineLayout vulkanPipelineLayout;

            var pipelineLayoutCreateInfo = new VkPipelineLayoutCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO
            };
            ThrowExternalExceptionIfNotSuccess(nameof(vkCreatePipelineLayout), vkCreatePipelineLayout(VulkanGraphicsDevice.VulkanDevice, &pipelineLayoutCreateInfo, pAllocator: null, (ulong*)&vulkanPipelineLayout));

            return vulkanPipelineLayout;
        }

        private void DisposeVulkanPipeline(VkPipeline vulkanPipeline)
        {
            if (vulkanPipeline != VK_NULL_HANDLE)
            {
                vkDestroyPipeline(VulkanGraphicsDevice.VulkanDevice, vulkanPipeline, pAllocator: null);
            }
        }

        private void DisposeVulkanPipelineLayout(VkPipelineLayout vulkanPipelineLayout)
        {
            if (vulkanPipelineLayout != VK_NULL_HANDLE)
            {
                vkDestroyPipelineLayout(VulkanGraphicsDevice.VulkanDevice, vulkanPipelineLayout, pAllocator: null);
            }
        }
    }
}
