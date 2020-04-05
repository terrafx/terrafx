// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.Vulkan.HelperUtilities;
using static TerraFX.Interop.VkCompareOp;
using static TerraFX.Interop.VkCullModeFlagBits;
using static TerraFX.Interop.VkDynamicState;
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
using TerraFX.Numerics;

namespace TerraFX.Graphics.Providers.Vulkan
{
    /// <inheritdoc />
    public sealed unsafe class VulkanGraphicsPipeline : GraphicsPipeline
    {
        private ValueLazy<VkPipeline> _vulkanPipeline;

        private State _state;

        internal VulkanGraphicsPipeline(VulkanGraphicsDevice graphicsDevice, VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader, VulkanGraphicsShader? pixelShader)
            : base(graphicsDevice, signature, vertexShader, pixelShader)
        {
            _vulkanPipeline = new ValueLazy<VkPipeline>(CreateVulkanGraphicsPipeline);

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

        /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
        public VulkanGraphicsShader? VulkanPixelShader => (VulkanGraphicsShader?)PixelShader;

        /// <inheritdoc cref="GraphicsPipeline.Signature" />
        public VulkanGraphicsPipelineSignature VulkanSignature => (VulkanGraphicsPipelineSignature)Signature;

        /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
        public VulkanGraphicsShader? VulkanVertexShader => (VulkanGraphicsShader?)VertexShader;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _vulkanPipeline.Dispose(DisposeVulkanPipeline);

                DisposeIfNotNull(Signature);
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

                var pipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                    viewportCount = 1,
                    scissorCount = 1,
                };

                var pipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                    frontFace = VK_FRONT_FACE_CLOCKWISE,
                    cullMode = (uint)VK_CULL_MODE_BACK_BIT,
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

                var dynamicStates = stackalloc VkDynamicState[2] {
                    VK_DYNAMIC_STATE_VIEWPORT,
                    VK_DYNAMIC_STATE_SCISSOR,
                };

                var pipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO,
                    dynamicStateCount = 2,
                    pDynamicStates = dynamicStates,
                };

                var graphicsPipelineCreateInfo = new VkGraphicsPipelineCreateInfo {
                    sType = VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                    pViewportState = &pipelineViewportStateCreateInfo,
                    pRasterizationState = &pipelineRasterizationStateCreateInfo,
                    pMultisampleState = &pipelineMultisampleStateCreateInfo,
                    pDepthStencilState = &pipelineDepthStencilStateCreateInfo,
                    pColorBlendState = &pipelineColorBlendStateCreateInfo,
                    pDynamicState = &pipelineDynamicStateCreateInfo,
                    layout = VulkanSignature.VulkanPipelineLayout,
                    renderPass = graphicsDevice.VulkanRenderPass,
                };

                var vertexShader = VulkanVertexShader;

                if (vertexShader != null)
                {
                    var shaderIndex = pipelineShaderStageCreateInfosCount++;

                    var entryPointName = MarshalStringToUtf8(vertexShader.EntryPointName);
                    var entryPointNameLength = entryPointName.Length + 1;

                    pipelineShaderStageCreateInfos[shaderIndex] = new VkPipelineShaderStageCreateInfo {
                        sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                        stage = VK_SHADER_STAGE_VERTEX_BIT,
                        module = vertexShader.VulkanShaderModule,
                        pName = (sbyte*)Allocate(entryPointNameLength),
                    };

                    var destination = new Span<sbyte>(pipelineShaderStageCreateInfos[shaderIndex].pName, entryPointNameLength);
                    entryPointName.CopyTo(destination);
                    destination[entryPointName.Length] = 0x00;

                    var inputs = VulkanSignature.Inputs;
                    var inputsLength = inputs.Length;

                    var inputElementsCount = GetInputElementsCount(inputs);
                    var inputElementsIndex = 0;

                    if (inputElementsCount != 0)
                    {
                        vertexInputAttributeDescriptions = new VkVertexInputAttributeDescription[inputElementsCount];

                        for (var inputIndex = 0; inputIndex < inputsLength; inputIndex++)
                        {
                            var input = inputs[inputIndex];

                            var inputElements = input.Elements;
                            var inputElementsLength = inputElements.Length;

                            uint inputBindingStride = 0;

                            for (var inputElementIndex = 0; inputElementIndex < inputElementsLength; inputElementIndex++)
                            {
                                var inputElement = inputElements[inputElementIndex];

                                vertexInputAttributeDescriptions[inputElementsIndex] = new VkVertexInputAttributeDescription {
                                    location = unchecked((uint)inputElementIndex),
                                    binding = unchecked((uint)inputIndex),
                                    format = GetInputElementFormat(inputElement.Type),
                                    offset = inputBindingStride,
                                };

                                inputBindingStride += inputElement.Size;
                                inputElementsIndex++;
                            }

                            vertexInputBindingDescription.stride = inputBindingStride;
                        }
                    }

                    graphicsPipelineCreateInfo.pVertexInputState = &pipelineVertexInputStateCreateInfo;
                    graphicsPipelineCreateInfo.pInputAssemblyState = &pipelineInputAssemblyStateCreateInfo;
                }

                var pixelShader = VulkanPixelShader;

                if (pixelShader != null)
                {
                    var shaderIndex = pipelineShaderStageCreateInfosCount++;

                    var entryPointName = MarshalStringToUtf8(pixelShader.EntryPointName);
                    var entryPointNameLength = entryPointName.Length + 1;

                    pipelineShaderStageCreateInfos[shaderIndex] = new VkPipelineShaderStageCreateInfo {
                        sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                        stage = VK_SHADER_STAGE_FRAGMENT_BIT,
                        module = pixelShader.VulkanShaderModule,
                        pName = (sbyte*)Allocate(entryPointNameLength),
                    };

                    var destination = new Span<sbyte>(pipelineShaderStageCreateInfos[shaderIndex].pName, entryPointNameLength);
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

            static int GetInputElementsCount(ReadOnlySpan<GraphicsPipelineInput> inputs)
            {
                var inputElementsCount = 0;

                foreach (var input in inputs)
                {
                    inputElementsCount += input.Elements.Length;
                }

                return inputElementsCount;
            }
        }

        private void DisposeVulkanPipeline(VkPipeline vulkanPipeline)
        {
            if (vulkanPipeline != VK_NULL_HANDLE)
            {
                vkDestroyPipeline(VulkanGraphicsDevice.VulkanDevice, vulkanPipeline, pAllocator: null);
            }
        }

        private VkFormat GetInputElementFormat(Type type)
        {
            var inputElementFormat = VK_FORMAT_UNDEFINED;

            if (type == typeof(Vector2))
            {
                inputElementFormat = VK_FORMAT_R32G32_SFLOAT;
            }
            else if (type == typeof(Vector3))
            {
                inputElementFormat = VK_FORMAT_R32G32B32_SFLOAT;
            }
            else if (type == typeof(Vector4))
            {
                inputElementFormat = VK_FORMAT_R32G32B32A32_SFLOAT;
            }

            return inputElementFormat;
        }
    }
}
