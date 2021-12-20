// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsComputeContext : GraphicsComputeContext
{
    private readonly VulkanGraphicsFence _fence;
    private readonly VkCommandBuffer _vkCommandBuffer;
    private readonly VkCommandPool _vkCommandPool;

    internal VulkanGraphicsComputeContext(VulkanGraphicsDevice device)
        : base(device)
    {
        var vkCommandPool = CreateVkCommandPool(device);
        _vkCommandPool = vkCommandPool;

        _vkCommandBuffer = CreateVkCommandBuffer(device, vkCommandPool);
        _fence = device.CreateFence(isSignalled: true);

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
                queueFamilyIndex = device.VkComputeCommandQueueFamilyIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateCommandPool(device.VkDevice, &commandPoolCreateInfo, pAllocator: null, &vkCommandPool));

            return vkCommandPool;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsComputeContext" /> class.</summary>
    ~VulkanGraphicsComputeContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc />
    public override VulkanGraphicsFence Fence => _fence;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandBuffer" /> used by the context.</summary>
    public VkCommandBuffer VkCommandBuffer
    {
        get
        {
            AssertNotDisposed();
            return _vkCommandBuffer;
        }
    }

    /// <summary>Gets the <see cref="Interop.Vulkan.VkCommandPool" /> used by the context.</summary>
    public VkCommandPool VkCommandPool
    {
        get
        {
            AssertNotDisposed();
            return _vkCommandPool;
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
    public override void SetName(string value)
    {
        value = Device.UpdateName(VK_OBJECT_TYPE_COMMAND_BUFFER, VkCommandBuffer, value);
        _ = Device.UpdateName(VK_OBJECT_TYPE_COMMAND_POOL, VkCommandPool, value);
        base.SetName(value);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var vkDevice = Device.VkDevice;
        var vkCommandPool = _vkCommandPool;

        DisposeVkCommandBuffer(vkDevice, vkCommandPool, _vkCommandBuffer);
        DisposeVkCommandPool(vkDevice, vkCommandPool);

        if (isDisposing)
        {
            _fence?.Dispose();
        }

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
