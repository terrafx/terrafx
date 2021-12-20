// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsBuffer : GraphicsBuffer
{
    private readonly ValueList<VulkanGraphicsBufferView> _bufferViews;
    private readonly ValueMutex _bufferViewsMutex;
    private readonly ValueMutex _mapMutex;
    private readonly GraphicsMemoryAllocator _memoryAllocator;
    private readonly VulkanGraphicsMemoryHeap _memoryHeap;
    private readonly VkBuffer _vkBuffer;
    private readonly nuint _vkMinTexelBufferOffsetAlignment;
    private readonly nuint _vkMinUniformBufferOffsetAlignment;
    private readonly nuint _vkNonCoherentAtomSize;

    private volatile void* _mappedAddress;
    private volatile uint _mappedCount;

    internal VulkanGraphicsBuffer(VulkanGraphicsDevice device, in CreateInfo createInfo)
        : base(device, in createInfo.MemoryRegion, createInfo.CpuAccess, createInfo.Kind)
    {
        _bufferViewsMutex = new ValueMutex();
        _mapMutex = new ValueMutex();
        _memoryAllocator = createInfo.CreateMemoryAllocator.Invoke(this, default, createInfo.MemoryRegion.Size, false);
        _memoryHeap = createInfo.MemoryRegion.Allocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();
        _vkBuffer = createInfo.VkBuffer;

        ref readonly var vkPhysicalDeviceLimits = ref Adapter.VkPhysicalDeviceProperties.limits;

        _vkMinTexelBufferOffsetAlignment = checked((nuint)vkPhysicalDeviceLimits.minTexelBufferOffsetAlignment);
        _vkMinUniformBufferOffsetAlignment = checked((nuint)vkPhysicalDeviceLimits.minUniformBufferOffsetAlignment);
        _vkNonCoherentAtomSize = checked((nuint)vkPhysicalDeviceLimits.nonCoherentAtomSize);

        ThrowExternalExceptionIfNotSuccess(vkBindBufferMemory(device.VkDevice, createInfo.VkBuffer, _memoryHeap.VkDeviceMemory, createInfo.MemoryRegion.Offset));
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBuffer" /> class.</summary>
    ~VulkanGraphicsBuffer() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc />
    public override int Count => _bufferViews.Count;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc />
    public override bool IsMapped => MemoryHeap.IsMapped;

    /// <inheritdoc />
    public override unsafe void* MappedAddress => MemoryHeap.MappedAddress;

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public VulkanGraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkBuffer" /> for the buffer.</summary>
    public VkBuffer VkBuffer
    {
        get
        {
            AssertNotDisposed();
            return _vkBuffer;
        }
    }

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.minTexelBufferOffsetAlignment" /> for <see cref="Adapter" />.</summary>
    public nuint VkMinTexelBufferOffsetAlignment => _vkMinTexelBufferOffsetAlignment;

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.minUniformBufferOffsetAlignment" /> for <see cref="Adapter" />.</summary>
    public nuint VkMinUniformBufferOffsetAlignment => _vkMinUniformBufferOffsetAlignment;

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.nonCoherentAtomSize" /> for <see cref="Adapter" />.</summary>
    public nuint VkNonCoherentAtomSize => _vkNonCoherentAtomSize;

    /// <inheritdoc />
    public override void DisposeAllViews()
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        DisposeAllViewsInternal();
    }

    /// <inheritdoc />
    public override IEnumerator<VulkanGraphicsBufferView> GetEnumerator() => _bufferViews.GetEnumerator();

    /// <inheritdoc />
    public override bool TryCreateView(uint count, uint stride, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        ThrowIfDisposed();

        nuint size = stride;
        size *= count;
        nuint alignment = 0;

        if (Kind == GraphicsBufferKind.Index)
        {
            if (stride is not 2 and not 4)
            {
                ThrowForInvalidKind(Kind);
            }
            alignment = stride;
        }
        else if (Kind == GraphicsBufferKind.Constant)
        {
            alignment = VkMinUniformBufferOffsetAlignment;
        }
        else if (Kind == GraphicsBufferKind.Default)
        {
            alignment = VkMinTexelBufferOffsetAlignment;
        }

        var succeeded = _memoryAllocator.TryAllocate(size, alignment, out var memoryRegion);
        bufferView = succeeded ? new VulkanGraphicsBufferView(this, in memoryRegion, stride) : null;

        return succeeded;
    }

    /// <inheritdoc />
    public override void Unmap()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapInternal();
    }

    /// <inheritdoc />
    public override void UnmapAndWrite()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapAndWriteInternal();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _bufferViewsMutex.Dispose();
        _mapMutex.Dispose();

        DisposeAllViewsInternal();

        if (isDisposing)
        {
            _memoryAllocator.Clear();
        }

        DisposeVulkanBuffer(Device.VkDevice, _vkBuffer);
        MemoryRegion.Dispose();

        static void DisposeVulkanBuffer(VkDevice vkDevice, VkBuffer vkBuffer)
        {
            if (vkBuffer != VkBuffer.NULL)
            {
                vkDestroyBuffer(vkDevice, vkBuffer, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override byte* Map()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapInternal();
    }

    /// <inheritdoc />
    protected override byte* MapForRead()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapForReadInternal();
    }

    /// <inheritdoc />
    protected override byte* MapForRead(nuint offset, nuint size)
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapForReadInternal(offset, size);
    }

    /// <inheritdoc />
    protected override void UnmapAndWrite(nuint offset, nuint size)
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapAndWriteInternal(offset, size);
    }

    internal void AddView(VulkanGraphicsBufferView bufferView)
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        _bufferViews.Add(bufferView);
    }

    internal bool RemoveView(VulkanGraphicsBufferView bufferView)
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        return _bufferViews.Remove(bufferView);
    }

    private void DisposeAllViewsInternal()
    {
        // This method should only be called under a mutex

        foreach (var bufferView in _bufferViews)
        {
            bufferView.Dispose();
        }

        _bufferViews.Clear();
    }

    private byte* MapInternal()
    {
        // This method should only be called under a mutex
        byte* mappedAddress;

        if (Interlocked.Increment(ref _mappedCount) == 1)
        {
            mappedAddress = MemoryHeap.Map() + MemoryRegion.Offset;
            _mappedAddress = mappedAddress;
        }
        else
        {
            mappedAddress = (byte*)_mappedAddress;
        }

        return mappedAddress;
    }

    private byte* MapForReadInternal()
    {
        // This method should only be called under a mutex
        return MapForReadInternal(0, Size);
    }

    private byte* MapForReadInternal(nuint offset, nuint size)
    {
        // This method should only be called under a mutex

        var mappedAddress = MapInternal();
        var vkNonCoherentAtomSize = VkNonCoherentAtomSize;

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = MemoryHeap.VkDeviceMemory,
            offset = MemoryRegion.Offset + offset,
            size = (size + vkNonCoherentAtomSize - 1) & ~(vkNonCoherentAtomSize - 1),
        };
        ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(Device.VkDevice, 1, &vkMappedMemoryRange));

        return mappedAddress;
    }

    private void UnmapInternal()
    {
        // This method should only be called under a mutex

        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        else if (mappedCount == 0)
        {
            MemoryHeap.Unmap();
        }
    }

    private void UnmapAndWriteInternal()
    {
        // This method should only be called under a mutex
        UnmapAndWriteInternal(0, Size);
    }

    private void UnmapAndWriteInternal(nuint offset, nuint size)
    {
        // This method should only be called under a mutex

        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        var vkNonCoherentAtomSize = VkNonCoherentAtomSize;

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            memory = MemoryHeap.VkDeviceMemory,
            offset = MemoryRegion.Offset + offset,
            size = (size + vkNonCoherentAtomSize - 1) & ~(vkNonCoherentAtomSize - 1),
        };
        ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(Device.VkDevice, 1, &vkMappedMemoryRange));

        if (mappedCount == 0)
        {
            MemoryHeap.Unmap();
        }
    }
}
