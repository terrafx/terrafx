// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Threading;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsMemoryHeap : GraphicsDeviceObject
{
    private readonly ValueMutex _mapMutex;
    private readonly VulkanGraphicsMemoryManager _memoryManager;
    private readonly nuint _size;
    private readonly VkDeviceMemory _vkDeviceMemory;

    private volatile void* _mappedAddress;
    private volatile uint _mappedCount;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsMemoryHeap(VulkanGraphicsMemoryManager memoryManager, nuint size, uint vkMemoryTypeIndex)
        : base(memoryManager.Device)
    {
        _mapMutex = new ValueMutex();
        _memoryManager = memoryManager;
        _size = size;
        _vkDeviceMemory = CreateVkDeviceMemory(memoryManager.Device, size, vkMemoryTypeIndex);

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsMemoryHeap);

        static VkDeviceMemory CreateVkDeviceMemory(VulkanGraphicsDevice device, nuint size, uint vkMemoryTypeIndex)
        {
            VkDeviceMemory vkDeviceMemory;

            var vkMemoryAllocateInfo = new VkMemoryAllocateInfo {
                sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = size,
                memoryTypeIndex = vkMemoryTypeIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateMemory(device.VkDevice, &vkMemoryAllocateInfo, pAllocator: null, &vkDeviceMemory));

            return vkDeviceMemory;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsMemoryHeap" /> class.</summary>
    ~VulkanGraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsResource.IsMapped" />
    public bool IsMapped => _mappedCount != 0;

    /// <inheritdoc cref="GraphicsResource.MappedAddress" />
    public unsafe void* MappedAddress => _mappedAddress;

    /// <summary>Gets the memory manager which created the memory heap.</summary>
    public VulkanGraphicsMemoryManager MemoryManager => _memoryManager;

    /// <inheritdoc />
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = Device.UpdateName(VK_OBJECT_TYPE_DEVICE_MEMORY, VkDeviceMemory, value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDeviceMemory" /> for the memory heap.</summary>
    public VkDeviceMemory VkDeviceMemory
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _vkDeviceMemory;
        }
    }

    /// <summary>Maps the memory heap into CPU memory.</summary>
    /// <returns>A pointer to the mapped memory heap.</returns>
    public byte* Map()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        return MapInternal();
    }

    /// <summary>Unmaps the memory heap from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The memory heap is not already mapped.</exception>
    public void Unmap()
    {
        using var mutex = new DisposableMutex(_mapMutex, isExternallySynchronized: false);
        UnmapInternal();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            DisposeVkDeviceMemory(Device.VkDevice, _vkDeviceMemory);
        }

        _state.EndDispose();

        static void DisposeVkDeviceMemory(VkDevice vkDevice, VkDeviceMemory vkDeviceMemory)
        {
            if (vkDeviceMemory != VkDeviceMemory.NULL)
            {
                vkFreeMemory(vkDevice, vkDeviceMemory, pAllocator: null);
            }
        }
    }

    private byte* MapInternal()
    {
        // This method should only be called under a mutex
        void* mappedAddress;

        if (Interlocked.Increment(ref _mappedCount) == 1)
        {
            ThrowExternalExceptionIfNotSuccess(vkMapMemory(Device.VkDevice, VkDeviceMemory, offset: 0, size: VK_WHOLE_SIZE, flags: 0, &mappedAddress));
            _mappedAddress = mappedAddress;
        }
        else
        {
            mappedAddress = _mappedAddress;
        }

        return (byte*)mappedAddress;
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
            vkUnmapMemory(Device.VkDevice, VkDeviceMemory);
        }
    }
}
