// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Runtime.InteropServices;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsBuffer : GraphicsBuffer
{
    private readonly VkBuffer _vkBuffer;
    private VolatileState _state;

    internal VulkanGraphicsBuffer(VulkanGraphicsDevice device, GraphicsBufferKind kind, in GraphicsMemoryHeapRegion heapRegion, GraphicsResourceCpuAccess cpuAccess, VkBuffer vkBuffer)
        : base(device, kind, in heapRegion, cpuAccess)
    {
        _vkBuffer = vkBuffer;
        ThrowExternalExceptionIfNotSuccess(vkBindBufferMemory(device.VkDevice, vkBuffer, Heap.VkDeviceMemory, heapRegion.Offset));

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBuffer" /> class.</summary>
    ~VulkanGraphicsBuffer() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsResource.Allocator" />
    public new VulkanGraphicsMemoryAllocator Allocator => base.Allocator.As<VulkanGraphicsMemoryAllocator>();

    /// <inheritdoc />
    public new VulkanGraphicsMemoryHeap Heap => base.Heap.As<VulkanGraphicsMemoryHeap>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkBuffer" /> for the buffer.</summary>
    public VkBuffer VkBuffer
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkBuffer;
        }
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>()
    {
        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, Heap.VkDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination));
        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
    {
        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, Heap.VkDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination));
        return (T*)(pDestination + rangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>()
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = Heap.VkDeviceMemory;

        void* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vkDevice, vkDeviceMemory, Offset, Size, flags: 0, &pDestination));

        var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = Offset;
        var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = vkDeviceMemory,
            offset = offset,
            size = size,
        };
        ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));

        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = Heap.VkDeviceMemory;

        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vkDevice, vkDeviceMemory, Offset, Size, flags: 0, (void**)&pDestination));

        if (readRangeLength != 0)
        {
            var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = Offset + readRangeOffset;
            var size = (readRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

            var vkMappedMemoryRange = new VkMappedMemoryRange {
                sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                memory = vkDeviceMemory,
                offset = offset,
                size = size,
            };
            ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));
        }
        return (T*)(pDestination + readRangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void Unmap()
    {
        vkUnmapMemory(Device.VkDevice, Heap.VkDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite()
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = Heap.VkDeviceMemory;

        var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = Offset;
        var size = (Size + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = vkDeviceMemory,
            offset = offset,
            size = size,
        };
        ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));

        vkUnmapMemory(vkDevice, vkDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength)
    {
        var device = Allocator.Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = Heap.VkDeviceMemory;

        if (writtenRangeLength != 0)
        {
            var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = Offset + writtenRangeOffset;
            var size = (writtenRangeLength + nonCoherentAtomSize - 1) & ~(nonCoherentAtomSize - 1);

            var vkMappedMemoryRange = new VkMappedMemoryRange {
                sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
                memory = vkDeviceMemory,
                offset = offset,
                size = size,
            };
            ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(vkDevice, 1, &vkMappedMemoryRange));
        }
        vkUnmapMemory(vkDevice, vkDeviceMemory);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            DisposeVulkanBuffer(Device.VkDevice, _vkBuffer);
            HeapRegion.Heap.Free(in HeapRegion);
        }

        _state.EndDispose();

        static void DisposeVulkanBuffer(VkDevice vkDevice, VkBuffer vkBuffer)
        {
            if (vkBuffer != VkBuffer.NULL)
            {
                vkDestroyBuffer(vkDevice, vkBuffer, pAllocator: null);
            }
        }
    }
}
