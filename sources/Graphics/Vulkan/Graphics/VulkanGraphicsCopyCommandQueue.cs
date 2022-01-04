// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsCopyCommandQueue : GraphicsCopyCommandQueue
{
    private VkQueue _vkQueue;

    private readonly uint _vkQueueFamilyIndex;
    private readonly uint _vkQueueIndex;

    private ValuePool<VulkanGraphicsCopyContext> _copyContexts;
    private readonly ValueMutex _copyContextsMutex;

    /// <inheritdoc />
    public VulkanGraphicsCopyCommandQueue(VulkanGraphicsDevice device, uint vkQueueFamilyIndex, uint vkQueueIndex) : base(device)
    {
        _vkQueueFamilyIndex = vkQueueFamilyIndex;
        _vkQueueIndex = vkQueueIndex;

        _vkQueue = GetVkCommandQueue();

        _copyContexts = new ValuePool<VulkanGraphicsCopyContext>();
        _copyContextsMutex = new ValueMutex();

        SetNameUnsafe(Name);

        VkQueue GetVkCommandQueue()
        {
            VkQueue vkCommandQueue;
            vkGetDeviceQueue(device.VkDevice, _vkQueueFamilyIndex, _vkQueueIndex, &vkCommandQueue);
            return vkCommandQueue;
        }
    }

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="VkQueue" /> for the context pool.</summary>
    public VkQueue VkQueue => _vkQueue;

    /// <summary>Gets the family index of <see cref="VkQueue" />.</summary>
    public uint VkQueueFamilyIndex => _vkQueueFamilyIndex;

    /// <summary>Gets the index of <see cref="VkQueue" />.</summary>
    public uint VkQueueIndex => _vkQueueIndex;

    private static VulkanGraphicsCopyContext CreateCopyContext(VulkanGraphicsCopyCommandQueue copyCommandQueue)
    {
        return new VulkanGraphicsCopyContext(copyCommandQueue);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _copyContexts.Dispose();
        }
        _copyContextsMutex.Dispose();

        _vkQueue = VkQueue.NULL;
    }

    /// <inheritdoc />
    protected override void ExecuteContextUnsafe(GraphicsCopyContext context)
    { 
        ExecuteContextUnsafe((VulkanGraphicsCopyContext)context);
    }

    /// <inheritdoc />
    protected override VulkanGraphicsCopyContext RentContextUnsafe()
    {
        return _copyContexts.Rent(&CreateCopyContext, this, _copyContextsMutex);
    }

    /// <inheritdoc />
    protected override void ReturnContextUnsafe(GraphicsCopyContext context)
    {
        ReturnContextUnsafe((VulkanGraphicsCopyContext)context);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_QUEUE, VkQueue, value);
    }

    /// <inheritdoc />
    protected override void SignalFenceUnsafe(GraphicsFence fence)
    { 
        SignalFenceUnsafe((VulkanGraphicsFence)fence);
    }

    /// <inheritdoc />
    protected override void WaitForFenceUnsafe(GraphicsFence fence)
    { 
        WaitForFenceUnsafe((VulkanGraphicsFence)fence);
    }

    /// <inheritdoc />
    protected override void WaitForIdleUnsafe()
    {
        ThrowExternalExceptionIfNotSuccess(vkQueueWaitIdle(VkQueue));
    }

    internal void ExecuteContextUnsafe(VulkanGraphicsCopyContext context)
    {
        var vkCommandBuffer = context.VkCommandBuffer;

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

        var fence = context.Fence;
        ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(VkQueue, submitCount: 1, &vkSubmitInfo, fence.VkFence));
        fence.Wait();
    }

    internal bool RemoveCopyContext(VulkanGraphicsCopyContext copyContext)
    {
        return IsDisposed || _copyContexts.Remove(copyContext);
    }

    internal void ReturnContextUnsafe(VulkanGraphicsCopyContext copyContext)
    {
        _copyContexts.Return(copyContext, _copyContextsMutex);
    }

    internal void SignalFenceUnsafe(VulkanGraphicsFence fence)
    {
        ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(VkQueue, submitCount: 0, pSubmits: null, fence.VkFence));
    }

    internal void WaitForFenceUnsafe(VulkanGraphicsFence fence)
    {
        var vkFence = fence.VkFence;
        ThrowExternalExceptionIfNotSuccess(vkWaitForFences(Device.VkDevice, fenceCount: 1, &vkFence, VkBool32.TRUE, ulong.MaxValue));
    }
}
