// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TerraFX.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBufferUsageFlags;
using static TerraFX.Interop.Vulkan.VkImageTiling;
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
using static TerraFX.Runtime.Configuration;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MarshalUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsDevice : GraphicsDevice
{
    private readonly VulkanGraphicsMemoryManager[] _memoryManagers;
    private readonly VkQueue _vkCommandQueue;
    private readonly uint _vkComputeCommandQueueFamilyIndex;
    private readonly VkDevice _vkDevice;
    private readonly uint _vkGraphicsCommandQueueFamilyIndex;
    private readonly uint _vkMemoryTypeCount;
    private readonly uint _vkTransferCommandQueueFamilyIndex;
    private readonly VkDeviceManualImports _vkDeviceManualImports;

    private ContextPool<VulkanGraphicsDevice, VulkanGraphicsComputeContext> _computeContextPool;
    private ContextPool<VulkanGraphicsDevice, VulkanGraphicsCopyContext> _copyContextPool;
    private MemoryBudgetInfo _memoryBudgetInfo;
    private ContextPool<VulkanGraphicsDevice, VulkanGraphicsRenderContext> _renderContextPool;

    internal VulkanGraphicsDevice(VulkanGraphicsAdapter adapter, delegate*<GraphicsDeviceObject, delegate*<in GraphicsMemoryRegion, void>, nuint, bool, GraphicsMemoryAllocator> createMemoryAllocator)
        : base(adapter)
    {
        _vkComputeCommandQueueFamilyIndex = GetVkCommandQueueFamilyIndex(adapter, VK_QUEUE_COMPUTE_BIT);
        _vkTransferCommandQueueFamilyIndex = GetVkCommandQueueFamilyIndex(adapter, VK_QUEUE_TRANSFER_BIT);

        var vkGraphicsCommandQueueFamilyIndex = GetVkCommandQueueFamilyIndex(adapter, VK_QUEUE_GRAPHICS_BIT);
        _vkGraphicsCommandQueueFamilyIndex = vkGraphicsCommandQueueFamilyIndex;

        var vkDevice = CreateVkDevice(adapter, vkGraphicsCommandQueueFamilyIndex);
        _vkDevice = vkDevice;

        InitializeVkDeviceManualImports(vkDevice, ref _vkDeviceManualImports);

        _vkCommandQueue = GetVkCommandQueue(vkDevice, vkGraphicsCommandQueueFamilyIndex);

        var vkMemoryTypeCount = adapter.VkPhysicalDeviceMemoryProperties.memoryTypeCount;
        _vkMemoryTypeCount = vkMemoryTypeCount;

        _memoryManagers = CreateMemoryManagers(this, createMemoryAllocator, vkMemoryTypeCount);

        _computeContextPool = new ContextPool<VulkanGraphicsDevice, VulkanGraphicsComputeContext>();
        _copyContextPool = new ContextPool<VulkanGraphicsDevice, VulkanGraphicsCopyContext>();
        _renderContextPool = new ContextPool<VulkanGraphicsDevice, VulkanGraphicsRenderContext>();

        _memoryBudgetInfo = new MemoryBudgetInfo();
        UpdateMemoryBudgetInfo(ref _memoryBudgetInfo, totalOperationCount: 0);

        static VulkanGraphicsMemoryManager[] CreateMemoryManagers(VulkanGraphicsDevice device, delegate*<GraphicsDeviceObject, delegate*<in GraphicsMemoryRegion, void>, nuint, bool, GraphicsMemoryAllocator> createMemoryAllocator, uint vkMemoryTypeCount)
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

            const int EnabledExtensionNamesCount = 2;

            var enabledVkExtensionNames = stackalloc sbyte*[EnabledExtensionNamesCount] {
                (sbyte*)VK_EXT_MEMORY_BUDGET_EXTENSION_NAME.GetPointer(),
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

        static uint GetVkCommandQueueFamilyIndex(VulkanGraphicsAdapter adapter, VkQueueFlags vkQueueFlags)
        {
            var vkCommandQueueFamilyIndex = uint.MaxValue;
            var vkPhysicalDevice = adapter.VkPhysicalDevice;

            uint vkQueueFamilyPropertyCount;
            vkGetPhysicalDeviceQueueFamilyProperties(vkPhysicalDevice, &vkQueueFamilyPropertyCount, pQueueFamilyProperties: null);

            var vkQueueFamilyProperties = stackalloc VkQueueFamilyProperties[(int)vkQueueFamilyPropertyCount];
            vkGetPhysicalDeviceQueueFamilyProperties(vkPhysicalDevice, &vkQueueFamilyPropertyCount, vkQueueFamilyProperties);

            for (uint i = 0; i < vkQueueFamilyPropertyCount; i++)
            {
                if ((vkQueueFamilyProperties[i].queueFlags & vkQueueFlags) == vkQueueFlags)
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

        static void InitializeVkDeviceManualImports(VkDevice vkDevice, ref VkDeviceManualImports vkDeviceManualImports)
        {
            ReadOnlySpan<sbyte> vkSetDebugUtilsObjectNameEXT = new sbyte[] { 0x76, 0x6B, 0x53, 0x65, 0x74, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x4E, 0x61, 0x6D, 0x65, 0x45, 0x58, 0x54, 0x00 };
            vkDeviceManualImports.vkSetDebugUtilsObjectNameEXT = (delegate* unmanaged<VkDevice, VkDebugUtilsObjectNameInfoEXT*, VkResult>)vkGetDeviceProcAddr(vkDevice, vkSetDebugUtilsObjectNameEXT.GetPointer());
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.</summary>
    ~VulkanGraphicsDevice() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="VkQueue" /> used by the device.</summary>
    public VkQueue VkCommandQueue
    {
        get
        {
            AssertNotDisposed();
            return _vkCommandQueue;
        }
    }

    /// <summary>Gets the index of the queue family that supports compute operations for <see cref="VkCommandQueue" />.</summary>
    public uint VkComputeCommandQueueFamilyIndex
    {
        get
        {
            AssertNotDisposed();
            return _vkComputeCommandQueueFamilyIndex;
        }
    }

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkDevice"/> for the device.</summary>
    public VkDevice VkDevice => _vkDevice;

    /// <summary>Gets the methods that must be manually imported for <see cref="VkDevice" />.</summary>
    public ref readonly VkDeviceManualImports VkDeviceManualImports => ref _vkDeviceManualImports;

    /// <summary>Gets the index of the queue family that supports graphics operations for <see cref="VkCommandQueue" />.</summary>
    public uint VkGraphicsCommandQueueFamilyIndex
    {
        get
        {
            AssertNotDisposed();
            return _vkGraphicsCommandQueueFamilyIndex;
        }
    }

    /// <summary>Gets the <see cref="VkPhysicalDeviceMemoryProperties.memoryTypeCount"/> for the <see cref="VulkanGraphicsAdapter.VkPhysicalDevice" />.</summary>
    public uint VkMemoryTypeCount => _vkMemoryTypeCount;

    /// <summary>Gets the index of the queue family that supports transfer operations for <see cref="VkCommandQueue" />.</summary>
    public uint VkTransferCommandQueueFamilyIndex
    {
        get
        {
            AssertNotDisposed();
            return _vkTransferCommandQueueFamilyIndex;
        }
    }

    // VK_LAYER_KHRONOS_validation
    private static ReadOnlySpan<sbyte> VK_LAYER_KHRONOS_VALIDATION_NAME => new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4B, 0x48, 0x52, 0x4F, 0x4E, 0x4F, 0x53, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };

    /// <inheritdoc />
    public override VulkanGraphicsBuffer CreateBuffer(in GraphicsBufferCreateInfo bufferCreateInfo)
    {
        var vkDevice = VkDevice;

        var vkBufferCreateInfo = new VkBufferCreateInfo {
            sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
            size = bufferCreateInfo.Size,
            usage = GetVkBufferUsageKind(bufferCreateInfo.CpuAccess, bufferCreateInfo.Kind)
        };

        VkBuffer vkBuffer;
        ThrowExternalExceptionIfNotSuccess(vkCreateBuffer(vkDevice, &vkBufferCreateInfo, pAllocator: null, &vkBuffer));

        VkMemoryRequirements vkMemoryRequirements;
        vkGetBufferMemoryRequirements(vkDevice, vkBuffer, &vkMemoryRequirements);

        var memoryManagerIndex = GetMemoryManagerIndex(bufferCreateInfo.CpuAccess, vkMemoryRequirements.memoryTypeBits);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(checked((nuint)vkMemoryRequirements.size), checked((nuint)vkMemoryRequirements.alignment), bufferCreateInfo.AllocationFlags);

        var createInfo = new VulkanGraphicsBuffer.CreateInfo {
            CpuAccess = bufferCreateInfo.CpuAccess,
            CreateMemoryAllocator = (bufferCreateInfo.CreateMemoryAllocator is not null) ? bufferCreateInfo.CreateMemoryAllocator : &GraphicsMemoryAllocator.CreateDefault,
            MemoryRegion = memoryRegion,
            Kind = bufferCreateInfo.Kind,
            VkBuffer = vkBuffer,
        };
        return new VulkanGraphicsBuffer(this, in createInfo);

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
        ThrowIfDisposed();
        return new VulkanGraphicsFence(this, isSignalled);
    }

    /// <inheritdoc />
    public override GraphicsPipelineSignature CreatePipelineSignature(ReadOnlySpan<GraphicsPipelineInput> inputs = default, ReadOnlySpan<GraphicsPipelineResourceInfo> resources = default)
    {
        ThrowIfDisposed();
        return new VulkanGraphicsPipelineSignature(this, inputs, resources);
    }

    /// <inheritdoc />
    public override VulkanGraphicsShader CreateShader(GraphicsShaderKind kind, ReadOnlySpan<byte> bytecode, string entryPointName)
    {
        ThrowIfDisposed();
        return new VulkanGraphicsShader(this, kind, bytecode, entryPointName);
    }

    /// <inheritdoc />
    public override VulkanGraphicsRenderPass CreateRenderPass(IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
    {
        ThrowIfDisposed();
        return new VulkanGraphicsRenderPass(this, surface, renderTargetFormat, minimumRenderTargetCount);
    }

    /// <inheritdoc />
    public override VulkanGraphicsTexture CreateTexture(in GraphicsTextureCreateInfo textureCreateInfo)
    {
        var vkDevice = VkDevice;
        var vkImageCreateInfo = GetVkImageCreateInfo(in textureCreateInfo);

        VkImage vkImage;
        ThrowExternalExceptionIfNotSuccess(vkCreateImage(vkDevice, &vkImageCreateInfo, pAllocator: null, &vkImage));

        VkMemoryRequirements vkMemoryRequirements;
        vkGetImageMemoryRequirements(vkDevice, vkImage, &vkMemoryRequirements);

        var memoryManagerIndex = GetMemoryManagerIndex(textureCreateInfo.CpuAccess, vkMemoryRequirements.memoryTypeBits);
        var memoryRegion = _memoryManagers[memoryManagerIndex].Allocate(checked((nuint)vkMemoryRequirements.size), checked((nuint)vkMemoryRequirements.alignment), textureCreateInfo.AllocationFlags);

        var format = textureCreateInfo.Format;

        var width = textureCreateInfo.Width;
        var height = textureCreateInfo.Height;
        var depth = textureCreateInfo.Depth;

        var rowPitch = width * format.GetSize();
        var slicePitch = rowPitch * height;

        var createInfo = new VulkanGraphicsTexture.CreateInfo {
            CpuAccess = textureCreateInfo.CpuAccess,
            MemoryRegion = memoryRegion,
            TextureInfo = new GraphicsTextureInfo {
                Depth = depth,
                Format = format,
                Height = height,
                Kind = textureCreateInfo.Kind,
                MipLevelCount = textureCreateInfo.MipLevelCount,
                RowPitch = rowPitch,
                SlicePitch = slicePitch,
                Width = width,
            },
            VkImage = vkImage,
        };
        return new VulkanGraphicsTexture(this, in createInfo);

        static VkImageCreateInfo GetVkImageCreateInfo(in GraphicsTextureCreateInfo textureCreateInfo)
        {
            var vkImageCreateInfo = new VkImageCreateInfo {
                sType = VK_STRUCTURE_TYPE_IMAGE_CREATE_INFO,
                imageType = GetVkImageType(textureCreateInfo.Kind),
                format = textureCreateInfo.Format.AsVkFormat(),
                extent = new VkExtent3D {
                    width = textureCreateInfo.Width,
                    height = textureCreateInfo.Height,
                    depth = textureCreateInfo.Depth,
                },
                mipLevels = textureCreateInfo.MipLevelCount,
                arrayLayers = 1,
                samples = VK_SAMPLE_COUNT_1_BIT,
                tiling = VK_IMAGE_TILING_OPTIMAL,
                usage = GetVkImageUsageFlags(textureCreateInfo.CpuAccess),
            };

            return vkImageCreateInfo;
        }

        static VkImageType GetVkImageType(GraphicsTextureKind kind)
        {
            VkImageType vkImageType = 0;

            switch (kind)
            {
                case GraphicsTextureKind.OneDimensional:
                {
                    vkImageType = VK_IMAGE_TYPE_1D;
                    break;
                }

                case GraphicsTextureKind.TwoDimensional:
                {
                    vkImageType = VK_IMAGE_TYPE_2D;
                    break;
                }

                case GraphicsTextureKind.ThreeDimensional:
                {
                    vkImageType = VK_IMAGE_TYPE_3D;
                    break;
                }

                default:
                {
                    ThrowForInvalidKind(kind);
                    break;
                }
            }

            return vkImageType;
        }

        static VkImageUsageFlags GetVkImageUsageFlags(GraphicsResourceCpuAccess cpuAccess)
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
    public GraphicsMemoryBudget GetMemoryBudget(VulkanGraphicsMemoryManager memoryManager)
    {
        ref var memoryBudgetInfo = ref _memoryBudgetInfo;
        var totalOperationCount = GetTotalOperationCount(memoryManager.VkMemoryTypeIndex);

        if ((totalOperationCount - memoryBudgetInfo.TotalOperationCountAtLastUpdate) >= 30)
        {
            UpdateMemoryBudgetInfo(ref memoryBudgetInfo, totalOperationCount);
        }

        using var readerLock = new DisposableReaderLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        return GetMemoryBudgetInternal(memoryManager.VkMemoryTypeIndex, ref memoryBudgetInfo);
    }

    /// <inheritdoc />
    public override VulkanGraphicsComputeContext RentComputeContext()
    {
        ThrowIfDisposed();
        return _computeContextPool.Rent(this, &CreateComputeContext);

        static VulkanGraphicsComputeContext CreateComputeContext(VulkanGraphicsDevice device)
        {
            AssertNotNull(device);
            return new VulkanGraphicsComputeContext(device);
        }
    }

    /// <inheritdoc />
    public override VulkanGraphicsCopyContext RentCopyContext()
    {
        ThrowIfDisposed();
        return _copyContextPool.Rent(this, &CreateCopyContext);

        static VulkanGraphicsCopyContext CreateCopyContext(VulkanGraphicsDevice device)
        {
            AssertNotNull(device);
            return new VulkanGraphicsCopyContext(device);
        }
    }

    /// <inheritdoc />
    public override VulkanGraphicsRenderContext RentRenderContext()
    {
        ThrowIfDisposed();
        return _renderContextPool.Rent(this, &CreateRenderContext);

        static VulkanGraphicsRenderContext CreateRenderContext(VulkanGraphicsDevice device)
        {
            AssertNotNull(device);
            return new VulkanGraphicsRenderContext(device);
        }
    }

    /// <inheritdoc />
    public override void ReturnContext(GraphicsComputeContext renderContext)
        => ReturnContext((VulkanGraphicsComputeContext)renderContext);

    /// <inheritdoc />
    public override void ReturnContext(GraphicsCopyContext renderContext)
        => ReturnContext((VulkanGraphicsCopyContext)renderContext);

    /// <inheritdoc />
    public override void ReturnContext(GraphicsRenderContext renderContext)
        => ReturnContext((VulkanGraphicsRenderContext)renderContext);

    /// <inheritdoc cref="ReturnContext(GraphicsComputeContext)" />
    public void ReturnContext(VulkanGraphicsComputeContext computeContext)
    {
        ThrowIfDisposed();
        ThrowIfNull(computeContext);

        if (computeContext.Device != this)
        {
            ThrowForInvalidParent(computeContext.Device);
        }
        _computeContextPool.Return(computeContext);
    }

    /// <inheritdoc cref="ReturnContext(GraphicsCopyContext)" />
    public void ReturnContext(VulkanGraphicsCopyContext copyContext)
    {
        ThrowIfDisposed();
        ThrowIfNull(copyContext);

        if (copyContext.Device != this)
        {
            ThrowForInvalidParent(copyContext.Device);
        }
        _copyContextPool.Return(copyContext);
    }

    /// <inheritdoc cref="ReturnContext(GraphicsRenderContext)" />
    public void ReturnContext(VulkanGraphicsRenderContext renderContext)
    {
        ThrowIfDisposed();
        ThrowIfNull(renderContext);

        if (renderContext.Device != this)
        {
            ThrowForInvalidParent(renderContext.Device);
        }
        _renderContextPool.Return(renderContext);
    }

    /// <inheritdoc />
    public override void SetName(string value)
    {
        value = UpdateName(VK_OBJECT_TYPE_DEVICE, VkDevice, value);
        _ = UpdateName(VK_OBJECT_TYPE_QUEUE, VkCommandQueue, value);
        base.SetName(value);
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
        WaitForIdle();

        if (isDisposing)
        {
            _computeContextPool.Dispose();
            _copyContextPool.Dispose();
            _renderContextPool.Dispose();
        }

        foreach (var memoryManager in _memoryManagers)
        {
            memoryManager.Dispose();
        }

        DisposeVkDevice(_vkDevice);

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

        ref readonly var vkDeviceManualImports = ref VkDeviceManualImports;

        if (GraphicsService.EnableDebugMode && (vkDeviceManualImports.vkSetDebugUtilsObjectNameEXT is not null) && (objectHandle != 0))
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
                _ = vkDeviceManualImports.vkSetDebugUtilsObjectNameEXT(VkDevice, &vkDebugUtilsObjectNameInfo);
            }
        }

        return name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal string UpdateName(VkObjectType objectType, void* objectHandle, string name, [CallerArgumentExpression("objectHandle")] string component = "")
        => UpdateName(objectType, (ulong)objectHandle, name, component);

    private GraphicsMemoryBudget GetMemoryBudgetInternal(uint vkMemoryTypeIndex, ref MemoryBudgetInfo memoryBudgetInfo)
    {
        // This method should only be called under the reader lock

        var estimatedMemoryBudget = 0UL;
        var estimatedMemoryUsage = 0UL;
        var totalFreeMemoryRegionSize = 0UL;
        var totalSize = 0UL;
        var totalSizeAtLastUpdate = 0UL;

        estimatedMemoryBudget = memoryBudgetInfo.VkPhysicalDeviceMemoryBudgetProperties.heapBudget[vkMemoryTypeIndex];
        estimatedMemoryUsage = memoryBudgetInfo.VkPhysicalDeviceMemoryBudgetProperties.heapUsage[vkMemoryTypeIndex];

        totalFreeMemoryRegionSize = GetTotalFreeMemoryRegionSize(vkMemoryTypeIndex);
        totalSize = GetTotalSize(vkMemoryTypeIndex);
        totalSizeAtLastUpdate = memoryBudgetInfo.GetTotalSizeAtLastUpdate(vkMemoryTypeIndex);

        if ((estimatedMemoryUsage + totalSize) > totalSizeAtLastUpdate)
        {
            estimatedMemoryUsage = estimatedMemoryUsage + totalSize - totalSizeAtLastUpdate;
        }
        else
        {
            estimatedMemoryUsage = 0;
        }

        return new GraphicsMemoryBudget {
            EstimatedMemoryBudget = estimatedMemoryBudget,
            EstimatedMemoryUsage = estimatedMemoryUsage,
            TotalFreeMemoryRegionSize = totalFreeMemoryRegionSize,
            TotalSize = totalSize,
        };
    }

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

    private void UpdateMemoryBudgetInfo(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        using var writerLock = new DisposableWriterLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        UpdateMemoryBudgetInfoInternal(ref memoryBudgetInfo, totalOperationCount);
    }

    private void UpdateMemoryBudgetInfoInternal(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        // This method should only be called under the writer lock

        var vkPhysicalDeviceMemoryBudgetProperties = new VkPhysicalDeviceMemoryBudgetPropertiesEXT {
            sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MEMORY_BUDGET_PROPERTIES_EXT,
        };

        if (Adapter.TryGetVkPhysicalDeviceMemoryBudgetProperties(&vkPhysicalDeviceMemoryBudgetProperties))
        {
            memoryBudgetInfo.VkPhysicalDeviceMemoryBudgetProperties = vkPhysicalDeviceMemoryBudgetProperties;
        }

        memoryBudgetInfo.TotalOperationCountAtLastUpdate = totalOperationCount;

        for (var memoryManagerIndex = 0u; memoryManagerIndex < _memoryManagers.Length; memoryManagerIndex++)
        {
            memoryBudgetInfo.SetTotalFreeMemoryRegionSizeAtLastUpdate(memoryManagerIndex, GetTotalFreeMemoryRegionSize(memoryManagerIndex));
            memoryBudgetInfo.SetTotalSizeAtLastUpdate(memoryManagerIndex, GetTotalSize(memoryManagerIndex));
        }
    }

    private ulong GetTotalOperationCount(uint vkMemoryTypeIndex)
    {
        Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
        return _memoryManagers[vkMemoryTypeIndex].OperationCount;
    }

    private ulong GetTotalFreeMemoryRegionSize(uint vkMemoryTypeIndex)
    {
        Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
        return _memoryManagers[vkMemoryTypeIndex].TotalFreeMemoryRegionSize;
    }

    private ulong GetTotalSize(uint vkMemoryTypeIndex)
    {
        Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
        return _memoryManagers[vkMemoryTypeIndex].Size;
    }
}
