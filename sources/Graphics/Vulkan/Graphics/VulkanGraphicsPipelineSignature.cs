// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkShaderStageFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPipelineSignature : GraphicsPipelineSignature
{
    private VkDescriptorSetLayout _vkDescriptorSetLayout;
    private VkPipelineLayout _vkPipelineLayout;

    internal VulkanGraphicsPipelineSignature(VulkanGraphicsDevice device, in GraphicsPipelineSignatureCreateOptions createOptions) : base(device)
    {
        device.AddPipelineSignature(this);

        if (createOptions.TakeInputsOwnership)
        {
            PipelineSignatureInfo.Inputs = createOptions.Inputs;
        }
        else
        {
            var inputs = createOptions.Inputs;
            PipelineSignatureInfo.Inputs = new UnmanagedArray<GraphicsPipelineInput>(inputs.Length);
            inputs.CopyTo(PipelineSignatureInfo.Inputs);
        }

        if (createOptions.TakeResourcesOwnership)
        {
            PipelineSignatureInfo.Resources = createOptions.Resources;
        }
        else
        {
            var resources = createOptions.Resources;
            PipelineSignatureInfo.Resources = new UnmanagedArray<GraphicsPipelineResource>(resources.Length);
            resources.CopyTo(PipelineSignatureInfo.Resources);
        }

        _vkDescriptorSetLayout = CreateVkDescriptorSetLayout(in createOptions);
        _vkPipelineLayout = CreateVkPipelineLayout();

        SetNameUnsafe(Name);

        VkDescriptorSetLayout CreateVkDescriptorSetLayout(in GraphicsPipelineSignatureCreateOptions createOptions)
        {
            var vkDescriptorSetLayout = VkDescriptorSetLayout.NULL;

            var resources = createOptions.Resources;

            if (resources.Length != 0)
            {
                var vkDescriptorSetLayoutCreateInfo = new VkDescriptorSetLayoutCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    bindingCount = 0,
                    pBindings = null,
                };

                var vkDescriptorSetLayoutBindingsCount = 0;
                var vkDescriptorSetLayoutBindingsIndex = 0;

                for (nuint index = 0; index < resources.Length; index++)
                {
                    ref readonly var resource = ref resources[index];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            vkDescriptorSetLayoutBindingsCount++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            vkDescriptorSetLayoutBindingsCount++;
                            break;
                        }

                        default:
                        {
                            ThrowForInvalidKind(resources[index].Kind);
                            break;
                        }
                    }
                }

                var vkDescriptorSetLayoutBindings =  stackalloc VkDescriptorSetLayoutBinding[vkDescriptorSetLayoutBindingsCount];

                for (nuint index = 0; index < resources.Length; index++)
                {
                    ref readonly var resource = ref resources[index];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            vkDescriptorSetLayoutBindings[vkDescriptorSetLayoutBindingsIndex] = new VkDescriptorSetLayoutBinding {
                                binding = resource.BindingIndex,
                                descriptorType = VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                                descriptorCount = 1,
                                stageFlags = GetVkShaderStageFlags(resource.ShaderVisibility),
                                pImmutableSamplers = null,
                            };

                            vkDescriptorSetLayoutBindingsIndex++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            vkDescriptorSetLayoutBindings[vkDescriptorSetLayoutBindingsIndex] = new VkDescriptorSetLayoutBinding {
                                binding = resource.BindingIndex,
                                descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                descriptorCount = 1,
                                stageFlags = GetVkShaderStageFlags(resource.ShaderVisibility),
                                pImmutableSamplers = null,
                            };

                            vkDescriptorSetLayoutBindingsIndex++;
                            break;
                        }

                        default:
                        {
                            ThrowForInvalidKind(resources[index].Kind);
                            break;
                        }
                    }
                }

                vkDescriptorSetLayoutCreateInfo.bindingCount = (uint)vkDescriptorSetLayoutBindingsCount;
                vkDescriptorSetLayoutCreateInfo.pBindings = vkDescriptorSetLayoutBindings;

                ThrowExternalExceptionIfNotSuccess(vkCreateDescriptorSetLayout(device.VkDevice, &vkDescriptorSetLayoutCreateInfo, pAllocator: null, &vkDescriptorSetLayout));
            }

            return vkDescriptorSetLayout;
        }

        VkPipelineLayout CreateVkPipelineLayout()
        {
            VkPipelineLayout vkPipelineLayout;

            var pipelineLayoutCreateInfo = new VkPipelineLayoutCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO,
                pNext = null,
                flags = 0,
                setLayoutCount = 0,
                pSetLayouts = null,
                pushConstantRangeCount = 0,
                pPushConstantRanges = null,
            };

            var vkDescriptorSetLayout = _vkDescriptorSetLayout;

            if (vkDescriptorSetLayout != VkDescriptorSetLayout.NULL)
            {
                pipelineLayoutCreateInfo.setLayoutCount = 1;
                pipelineLayoutCreateInfo.pSetLayouts = &vkDescriptorSetLayout;
            }

            ThrowExternalExceptionIfNotSuccess(vkCreatePipelineLayout(device.VkDevice, &pipelineLayoutCreateInfo, pAllocator: null, &vkPipelineLayout));

            return vkPipelineLayout;
        }

        static VkShaderStageFlags GetVkShaderStageFlags(GraphicsShaderVisibility shaderVisibility)
        {
            VkShaderStageFlags vkShaderStageFlags = 0;

            if (shaderVisibility == GraphicsShaderVisibility.All)
            {
                vkShaderStageFlags = VK_SHADER_STAGE_ALL;
            }
            else
            {
                if (shaderVisibility.HasFlag(GraphicsShaderVisibility.Vertex))
                {
                    vkShaderStageFlags |= VK_SHADER_STAGE_VERTEX_BIT;
                }

                if (shaderVisibility.HasFlag(GraphicsShaderVisibility.Pixel))
                {
                    vkShaderStageFlags |= VK_SHADER_STAGE_FRAGMENT_BIT;
                }
            }

            return vkShaderStageFlags;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipelineSignature" /> class.</summary>
    ~VulkanGraphicsPipelineSignature() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkDescriptorSetLayout" /> for the pipeline.</summary>
    public VkDescriptorSetLayout VkDescriptorSetLayout => _vkDescriptorSetLayout;

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPipelineLayout" /> for the pipeline.</summary>
    public VkPipelineLayout VkPipelineLayout => _vkPipelineLayout;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var vkDevice = Device.VkDevice;

        DisposeVkDescriptorSetLayout(vkDevice, _vkDescriptorSetLayout);
        _vkDescriptorSetLayout = VkDescriptorSetLayout.NULL;

        DisposeVkPipelineLayout(vkDevice, _vkPipelineLayout);
        _vkPipelineLayout = VkPipelineLayout.NULL;

        _ = Device.RemovePipelineSignature(this);

        static void DisposeVkDescriptorSetLayout(VkDevice vkDevice, VkDescriptorSetLayout vulkanDescriptorSetLayout)
        {
            if (vulkanDescriptorSetLayout != VkDescriptorSetLayout.NULL)
            {
                vkDestroyDescriptorSetLayout(vkDevice, vulkanDescriptorSetLayout, pAllocator: null);
            }
        }

        static void DisposeVkPipelineLayout(VkDevice vkDevice, VkPipelineLayout vkPipelineLayout)
        {
            if (vkPipelineLayout != VkPipelineLayout.NULL)
            {
                vkDestroyPipelineLayout(vkDevice, vkPipelineLayout, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_PIPELINE_LAYOUT, VkPipelineLayout, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_DESCRIPTOR_SET_LAYOUT, VkDescriptorSetLayout, value);
    }
}
