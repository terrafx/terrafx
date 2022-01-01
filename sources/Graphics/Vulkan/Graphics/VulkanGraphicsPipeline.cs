// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkBlendFactor;
using static TerraFX.Interop.Vulkan.VkBlendOp;
using static TerraFX.Interop.Vulkan.VkColorComponentFlags;
using static TerraFX.Interop.Vulkan.VkCompareOp;
using static TerraFX.Interop.Vulkan.VkCullModeFlags;
using static TerraFX.Interop.Vulkan.VkDynamicState;
using static TerraFX.Interop.Vulkan.VkFrontFace;
using static TerraFX.Interop.Vulkan.VkLogicOp;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPolygonMode;
using static TerraFX.Interop.Vulkan.VkPrimitiveTopology;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkShaderStageFlags;
using static TerraFX.Interop.Vulkan.VkStencilOp;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.VkVertexInputRate;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.MemoryUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPipeline : GraphicsPipeline
{
    private VkPipeline _vkPipeline;

    internal VulkanGraphicsPipeline(VulkanGraphicsRenderPass renderPass, in GraphicsPipelineCreateOptions createOptions) : base(renderPass)
    {
        PipelineInfo.Signature = createOptions.Signature;
        PipelineInfo.PixelShader = createOptions.PixelShader;
        PipelineInfo.VertexShader = createOptions.VertexShader;

        _vkPipeline = CreateVkPipeline(in createOptions);

        SetNameUnsafe(Name);

        VkPipeline CreateVkPipeline(in GraphicsPipelineCreateOptions createOptions)
        {
            const int MaxShaderCount = 2;

            VkPipeline vkPipeline;

            var vkVertexInputBindingDescription = new VkVertexInputBindingDescription {
                binding = 0,
                stride = 0,
                inputRate = VK_VERTEX_INPUT_RATE_VERTEX,
            };

            var vkPipelineVertexInputStateCreateInfo = new VkPipelineVertexInputStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_VERTEX_INPUT_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                vertexBindingDescriptionCount = 1,
                pVertexBindingDescriptions = &vkVertexInputBindingDescription,
                vertexAttributeDescriptionCount = 0,
                pVertexAttributeDescriptions = null,
            };

            var vkPipelineInputAssemblyStateCreateInfo = new VkPipelineInputAssemblyStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_INPUT_ASSEMBLY_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                topology = VK_PRIMITIVE_TOPOLOGY_TRIANGLE_LIST,
                primitiveRestartEnable = VkBool32.FALSE,
            };

            var vkPipelineViewportStateCreateInfo = new VkPipelineViewportStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_VIEWPORT_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                viewportCount = 1,
                pViewports = null,
                scissorCount = 1,
                pScissors = null,
            };

            var vkPipelineRasterizationStateCreateInfo = new VkPipelineRasterizationStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_RASTERIZATION_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                depthClampEnable = VkBool32.FALSE,
                rasterizerDiscardEnable = VkBool32.FALSE,
                polygonMode = VK_POLYGON_MODE_FILL,
                cullMode = VK_CULL_MODE_BACK_BIT,
                frontFace = VK_FRONT_FACE_CLOCKWISE,
                depthBiasEnable = VkBool32.FALSE,
                depthBiasConstantFactor = 0.0f,
                depthBiasClamp = 0.0f,
                depthBiasSlopeFactor = 0.0f,
                lineWidth = 1.0f,
            };

            var vkPipelineMultisampleStateCreateInfo = new VkPipelineMultisampleStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_MULTISAMPLE_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                rasterizationSamples = VK_SAMPLE_COUNT_1_BIT,
                sampleShadingEnable = VkBool32.FALSE,
                minSampleShading = 0.0f,
                pSampleMask = null,
                alphaToCoverageEnable = VkBool32.FALSE,
                alphaToOneEnable = VkBool32.FALSE,
            };

            var vkPipelineDepthStencilStateCreateInfo = new VkPipelineDepthStencilStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_DEPTH_STENCIL_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                depthTestEnable = VkBool32.FALSE,
                depthWriteEnable = VkBool32.FALSE,
                depthCompareOp = VK_COMPARE_OP_ALWAYS,
                depthBoundsTestEnable = VkBool32.FALSE,
                stencilTestEnable = VkBool32.FALSE,
                front = new VkStencilOpState {
                    failOp = VK_STENCIL_OP_KEEP,
                    passOp = VK_STENCIL_OP_KEEP,
                    depthFailOp = VK_STENCIL_OP_KEEP,
                    compareOp = VK_COMPARE_OP_ALWAYS,
                    compareMask = 0,
                    writeMask = 0,
                    reference = 0,
                },
                back = new VkStencilOpState {
                    failOp = VK_STENCIL_OP_KEEP,
                    passOp = VK_STENCIL_OP_KEEP,
                    depthFailOp = VK_STENCIL_OP_KEEP,
                    compareOp = VK_COMPARE_OP_ALWAYS,
                    compareMask = 0,
                    writeMask = 0,
                    reference = 0,
                },
                minDepthBounds = 0.0f,
                maxDepthBounds = 0.0f,
            };

            var vkPipelineColorBlendAttachmentState = new VkPipelineColorBlendAttachmentState {
                blendEnable = VkBool32.FALSE,
                srcColorBlendFactor = VK_BLEND_FACTOR_ZERO,
                dstColorBlendFactor = VK_BLEND_FACTOR_ZERO,
                colorBlendOp = VK_BLEND_OP_ADD,
                srcAlphaBlendFactor = VK_BLEND_FACTOR_ZERO,
                dstAlphaBlendFactor = VK_BLEND_FACTOR_ZERO,
                alphaBlendOp = VK_BLEND_OP_ADD,
                colorWriteMask = VK_COLOR_COMPONENT_A_BIT | VK_COLOR_COMPONENT_B_BIT | VK_COLOR_COMPONENT_G_BIT | VK_COLOR_COMPONENT_R_BIT,
            };

            var vkPipelineColorBlendStateCreateInfo = new VkPipelineColorBlendStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_COLOR_BLEND_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                logicOpEnable = VkBool32.FALSE,
                logicOp = VK_LOGIC_OP_CLEAR,
                attachmentCount = 1,
                pAttachments = &vkPipelineColorBlendAttachmentState,
            };

            var vkDynamicStates = stackalloc VkDynamicState[2] {
                VK_DYNAMIC_STATE_VIEWPORT,
                VK_DYNAMIC_STATE_SCISSOR,
            };

            var vkPipelineDynamicStateCreateInfo = new VkPipelineDynamicStateCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_DYNAMIC_STATE_CREATE_INFO,
                pNext = null,
                flags = 0,
                dynamicStateCount = 2,
                pDynamicStates = vkDynamicStates,
            };

            var vkPipelineCreateInfo = new VkGraphicsPipelineCreateInfo {
                sType = VK_STRUCTURE_TYPE_GRAPHICS_PIPELINE_CREATE_INFO,
                pNext = null,
                flags = 0,
                stageCount = 0,
                pStages = null,
                pVertexInputState = null,
                pInputAssemblyState = null,
                pTessellationState = null,
                pViewportState = &vkPipelineViewportStateCreateInfo,
                pRasterizationState = &vkPipelineRasterizationStateCreateInfo,
                pMultisampleState = &vkPipelineMultisampleStateCreateInfo,
                pDepthStencilState = &vkPipelineDepthStencilStateCreateInfo,
                pColorBlendState = &vkPipelineColorBlendStateCreateInfo,
                pDynamicState = &vkPipelineDynamicStateCreateInfo,
                layout = createOptions.Signature.As<VulkanGraphicsPipelineSignature>().VkPipelineLayout,
                renderPass = renderPass.VkRenderPass,
                subpass = 0,
                basePipelineHandle = VkPipeline.NULL,
                basePipelineIndex = 0,
            };

            var shaderIndex = 0u;

            var vkPipelineShaderStageCreateInfos = stackalloc VkPipelineShaderStageCreateInfo[MaxShaderCount];
            var vkShaderNames = stackalloc sbyte*[MaxShaderCount];
            var vkVertexInputAttributeDescriptions = UnmanagedArray<VkVertexInputAttributeDescription>.Empty;

            if (VertexShader is VulkanGraphicsShader vertexShader)
            {
                var entryPointName = vertexShader.EntryPointName.GetUtf8Span();
                var entryPointNameLength = entryPointName.Length + 1;

                var pName = AllocateArray<sbyte>((uint)entryPointNameLength);
                var destination = new Span<sbyte>(pName, entryPointNameLength);

                entryPointName.CopyTo(destination);
                destination[entryPointName.Length] = 0x00;

                vkShaderNames[shaderIndex] = pName;
                vkPipelineShaderStageCreateInfos[shaderIndex] = new VkPipelineShaderStageCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    stage = VK_SHADER_STAGE_VERTEX_BIT,
                    module = vertexShader.VkShaderModule,
                    pName = pName,
                    pSpecializationInfo = null,
                };

                var inputs = createOptions.Signature.Inputs;

                if (inputs.Length != 0)
                {
                    vkVertexInputAttributeDescriptions = new UnmanagedArray<VkVertexInputAttributeDescription>(inputs.Length);

                    var alignedByteOffset = 0u;
                    var maxByteAlignment = 0u;

                    for (nuint index = 0; index < inputs.Length; index++)
                    {
                        ref readonly var input = ref inputs[index];

                        var inputByteAlignment = input.ByteAlignment;
                        alignedByteOffset = AlignUp(alignedByteOffset, inputByteAlignment);

                        vkVertexInputAttributeDescriptions[index] = new VkVertexInputAttributeDescription {
                            location = input.BindingIndex,
                            binding = 0,
                            format = input.Format.AsVkFormat(),
                            offset = alignedByteOffset,
                        };

                        maxByteAlignment = Max(maxByteAlignment, inputByteAlignment);

                        alignedByteOffset += input.ByteLength;
                        alignedByteOffset = AlignUp(alignedByteOffset, maxByteAlignment);

                        vkVertexInputBindingDescription.stride = alignedByteOffset;
                    }
                }

                vkPipelineCreateInfo.pVertexInputState = &vkPipelineVertexInputStateCreateInfo;
                vkPipelineCreateInfo.pInputAssemblyState = &vkPipelineInputAssemblyStateCreateInfo;

                shaderIndex++;
            }

            if (PixelShader is VulkanGraphicsShader pixelShader)
            {
                var entryPointName = pixelShader.EntryPointName.GetUtf8Span();
                var entryPointNameLength = entryPointName.Length + 1;

                var pName = AllocateArray<sbyte>((uint)entryPointNameLength);
                var destination = new Span<sbyte>(pName, entryPointNameLength);

                entryPointName.CopyTo(destination);
                destination[entryPointName.Length] = 0x00;

                vkShaderNames[shaderIndex] = pName;
                vkPipelineShaderStageCreateInfos[shaderIndex] = new VkPipelineShaderStageCreateInfo {
                    sType = VK_STRUCTURE_TYPE_PIPELINE_SHADER_STAGE_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    stage = VK_SHADER_STAGE_FRAGMENT_BIT,
                    module = pixelShader.VkShaderModule,
                    pName = pName,
                    pSpecializationInfo = null,
                };

                shaderIndex++;
            }

            if (shaderIndex != 0)
            {
                vkPipelineCreateInfo.stageCount = shaderIndex;
                vkPipelineCreateInfo.pStages = vkPipelineShaderStageCreateInfos;
            }

            vkPipelineVertexInputStateCreateInfo.vertexAttributeDescriptionCount = (uint)vkVertexInputAttributeDescriptions.Length;
            vkPipelineVertexInputStateCreateInfo.pVertexAttributeDescriptions = vkVertexInputAttributeDescriptions.GetPointerUnsafe(0);

            var result = vkCreateGraphicsPipelines(renderPass.Device.VkDevice, pipelineCache: VkPipelineCache.NULL, createInfoCount: 1, &vkPipelineCreateInfo, pAllocator: null, &vkPipeline);

            for (var index = 0; index < MaxShaderCount; index++)
            {
                Free(vkShaderNames[index]);
            }
            vkVertexInputAttributeDescriptions.Dispose();

            ThrowExternalExceptionIfNotSuccess(result, nameof(vkCreateGraphicsPipelines));
            return vkPipeline;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipeline" /> class.</summary>
    ~VulkanGraphicsPipeline() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
    public new VulkanGraphicsShader? PixelShader => base.PixelShader.As<VulkanGraphicsShader>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new VulkanGraphicsRenderPass RenderPass => base.RenderPass.As<VulkanGraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc cref="GraphicsPipeline.Signature" />
    public new VulkanGraphicsPipelineSignature Signature => base.Signature.As<VulkanGraphicsPipelineSignature>();

    /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
    public new VulkanGraphicsShader? VertexShader => base.VertexShader.As<VulkanGraphicsShader>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPipeline" /> for the pipeline.</summary>
    public VkPipeline VkPipeline => _vkPipeline;

    /// <inheritdoc />
    protected override VulkanGraphicsPipelineDescriptorSet CreateDescriptorSetUnsafe(in GraphicsPipelineDescriptorSetCreateOptions createOptions)
    {
        return new VulkanGraphicsPipelineDescriptorSet(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            PipelineInfo.Signature = null!;
            PipelineInfo.PixelShader = null!;
            PipelineInfo.VertexShader = null!;
        }

        DisposeVkPipeline(Device.VkDevice, _vkPipeline);
        _vkPipeline = VkPipeline.NULL;

        static void DisposeVkPipeline(VkDevice vkDevice, VkPipeline vkPipeline)
        {
            if (vkPipeline != VkPipeline.NULL)
            {
                vkDestroyPipeline(vkDevice, vkPipeline, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_PIPELINE, VkPipeline, value);
    }
}
