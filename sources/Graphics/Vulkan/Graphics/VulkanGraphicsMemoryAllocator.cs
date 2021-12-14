// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections.Generic;
using System.Numerics;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkImageType;
using static TerraFX.Interop.Vulkan.VkMemoryHeapFlags;
using static TerraFX.Interop.Vulkan.VkMemoryPropertyFlags;
using static TerraFX.Interop.Vulkan.VkPhysicalDeviceType;
using static TerraFX.Interop.Vulkan.VkSampleCountFlags;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsMemoryAllocator : GraphicsMemoryAllocator
{
    private readonly VulkanGraphicsMemoryHeapCollection[] _heapCollections;
    private VolatileState _state;

    internal VulkanGraphicsMemoryAllocator(VulkanGraphicsDevice device, in GraphicsMemoryAllocatorSettings settings)
        : base(device, in settings)
    {
        var memoryTypeCount = device.Adapter.VkPhysicalDeviceMemoryProperties.memoryTypeCount;
        _heapCollections = new VulkanGraphicsMemoryHeapCollection[memoryTypeCount];

        for (uint memoryTypeIndex = 0; memoryTypeIndex < memoryTypeCount; memoryTypeIndex++)
        {
            _heapCollections[memoryTypeIndex] = new VulkanGraphicsMemoryHeapCollection(device, this, memoryTypeIndex);
        }

        // TODO: UpdateBudget
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsMemoryAllocator" /> class.</summary>
    ~VulkanGraphicsMemoryAllocator() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc />
    public override int Count => _heapCollections.Length;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    public override VulkanGraphicsBuffer CreateBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, GraphicsMemoryHeapRegionAllocationFlags allocationFlags = GraphicsMemoryHeapRegionAllocationFlags.None)
    {
        var device = Device;
        var vkDevice = device.VkDevice;

        var vkBufferCreateInfo = new VkBufferCreateInfo {
            sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
            size = size,
            usage = GetVkBufferUsageKind(kind, cpuAccess)
        };

        VkBuffer vkBuffer;
        ThrowExternalExceptionIfNotSuccess(vkCreateBuffer(vkDevice, &vkBufferCreateInfo, pAllocator: null, &vkBuffer));

        VkMemoryRequirements vkMemoryRequirements;
        vkGetBufferMemoryRequirements(vkDevice, vkBuffer, &vkMemoryRequirements);

        var heapCollectionIndex = GetHeapCollectionIndex(cpuAccess, vkMemoryRequirements.memoryTypeBits);
        ref readonly var heapCollection = ref _heapCollections[heapCollectionIndex];

        var heapRegion = heapCollection.Allocate(vkMemoryRequirements.size, vkMemoryRequirements.alignment, allocationFlags);
        return new VulkanGraphicsBuffer(device, kind, in heapRegion, cpuAccess, vkBuffer);
    }

    /// <inheritdoc />
    public override VulkanGraphicsTexture CreateTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, GraphicsMemoryHeapRegionAllocationFlags allocationFlags = GraphicsMemoryHeapRegionAllocationFlags.None, GraphicsFormat format = GraphicsFormat.Unknown)
    {
        if (format == GraphicsFormat.Unknown)
        {
            format = GraphicsFormat.R8G8B8A8_UNORM;
        }

        var device = Device;
        var vkDevice = device.VkDevice;

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
            usage = GetVulkanImageUsageKind(kind, cpuAccess),
        };

        VkImage vkImage;
        ThrowExternalExceptionIfNotSuccess(vkCreateImage(vkDevice, &vkImageCreateInfo, pAllocator: null, &vkImage));

        VkMemoryRequirements vkMemoryRequirements;
        vkGetImageMemoryRequirements(vkDevice, vkImage, &vkMemoryRequirements);

        var heapCollectionIndex = GetHeapCollectionIndex(cpuAccess, vkMemoryRequirements.memoryTypeBits);
        ref readonly var heapCollection = ref _heapCollections[heapCollectionIndex];

        var heapRegion = heapCollection.Allocate(vkMemoryRequirements.size, vkMemoryRequirements.alignment, allocationFlags);
        return new VulkanGraphicsTexture(device, kind, in heapRegion, cpuAccess, width, height, depth, vkImage);
    }

    /// <inheritdoc />
    public override void GetBudget(GraphicsMemoryHeapCollection heapCollection, out GraphicsMemoryBudget budget) => GetBudget((VulkanGraphicsMemoryHeapCollection)heapCollection, out budget);

    /// <inheritdoc cref="GetBudget(GraphicsMemoryHeapCollection, out GraphicsMemoryBudget)" />
    public void GetBudget(VulkanGraphicsMemoryHeapCollection heapCollection, out GraphicsMemoryBudget budget) => budget = new GraphicsMemoryBudget {
        EstimatedBudget = ulong.MaxValue,
        EstimatedUsage = 0,
        TotalAllocatedRegionSize = 0,
        TotalHeapSize = 0,
    };

    /// <inheritdoc />
    public override IEnumerator<VulkanGraphicsMemoryHeapCollection> GetEnumerator() => throw new NotImplementedException();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                foreach (var heapCollection in _heapCollections)
                {
                    heapCollection?.Dispose();
                }
            }
        }

        _state.EndDispose();
    }

    private int GetHeapCollectionIndex(GraphicsResourceCpuAccess cpuAccess, uint memoryTypeBits)
    {
        var isVkPhysicalDeviceTypeIntegratedGpu = Device.Adapter.VkPhysicalDeviceProperties.deviceType == VK_PHYSICAL_DEVICE_TYPE_INTEGRATED_GPU;
        var canBeMultiInstance = false;

        VkMemoryPropertyFlags vkRequiredMemoryPropertyFlags = 0;
        VkMemoryPropertyFlags vkPreferredMemoryPropertyFlags = 0;
        VkMemoryPropertyFlags vkUnpreferredMemoryPropertyFlags = 0;

        switch (cpuAccess)
        {
            case GraphicsResourceCpuAccess.GpuOnly:
            {
                vkPreferredMemoryPropertyFlags |= isVkPhysicalDeviceTypeIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                canBeMultiInstance = true;
                break;
            }

            case GraphicsResourceCpuAccess.GpuToCpu:
            {
                vkRequiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                vkPreferredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_CACHED_BIT;
                break;
            }

            case GraphicsResourceCpuAccess.CpuToGpu:
            {
                vkRequiredMemoryPropertyFlags |= VK_MEMORY_PROPERTY_HOST_VISIBLE_BIT;
                vkPreferredMemoryPropertyFlags |= isVkPhysicalDeviceTypeIntegratedGpu ? default : VK_MEMORY_PROPERTY_DEVICE_LOCAL_BIT;
                break;
            }
        }

        var index = -1;
        var lowestCost = int.MaxValue;

        ref readonly var vkPhysicalDeviceMemoryProperties = ref Device.Adapter.VkPhysicalDeviceMemoryProperties;

        for (var i = 0; i < _heapCollections.Length; i++)
        {
            if ((memoryTypeBits & (1 << i)) == 0)
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
