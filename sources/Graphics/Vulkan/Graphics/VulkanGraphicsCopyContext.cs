// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkAccessFlags;
using static TerraFX.Interop.Vulkan.VkCommandBufferLevel;
using static TerraFX.Interop.Vulkan.VkImageAspectFlags;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPipelineStageFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsCopyContext : GraphicsCopyContext
{
    private VkCommandBuffer _vkCommandBuffer;
    private VkCommandPool _vkCommandPool;

    internal VulkanGraphicsCopyContext(VulkanGraphicsCopyCommandQueue copyCommandQueue) : base(copyCommandQueue)
    {
        // No need for a ContextPool.AddComputeContext(this) as it will be done by the underlying pool

        ContextInfo.Fence = Device.CreateFence(isSignalled: true);

        _vkCommandPool = CreateVkCommandPool();
        _vkCommandBuffer = CreateVkCommandBuffer();

        SetNameUnsafe(Name);

        VkCommandBuffer CreateVkCommandBuffer()
        {
            VkCommandBuffer vkCommandBuffer;

            var vkCommandBufferAllocateInfo = new VkCommandBufferAllocateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_ALLOCATE_INFO,
                pNext = null,
                commandPool = _vkCommandPool,
                level = VK_COMMAND_BUFFER_LEVEL_PRIMARY,
                commandBufferCount = 1,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateCommandBuffers(Device.VkDevice, &vkCommandBufferAllocateInfo, &vkCommandBuffer));

            return vkCommandBuffer;
        }

        VkCommandPool CreateVkCommandPool()
        {
            VkCommandPool vkCommandPool;

            var device = Device;

            var commandPoolCreateInfo = new VkCommandPoolCreateInfo {
                sType = VK_STRUCTURE_TYPE_COMMAND_POOL_CREATE_INFO,
                pNext = null,
                flags = 0,
                queueFamilyIndex = CommandQueue.VkQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(device.VkDevice, &commandPoolCreateInfo, pAllocator: null, &vkCommandPool));

            return vkCommandPool;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsCopyContext" /> class.</summary>
    ~VulkanGraphicsCopyContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new VulkanGraphicsCopyCommandQueue CommandQueue => base.CommandQueue.As<VulkanGraphicsCopyCommandQueue>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsContext{TGraphicsContext}.Fence" />
    public new VulkanGraphicsFence Fence => base.Fence.As<VulkanGraphicsFence>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandBuffer" /> used by the context.</summary>
    public VkCommandBuffer VkCommandBuffer => _vkCommandBuffer;

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandPool" /> used by the context.</summary>
    public VkCommandPool VkCommandPool => _vkCommandPool;

    /// <inheritdoc />
    protected override void CloseUnsafe()
    {
        ThrowExternalExceptionIfNotSuccess(vkEndCommandBuffer(VkCommandBuffer));
    }

    /// <inheritdoc />
    protected override void CopyUnsafe(GraphicsBufferView destination, GraphicsBufferView source)
        => Copy((VulkanGraphicsBufferView)destination, (VulkanGraphicsBufferView)source);

    /// <inheritdoc />
    protected override void CopyUnsafe(GraphicsTextureView destination, GraphicsBufferView source)
        => Copy((VulkanGraphicsTextureView)destination, (VulkanGraphicsBufferView)source);

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            var fence = ContextInfo.Fence;
            fence.Wait();
            fence.Reset();

            fence.Dispose();
            ContextInfo.Fence = null!;
        }

        var vkDevice = Device.VkDevice;
        var vkCommandPool = _vkCommandPool;

        DisposeVkCommandBuffer(vkDevice, vkCommandPool, _vkCommandBuffer);
        _vkCommandBuffer = VkCommandBuffer.NULL;

        DisposeVkCommandPool(vkDevice, vkCommandPool);
        _vkCommandPool = VkCommandPool.NULL;

        _ = CommandQueue.RemoveCopyContext(this);

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

    /// <inheritdoc />
    protected override void ExecuteUnsafe()
    {
        var vkCommandBuffer = VkCommandBuffer;

        var vkSubmitInfo = new VkSubmitInfo {
            sType = VK_STRUCTURE_TYPE_SUBMIT_INFO,
            pNext = null,
            waitSemaphoreCount = 0,
            pWaitSemaphores = null,
            pWaitDstStageMask = null,
            commandBufferCount = 1,
            pCommandBuffers = &vkCommandBuffer,
            signalSemaphoreCount = 0,
            pSignalSemaphores = null,
        };

        var fence = Fence;
        ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(CommandQueue.VkQueue, submitCount: 1, &vkSubmitInfo, fence.VkFence));
        fence.Wait();
    }

    /// <inheritdoc />
    protected override void ResetUnsafe()
    {
        var fence = Fence;
        fence.Wait();
        fence.Reset();

        ThrowExternalExceptionIfNotSuccess(vkResetCommandPool(Device.VkDevice, VkCommandPool, 0));

        var vkCommandBufferBeginInfo = new VkCommandBufferBeginInfo {
            sType = VK_STRUCTURE_TYPE_COMMAND_BUFFER_BEGIN_INFO,
            pNext = null,
            flags = 0,
            pInheritanceInfo = null,
        };
        ThrowExternalExceptionIfNotSuccess(vkBeginCommandBuffer(VkCommandBuffer, &vkCommandBufferBeginInfo));
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_COMMAND_BUFFER, VkCommandBuffer.Value, value);
        Device.SetVkObjectName(VK_OBJECT_TYPE_COMMAND_POOL, VkCommandPool.Value, value);
    }

    private void Copy(VulkanGraphicsBufferView destination, VulkanGraphicsBufferView source)
    {
        var vkBufferCopy = new VkBufferCopy {
            srcOffset = source.ByteOffset,
            dstOffset = destination.ByteOffset,
            size = source.ByteLength,
        };
        vkCmdCopyBuffer(VkCommandBuffer, source.Resource.VkBuffer, destination.Resource.VkBuffer, 1, &vkBufferCopy);
    }

    private void Copy(VulkanGraphicsTextureView destination, VulkanGraphicsBufferView source)
    {
        var vkCommandBuffer = VkCommandBuffer;

        BeginCopy(vkCommandBuffer, destination);
        {
            var vkBufferImageCopy = new VkBufferImageCopy {
                bufferOffset = source.ByteOffset,
                bufferRowLength = 0,
                bufferImageHeight = 0,
                imageSubresource = new VkImageSubresourceLayers {
                    aspectMask = VK_IMAGE_ASPECT_COLOR_BIT,
                    mipLevel = destination.MipLevelIndex,
                    baseArrayLayer = 0,
                    layerCount = 1,
                },
                imageExtent = new VkExtent3D {
                    width = destination.PixelWidth,
                    height = destination.PixelHeight,
                    depth = destination.PixelDepth,
                },
            };

            vkCmdCopyBufferToImage(vkCommandBuffer, source.Resource.VkBuffer, destination.Resource.VkImage, VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL, 1, &vkBufferImageCopy);
        }
        EndCopy(vkCommandBuffer, destination);

        static void BeginCopy(VkCommandBuffer vkCommandBuffer, VulkanGraphicsTextureView destination)
        {
            var vkImageMemoryBarrier = new VkImageMemoryBarrier {
                sType = VK_STRUCTURE_TYPE_IMAGE_MEMORY_BARRIER,
                pNext = null,
                srcAccessMask = 0,
                dstAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                oldLayout = VK_IMAGE_LAYOUT_UNDEFINED,
                newLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                srcQueueFamilyIndex = 0,
                dstQueueFamilyIndex = 0,
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
                pNext = null,
                srcAccessMask = VK_ACCESS_TRANSFER_WRITE_BIT,
                dstAccessMask = VK_ACCESS_SHADER_READ_BIT,
                oldLayout = VK_IMAGE_LAYOUT_TRANSFER_DST_OPTIMAL,
                newLayout = VK_IMAGE_LAYOUT_SHADER_READ_ONLY_OPTIMAL,
                srcQueueFamilyIndex = 0,
                dstQueueFamilyIndex = 0,
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
}
