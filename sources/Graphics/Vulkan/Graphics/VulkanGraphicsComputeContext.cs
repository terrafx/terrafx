// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using static TerraFX.Interop.Vulkan.VkCommandBufferLevel;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsComputeContext : GraphicsComputeContext
{
    private VkCommandBuffer _vkCommandBuffer;
    private VkCommandPool _vkCommandPool;

    internal VulkanGraphicsComputeContext(VulkanGraphicsComputeCommandQueue computeCommandQueue) : base(computeCommandQueue)
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

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsComputeContext" /> class.</summary>
    ~VulkanGraphicsComputeContext() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsCommandQueueObject{TGraphicsContext}.CommandQueue" />
    public new VulkanGraphicsComputeCommandQueue CommandQueue => base.CommandQueue.As<VulkanGraphicsComputeCommandQueue>();

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

        _ = CommandQueue.RemoveComputeContext(this);

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
        CommandQueue.ExecuteContextUnsafe(this);
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
}
