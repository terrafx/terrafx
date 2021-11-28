// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkAttachmentLoadOp;
using static TerraFX.Interop.Vulkan.VkAttachmentStoreOp;
using static TerraFX.Interop.Vulkan.VkFormat;
using static TerraFX.Interop.Vulkan.VkImageLayout;
using static TerraFX.Interop.Vulkan.VkPipelineBindPoint;
using static TerraFX.Interop.Vulkan.VkQueueFlags;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
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
    private readonly VulkanGraphicsContext[] _contexts;

    private ValueLazy<VkQueue> _vulkanCommandQueue;
    private ValueLazy<uint> _vulkanCommandQueueFamilyIndex;
    private ValueLazy<VkDevice> _vulkanDevice;
    private ValueLazy<VkRenderPass> _vulkanRenderPass;
    private ValueLazy<VulkanGraphicsMemoryAllocator> _memoryAllocator;

    private VolatileState _state;

    internal VulkanGraphicsDevice(VulkanGraphicsAdapter adapter)
        : base(adapter)
    {
        _vulkanCommandQueue = new ValueLazy<VkQueue>(GetVulkanCommandQueue);
        _vulkanCommandQueueFamilyIndex = new ValueLazy<uint>(GetVulkanCommandQueueFamilyIndex);
        _vulkanDevice = new ValueLazy<VkDevice>(CreateVulkanDevice);
        _vulkanRenderPass = new ValueLazy<VkRenderPass>(CreateVulkanRenderPass);
        _memoryAllocator = new ValueLazy<VulkanGraphicsMemoryAllocator>(CreateMemoryAllocator);

        _contexts = CreateGraphicsContexts(this, contextCount: 2);

        _ = _state.Transition(to: Initialized);

        static VulkanGraphicsContext[] CreateGraphicsContexts(VulkanGraphicsDevice device, int contextCount)
        {
            var contexts = new VulkanGraphicsContext[contextCount];

            for (var index = 0; index < contexts.Length; index++)
            {
                contexts[index] = new VulkanGraphicsContext(device);
            }

            return contexts;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.</summary>
    ~VulkanGraphicsDevice() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDevice.Adapter" />
    public new VulkanGraphicsAdapter Adapter => (VulkanGraphicsAdapter)base.Adapter;

    /// <inheritdoc />
    public override ReadOnlySpan<GraphicsContext> Contexts => _contexts;

    /// <inheritdoc />
    public override VulkanGraphicsMemoryAllocator MemoryAllocator => _memoryAllocator.Value;

    /// <summary>Gets the <see cref="VkQueue" /> used by the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public VkQueue VulkanCommandQueue => _vulkanCommandQueue.Value;

    /// <summary>Gets the index of the queue family for <see cref="VulkanCommandQueue" />.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public uint VulkanCommandQueueFamilyIndex => _vulkanCommandQueueFamilyIndex.Value;

    /// <summary>Gets the underlying <see cref="VkDevice"/> for the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public VkDevice VulkanDevice => _vulkanDevice.Value;

    /// <summary>Gets the <see cref="VkRenderPass" /> used by the device.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public VkRenderPass VulkanRenderPass => _vulkanRenderPass.Value;

    // VK_LAYER_KHRONOS_validation
    private static ReadOnlySpan<sbyte> VK_LAYER_KHRONOS_VALIDATION_NAME => new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4B, 0x48, 0x52, 0x4F, 0x4E, 0x4F, 0x53, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };

    /// <inheritdoc />
    public override VulkanGraphicsFence CreateFence()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsFence(this, isSignaled: false);
    }

    /// <inheritdoc />
    public override VulkanGraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null)
        => CreatePipeline((VulkanGraphicsPipelineSignature)signature, (VulkanGraphicsShader?)vertexShader, (VulkanGraphicsShader?)pixelShader);

    /// <inheritdoc cref="CreatePipeline(GraphicsPipelineSignature, GraphicsShader?, GraphicsShader?)" />
    public VulkanGraphicsPipeline CreatePipeline(VulkanGraphicsPipelineSignature signature, VulkanGraphicsShader? vertexShader = null, VulkanGraphicsShader? pixelShader = null)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsPipeline(this, signature, vertexShader, pixelShader);
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
    public override VulkanGraphicsSwapchain CreateSwapchain(IGraphicsSurface surface)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsDevice));
        return new VulkanGraphicsSwapchain(this, surface);
    }

    /// <inheritdoc />
    public override void Signal(GraphicsFence fence)
        => Signal((VulkanGraphicsFence)fence);

    /// <inheritdoc cref="Signal(GraphicsFence)" />
    public void Signal(VulkanGraphicsFence fence)
        => ThrowExternalExceptionIfNotSuccess(vkQueueSubmit(VulkanCommandQueue, submitCount: 0, pSubmits: null, fence.VulkanFence));

    /// <inheritdoc />
    public override void WaitForIdle()
    {
        if (_vulkanDevice.IsValueCreated)
        {
            ThrowExternalExceptionIfNotSuccess(vkDeviceWaitIdle(_vulkanDevice.Value));
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            WaitForIdle();

            foreach (var context in _contexts)
            {
                context?.Dispose();
            }

            _memoryAllocator.Dispose(DisposeMemoryAllocator);
            _vulkanRenderPass.Dispose(DisposeVulkanRenderPass);
            _vulkanDevice.Dispose(DisposeVulkanDevice);
        }

        _state.EndDispose();
    }

    private VulkanGraphicsMemoryAllocator CreateMemoryAllocator()
    {
        var allocatorSettings = default(GraphicsMemoryAllocatorSettings);
        return new VulkanGraphicsMemoryAllocator(this, in allocatorSettings);
    }

    private VkDevice CreateVulkanDevice()
    {
        VkDevice vulkanDevice;

        var queuePriority = 1.0f;

        var deviceQueueCreateInfo = new VkDeviceQueueCreateInfo {
            sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
            queueFamilyIndex = VulkanCommandQueueFamilyIndex,
            queueCount = 1,
            pQueuePriorities = &queuePriority,
        };

        var debugModeEnabled = Adapter.Service.DebugModeEnabled;

        const int EnabledExtensionNamesCount = 1;

        var enabledExtensionNames = stackalloc sbyte*[EnabledExtensionNamesCount] {
            (sbyte*)VK_KHR_SWAPCHAIN_EXTENSION_NAME.GetPointer(),
        };

        var enabledLayersNamesCount = debugModeEnabled ? 1u : 0u;
        var enabledLayerNames = stackalloc sbyte*[(int)enabledLayersNamesCount];

        var physicalDeviceFeatures = new VkPhysicalDeviceFeatures();

        var deviceCreateInfo = new VkDeviceCreateInfo {
            sType = VK_STRUCTURE_TYPE_DEVICE_CREATE_INFO,
            queueCreateInfoCount = 1,
            pQueueCreateInfos = &deviceQueueCreateInfo,
            enabledLayerCount = enabledLayersNamesCount,
            ppEnabledLayerNames = enabledLayerNames,
            enabledExtensionCount = EnabledExtensionNamesCount,
            ppEnabledExtensionNames = enabledExtensionNames,
            pEnabledFeatures = &physicalDeviceFeatures,
        };

        if (debugModeEnabled)
        {
            enabledLayerNames[enabledLayersNamesCount - 1] = VK_LAYER_KHRONOS_VALIDATION_NAME.GetPointer();
        }

        ThrowExternalExceptionIfNotSuccess(vkCreateDevice(Adapter.VulkanPhysicalDevice, &deviceCreateInfo, pAllocator: null, &vulkanDevice));

        return vulkanDevice;
    }

    private VkRenderPass CreateVulkanRenderPass()
    {
        VkRenderPass vulkanRenderPass;

        var attachmentDescription = new VkAttachmentDescription {
            format = VK_FORMAT_R8G8B8A8_UNORM,
            samples = VK_SAMPLE_COUNT_1_BIT,
            loadOp = VK_ATTACHMENT_LOAD_OP_CLEAR,
            stencilLoadOp = VK_ATTACHMENT_LOAD_OP_DONT_CARE,
            stencilStoreOp = VK_ATTACHMENT_STORE_OP_DONT_CARE,
            finalLayout = VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
        };

        var colorAttachmentReference = new VkAttachmentReference {
            layout = VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
        };

        var subpass = new VkSubpassDescription {
            pipelineBindPoint = VK_PIPELINE_BIND_POINT_GRAPHICS,
            colorAttachmentCount = 1,
            pColorAttachments = &colorAttachmentReference,
        };

        var renderPassCreateInfo = new VkRenderPassCreateInfo {
            sType = VK_STRUCTURE_TYPE_RENDER_PASS_CREATE_INFO,
            attachmentCount = 1,
            pAttachments = &attachmentDescription,
            subpassCount = 1,
            pSubpasses = &subpass,
        };
        ThrowExternalExceptionIfNotSuccess(vkCreateRenderPass(VulkanDevice, &renderPassCreateInfo, pAllocator: null, &vulkanRenderPass));

        return vulkanRenderPass;
    }

    private void DisposeMemoryAllocator(VulkanGraphicsMemoryAllocator memoryAllocator) => memoryAllocator?.Dispose();

    private void DisposeVulkanDevice(VkDevice vulkanDevice)
    {
        AssertDisposing(_state);

        if (vulkanDevice != VkDevice.NULL)
        {
            vkDestroyDevice(vulkanDevice, pAllocator: null);
        }
    }

    private void DisposeVulkanRenderPass(VkRenderPass vulkanRenderPass)
    {
        AssertDisposing(_state);

        if (vulkanRenderPass != VkRenderPass.NULL)
        {
            vkDestroyRenderPass(VulkanDevice, vulkanRenderPass, pAllocator: null);
        }
    }

    private VkQueue GetVulkanCommandQueue()
    {
        VkQueue vulkanCommandQueue;
        vkGetDeviceQueue(VulkanDevice, VulkanCommandQueueFamilyIndex, queueIndex: 0, &vulkanCommandQueue);
        return vulkanCommandQueue;
    }

    private uint GetVulkanCommandQueueFamilyIndex()
    {
        var vulkanCommandQueueFamilyIndex = uint.MaxValue;

        var vulkanPhysicalDevice = Adapter.VulkanPhysicalDevice;

        uint queueFamilyPropertyCount;
        vkGetPhysicalDeviceQueueFamilyProperties(vulkanPhysicalDevice, &queueFamilyPropertyCount, pQueueFamilyProperties: null);

        var queueFamilyProperties = stackalloc VkQueueFamilyProperties[(int)queueFamilyPropertyCount];
        vkGetPhysicalDeviceQueueFamilyProperties(vulkanPhysicalDevice, &queueFamilyPropertyCount, queueFamilyProperties);

        for (uint i = 0; i < queueFamilyPropertyCount; i++)
        {
            if ((queueFamilyProperties[i].queueFlags & VK_QUEUE_GRAPHICS_BIT) != 0)
            {
                vulkanCommandQueueFamilyIndex = i;
                break;
            }
        }

        if (vulkanCommandQueueFamilyIndex == uint.MaxValue)
        {
            ThrowForMissingFeature();
        }
        return vulkanCommandQueueFamilyIndex;
    }
}
