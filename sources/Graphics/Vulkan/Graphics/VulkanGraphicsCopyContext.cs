// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkAccessFlags;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPipelineStageFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsCopyContext : GraphicsCopyContext
{
    private readonly VulkanGraphicsFence _fence;
    private readonly VkCommandBuffer _vkCommandBuffer;
    private readonly VkCommandPool _vkCommandPool;

    private string _name = null!;

    private VolatileState _state;

    internal VulkanGraphicsCopyContext(VulkanGraphicsDevice device)
        : base(device)
    {
        var vkCommandPool = CreateVkCommandPool(device);
        _vkCommandPool = vkCommandPool;

        _vkCommandBuffer = CreateVkCommandBuffer(device, vkCommandPool);
        _fence = device.CreateFence(isSignalled: true);

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsCopyContext);

        static VkCommandBuffer CreateVkCommandBuffer(VulkanGraphicsDevice device, VkCommandPool vkCommandPool)
        {
            VkCommandBuffer vkCommandBuffer;

            var vkCommandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                commandPool = vkCommandPool,
                commandBufferCount = 1,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateCommandBuffers(device.VkDevice, &vkCommandBufferAllocateInfo, &vkCommandBuffer));

            return vkCommandBuffer;
        }

        static VkCommandPool CreateVkCommandPool(VulkanGraphicsDevice device)
        {
            VkCommandPool vkCommandPool;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                queueFamilyIndex = device.VkTransferCommandQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(device.VkDevice, &commandPoolCreateInfo, pAllocator: null, &vkCommandPool));

            return vkCommandPool;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsCopyContext" /> class.</summary>
    ~VulkanGraphicsCopyContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc />
    public override VulkanGraphicsFence Fence => _fence;

    /// <inheritdoc />
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = Device.UpdateName(VK_OBJECT_TYPE_COMMAND_BUFFER, VkCommandBuffer, value);
            _ = Device.UpdateName(VK_OBJECT_TYPE_COMMAND_POOL, VkCommandPool, value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandBuffer" /> used by the context.</summary>
    public VkCommandBuffer VkCommandBuffer
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkCommandBuffer;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandPool" /> used by the context.</summary>
    public VkCommandPool VkCommandPool
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkCommandPool;
        }
    }

    /// <inheritdoc />
    public override void Copy(GraphicsBufferView destination, GraphicsBufferView source)
        => Copy((VulkanGraphicsBufferView)destination, (VulkanGraphicsBufferView)source);

    /// <inheritdoc />
    public override void Copy(GraphicsTextureView destination, GraphicsBufferView source)
        => Copy((VulkanGraphicsTextureView)destination, (VulkanGraphicsBufferView)source);

    /// <inheritdoc cref="Copy(GraphicsBufferView, GraphicsBufferView)" />
    public void Copy(VulkanGraphicsBufferView destination, VulkanGraphicsBufferView source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);
        ThrowIfNotInInsertBounds(source.Size, destination.Size);

        var vkBufferCopy = new VkBufferCopy {
            srcOffset = source.Offset,
            dstOffset = destination.Offset,
            size = source.Size,
        };
        vkCmdCopyBuffer(VkCommandBuffer, source.Resource.VkBuffer, destination.Resource.VkBuffer, 1, &vkBufferCopy);
    }

    /// <inheritdoc cref="Copy(GraphicsTextureView, GraphicsBufferView)" />
    public void Copy(VulkanGraphicsTextureView destination, VulkanGraphicsBufferView source)
    {
        ThrowIfNull(destination);
        ThrowIfNull(source);

        var vkCommandBuffer = VkCommandBuffer;

        BeginCopy(vkCommandBuffer, destination);
        {
            var vkBufferImageCopy = new VkBufferImageCopy {
                bufferOffset = source.Offset,
                bufferRowLength = 0,
                bufferImageHeight = 0,
                imageSubresource = new VkImageSubresourceLayers {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    mipLevel = destination.MipLevelIndex,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
                imageExtent = new VkExtent3D {
                    width = destination.Width,
                    height = destination.Height,
                    depth = destination.Depth,
                },
            };

            vkCmdCopyBufferToImage(vkCommandBuffer, source.Resource.VkBuffer, destination.Resource.VkImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &vkBufferImageCopy);
        }
        EndCopy(vkCommandBuffer, destination);

        static void BeginCopy(VkCommandBuffer vkCommandBuffer, VulkanGraphicsTextureView destination)
        {
            var vkImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                oldLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                newLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                image = destination.Resource.VkImage,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    baseMipLevel = destination.MipLevelIndex,
                    levelCount = destination.MipLevelCount,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
            };

            vkCmdPipelineBarrier(vkCommandBuffer, VK_PIPELINE_STAGE_HOST_BIT, VK_PIPELINE_STAGE_TRANSFER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vkImageMemoryBarrier);
        }

        static void EndCopy(VkCommandBuffer vkCommandBuffer, VulkanGraphicsTextureView destination)
        {
            var vkImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                dstAccessMask = VK_ACCESS_SHADER_READ_BIT,
                oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                image = destination.Resource.VkImage,
                subresourceRange = new VkImageSubresourceRange {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    baseMipLevel = destination.MipLevelIndex,
                    levelCount = destination.MipLevelCount,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
            };

            vkCmdPipelineBarrier(vkCommandBuffer, VK_PIPELINE_STAGE_TRANSFER_BIT, VK_PIPELINE_STAGE_FRAGMENT_SHADER_BIT, dependencyFlags: 0, memoryBarrierCount: 0, pMemoryBarriers: null, bufferMemoryBarrierCount: 0, pBufferMemoryBarriers: null, imageMemoryBarrierCount: 1, &vkImageMemoryBarrier);
        }
    }

    /// <inheritdoc />
    public override void Flush()
    {
        var vkCommandBuffer = VkCommandBuffer;
        ThrowExternalExceptionIfNotSuccess(vkEndCommandBuffer(vkCommandBuffer));

        var vkSubmitInfo = new VkSubmitInfo {
            sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
            commandBufferCount = 1,
            pCommandBuffers = &vkCommandBuffer,
        };

        var fence = Fence;
        ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(Device.VkCommandQueue, submitCount: 1, &vkSubmitInfo, fence.VkFence));
        fence.Wait();
    }

    /// <inheritdoc />
    public override void Reset()
    {
        Fence.Reset();

        ThrowExternalExceptionIfNotSuccess(vkResetCommandPool(Device.VkDevice, VkCommandPool, 0));

        var vkCommandBufferBeginInfo = new VkCommandBufferBeginInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
        };
        ThrowExternalExceptionIfNotSuccess(vkBeginCommandBuffer(VkCommandBuffer, &vkCommandBufferBeginInfo));
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            var vkDevice = Device.VkDevice;
            var vkCommandPool = _vkCommandPool;

            DisposeVkCommandBuffer(vkDevice, vkCommandPool, _vkCommandBuffer);
            DisposeVkCommandPool(vkDevice, vkCommandPool);

            if (isDisposing)
            {
                _fence?.Dispose();
            }
        }

        _state.EndDispose();

        static void DisposeVkCommandBuffer(VkDevice vkDevice, VkCommandPool vkCommandPool, VkCommandBuffer vkCommandBuffer)
        {
            if (vkCommandBuffer != VkCommandBuffer.NULL)
            {
                vkFreeCommandBuffers(vkDevice, vkCommandPool, 1, &vkCommandBuffer);
            }
        }

        static void DisposeVkCommandPool(VkDevice vkDevice, VkCommandPool vkCommandPool)
        {
            if (vkCommandPool != VkCommandPool.NULL)
            {
                vkDestroyCommandPool(vkDevice, vkCommandPool, pAllocator: null);
            }
        }
    }
}
