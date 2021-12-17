// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkShaderStageFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPipelineSignature : GraphicsPipelineSignature
{
    private readonly VkDescriptorSetLayout _vkDescriptorSetLayout;
    private readonly VkPipelineLayout _vkPipelineLayout;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsPipelineSignature(VulkanGraphicsDevice device, ReadOnlySpan<GraphicsPipelineInput> inputs, ReadOnlySpan<GraphicsPipelineResourceInfo> resources)
        : base(device, inputs, resources)
    {
        var vkDescriptorSetLayout = CreateVkDescriptorSetLayout(device, resources);
        _vkDescriptorSetLayout = vkDescriptorSetLayout;

        _vkPipelineLayout = CreateVkPipelineLayout(device, vkDescriptorSetLayout);

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsPipelineSignature);

        static VkDescriptorSetLayout CreateVkDescriptorSetLayout(VulkanGraphicsDevice device, ReadOnlySpan<GraphicsPipelineResourceInfo> resources)
        {
            var vkDescriptorSetLayoutBindings = UnmanagedArray<VkDescriptorSetLayoutBinding>.Empty;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return CreateVkDescriptorSetLayoutInternal(device, resources, ref vkDescriptorSetLayoutBindings);
            }
            finally
            {
                vkDescriptorSetLayoutBindings.Dispose();
            }
        }

        static VkDescriptorSetLayout CreateVkDescriptorSetLayoutInternal(VulkanGraphicsDevice device, ReadOnlySpan<GraphicsPipelineResourceInfo> resources, ref UnmanagedArray<VkDescriptorSetLayoutBinding> vkDescriptorSetLayoutBindings)
        {
            var vkDescriptorSetLayout = VkDescriptorSetLayout.NULL;

            if (resources.Length != 0)
            {
                var vkDescriptorSetLayoutCreateInfo = new VkDescriptorSetLayoutCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_LAYOUT_CREATE_INFO,
                };

                var vkDescriptorSetLayoutBindingsIndex = 0u;

                vkDescriptorSetLayoutBindings = new UnmanagedArray<VkDescriptorSetLayoutBinding>((uint)resources.Length);

                for (var resourceIndex = 0; resourceIndex < resources.Length; resourceIndex++)
                {
                    var resource = resources[resourceIndex];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            var stageFlags = GetVkShaderStageFlags(resource.ShaderVisibility);

                            vkDescriptorSetLayoutBindings[vkDescriptorSetLayoutBindingsIndex] = new VkDescriptorSetLayoutBinding {
                                binding = vkDescriptorSetLayoutBindingsIndex,
                                descriptorType = VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                                descriptorCount = 1,
                                stageFlags = stageFlags,
                            };

                            vkDescriptorSetLayoutBindingsIndex++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            var stageFlags = GetVkShaderStageFlags(resource.ShaderVisibility);

                            vkDescriptorSetLayoutBindings[vkDescriptorSetLayoutBindingsIndex] = new VkDescriptorSetLayoutBinding {
                                binding = vkDescriptorSetLayoutBindingsIndex,
                                descriptorType = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                descriptorCount = 1,
                                stageFlags = stageFlags,
                            };

                            vkDescriptorSetLayoutBindingsIndex++;
                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }
                }

                vkDescriptorSetLayoutCreateInfo.bindingCount = (uint)vkDescriptorSetLayoutBindings.Length;
                vkDescriptorSetLayoutCreateInfo.pBindings = vkDescriptorSetLayoutBindings.GetPointerUnsafe(0);

                ThrowExternalExceptionIfNotSuccess(vkCreateDescriptorSetLayout(device.VkDevice, &vkDescriptorSetLayoutCreateInfo, pAllocator: null, &vkDescriptorSetLayout));
            }

            return vkDescriptorSetLayout;
        }

        static VkPipelineLayout CreateVkPipelineLayout(VulkanGraphicsDevice device, VkDescriptorSetLayout vkDescriptorSetLayout)
        {
            VkPipelineLayout vkPipelineLayout;

            var pipelineLayoutCreateInfo = new VkPipelineLayoutCreateInfo {
                sType = VK_STRUCTURE_TYPE_PIPELINE_LAYOUT_CREATE_INFO
            };

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
            var stageFlags = VK_SHADER_STAGE_ALL;

            if (shaderVisibility != GraphicsShaderVisibility.All)
            {
                if (!shaderVisibility.HasFlag(GraphicsShaderVisibility.Vertex))
                {
                    stageFlags &= ~VK_SHADER_STAGE_VERTEX_BIT;
                }

                if (!shaderVisibility.HasFlag(GraphicsShaderVisibility.Pixel))
                {
                    stageFlags &= ~VK_SHADER_STAGE_FRAGMENT_BIT;
                }
            }

            return stageFlags;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipelineSignature" /> class.</summary>
    ~VulkanGraphicsPipelineSignature() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc />
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = Device.UpdateName(VK_OBJECT_TYPE_PIPELINE_LAYOUT, VkPipelineLayout, value);
            _ = Device.UpdateName(VK_OBJECT_TYPE_DESCRIPTOR_SET_LAYOUT, VkDescriptorSetLayout, value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkDescriptorSetLayout" /> for the pipeline.</summary>
    public VkDescriptorSetLayout VkDescriptorSetLayout
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkDescriptorSetLayout;
        }
    }

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkPipelineLayout" /> for the pipeline.</summary>
    public VkPipelineLayout VkPipelineLayout
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkPipelineLayout;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var vkDevice = Device.VkDevice;

            DisposeVkDescriptorSetLayout(vkDevice, _vkDescriptorSetLayout);
            DisposeVkPipelineLayout(vkDevice, _vkPipelineLayout);
        }

        _state.EndDispose();

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
}
