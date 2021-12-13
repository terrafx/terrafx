// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkQueueFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsDevice : GraphicsDevice
{
    private readonly VkQueue _vkCommandQueue;
    private readonly uint _vkCommandQueueFamilyIndex;
    private readonly VkDevice _vkDevice;
    private readonly VulkanGraphicsMemoryAllocator _memoryAllocator;

    private ContextPool<VulkanGraphicsDevice, VulkanGraphicsRenderContext> _renderContextPool;
    private VolatileState _state;

    internal VulkanGraphicsDevice(VulkanGraphicsAdapter adapter)
        : base(adapter)
    {
        var vkCommandQueueFamilyIndex = GetVkCommandQueueFamilyIndex(adapter);
        _vkCommandQueueFamilyIndex = vkCommandQueueFamilyIndex;

        var vkDevice = CreateVkDevice(adapter, vkCommandQueueFamilyIndex);
        _vkDevice = vkDevice;

        _vkCommandQueue = GetVkCommandQueue(vkDevice, vkCommandQueueFamilyIndex);

        _memoryAllocator = CreateMemoryAllocator(this);
        _renderContextPool = new ContextPool<VulkanGraphicsDevice, VulkanGraphicsRenderContext>();

        _ = _state.Transition(to: Initialized);

        static VulkanGraphicsMemoryAllocator CreateMemoryAllocator(VulkanGraphicsDevice device)
        {
            var allocatorSettings = default(GraphicsMemoryAllocatorSettings);
            return new VulkanGraphicsMemoryAllocator(device, in allocatorSettings);
        }

        static VkDevice CreateVkDevice(VulkanGraphicsAdapter adapter, uint vkCommandQueueFamilyIndex)
        {
            VkDevice vkDevice;

            var vkQueuePriority = 1.0f;

            var vkDeviceQueueCreateInfo = new VkDeviceQueueCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                queueFamilyIndex = vkCommandQueueFamilyIndex,
                queueCount = 1,
                pQueuePriorities = &vkQueuePriority,
            };

            var debugModeEnabled = adapter.Service.DebugModeEnabled;

            const int EnabledExtensionNamesCount = 1;

            var enabledVkExtensionNames = stackalloc sbyte*[EnabledExtensionNamesCount] {
                (sbyte*)VK_KHR_SWAPCHAIN_EXTENSION_NAME.GetPointer(),
            };

            var enabledVkLayersNamesCount = debugModeEnabled ? 1u : 0u;
            var enabledVkLayerNames = stackalloc sbyte*[(int)enabledVkLayersNamesCount];

            var vkPhysicalDeviceFeatures = new VkPhysicalDeviceFeatures();

            var vkDeviceCreateInfo = new VkDeviceCreateInfo {
                sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
                queueCreateInfoCount = 1,
                pQueueCreateInfos = &vkDeviceQueueCreateInfo,
                enabledLayerCount = enabledVkLayersNamesCount,
                ppEnabledLayerNames = enabledVkLayerNames,
                enabledExtensionCount = EnabledExtensionNamesCount,
                ppEnabledExtensionNames = enabledVkExtensionNames,
                pEnabledFeatures = &vkPhysicalDeviceFeatures,
            };

            if (debugModeEnabled)
            {
                enabledVkLayerNames[enabledVkLayersNamesCount - 1] = VK_LAYER_KHRONOS_VALIDATION_NAME.GetPointer();
            }

            ThrowExternalExceptionIfNotSuccess(vkCreateDevice(adapter.VkPhysicalDevice, &vkDeviceCreateInfo, pAllocator: null, &vkDevice));

            return vkDevice;
        }

        static VkQueue GetVkCommandQueue(VkDevice vkDevice, uint vkCommandQueueFamilyIndex)
        {
            VkQueue vkCommandQueue;
            vkGetDeviceQueue(vkDevice, vkCommandQueueFamilyIndex, queueIndex: 0, &vkCommandQueue);
            return vkCommandQueue;
        }

        static uint GetVkCommandQueueFamilyIndex(VulkanGraphicsAdapter adapter)
        {
            var vkCommandQueueFamilyIndex = uint.MaxValue;
            var vkPhysicalDevice = adapter.VkPhysicalDevice;

            uint vkQueueFamilyPropertyCount;
            vkGetPhysicalDeviceQueueFamilyProperties(vkPhysicalDevice, &vkQueueFamilyPropertyCount, pQueueFamilyProperties: null);

            var vkQueueFamilyProperties = stackalloc VkQueueFamilyProperties[(int)vkQueueFamilyPropertyCount];
            vkGetPhysicalDeviceQueueFamilyProperties(vkPhysicalDevice, &vkQueueFamilyPropertyCount, vkQueueFamilyProperties);

            for (uint i = 0; i < vkQueueFamilyPropertyCount; i++)
            {
                if ((vkQueueFamilyProperties[i].queueFlags & VK_QUEUE_GRAPHICS_BIT) != 0)
                {
                    vkCommandQueueFamilyIndex = i;
                    break;
                }
            }

            if (vkCommandQueueFamilyIndex == uint.MaxValue)
            {
                ThrowForMissingFeature();
            }
            return vkCommandQueueFamilyIndex;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.</summary>
    ~VulkanGraphicsDevice() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDevice.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc />
    public override VulkanGraphicsMemoryAllocator MemoryAllocator => _memoryAllocator;

    /// <inheritdoc cref="GraphicsDevice.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="VkQueue" /> used by the device.</summary>
    public VkQueue VkCommandQueue
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkCommandQueue;
        }
    }

    /// <summary>Gets the index of the queue family for <see cref="VkCommandQueue" />.</summary>
    public uint VkCommandQueueFamilyIndex
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkCommandQueueFamilyIndex;
        }
    }

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkDevice"/> for the device.</summary>
    public VkDevice VkDevice => _vkDevice;

    // VK_LAYER_KHRONOS_validation
    private static ReadOnlySpan<sbyte> VK_LAYER_KHRONOS_VALIDATION_NAME => new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4B, 0x48, 0x52, 0x4F, 0x4E, 0x4F, 0x53, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };

    /// <inheritdoc />
    public override VulkanGraphicsFence CreateFence(bool isSignalled)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsFence(this, isSignalled);
    }

    /// <inheritdoc />
    public override GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResource> resources = default)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsPipelineSignature(this, inputs, resources);
    }

    /// <inheritdoc />
    public override VulkanGraphicsPrimitive CreatePrimitive(GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default)
        => CreatePrimitive((VulkanGraphicsPipeline)pipeline, in vertexBufferView, in indexBufferView, inputResourceViews);

    /// <inheritdoc cref="CreatePrimitive(GraphicsPipeline, in GraphicsResourceView, in GraphicsResourceView, ReadOnlySpan{GraphicsResourceView})" />
    public VulkanGraphicsPrimitive CreatePrimitive(VulkanGraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView = default, ReadOnlySpan<GraphicsResourceView> inputResourceViews = default)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsPrimitive(this, pipeline, in vertexBufferView, in indexBufferView, inputResourceViews);
    }

    /// <inheritdoc />
    public override VulkanGraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsShader(this, kind, bytecode, entryPointName);
    }

    /// <inheritdoc />
    public override VulkanGraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsRenderPass));
        return new VulkanGraphicsRenderPass(this, surface, renderTargetFormat, minimumRenderTargetCount);
    }

    /// <inheritdoc />
    public override VulkanGraphicsRenderContext RentRenderContext()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return _renderContextPool.Rent(this, &CreateRenderContext);

        static VulkanGraphicsRenderContext CreateRenderContext(VulkanGraphicsDevice device)
        {
            AssertNotNull(device);
            return new VulkanGraphicsRenderContext(device);
        }
    }

    /// <inheritdoc />
    public override void ReturnRenderContext(GraphicsRenderContext renderContext)
        => ReturnRenderContext((VulkanGraphicsRenderContext)renderContext);

    /// <inheritdoc cref="ReturnRenderContext(GraphicsRenderContext)" />
    public void ReturnRenderContext(VulkanGraphicsRenderContext renderContext)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        ThrowIfNull(renderContext);

        if (renderContext.Device != this)
        {
            ThrowForInvalidParent(renderContext.Device);
        }
        _renderContextPool.Return(renderContext);
    }

    /// <inheritdoc />
    public override void Signal(GraphicsFence fence)
        => Signal((VulkanGraphicsFence)fence);

    /// <inheritdoc cref="Signal(GraphicsFence)" />
    public void Signal(VulkanGraphicsFence fence)
        => ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(VkCommandQueue, submitCount: 0, pSubmits: null, fence.VkFence));

    /// <inheritdoc />
    public override void WaitForIdle()
    {
        ThrowExternalExceptionIfNotSuccess(vkDeviceWaitIdle(_vkDevice));
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            WaitForIdle();

            if (isDisposing)
            {
                _renderContextPool.Dispose();
                _memoryAllocator?.Dispose();
            }

            DisposeVkDevice(_vkDevice);
        }

        _state.EndDispose();

        static void DisposeVkDevice(VkDevice vkDevice)
        {
            if (vkDevice != VkDevice.NULL)
            {
                vkDestroyDevice(vkDevice, pAllocator: null);
            }
        }
    }
}
