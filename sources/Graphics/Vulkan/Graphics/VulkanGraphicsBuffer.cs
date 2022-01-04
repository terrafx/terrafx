// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using TerraFX.Advanced;
using TerraFX.Collections;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkBufferUsageFlags;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkSharingMode;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Utilities.CollectionsUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe partial class VulkanGraphicsBuffer : GraphicsBuffer
{
    private VkBuffer _vkBuffer;

    private readonly nuint _vkMinTexelBufferOffsetAlignment;
    private readonly nuint _vkMinUniformBufferOffsetAlignment;
    private readonly nuint _vkNonCoherentAtomSize;

    private GraphicsMemoryAllocator _memoryAllocator;
    private VulkanGraphicsMemoryHeap _memoryHeap;

    private ValueList<VulkanGraphicsBufferView> _bufferViews;
    private readonly ValueMutex _bufferViewsMutex;

    private volatile uint _mappedCount;
    private readonly ValueMutex _mappedMutex;

    internal VulkanGraphicsBuffer(VulkanGraphicsDevice device, in GraphicsBufferCreateOptions createOptions) : base(device)
    {
        device.AddBuffer(this);

        BufferInfo.Kind = createOptions.Kind;

        var vkBuffer = CreateVkBuffer(in createOptions);
        _vkBuffer = vkBuffer;

        ref readonly var vkPhysicalDeviceLimits = ref Adapter.VkPhysicalDeviceProperties.limits;

        _vkMinTexelBufferOffsetAlignment = checked((nuint)vkPhysicalDeviceLimits.minTexelBufferOffsetAlignment);
        _vkMinUniformBufferOffsetAlignment = checked((nuint)vkPhysicalDeviceLimits.minUniformBufferOffsetAlignment);
        _vkNonCoherentAtomSize = checked((nuint)vkPhysicalDeviceLimits.nonCoherentAtomSize);

        var memoryAllocatorCreateOptions = new GraphicsMemoryAllocatorCreateOptions {
            ByteLength = MemoryRegion.ByteLength,
            IsDedicated = false,
            OnFree = default,
        };

        if (createOptions.CreateMemorySuballocator.IsNotNull)
        {
            _memoryAllocator = createOptions.CreateMemorySuballocator.Invoke(this, in memoryAllocatorCreateOptions);
        }
        else
        {
            _memoryAllocator = GraphicsMemoryAllocator.CreateDefault(this, in memoryAllocatorCreateOptions);
        }

        _memoryHeap = MemoryRegion.MemoryAllocator.DeviceObject.As<VulkanGraphicsMemoryHeap>();

        _bufferViews = new ValueList<VulkanGraphicsBufferView>();
        _bufferViewsMutex = new ValueMutex();

        _mappedCount = 0;
        _mappedMutex = new ValueMutex();

        SetNameUnsafe(Name);

        VkBuffer CreateVkBuffer(in GraphicsBufferCreateOptions createOptions)
        {
            VkBuffer vkBuffer;

            var vkDevice = Device.VkDevice;
            var cpuAccess = createOptions.CpuAccess;

            var vkBufferCreateInfo = new VkBufferCreateInfo {
                sType = VK_STRUCTURE_TYPE_BUFFER_CREATE_INFO,
                pNext = null,
                flags = 0,
                size = createOptions.ByteLength,
                usage = GetVkBufferUsageKind(cpuAccess, createOptions.Kind),
                sharingMode = VK_SHARING_MODE_EXCLUSIVE,
                queueFamilyIndexCount = 0,
                pQueueFamilyIndices = null,
            };
            ThrowExternalExceptionIfNotSuccess(vkCreateBuffer(vkDevice, &vkBufferCreateInfo, pAllocator: null, &vkBuffer));

            VkMemoryRequirements vkMemoryRequirements;
            vkGetBufferMemoryRequirements(vkDevice, vkBuffer, &vkMemoryRequirements);

            ResourceInfo.CpuAccess = cpuAccess;
            ResourceInfo.MappedAddress = null;

            var memoryManager = device.GetMemoryManager(cpuAccess, vkMemoryRequirements.memoryTypeBits);

            var memoryAllocationOptions = new GraphicsMemoryAllocationOptions {
                ByteLength = (nuint)vkMemoryRequirements.size,
                ByteAlignment = (nuint)vkMemoryRequirements.alignment,
                AllocationFlags = createOptions.AllocationFlags,
            };
            var memoryRegion = memoryManager.Allocate(in memoryAllocationOptions);
            ResourceInfo.MemoryRegion = memoryRegion;

            var vkDeviceMemory = ResourceInfo.MemoryRegion.MemoryAllocator.DeviceObject.As<VulkanGraphicsMemoryHeap>().VkDeviceMemory;
            ThrowExternalExceptionIfNotSuccess(vkBindBufferMemory(
                vkDevice,
                vkBuffer,
                vkDeviceMemory,
                memoryRegion.ByteOffset
            ));

            return vkBuffer;
        }

        static VkBufferUsageFlags GetVkBufferUsageKind(GraphicsCpuAccess cpuAccess, GraphicsBufferKind bufferKind)
        {
            return cpuAccess switch {
                GraphicsCpuAccess.Read => VK_BUFFER_USAGE_TRANSFER_DST_BIT,
                GraphicsCpuAccess.Write => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT | VK_BUFFER_USAGE_INDEX_BUFFER_BIT | VK_BUFFER_USAGE_INDIRECT_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                _ => bufferKind switch {
                    GraphicsBufferKind.Vertex => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    GraphicsBufferKind.Index => VK_BUFFER_USAGE_INDEX_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    GraphicsBufferKind.Constant => VK_BUFFER_USAGE_VERTEX_BUFFER_BIT | VK_BUFFER_USAGE_UNIFORM_BUFFER_BIT | VK_BUFFER_USAGE_TRANSFER_DST_BIT | VK_BUFFER_USAGE_TRANSFER_SRC_BIT,
                    _ => default,
                }
            };
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsBuffer" /> class.</summary>
    ~VulkanGraphicsBuffer() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <summary>Gets the memory heap in which the buffer exists.</summary>
    public VulkanGraphicsMemoryHeap MemoryHeap => _memoryHeap;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the underlying <see cref="Interop.Vulkan.VkBuffer" /> for the buffer.</summary>
    public VkBuffer VkBuffer => _vkBuffer;

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.minTexelBufferOffsetAlignment" /> for <see cref="Adapter" />.</summary>
    public nuint VkMinTexelBufferOffsetAlignment => _vkMinTexelBufferOffsetAlignment;

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.minUniformBufferOffsetAlignment" /> for <see cref="Adapter" />.</summary>
    public nuint VkMinUniformBufferOffsetAlignment => _vkMinUniformBufferOffsetAlignment;

    /// <summary>Gets the <see cref="VkPhysicalDeviceLimits.nonCoherentAtomSize" /> for <see cref="Adapter" />.</summary>
    public nuint VkNonCoherentAtomSize => _vkNonCoherentAtomSize;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            DisposeAllViewsUnsafe();

            _memoryAllocator.Clear();

            _memoryAllocator = null!;
            _memoryHeap = null!;
        }
        _bufferViewsMutex.Dispose();

        DisposeVkBuffer(Device.VkDevice, _vkBuffer);
        _vkBuffer = VkBuffer.NULL;

        _mappedCount = 0;
        _mappedMutex.Dispose();

        ResourceInfo.MappedAddress = null;
        ResourceInfo.MemoryRegion.Dispose();

        _ = Device.RemoveBuffer(this);

        static void DisposeVkBuffer(VkDevice vkDevice, VkBuffer vkBuffer)
        {
            if (vkBuffer != VkBuffer.NULL)
            {
                vkDestroyBuffer(vkDevice, vkBuffer, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void DisposeAllViewsUnsafe()
    {
        _bufferViews.Dispose();
    }

    /// <inheritdoc />
    protected override byte* MapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex();
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex();
    }

    /// <inheritdoc />
    protected override byte* MapForReadUnsafe(nuint offset, nuint size)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapForReadNoMutex(offset, size);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_BUFFER, VkBuffer, value);
    }

    /// <inheritdoc />
    protected override bool TryCreateBufferViewUnsafe(in GraphicsBufferViewCreateOptions createOptions, [NotNullWhen(true)] out GraphicsBufferView? bufferView)
    {
        nuint byteLength = createOptions.BytesPerElement;
        byteLength *= createOptions.ElementCount;
        nuint byteAlignment = 0;

        if (Kind == GraphicsBufferKind.Index)
        {
            byteAlignment = createOptions.BytesPerElement;
        }
        else if (Kind == GraphicsBufferKind.Constant)
        {
            byteAlignment = VkMinUniformBufferOffsetAlignment;
        }
        else if (Kind == GraphicsBufferKind.Default)
        {
            byteAlignment = VkMinTexelBufferOffsetAlignment;
        }

        var succeeded = _memoryAllocator.TryAllocate(byteLength, byteAlignment, out var memoryRegion);
        bufferView = succeeded ? new VulkanGraphicsBufferView(this, in createOptions, in memoryRegion) : null;

        return succeeded;
    }

    /// <inheritdoc />
    protected override void UnmapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex();
    }

    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex();
    }


    /// <inheritdoc />
    protected override void UnmapAndWriteUnsafe(nuint offset, nuint size)
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapAndWriteNoMutex(offset, size);
    }

    internal void AddBufferView(VulkanGraphicsBufferView bufferView)
    {
        _bufferViews.Add(bufferView, _bufferViewsMutex);
    }

    internal byte* MapView(nuint byteStart)
    {
        return MapUnsafe() + byteStart;
    }

    internal byte* MapViewForRead(nuint byteStart, nuint byteLength)
    {
        return MapForReadUnsafe(byteStart, byteLength) + byteStart;
    }

    internal bool RemoveBufferView(VulkanGraphicsBufferView bufferView)
    {
        using var mutex = new DisposableMutex(_bufferViewsMutex, isExternallySynchronized: false);
        return _bufferViews.Remove(bufferView);
    }

    internal void UnmapView()
    {
        UnmapUnsafe();
    }

    internal void UnmapViewAndWrite(nuint byteStart, nuint byteLength)
    {
        UnmapAndWriteUnsafe(byteStart, byteLength);
    }

    private byte* MapNoMutex()
    {
        byte* mappedAddress;

        if (Interlocked.Increment(ref _mappedCount) == 1)
        {
            mappedAddress = MemoryHeap.Map() + MemoryRegion.ByteOffset;
            ResourceInfo.MappedAddress = mappedAddress;
        }
        else
        {
            mappedAddress = (byte*)ResourceInfo.MappedAddress;
        }

        return mappedAddress;
    }

    private byte* MapForReadNoMutex()
    {
        return MapForReadNoMutex(0, ByteLength);
    }

    private byte* MapForReadNoMutex(nuint byteStart, nuint byteLength)
    {
        var mappedAddress = MapNoMutex();
        var vkNonCoherentAtomSize = VkNonCoherentAtomSize;

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            pNext = null,
            memory = MemoryHeap.VkDeviceMemory,
            offset = AlignDown(MemoryRegion.ByteOffset + byteStart, vkNonCoherentAtomSize),
            size = AlignUp(byteLength, vkNonCoherentAtomSize),
        };
        ThrowExternalExceptionIfNotSuccess(vkInvalidateMappedMemoryRanges(Device.VkDevice, 1, &vkMappedMemoryRange));

        return mappedAddress;
    }

    private void UnmapNoMutex()
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        else if (mappedCount == 0)
        {
            MemoryHeap.Unmap();
            ResourceInfo.MappedAddress = null;
        }
    }

    private void UnmapAndWriteNoMutex()
    {
        UnmapAndWriteNoMutex(0, ByteLength);
    }

    private void UnmapAndWriteNoMutex(nuint byteStart, nuint byteLength)
    {
        var mappedCount = Interlocked.Decrement(ref _mappedCount);

        if (mappedCount == uint.MaxValue)
        {
            ThrowForInvalidState(nameof(IsMapped));
        }
        var vkNonCoherentAtomSize = VkNonCoherentAtomSize;

        var vkMappedMemoryRange = new VkMappedMemoryRange {
            sType = VK_STRUCTURE_TYPE_MAPPED_MEMORY_RANGE,
            pNext = null,
            memory = MemoryHeap.VkDeviceMemory,
            offset = AlignDown(MemoryRegion.ByteOffset + byteStart, vkNonCoherentAtomSize),
            size = AlignUp(byteLength, vkNonCoherentAtomSize),
        };
        ThrowExternalExceptionIfNotSuccess(vkFlushMappedMemoryRanges(Device.VkDevice, 1, &vkMappedMemoryRange));

        if (mappedCount == 0)
        {
            MemoryHeap.Unmap();
            ResourceInfo.MappedAddress = null;
        }
    }
}
