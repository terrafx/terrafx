// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBufferUsageFlags;
using static TerraFX.Interop.Vulkan.VkImageType;
using static TerraFX.Interop.Vulkan.VkImageUsageFlags;
using static TerraFX.Interop.Vulkan.VkMemoryHeapFlags;
using static TerraFX.Interop.Vulkan.VkMemoryPropertyFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPhysicalDeviceType;
using static TerraFX.Interop.Vulkan.VkQueueFlags;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsDevice : GraphicsDevice
{
    private readonly VulkanGraphicsMemoryManager[] _memoryManagers;
    private readonly VkQueue _vkCommandQueue;
    private readonly uint _vkCommandQueueFamilyIndex;
    private readonly VkDevice _vkDevice;
    private readonly uint _vkMemoryTypeCount;

    private string _name = null!;
    private ContextPool<VulkanGraphicsDevice, VulkanGraphicsRenderContext> _renderContextPool;
    private VolatileState _state;

    internal VulkanGraphicsDevice(VulkanGraphicsAdapter adapter, delegate*<GraphicsDeviceObject, ulong, GraphicsMemoryAllocator> createMemoryAllocator)
        : base(adapter)
    {
        var vkCommandQueueFamilyIndex = GetVkCommandQueueFamilyIndex(adapter);
        _vkCommandQueueFamilyIndex = vkCommandQueueFamilyIndex;

        var vkDevice = CreateVkDevice(adapter, vkCommandQueueFamilyIndex);
        _vkDevice = vkDevice;

        _vkCommandQueue = GetVkCommandQueue(vkDevice, vkCommandQueueFamilyIndex);

        var vkMemoryTypeCount = adapter.VkPhysicalDeviceMemoryProperties.memoryTypeCount;
        _vkMemoryTypeCount = vkMemoryTypeCount;

        _memoryManagers = CreateMemoryManagers(this, createMemoryAllocator, vkMemoryTypeCount);
        // TODO: UpdateBudget

        _renderContextPool = new ContextPool<VulkanGraphicsDevice, VulkanGraphicsRenderContext>();

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsDevice);

        static VulkanGraphicsMemoryManager[] CreateMemoryManagers(VulkanGraphicsDevice device, delegate*<GraphicsDeviceObject, ulong, GraphicsMemoryAllocator> createMemoryAllocator, uint vkMemoryTypeCount)
        {
            var memoryManagers = new VulkanGraphicsMemoryManager[vkMemoryTypeCount];

            for (var vkMemoryTypeIndex = 0u; vkMemoryTypeIndex < vkMemoryTypeCount; vkMemoryTypeIndex++)
            {
                memoryManagers[vkMemoryTypeIndex] = new VulkanGraphicsMemoryManager(device, createMemoryAllocator, vkMemoryTypeIndex);
            }

            return memoryManagers;
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

            const int EnabledExtensionNamesCount = 1;

            var enabledVkExtensionNames = stackalloc sbyte*[EnabledExtensionNamesCount] {
                (sbyte*)VK_KHR_SWAPCHAIN_EXTENSION_NAME.GetPointer(),
            };

            var enabledVkLayersNamesCount = GraphicsService.EnableDebugMode ? 1u : 0u;
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

            if (GraphicsService.EnableDebugMode)
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
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = UpdateName(VK_OBJECT_TYPE_DEVICE, VkDevice, value);
            _ = UpdateName(VK_OBJECT_TYPE_QUEUE, VkCommandQueue, value);
        }
    }

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

    /// <summary>Gets the <see cref="VkPhysicalDeviceMemoryProperties.memoryTypeCount"/> for the <see cref="VulkanGraphicsAdapter.VkPhysicalDevice" />.</summary>
    public uint VkMemoryTypeCount => _vkMemoryTypeCount;

    // VK_LAYER_KHRONOS_validation
    private static ReadOnlySpan<sbyte> VK_LAYER_KHRONOS_VALIDATION_NAME => new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4B, 0x48, 0x52, 0x4F, 0x4E, 0x4F, 0x53, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };

    /// <inheritdoc />
    public override VulkanGraphicsBuffer CreateBuffer(GraphicsResourceCpuAccess cpuAccess, GraphicsBufferKind kind, ulong size)
    {
        var vkDevice = VkDevice;

        var vkBufferCreateInfo = new VkBufferCreateInfo {
            sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
            size = size,
            usage = GetVkBufferUsageKind(cpuAccess, kind)
        };

        VkBuffer vkBuffer;
        ThrowExternalExceptionIfNotSuccess(vkCreateBuffer(vkDevice, &vkBufferCreateInfo, pAllocator: null, &vkBuffer));

        VkMemoryRequirements vkMemoryRequirements;
        vkGetBufferMemoryRequirements(vkDevice, vkBuffer, &vkMemoryRequirements);

        var memoryManagerIndex = GetMemoryManagerIndex(cpuAccess, vkMemoryRequirements.memoryTypeBits);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(vkMemoryRequirements.size, vkMemoryRequirements.alignment, GraphicsMemoryAllocationFlags.None);

        return new VulkanGraphicsBuffer(this, cpuAccess, vkMemoryRequirements.size, vkMemoryRequirements.alignment, in memoryRegion, kind, vkBuffer);

        static VkBufferUsageFlags GetVkBufferUsageKind(GraphicsResourceCpuAccess cpuAccess, GraphicsBufferKind kind)
        {
            return cpuAccess switch {
                GraphicsResourceCpuAccess.Read => VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Write => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT | VK_BUFFER_USAGE_INDEX_BUFFER_BIT | VK_BUFFER_USAGE_INDIRECT_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                _ => kind switch {
                    GraphicsBufferKind.Vertex => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    GraphicsBufferKind.Index => VK_BUFFER_USAGE_INDEX_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    GraphicsBufferKind.Constant => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    _ => default,
                }
            };
        }
    }

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
    public override VulkanGraphicsTexture CreateTexture(GraphicsResourceCpuAccess cpuAccess, GraphicsTextureKind kind, GraphicsFormat format, uint width, uint height = 1, ushort depth = 1)
    {
        var vkDevice = VkDevice;

        var vkImageCreateInfo = new VkImageCreateInfo {
            sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
            imageType = kind switch {
                GraphicsTextureKind.OneDimensional => VK_IMAGE_TYPE_1D,
                GraphicsTextureKind.TwoDimensional => VK_IMAGE_TYPE_2D,
                GraphicsTextureKind.ThreeDimensional => VK_IMAGE_TYPE_3D,
                _ => default,
            },
            format = format.AsVkFormat(),
            extent = new VkExtent3D {
                width = width,
                height = height,
                depth = depth,
            },
            mipLevels = 1,
            arrayLayers = 1,
            samples = VK_SAMPLE_COUNT_1_BIT,
            usage = GetVulkanImageUsageKind(cpuAccess, kind),
        };

        VkImage vkImage;
        ThrowExternalExceptionIfNotSuccess(vkCreateImage(vkDevice, &vkImageCreateInfo, pAllocator: null, &vkImage));

        VkMemoryRequirements vkMemoryRequirements;
        vkGetImageMemoryRequirements(vkDevice, vkImage, &vkMemoryRequirements);

        var memoryManagerIndex = GetMemoryManagerIndex(cpuAccess, vkMemoryRequirements.memoryTypeBits);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(vkMemoryRequirements.size, vkMemoryRequirements.alignment, GraphicsMemoryAllocationFlags.None);

        return new VulkanGraphicsTexture(this, cpuAccess, vkMemoryRequirements.size, vkMemoryRequirements.alignment, in memoryRegion, kind, format, width, height, depth, vkImage);

        static VkImageUsageFlags GetVulkanImageUsageKind(GraphicsResourceCpuAccess cpuAccess, GraphicsTextureKind kind)
        {
            return cpuAccess switch {
                GraphicsResourceCpuAccess.Read => VK_IMAGE_USAGE_TRANSFER_DST_BIT,
                GraphicsResourceCpuAccess.Write => VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
                _ => VK_IMAGE_USAGE_SAMPLED_BIT | VK_IMAGE_USAGE_TRANSFER_DST_BIT | VK_IMAGE_USAGE_TRANSFER_SRC_BIT,
            };
        }
    }

    /// <inheritdoc />
    public override GraphicsMemoryBudget GetMemoryBudget(GraphicsMemoryManager memoryManager)
        => GetMemoryBudget((VulkanGraphicsMemoryManager)memoryManager);

    /// <inheritdoc cref="GetMemoryBudget(GraphicsMemoryManager)" />
    public GraphicsMemoryBudget GetMemoryBudget(VulkanGraphicsMemoryManager memoryManager) => new GraphicsMemoryBudget {
        EstimatedBudget = ulong.MaxValue,
        EstimatedUsage = 0,
        TotalAllocatedMemoryRegionSize = 0,
        TotalAllocatorSize = 0,
    };

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
            }

            foreach (var memoryManager in _memoryManagers)
            {
                memoryManager.Dispose();
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal string UpdateName(VkObjectType objectType, ulong objectHandle, string name, [CallerArgumentExpression("objectHandle")] string component = "")
    {
        name ??= "";

        if (GraphicsService.EnableDebugMode && (Service.VkSetDebugUtilsObjectName != null) && (objectHandle != 0))
        {
            var componentName = $"{name}: {component}";

            fixed (sbyte* pName = componentName.GetUtf8Span())
            {
                var vkDebugUtilsObjectNameInfo = new VkDebugUtilsObjectNameInfoEXT {
                    sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_OBJECT_NAME_INFO_EXT,
                    objectType = objectType,
                    objectHandle = objectHandle,
                    pObjectName = pName
                };
                _ = Service.VkSetDebugUtilsObjectName(VkDevice, &vkDebugUtilsObjectNameInfo);
            }
        }

        return name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal string UpdateName(VkObjectType objectType, void* objectHandle, string name, [CallerArgumentExpression("objectHandle")] string component = "")
        => UpdateName(objectType, (ulong)objectHandle, name, component);

    private int GetMemoryManagerIndex(GraphicsResourceCpuAccess cpuAccess, uint vkMemoryTypeBits)
    {
        var isVkPhysicalDeviceTypeIntegratedGpu = Adapter.VkPhysicalDeviceProperties.deviceType == VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU;
        var canBeMultiInstance = false;

        VkMemoryPropertyFlags vkRequiredMemoryPropertyFlags = 0;
        VkMemoryPropertyFlags vkPreferredMemoryPropertyFlags = 0;
        VkMemoryPropertyFlags vkUnpreferredMemoryPropertyFlags = 0;

        switch (cpuAccess)
        {
            case GraphicsResourceCpuAccess.None:
            {
                vkPreferredMemoryPropertyFlags |= isVkPhysicalDeviceTypeIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                canBeMultiInstance = true;
                break;
            }

            case GraphicsResourceCpuAccess.Read:
            {
                vkRequiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                vkPreferredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_CACHED_BIT;
                break;
            }

            case GraphicsResourceCpuAccess.Write:
            {
                vkRequiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                vkPreferredMemoryPropertyFlags |= isVkPhysicalDeviceTypeIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                break;
            }
        }

        var index = -1;
        var lowestCost = int.MaxValue;

        ref readonly var vkPhysicalDeviceMemoryProperties = ref Adapter.VkPhysicalDeviceMemoryProperties;

        for (var i = 0; i < _memoryManagers.Length; i++)
        {
            if ((vkMemoryTypeBits & (1 << i)) == 0)
            {
                continue;
            }

            ref var memoryType = ref vkPhysicalDeviceMemoryProperties.memoryTypes[i];
            ref var memoryHeap = ref vkPhysicalDeviceMemoryProperties.memoryHeaps[(int)memoryType.heapIndex];

            var memoryTypePropertyFlags = memoryType.propertyFlags;

            if ((vkRequiredMemoryPropertyFlags & ~memoryTypePropertyFlags) != 0)
            {
                continue;
            }

            if (!canBeMultiInstance && memoryHeap.flags.HasFlag(VK_MEMORY_HEAP_MULTI_INSTANCE_BIT))
            {
                continue;
            }

            // The cost is calculated as the number of preferred bits not present
            // added to the the number of unpreferred bits that are present. A value
            // of zero represents an ideal match and allows us to return early.

            var cost = BitOperations.PopCount((uint)(vkPreferredMemoryPropertyFlags & ~memoryTypePropertyFlags))
                     + BitOperations.PopCount((uint)(vkUnpreferredMemoryPropertyFlags & memoryTypePropertyFlags));

            if (cost >= lowestCost)
            {
                continue;
            }

            index = i;

            if (cost == 0)
            {
                break;
            }

            lowestCost = cost;
        }

        return index;
    }
}
