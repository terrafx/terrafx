// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkDescriptorPoolCreateFlags;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPipelineResourceViewSet : GraphicsPipelineResourceViewSet
{
    private readonly VkDescriptorPool _vkDescriptorPool;
    private readonly VkDescriptorSet _vkDescriptorSet;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsPipelineResourceViewSet(VulkanGraphicsPipeline pipeline, ReadOnlySpan<GraphicsResourceView> resourceViews)
        : base(pipeline, resourceViews)
    {
        var vkDescriptorPool = CreateVkDescriptorPool(pipeline, pipeline.Signature.Resources);
        _vkDescriptorPool = vkDescriptorPool;

        _vkDescriptorSet = CreateVkDescriptorSet(pipeline, vkDescriptorPool, pipeline.Signature.VkDescriptorSetLayout);

        _ = _state.Transition(to: Initialized);
        Name = nameof(GraphicsPipelineResourceViewSet);

        static VkDescriptorPool CreateVkDescriptorPool(VulkanGraphicsPipeline pipeline, UnmanagedReadOnlySpan<GraphicsPipelineResourceInfo> resources)
        {
            var vkDescriptorPoolSizes = UnmanagedArray<VkDescriptorPoolSize>.Empty;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return CreateVkDescriptorPoolInternal(pipeline, resources, ref vkDescriptorPoolSizes);
            }
            finally
            {
                vkDescriptorPoolSizes.Dispose();
            }
        }

        static VkDescriptorPool CreateVkDescriptorPoolInternal(VulkanGraphicsPipeline pipeline, UnmanagedReadOnlySpan<GraphicsPipelineResourceInfo> resources, ref UnmanagedArray<VkDescriptorPoolSize> vkDescriptorPoolSizes)
        {
            var vkDescriptorPool = VkDescriptorPool.NULL;

            if (resources.Length != 0)
            {
                var vkDescriptorPoolCreateInfo = new VkDescriptorPoolCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                    flags = VK_DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT,
                    maxSets = 1,
                };

                var vkDescriptorPoolSizesCount = 0u;
                var constantBufferCount = 0u;
                var textureCount = 0u;

                for (nuint resourceIndex = 0; resourceIndex < resources.Length; resourceIndex++)
                {
                    var resource = resources[resourceIndex];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            if (constantBufferCount == 0)
                            {
                                vkDescriptorPoolSizesCount++;
                            }
                            constantBufferCount++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            if (textureCount == 0)
                            {
                                vkDescriptorPoolSizesCount++;
                            }
                            textureCount++;
                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }
                }

                vkDescriptorPoolSizes = new UnmanagedArray<VkDescriptorPoolSize>(vkDescriptorPoolSizesCount);
                var vkDescriptorPoolSizesIndex = 0u;

                if (constantBufferCount != 0)
                {
                    vkDescriptorPoolSizes[vkDescriptorPoolSizesIndex] = new VkDescriptorPoolSize {
                        type = VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                        descriptorCount = constantBufferCount,
                    };
                    vkDescriptorPoolSizesIndex++;
                }

                if (textureCount != 0)
                {
                    vkDescriptorPoolSizes[vkDescriptorPoolSizesIndex] = new VkDescriptorPoolSize {
                        type = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                        descriptorCount = textureCount,
                    };
                    vkDescriptorPoolSizesIndex++;
                }

                vkDescriptorPoolCreateInfo.poolSizeCount = (uint)vkDescriptorPoolSizes.Length;
                vkDescriptorPoolCreateInfo.pPoolSizes = vkDescriptorPoolSizes.GetPointerUnsafe(0);

                ThrowExternalExceptionIfNotSuccess(vkCreateDescriptorPool(pipeline.Device.VkDevice, &vkDescriptorPoolCreateInfo, pAllocator: null, &vkDescriptorPool));
            }

            return vkDescriptorPool;
        }

        static VkDescriptorSet CreateVkDescriptorSet(VulkanGraphicsPipeline pipeline, VkDescriptorPool vkDescriptorPool, VkDescriptorSetLayout vkDescriptorSetLayout)
        {
            var vkDescriptorSet = VkDescriptorSet.NULL;

            if (vkDescriptorPool != VkDescriptorPool.NULL)
            {
                var descriptorSetAllocateInfo = new VkDescriptorSetAllocateInfo {
                    sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                    descriptorPool = vkDescriptorPool,
                    descriptorSetCount = 1,
                    pSetLayouts = &vkDescriptorSetLayout,
                };
                ThrowExternalExceptionIfNotSuccess(vkAllocateDescriptorSets(pipeline.Device.VkDevice, &descriptorSetAllocateInfo, &vkDescriptorSet));
            }

            return vkDescriptorSet;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipelineResourceViewSet" /> class.</summary>
    ~VulkanGraphicsPipelineResourceViewSet() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsPipelineObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsPipelineObject.Device" />
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
            _name = Device.UpdateName(VK_OBJECT_TYPE_DESCRIPTOR_POOL, VkDescriptorPool, value);
            _ = Device.UpdateName(VK_OBJECT_TYPE_DESCRIPTOR_SET, VkDescriptorSet, value);
        }
    }

    /// <inheritdoc cref="GraphicsPipelineObject.Pipeline" />
    public new VulkanGraphicsPipeline Pipeline => base.Pipeline.As<VulkanGraphicsPipeline>();

    /// <inheritdoc cref="GraphicsPipelineObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDescriptorPool" /> for the resource view set.</summary>
    public VkDescriptorPool VkDescriptorPool
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkDescriptorPool;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDescriptorSet" /> for the resource view set.</summary>
    public VkDescriptorSet VkDescriptorSet
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkDescriptorSet;
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                var vkDevice = Device.VkDevice;
                var vkDescriptorPool = _vkDescriptorPool;

                DisposeVkDescriptorSet(vkDevice, vkDescriptorPool, _vkDescriptorSet);
                DisposeVkDescriptorPool(vkDevice, _vkDescriptorPool);
            }
        }

        _state.EndDispose();

        static void DisposeVkDescriptorPool(VkDevice vkDevice, VkDescriptorPool vkDescriptorPool)
        {
            if (vkDescriptorPool != VkDescriptorPool.NULL)
            {
                vkDestroyDescriptorPool(vkDevice, vkDescriptorPool, pAllocator: null);
            }
        }

        static void DisposeVkDescriptorSet(VkDevice vkDevice, VkDescriptorPool vkDescriptorPool, VkDescriptorSet vkDescriptorSet)
        {
            if (vkDescriptorSet != VkDescriptorSet.NULL)
            {
                _ = vkFreeDescriptorSets(vkDevice, vkDescriptorPool, 1, &vkDescriptorSet);
            }
        }
    }
}
