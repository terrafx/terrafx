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
    private readonly VulkanGraphicsMemoryHeap _memoryHeap;
    private readonly VkBuffer _vkBuffer;

    private VolatileState _state;

    internal VulkanGraphicsBuffer(VulkanGraphicsDevice device, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment, in GraphicsMemoryRegion memoryRegion, GraphicsBufferKind kind, VkBuffer vkBuffer)
        : base(device, cpuAccess, size, alignment, in memoryRegion, kind)
    {
        var memoryHeap = memoryRegion.Allocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();
        _memoryHeap = memoryHeap;

        ThrowExternalExceptionIfNotSuccess(vkBindBufferMemory(device.VkDevice, vkBuffer, memoryHeap.VkDeviceMemory, memoryRegion.Offset));
        _vkBuffer = vkBuffer;

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBuffer" /> class.</summary>
    ~VulkanGraphicsBuffer() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public VulkanGraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

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
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, MemoryHeap.VkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, (void**)&pDestination));
        return (T*)pDestination;
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
    {
        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, MemoryHeap.VkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, (void**)&pDestination));
        return (T*)(pDestination + rangeOffset);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkMapMemory(VkDevice, VkDeviceMemory, ulong, ulong, uint, void**)" /> failed.</exception>
    public override T* MapForRead<T>()
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        void* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vkDevice, vkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, &pDestination));

        var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = MemoryRegion.Offset;
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
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        byte* pDestination;
        ThrowExternalExceptionIfNotSuccess(vkMapMemory(vkDevice, vkDeviceMemory, MemoryRegion.Offset, Size, flags: 0, (void**)&pDestination));

        if (readRangeLength != 0)
        {
            var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = MemoryRegion.Offset + readRangeOffset;
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
        vkUnmapMemory(Device.VkDevice, MemoryHeap.VkDeviceMemory);
    }

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="vkFlushMappedMemoryRanges(VkDevice, uint, VkMappedMemoryRange*)" /> failed.</exception>
    public override void UnmapAndWrite()
    {
        var device = Device;

        var vkDevice = device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        var nonCoherentAtomSize = device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

        var offset = MemoryRegion.Offset;
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
        var vkDevice = Device.VkDevice;
        var vkDeviceMemory = MemoryHeap.VkDeviceMemory;

        if (writtenRangeLength != 0)
        {
            var nonCoherentAtomSize = Device.Adapter.VkPhysicalDeviceProperties.limits.nonCoherentAtomSize;

            var offset = MemoryRegion.Offset + writtenRangeOffset;
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
            MemoryRegion.Dispose();
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
