// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkDescriptorPoolCreateFlags;
using static TerraFX.Interop.Vulkan.VkDescriptorType;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPipelineDescriptorSet : GraphicsPipelineDescriptorSet
{
    private VkDescriptorPool _vkDescriptorPool;
    private VkDescriptorSet _vkDescriptorSet;

    internal VulkanGraphicsPipelineDescriptorSet(VulkanGraphicsPipeline pipeline, in GraphicsPipelineDescriptorSetCreateOptions createOptions) : base(pipeline)
    {
        if (createOptions.TakeResourceViewsOwnership)
        {
            PipelineDescriptorSetInfo.ResourceViews = createOptions.ResourceViews;
        }
        else
        {
            var resourceViews = createOptions.ResourceViews;
            PipelineDescriptorSetInfo.ResourceViews = new GraphicsResourceView[resourceViews.Length];
            resourceViews.CopyTo(PipelineDescriptorSetInfo.ResourceViews, 0);
        }

        _vkDescriptorPool = CreateVkDescriptorPool();
        _vkDescriptorSet = CreateVkDescriptorSet();

        SetNameUnsafe(Name);

        VkDescriptorPool CreateVkDescriptorPool()
        {
            const int MaxDescriptorPoolCount = 2;

            var vkDescriptorPool = VkDescriptorPool.NULL;

            var resources = Pipeline.Signature.Resources;

            if (resources.Length != 0)
            {
                var vkDescriptorPoolCreateInfo = new VkDescriptorPoolCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DESCRIPTOR_POOL_CREATE_INFO,
                    flags = VK_DESCRIPTOR_POOL_CREATE_FREE_DESCRIPTOR_SET_BIT,
                    maxSets = 1,
                };
                
                var vkDescriptorPoolSizesIndex = 0u;
                var constantBufferDescriptorPoolIndex = uint.MaxValue;
                var textureDescriptorPoolIndex = uint.MaxValue;

                var vkDescriptorPoolSizes = stackalloc VkDescriptorPoolSize[MaxDescriptorPoolCount];

                for (nuint index = 0; index < resources.Length; index++)
                {
                    ref readonly var resource = ref resources[index];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            if (constantBufferDescriptorPoolIndex == uint.MaxValue)
                            {
                                vkDescriptorPoolSizes[vkDescriptorPoolSizesIndex] = new VkDescriptorPoolSize {
                                    type = VK_DESCRIPTOR_TYPE_UNIFORM_BUFFER,
                                    descriptorCount = 0,
                                };

                                constantBufferDescriptorPoolIndex = vkDescriptorPoolSizesIndex;
                                vkDescriptorPoolSizesIndex++;
                            }

                            vkDescriptorPoolSizes[constantBufferDescriptorPoolIndex].descriptorCount++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            if (textureDescriptorPoolIndex == uint.MaxValue)
                            {
                                vkDescriptorPoolSizes[vkDescriptorPoolSizesIndex] = new VkDescriptorPoolSize {
                                    type = VK_DESCRIPTOR_TYPE_COMBINED_IMAGE_SAMPLER,
                                    descriptorCount = 0,
                                };

                                textureDescriptorPoolIndex = vkDescriptorPoolSizesIndex;
                                vkDescriptorPoolSizesIndex++;
                            }

                            vkDescriptorPoolSizes[textureDescriptorPoolIndex].descriptorCount++;
                            break;
                        }

                        default:
                        {
                            ThrowForInvalidKind(resources[index].Kind);
                            break;
                        }
                    }
                }

                if (vkDescriptorPoolSizesIndex != 0)
                {
                    vkDescriptorPoolCreateInfo.poolSizeCount = vkDescriptorPoolSizesIndex;
                    vkDescriptorPoolCreateInfo.pPoolSizes = vkDescriptorPoolSizes;

                    ThrowExternalExceptionIfNotSuccess(vkCreateDescriptorPool(pipeline.Device.VkDevice, &vkDescriptorPoolCreateInfo, pAllocator: null, &vkDescriptorPool));
                }
            }

            return vkDescriptorPool;
        }

        VkDescriptorSet CreateVkDescriptorSet()
        {
            var vkDescriptorSet = VkDescriptorSet.NULL;
            var vkDescriptorPool = _vkDescriptorPool;

            if (vkDescriptorPool != VkDescriptorPool.NULL)
            {
                var vkDescriptorSetLayout = Pipeline.Signature.VkDescriptorSetLayout;

                var descriptorSetAllocateInfo = new VkDescriptorSetAllocateInfo {
                    sType = VK_STRUCTURE_TYPE_DESCRIPTOR_SET_ALLOCATE_INFO,
                    pNext = null,
                    descriptorPool = vkDescriptorPool,
                    descriptorSetCount = 1,
                    pSetLayouts = &vkDescriptorSetLayout,
                };
                ThrowExternalExceptionIfNotSuccess(vkAllocateDescriptorSets(pipeline.Device.VkDevice, &descriptorSetAllocateInfo, &vkDescriptorSet));
            }

            return vkDescriptorSet;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPipelineDescriptorSet" /> class.</summary>
    ~VulkanGraphicsPipelineDescriptorSet() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsPipelineObject.Pipeline" />
    public new VulkanGraphicsPipeline Pipeline => base.Pipeline.As<VulkanGraphicsPipeline>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDescriptorPool" /> for the resource view set.</summary>
    public VkDescriptorPool VkDescriptorPool => _vkDescriptorPool;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDescriptorSet" /> for the resource view set.</summary>
    public VkDescriptorSet VkDescriptorSet => _vkDescriptorSet;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var vkDevice = Device.VkDevice;
        var vkDescriptorPool = _vkDescriptorPool;

        DisposeVkDescriptorSet(vkDevice, vkDescriptorPool, _vkDescriptorSet);
        _vkDescriptorSet = VkDescriptorSet.NULL;

        DisposeVkDescriptorPool(vkDevice, vkDescriptorPool);
        _vkDescriptorPool = VkDescriptorPool.NULL;

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

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_DESCRIPTOR_POOL, VkDescriptorPool, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_DESCRIPTOR_SET, VkDescriptorSet, value);
    }
}
