// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.Vulkan.VkMemoryHeapFlags;
using static TerraFX.Interop.Vulkan.VkMemoryPropertyFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkPhysicalDeviceType;
using static TerraFX.Interop.Vulkan.VkQueueFlags;
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
    private const int VkQueueFamilyCount = 3;

    private VkDevice _vkDevice;
    private readonly VkDeviceManualImports _vkDeviceManualImports;

    private readonly VulkanGraphicsMemoryManager[] _memoryManagers;

    private ValueList<VulkanGraphicsBuffer> _buffers;
    private readonly ValueMutex _buffersMutex;

    private ValueList<VulkanGraphicsFence> _fences;
    private readonly ValueMutex _fencesMutex;

    private ValueList<VulkanGraphicsPipelineSignature> _pipelineSignatures;
    private readonly ValueMutex _pipelineSignaturesMutex;

    private ValueList<VulkanGraphicsRenderPass> _renderPasses;
    private readonly ValueMutex _renderPassesMutex;

    private ValueList<VulkanGraphicsShader> _shaders;
    private readonly ValueMutex _shadersMutex;

    private ValueList<VulkanGraphicsTexture> _textures;
    private readonly ValueMutex _texturesMutex;

    private MemoryBudgetInfo _memoryBudgetInfo;

    internal VulkanGraphicsDevice(VulkanGraphicsAdapter adapter, in GraphicsDeviceCreateOptions createOptions) : base(adapter)
    {
        var vkQueueFamilyIndices = stackalloc uint[VkQueueFamilyCount] {
            GetVkQueueFamilyIndex(VK_QUEUE_GRAPHICS_BIT | VK_QUEUE_COMPUTE_BIT, 0),
            GetVkQueueFamilyIndex(VK_QUEUE_GRAPHICS_BIT | VK_QUEUE_COMPUTE_BIT, 0),
            GetVkQueueFamilyIndex(VK_QUEUE_GRAPHICS_BIT | VK_QUEUE_COMPUTE_BIT, 0),
        };

        var vkQueueIndices = stackalloc uint[VkQueueFamilyCount] {
            0,
            0,
            0,
        };

        _vkDevice = CreateVkDevice(vkQueueFamilyIndices, vkQueueIndices, ref _vkDeviceManualImports);

        _buffers = new ValueList<VulkanGraphicsBuffer>();
        _buffersMutex = new ValueMutex();

        _fences = new ValueList<VulkanGraphicsFence>();
        _fencesMutex = new ValueMutex();

        _pipelineSignatures = new ValueList<VulkanGraphicsPipelineSignature>();
        _pipelineSignaturesMutex = new ValueMutex();

        _renderPasses = new ValueList<VulkanGraphicsRenderPass>();
        _renderPassesMutex = new ValueMutex();

        _shaders = new ValueList<VulkanGraphicsShader>();
        _shadersMutex = new ValueMutex();

        _textures = new ValueList<VulkanGraphicsTexture>();
        _texturesMutex = new ValueMutex();

        DeviceInfo.ComputeQueue = new VulkanGraphicsComputeCommandQueue(this, vkQueueFamilyIndices[0], vkQueueIndices[0]);
        DeviceInfo.CopyQueue = new VulkanGraphicsCopyCommandQueue(this, vkQueueFamilyIndices[1], vkQueueIndices[1]);
        DeviceInfo.RenderQueue = new VulkanGraphicsRenderCommandQueue(this, vkQueueFamilyIndices[2], vkQueueIndices[2]);

        _memoryManagers = CreateMemoryManagers(createOptions.CreateMemoryAllocator);

        _memoryBudgetInfo = new MemoryBudgetInfo();
        UpdateMemoryBudgetInfo(ref _memoryBudgetInfo, totalOperationCount: 0);

        SetNameUnsafe(Name);

        VulkanGraphicsMemoryManager[] CreateMemoryManagers(GraphicsMemoryAllocatorCreateFunc memoryAllocatorCreateFunc)
        {
            var vkMemoryTypeCount = Adapter.VkPhysicalDeviceMemoryProperties.memoryTypeCount;
            var memoryManagers = new VulkanGraphicsMemoryManager[vkMemoryTypeCount];

            var createOptions = new VulkanGraphicsMemoryManagerCreateOptions {
                CreateMemoryAllocator = memoryAllocatorCreateFunc,
            };

            for (var index = 0u; index < memoryManagers.Length; index++)
            {
                createOptions.VkMemoryTypeIndex = index;
                memoryManagers[index] = new VulkanGraphicsMemoryManager(this, in createOptions);
            }

            return memoryManagers;
        }

        VkDevice CreateVkDevice(uint* vkQueueFamilyIndices, uint* vkQueueIndices, ref VkDeviceManualImports vkDeviceManualImports)
        {
            VkDevice vkDevice;

            var vkQueuePriorities = stackalloc float[VkQueueFamilyCount] {
                1.0f,
                1.0f,
                1.0f
            };

            var vkDeviceQueueCreateInfos = stackalloc VkDeviceQueueCreateInfo[VkQueueFamilyCount] {
                new VkDeviceQueueCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    queueFamilyIndex = vkQueueFamilyIndices[0],
                    queueCount = 1,
                    pQueuePriorities = vkQueuePriorities,
                },
                new VkDeviceQueueCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    queueFamilyIndex = vkQueueFamilyIndices[1],
                    queueCount = 1,
                    pQueuePriorities = vkQueuePriorities,
                },
                new VkDeviceQueueCreateInfo {
                    sType = VK_STRUCTURE_TYPE_DEVICE_QUEUE_CREATE_INFO,
                    pNext = null,
                    flags = 0,
                    queueFamilyIndex = vkQueueFamilyIndices[2],
                    queueCount = 1,
                    pQueuePriorities = vkQueuePriorities,
                },
            };
            var vkDeviceQueueCreateInfoCount = (uint)VkQueueFamilyCount;

            if (vkDeviceQueueCreateInfos[1].queueFamilyIndex == vkDeviceQueueCreateInfos[0].queueFamilyIndex)
            {
                // TODO: Ideally we'd correctly handle multiple queues and ownership transfer
                // but that's a lot of complexity for very little benefit this early on.
                vkQueueIndices[1] = 0;
                vkDeviceQueueCreateInfos[0].queueCount = 1;
                vkDeviceQueueCreateInfoCount--;
            }

            if (vkDeviceQueueCreateInfos[2].queueFamilyIndex == vkDeviceQueueCreateInfos[0].queueFamilyIndex)
            {
                // TODO: Ideally we'd correctly handle multiple queues and ownership transfer
                // but that's a lot of complexity for very little benefit this early on.
                vkQueueIndices[1] = 0;
                vkDeviceQueueCreateInfos[0].queueCount = 1;
                vkDeviceQueueCreateInfoCount--;
            }
            else if (vkDeviceQueueCreateInfos[2].queueFamilyIndex == vkDeviceQueueCreateInfos[1].queueFamilyIndex)
            {
                // TODO: Ideally we'd correctly handle multiple queues and ownership transfer
                // but that's a lot of complexity for very little benefit this early on.
                vkQueueIndices[1] = 0;
                vkDeviceQueueCreateInfos[0].queueCount = 1;
                vkDeviceQueueCreateInfoCount--;
            }

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
                queueCreateInfoCount = vkDeviceQueueCreateInfoCount,
                pQueueCreateInfos = vkDeviceQueueCreateInfos,
                enabledLayerCount = enabledVkLayersNamesCount,
                ppEnabledLayerNames = enabledVkLayerNames,
                enabledExtensionCount = EnabledExtensionNamesCount,
                ppEnabledExtensionNames = enabledVkExtensionNames,
                pEnabledFeatures = &vkPhysicalDeviceFeatures,
            };

            if (GraphicsService.EnableDebugMode)
            {
                // VK_LAYER_KHRONOS_validation
                ReadOnlySpan<sbyte> vkLayerKhronosValidation = new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4B, 0x48, 0x52, 0x4F, 0x4E, 0x4F, 0x53, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };
                enabledVkLayerNames[enabledVkLayersNamesCount - 1] = vkLayerKhronosValidation.GetPointer();
            }

            ThrowExternalExceptionIfNotSuccess(vkCreateDevice(adapter.VkPhysicalDevice, &vkDeviceCreateInfo, pAllocator: null, &vkDevice));

            // vkSetDebugUtilsObjectNameEXT
            ReadOnlySpan<sbyte> vkSetDebugUtilsObjectNameEXT = new sbyte[] { 0x76, 0x6B, 0x53, 0x65, 0x74, 0x44, 0x65, 0x62, 0x75, 0x67, 0x55, 0x74, 0x69, 0x6C, 0x73, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x4E, 0x61, 0x6D, 0x65, 0x45, 0x58, 0x54, 0x00 };
            vkDeviceManualImports.vkSetDebugUtilsObjectNameEXT = (delegate* unmanaged<VkDevice, VkDebugUtilsObjectNameInfoEXT*, VkResult>)vkGetDeviceProcAddr(vkDevice, vkSetDebugUtilsObjectNameEXT.GetPointer());

            return vkDevice;
        }

        uint GetVkQueueFamilyIndex(VkQueueFlags vkQueueFlags, VkQueueFlags vkDisallowedQueueFlags)
        {
            var vkQueueFamilyIndex = uint.MaxValue;
            var vkQueueFamilyProperties = Adapter.VkQueueFamilyProperties;

            for (nuint i = 0; i < vkQueueFamilyProperties.Length; i++)
            {
                var queueFlags = vkQueueFamilyProperties[i].queueFlags;

                if ((queueFlags & vkDisallowedQueueFlags) != 0)
                {
                    continue;
                }

                if ((queueFlags & vkQueueFlags) == vkQueueFlags)
                {
                    vkQueueFamilyIndex = (uint)i;
                    break;
                }
            }

            if (vkQueueFamilyIndex == uint.MaxValue)
            {
                ThrowForMissingFeature();
            }
            return vkQueueFamilyIndex;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsDevice" /> class.</summary>
    ~VulkanGraphicsDevice() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDevice.ComputeCommandQueue" />
    public new VulkanGraphicsComputeCommandQueue ComputeCommandQueue => base.ComputeCommandQueue.As<VulkanGraphicsComputeCommandQueue>();

    /// <inheritdoc cref="GraphicsDevice.CopyCommandQueue" />
    public new VulkanGraphicsCopyCommandQueue CopyCommandQueue => base.CopyCommandQueue.As<VulkanGraphicsCopyCommandQueue>();

    /// <inheritdoc cref="GraphicsDevice.RenderCommandQueue" />
    public new VulkanGraphicsRenderCommandQueue RenderCommandQueue => base.RenderCommandQueue.As<VulkanGraphicsRenderCommandQueue>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkDevice"/> for the device.</summary>
    public VkDevice VkDevice => _vkDevice;

    /// <summary>Gets the methods that must be manually imported for <see cref="VkDevice" />.</summary>
    public ref readonly VkDeviceManualImports VkDeviceManualImports => ref _vkDeviceManualImports;

    // VK_LAYER_KHRONOS_validation
    private static ReadOnlySpan<sbyte> VK_LAYER_KHRONOS_VALIDATION_NAME => new sbyte[] { 0x56, 0x4B, 0x5F, 0x4C, 0x41, 0x59, 0x45, 0x52, 0x5F, 0x4B, 0x48, 0x52, 0x4F, 0x4E, 0x4F, 0x53, 0x5F, 0x76, 0x61, 0x6C, 0x69, 0x64, 0x61, 0x74, 0x69, 0x6F, 0x6E, 0x00 };

    /// <inheritdoc />
    protected override VulkanGraphicsBuffer CreateBufferUnsafe(in GraphicsBufferCreateOptions createOptions)
    {
        return new VulkanGraphicsBuffer(this, in createOptions);
    }

    /// <inheritdoc />
    protected override VulkanGraphicsFence CreateFenceUnsafe(in GraphicsFenceCreateOptions createOptions)
    {
        return new VulkanGraphicsFence(this, in createOptions);
    }

    /// <inheritdoc />
    protected override VulkanGraphicsPipelineSignature CreatePipelineSignatureUnsafe(in GraphicsPipelineSignatureCreateOptions createOptions)
    {
        return new VulkanGraphicsPipelineSignature(this, in createOptions);
    }

    /// <inheritdoc />
    protected override VulkanGraphicsRenderPass CreateRenderPassUnsafe(in GraphicsRenderPassCreateOptions createOptions)
    {
        return new VulkanGraphicsRenderPass(this, in createOptions);
    }

    /// <inheritdoc />
    protected override VulkanGraphicsShader CreateShaderUnsafe(in GraphicsShaderCreateOptions createOptions)
    {
        return new VulkanGraphicsShader(this, in createOptions);
    }

    /// <inheritdoc />
    protected override VulkanGraphicsTexture CreateTextureUnsafe(in GraphicsTextureCreateOptions createOptions)
    {
        return new VulkanGraphicsTexture(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            for (var index = _buffers.Count - 1; index >= 0; index--)
            {
                var buffer = _buffers.GetReferenceUnsafe(index);
                buffer.Dispose();
            }
            _buffers.Clear();

            for (var index = _pipelineSignatures.Count - 1; index >= 0; index--)
            {
                var pipelineSignature = _pipelineSignatures.GetReferenceUnsafe(index);
                pipelineSignature.Dispose();
            }
            _pipelineSignatures.Clear();

            for (var index = _renderPasses.Count - 1; index >= 0; index--)
            {
                var renderPass = _renderPasses.GetReferenceUnsafe(index);
                renderPass.Dispose();
            }
            _renderPasses.Clear();

            for (var index = _shaders.Count - 1; index >= 0; index--)
            {
                var shader = _shaders.GetReferenceUnsafe(index);
                shader.Dispose();
            }
            _shaders.Clear();

            for (var index = _textures.Count - 1; index >= 0; index--)
            {
                var texture = _textures.GetReferenceUnsafe(index);
                texture.Dispose();
            }
            _textures.Clear();

            DeviceInfo.ComputeQueue.Dispose();
            DeviceInfo.CopyQueue.Dispose();
            DeviceInfo.RenderQueue.Dispose();

            foreach (var memoryManager in _memoryManagers)
            {
                memoryManager.Dispose();
            }

            for (var index = _fences.Count - 1; index >= 0; index--)
            {
                var fence = _fences.GetReferenceUnsafe(index);
                fence.Dispose();
            }
            _fences.Clear();
        }

        DisposeVkDevice(_vkDevice);
        _vkDevice = VkDevice.NULL;

        _ = Adapter.RemoveDevice(this);

        static void DisposeVkDevice(VkDevice vkDevice)
        {
            if (vkDevice != VkDevice.NULL)
            {
                vkDestroyDevice(vkDevice, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        SetVkObjectName(VK_OBJECT_TYPE_DEVICE, VkDevice, value);
    }

    internal void AddBuffer(VulkanGraphicsBuffer buffer)
    {
        _buffers.Add(buffer, _buffersMutex);
    }

    internal void AddFence(VulkanGraphicsFence fence)
    {
        _fences.Add(fence, _fencesMutex);
    }

    internal void AddPipelineSignature(VulkanGraphicsPipelineSignature pipelineSignature)
    {
        _pipelineSignatures.Add(pipelineSignature, _pipelineSignaturesMutex);
    }

    internal void AddRenderPass(VulkanGraphicsRenderPass renderPass)
    {
        _renderPasses.Add(renderPass, _renderPassesMutex);
    }

    internal void AddShader(VulkanGraphicsShader shader)
    {
        _shaders.Add(shader, _shadersMutex);
    }

    internal void AddTexture(VulkanGraphicsTexture texture)
    {
        _textures.Add(texture, _texturesMutex);
    }

    internal GraphicsMemoryBudget GetMemoryBudget(VulkanGraphicsMemoryManager memoryManager)
    {
        var totalOperationCount = GetTotalOperationCount(memoryManager.VkMemoryTypeIndex);

        ref var memoryBudgetInfo = ref _memoryBudgetInfo;

        if ((totalOperationCount - memoryBudgetInfo.TotalOperationCountAtLastUpdate) >= 30)
        {
            UpdateMemoryBudgetInfo(ref memoryBudgetInfo, totalOperationCount);
        }

        using var readerLock = new DisposableReaderLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        return GetMemoryBudgetNoLock(memoryManager, in memoryBudgetInfo);
    }

    internal VulkanGraphicsMemoryManager GetMemoryManager(GraphicsCpuAccess cpuAccess, uint vkMemoryTypeBits)
    {
        var memoryManagerIndex = GetMemoryManagerIndex(cpuAccess, vkMemoryTypeBits);
        return _memoryManagers[memoryManagerIndex];
    }

    internal bool RemoveBuffer(VulkanGraphicsBuffer buffer)
    {
        return IsDisposed || _buffers.Remove(buffer, _buffersMutex);
    }

    internal bool RemoveFence(VulkanGraphicsFence fence)
    {
        return IsDisposed || _fences.Remove(fence, _fencesMutex);
    }

    internal bool RemovePipelineSignature(VulkanGraphicsPipelineSignature pipelineSignature)
    {
        return IsDisposed || _pipelineSignatures.Remove(pipelineSignature, _pipelineSignaturesMutex);
    }

    internal bool RemoveRenderPass(VulkanGraphicsRenderPass renderPass)
    {
        return IsDisposed || _renderPasses.Remove(renderPass, _renderPassesMutex);
    }

    internal bool RemoveShader(VulkanGraphicsShader shader)
    {
        return IsDisposed || _shaders.Remove(shader, _shadersMutex);
    }

    internal bool RemoveTexture(VulkanGraphicsTexture texture)
    {
        return IsDisposed || _textures.Remove(texture, _texturesMutex);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetVkObjectName(VkObjectType vkObjectType, ulong vkObjectHandle, string name, [CallerArgumentExpression("vkObjectHandle")] string component = "")
    {
        ref readonly var vkDeviceManualImports = ref VkDeviceManualImports;

        if (GraphicsService.EnableDebugMode && (vkDeviceManualImports.vkSetDebugUtilsObjectNameEXT is not null) && (vkObjectHandle != 0))
        {
            var componentName = $"{name}: {component}";

            fixed (sbyte* pName = componentName.GetUtf8Span())
            {
                var vkDebugUtilsObjectNameInfo = new VkDebugUtilsObjectNameInfoEXT {
                    sType = VK_STRUCTURE_TYPE_DEBUG_UTILS_OBJECT_NAME_INFO_EXT,
                    objectType = vkObjectType,
                    objectHandle = vkObjectHandle,
                    pObjectName = pName
                };
                _ = vkDeviceManualImports.vkSetDebugUtilsObjectNameEXT(VkDevice, &vkDebugUtilsObjectNameInfo);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void SetVkObjectName(VkObjectType vkObjectType, void* vkObjectHandle, string name, [CallerArgumentExpression("vkObjectHandle")] string component = "")
    {
        SetVkObjectName(vkObjectType, (ulong)vkObjectHandle, name, component);
    }

    private GraphicsMemoryBudget GetMemoryBudgetNoLock(VulkanGraphicsMemoryManager memoryManager, in MemoryBudgetInfo memoryBudgetInfo)
    {
        var estimatedMemoryBudget = 0UL;
        var estimatedMemoryUsage = 0UL;
        var totalFreeMemoryRegionSize = 0UL;
        var totalSize = 0UL;
        var totalSizeAtLastUpdate = 0UL;

        var vkMemoryTypeIndex = memoryManager.VkMemoryTypeIndex;

        estimatedMemoryBudget = memoryBudgetInfo.VkPhysicalDeviceMemoryBudgetProperties.heapBudget[vkMemoryTypeIndex];
        estimatedMemoryUsage = memoryBudgetInfo.VkPhysicalDeviceMemoryBudgetProperties.heapUsage[vkMemoryTypeIndex];

        totalFreeMemoryRegionSize = GetTotalFreeMemoryRegionByteLength(vkMemoryTypeIndex);
        totalSize = GetTotalByteLength(vkMemoryTypeIndex);
        totalSizeAtLastUpdate = memoryBudgetInfo.GetTotalByteLengthAtLastUpdate(vkMemoryTypeIndex);

        if ((estimatedMemoryUsage + totalSize) > totalSizeAtLastUpdate)
        {
            estimatedMemoryUsage = estimatedMemoryUsage + totalSize - totalSizeAtLastUpdate;
        }
        else
        {
            estimatedMemoryUsage = 0;
        }

        return new GraphicsMemoryBudget {
            EstimatedMemoryByteBudget = estimatedMemoryBudget,
            EstimatedMemoryByteUsage = estimatedMemoryUsage,
            TotalFreeMemoryRegionByteLength = totalFreeMemoryRegionSize,
            TotalByteLength = totalSize,
        };
    }

    private int GetMemoryManagerIndex(GraphicsCpuAccess cpuAccess, uint vkMemoryTypeBits)
    {
        var isVkPhysicalDeviceTypeIntegratedGpu = Adapter.VkPhysicalDeviceProperties.deviceType == VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU;
        var canBeMultiInstance = false;

        VkMemoryPropertyFlags vkRequiredMemoryPropertyFlags = 0;
        VkMemoryPropertyFlags vkPreferredMemoryPropertyFlags = 0;
        VkMemoryPropertyFlags vkUnpreferredMemoryPropertyFlags = 0;

        switch (cpuAccess)
        {
            case GraphicsCpuAccess.None:
            {
                vkPreferredMemoryPropertyFlags |= isVkPhysicalDeviceTypeIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                canBeMultiInstance = true;
                break;
            }

            case GraphicsCpuAccess.Read:
            {
                vkRequiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                vkPreferredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_CACHED_BIT;
                break;
            }

            case GraphicsCpuAccess.Write:
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

    private ulong GetTotalFreeMemoryRegionByteLength(uint vkMemoryTypeIndex)
    {
        Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
        return _memoryManagers[vkMemoryTypeIndex].TotalFreeMemoryRegionByteLength;
    }

    private ulong GetTotalOperationCount(uint vkMemoryTypeIndex)
    {
        Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
        return _memoryManagers[vkMemoryTypeIndex].OperationCount;
    }

    private ulong GetTotalByteLength(uint vkMemoryTypeIndex)
    {
        Assert(AssertionsEnabled && (vkMemoryTypeIndex < MaxMemoryManagerTypes));
        return _memoryManagers[vkMemoryTypeIndex].ByteLength;
    }

    private void UpdateMemoryBudgetInfo(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        using var writerLock = new DisposableWriterLock(memoryBudgetInfo.RWLock, isExternallySynchronized: false);
        UpdateMemoryBudgetInfoNoLock(ref memoryBudgetInfo, totalOperationCount);
    }

    private void UpdateMemoryBudgetInfoNoLock(ref MemoryBudgetInfo memoryBudgetInfo, ulong totalOperationCount)
    {
        var vkPhysicalDeviceMemoryBudgetProperties = new VkPhysicalDeviceMemoryBudgetPropertiesEXT {
            sType = VK_STRUCTURE_TYPE_PHYSICAL_DEVICE_MEMORY_BUDGET_PROPERTIES_EXT,
            pNext = null,
        };

        if (Adapter.TryGetVkPhysicalDeviceMemoryBudgetProperties(&vkPhysicalDeviceMemoryBudgetProperties))
        {
            memoryBudgetInfo.VkPhysicalDeviceMemoryBudgetProperties = vkPhysicalDeviceMemoryBudgetProperties;
        }

        memoryBudgetInfo.TotalOperationCountAtLastUpdate = totalOperationCount;

        for (var memoryManagerIndex = 0u; memoryManagerIndex < _memoryManagers.Length; memoryManagerIndex++)
        {
            memoryBudgetInfo.SetTotalFreeMemoryRegionByteLengthAtLastUpdate(memoryManagerIndex, GetTotalFreeMemoryRegionByteLength(memoryManagerIndex));
            memoryBudgetInfo.SetTotalByteLengthAtLastUpdate(memoryManagerIndex, GetTotalByteLength(memoryManagerIndex));
        }
    }
}
