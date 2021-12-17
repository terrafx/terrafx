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
public sealed unsafe class VulkanGraphicsPrimitive : GraphicsPrimitive
{
    private readonly VkDescriptorPool _vkDescriptorPool;
    private readonly VkDescriptorSet _vkDescriptorSet;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsPrimitive(VulkanGraphicsDevice device, VulkanGraphicsPipeline pipeline, VulkanGraphicsBufferView vertexBufferView, VulkanGraphicsBufferView? indexBufferView, ReadOnlySpan<GraphicsResourceView> inputResourceViews)
        : base(device, pipeline, vertexBufferView, indexBufferView, inputResourceViews)
    {
        var vkDescriptorPool = CreateVkDescriptorPool(device, pipeline.Signature.Resources);
        _vkDescriptorPool = vkDescriptorPool;

        _vkDescriptorSet = CreateVkDescriptorSet(device, vkDescriptorPool, pipeline.Signature.VkDescriptorSetLayout);

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsPrimitive);

        static VkDescriptorPool CreateVkDescriptorPool(VulkanGraphicsDevice device, UnmanagedReadOnlySpan<GraphicsPipelineResource> resources)
        {
            var vkDescriptorPoolSizes = UnmanagedArray<VkDescriptorPoolSize>.Empty;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return CreateVkDescriptorPoolInternal(device, resources, ref vkDescriptorPoolSizes);
            }
            finally
            {
                vkDescriptorPoolSizes.Dispose();
            }
        }

        static VkDescriptorPool CreateVkDescriptorPoolInternal(VulkanGraphicsDevice device, UnmanagedReadOnlySpan<GraphicsPipelineResource> resources, ref UnmanagedArray<VkDescriptorPoolSize> vkDescriptorPoolSizes)
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

                ThrowExternalExceptionIfNotSuccess(vkCreateDescriptorPool(device.VkDevice, &vkDescriptorPoolCreateInfo, pAllocator: null, &vkDescriptorPool));
            }

            return vkDescriptorPool;
        }

        static VkDescriptorSet CreateVkDescriptorSet(VulkanGraphicsDevice device, VkDescriptorPool vkDescriptorPool, VkDescriptorSetLayout vkDescriptorSetLayout)
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
                ThrowExternalExceptionIfNotSuccess(vkAllocateDescriptorSets(device.VkDevice, &descriptorSetAllocateInfo, &vkDescriptorSet));
            }

            return vkDescriptorSet;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPrimitive" /> class.</summary>
    ~VulkanGraphicsPrimitive() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsPrimitive.IndexBufferView" />
    public new VulkanGraphicsBufferView? IndexBufferView => base.IndexBufferView.As<VulkanGraphicsBufferView>();

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

    /// <inheritdoc cref="GraphicsPrimitive.Pipeline" />
    public new VulkanGraphicsPipeline Pipeline => base.Pipeline.As<VulkanGraphicsPipeline>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc cref="GraphicsPrimitive.VertexBufferView" />
    public new VulkanGraphicsBufferView VertexBufferView => base.VertexBufferView.As<VulkanGraphicsBufferView>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDescriptorPool" /> for the pipeline.</summary>
    public VkDescriptorPool VkDescriptorPool
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkDescriptorPool;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDescriptorSet" /> for the pipeline.</summary>
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

                Pipeline?.Dispose();
                IndexBufferView?.Dispose();

                foreach (var inputResourceView in InputResourceViews)
                {
                    inputResourceView?.Dispose();
                }

                VertexBufferView?.Dispose();
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
