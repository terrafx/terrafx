// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkColorComponentFlags;
using static TerraFX.Interop.Vulkan.VkCompareOp;
using static TerraFX.Interop.Vulkan.VkCullModeFlags;
using static TerraFX.Interop.Vulkan.VkDynamicState;
using static TerraFX.Interop.Vulkan.VkFrontFace;
using static TerraFX.Interop.Vulkan.VkPrimitiveTopology;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkShaderStageFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkVertexInputRate;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPipeline : GraphicsPipeline
{
    private readonly VkPipeline _vkPipeline;

    private VolatileState _state;

    internal VulkanGraphicsPipeline(VulkanGraphicsRenderPass renderPass, VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader, VulkanGraphicsShader? pixelShader)
        : base(renderPass, signature, vertexShader, pixelShader)
    {
        _vkPipeline = CreateVkPipeline(renderPass, signature, vertexShader, pixelShader);

        _ = _state.Transition(to: Initialized);

        static VkPipeline CreateVkPipeline(VulkanGraphicsRenderPass renderPass, VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader, VulkanGraphicsShader? pixelShader)
        {
            var vkPipelineShaderStageCreateInfos = new UnmanagedArray<VkPipelineShaderStageCreateInfo>(2);
            var vkVertexInputAttributeDescriptions = UnmanagedArray<VkVertexInputAttributeDescription>.Empty;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return CreateVkPipelineInternal(renderPass, signature, vertexShader, pixelShader, vkPipelineShaderStageCreateInfos, ref vkVertexInputAttributeDescriptions);
            }
            finally
            {
                for (var index = 0u; index < vkPipelineShaderStageCreateInfos.Length; index++)
                {
                    var entryPointName = vkPipelineShaderStageCreateInfos[index].pName;
                    Free(entryPointName);
                }
                vkPipelineShaderStageCreateInfos.Dispose();
            }
        }

        static VkPipeline CreateVkPipelineInternal(VulkanGraphicsRenderPass renderPass, VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader, VulkanGraphicsShader? pixelShader, UnmanagedArray<VkPipelineShaderStageCreateInfo> vkPipelineShaderStageCreateInfos, ref UnmanagedArray<VkVertexInputAttributeDescription> vkVertexInputAttributeDescriptions)
        {
            VkPipeline vkPipeline;

            var vkVertexInputBindingDescription = new VkVertexInputBindingDescription {
                inputRate = VK_VERTEX_INPUT_RATE_VERTEX,
            };

            var vkPipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
                vertexBindingDescriptionCount = 1,
                pVertexBindingDescriptions = &vkVertexInputBindingDescription,
            };

            var vkPipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                topology = VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
            };

            var vkPipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                viewportCount = 1,
                scissorCount = 1,
            };

            var vkPipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                frontFace = VK_FRONT_FACE_CLOCKWISE,
                cullMode = VK_CULL_MODE_BACK_BIT,
                lineWidth = 1.0f,
            };

            var vkPipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                rasterizationSamples = VK_SAMPLE_COUNT_1_BIT,
            };

            var vkPipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                depthCompareOp = VK_COMPARE_OP_ALWAYS,
                front = new VkStencilOpState {
                    compareOp = VK_COMPARE_OP_ALWAYS,
                },
                back = new VkStencilOpState {
                    compareOp = VK_COMPARE_OP_ALWAYS,
                },
            };

            var vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState {
                colorWriteMask = VK_COLOR_COMPONENT_A_BIT | VK_COLOR_COMPONENT_B_BIT | VK_COLOR_COMPONENT_G_BIT | VK_COLOR_COMPONENT_R_BIT,
            };

            var vkPipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                attachmentCount = 1,
                pAttachments = &vkPipelineColorBlendAttachmentState,
            };

            var vkDynamicStates = stackalloc VkDynamicState[2] {
                VK_DYNAMIC_STATE_VIEWPORT,
                VK_DYNAMIC_STATE_SCISSOR,
            };

            var vkPipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO,
                dynamicStateCount = 2,
                pDynamicStates = vkDynamicStates,
            };

            var vkPipelineCreateInfo = new VkGraphicsPipelineCreateInfo {
                sType = VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                pViewportState = &vkPipelineViewportStateCreateInfo,
                pRasterizationState = &vkPipelineRasterizationStateCreateInfo,
                pMultisampleState = &vkPipelineMultisampleStateCreateInfo,
                pDepthStencilState = &vkPipelineDepthStencilStateCreateInfo,
                pColorBlendState = &vkPipelineColorBlendStateCreateInfo,
                pDynamicState = &vkPipelineDynamicStateCreateInfo,
                layout = signature.VkPipelineLayout,
                renderPass = renderPass.VkRenderPass,
            };

            var shaderIndex = 0u;

            if (vertexShader is not null)
            {
                var entryPointName = vertexShader.EntryPointName.GetUtf8Span();
                var entryPointNameLength = entryPointName.Length + 1;
                var pName = AllocateArray<sbyte>((uint)entryPointNameLength);

                vkPipelineShaderStageCreateInfos[shaderIndex] = new VkPipelineShaderStageCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = VK_SHADER_STAGE_VERTEX_BIT,
                    module = vertexShader.VulkanShaderModule,
                    pName = pName,
                };

                var destination = new Span<sbyte>(pName, entryPointNameLength);
                entryPointName.CopyTo(destination);
                destination[entryPointName.Length] = 0x00;

                var inputs = signature.Inputs;

                var inputElementsCount = GetInputElementCount(inputs);
                var inputElementsIndex = 0u;

                if (inputElementsCount != 0)
                {
                    vkVertexInputAttributeDescriptions = new UnmanagedArray<VkVertexInputAttributeDescription>(inputElementsCount);

                    for (nuint inputIndex = 0; inputIndex < inputs.Length; inputIndex++)
                    {
                        var input = inputs[inputIndex];
                        var inputElements = input.Elements;

                        var inputBindingStride = 0u;
                        var maxAlignment = 0u;

                        for (nuint inputElementIndex = 0; inputElementIndex < inputElements.Length; inputElementIndex++)
                        {
                            var inputElement = inputElements[inputElementIndex];

                            var inputElementAlignment = inputElement.Alignment;
                            inputBindingStride = AlignUp(inputBindingStride, inputElementAlignment);

                            maxAlignment = Max(maxAlignment, inputElementAlignment);

                            vkVertexInputAttributeDescriptions[inputElementsIndex] = new VkVertexInputAttributeDescription {
                                location = unchecked((uint)inputElementIndex),
                                binding = unchecked((uint)inputIndex),
                                format = inputElement.Format.AsVkFormat(),
                                offset = inputBindingStride,
                            };

                            inputBindingStride += inputElement.Size;
                            inputElementsIndex++;
                        }

                        inputBindingStride = AlignUp(inputBindingStride, maxAlignment);
                        vkVertexInputBindingDescription.stride = inputBindingStride;
                    }
                }

                vkPipelineCreateInfo.pVertexInputState = &vkPipelineVertexInputStateCreateInfo;
                vkPipelineCreateInfo.pInputAssemblyState = &vkPipelineInputAssemblyStateCreateInfo;

                shaderIndex++;
            }

            if (pixelShader is not null)
            {
                var entryPointName = pixelShader.EntryPointName.GetUtf8Span();
                var entryPointNameLength = (nuint)entryPointName.Length + 1;
                var pName = AllocateArray<sbyte>(entryPointNameLength);

                vkPipelineShaderStageCreateInfos[shaderIndex] = new VkPipelineShaderStageCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    stage = VK_SHADER_STAGE_FRAGMENT_BIT,
                    module = pixelShader.VulkanShaderModule,
                    pName = pName,
                };

                var destination = new Span<sbyte>(pName, (int)entryPointNameLength);
                entryPointName.CopyTo(destination);
                destination[entryPointName.Length] = 0x00;

                shaderIndex++;
            }

            if (shaderIndex != 0)
            {
                vkPipelineCreateInfo.stageCount = shaderIndex;
                vkPipelineCreateInfo.pStages = vkPipelineShaderStageCreateInfos.GetPointerUnsafe(0);
            }

            vkPipelineVertexInputStateCreateInfo.vertexAttributeDescriptionCount = (uint)vkVertexInputAttributeDescriptions.Length;
            vkPipelineVertexInputStateCreateInfo.pVertexAttributeDescriptions = vkVertexInputAttributeDescriptions.GetPointerUnsafe(0);

            ThrowExternalExceptionIfNotSuccess(vkCreateGraphicsPipelines(renderPass.Device.VkDevice, pipelineCache: VkPipelineCache.NULL, 1, &vkPipelineCreateInfo, pAllocator: null, &vkPipeline));
            return vkPipeline;
        }

        static nuint GetInputElementCount(UnmanagedReadOnlySpan<GraphicsPipelineInput> inputs)
        {
            nuint inputElementsCount = 0;

            for (nuint i = 0; i <inputs.Length; i++)
            {
                inputElementsCount += inputs[i].Elements.Length;
            }

            return inputElementsCount;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipeline" /> class.</summary>
    ~VulkanGraphicsPipeline() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
    public new VulkanGraphicsShader? PixelShader => base.PixelShader.As<VulkanGraphicsShader>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc cref="GraphicsPipeline.Signature" />
    public new VulkanGraphicsPipelineSignature Signature => base.Signature.As<VulkanGraphicsPipelineSignature>();

    /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
    public new VulkanGraphicsShader? VertexShader => base.VertexShader.As<VulkanGraphicsShader>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPipeline" /> for the pipeline.</summary>
    public VkPipeline VkPipeline
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkPipeline;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            DisposeVkPipeline(Device.VkDevice, _vkPipeline);

            if (isDisposing)
            {
                Signature?.Dispose();
                PixelShader?.Dispose();
                VertexShader?.Dispose();
            }
        }

        _state.EndDispose();

        static void DisposeVkPipeline(VkDevice vkDevice, VkPipeline vkPipeline)
        {
            if (vkPipeline != VkPipeline.NULL)
            {
                vkDestroyPipeline(vkDevice, vkPipeline, pAllocator: null);
            }
        }
    }
}
