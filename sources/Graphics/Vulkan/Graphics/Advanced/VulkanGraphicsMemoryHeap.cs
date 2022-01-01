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
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsMemoryHeap : GraphicsDeviceObject
{
    private readonly VulkanGraphicsMemoryManager _memoryManager;

    private VkDeviceMemory _vkDeviceMemory;

    private readonly nuint _byteLength;

    private volatile void* _mappedAddress;
    private volatile uint _mappedCount;
    private readonly ValueMutex _mappedMutex;

    internal VulkanGraphicsMemoryHeap(VulkanGraphicsMemoryManager memoryManager, in VulkanGraphicsMemoryHeapCreateOptions createOptions) : base(memoryManager.Device)
    {
        _memoryManager = memoryManager;

        _vkDeviceMemory = CreateVkDeviceMemory(in createOptions);

        _byteLength = createOptions.ByteLength;

        _mappedAddress = null;
        _mappedCount = 0;
        _mappedMutex = new ValueMutex();

        SetNameUnsafe(Name);

        VkDeviceMemory CreateVkDeviceMemory(in VulkanGraphicsMemoryHeapCreateOptions createOptions)
        {
            VkDeviceMemory vkDeviceMemory;

            var vkMemoryAllocateInfo = new VkMemoryAllocateInfo {
                sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                pNext = null,
                allocationSize = createOptions.ByteLength,
                memoryTypeIndex = createOptions.VkMemoryTypeIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateMemory(Device.VkDevice, &vkMemoryAllocateInfo, pAllocator: null, &vkDeviceMemory));

            return vkDeviceMemory;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsMemoryHeap" /> class.</summary>
    ~VulkanGraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <summary>Gets the length, in bytes, of the memory heap.</summary>
    public nuint ByteLength => _byteLength;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsResource.IsMapped" />
    public bool IsMapped => MappedAddress is not null;

    /// <inheritdoc cref="GraphicsResource.MappedAddress" />
    public unsafe void* MappedAddress => _mappedAddress;

    /// <summary>Gets the memory manager which created the memory heap.</summary>
    public VulkanGraphicsMemoryManager MemoryManager => _memoryManager;

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the <see cref="Interop.Vulkan.VkDeviceMemory" /> for the memory heap.</summary>
    public VkDeviceMemory VkDeviceMemory => _vkDeviceMemory;

    /// <summary>Maps the memory heap into CPU memory.</summary>
    /// <returns>A pointer to the mapped memory heap.</returns>
    /// <exception cref="ObjectDisposedException">The memory heap has been disposed.</exception>
    public byte* Map()
    {
        ThrowIfDisposed();
        return MapUnsafe();
    }

    /// <summary>Unmaps the memory heap from CPU memory.</summary>
    /// <remarks>This overload should be used when no memory was written.</remarks>
    /// <exception cref="InvalidOperationException">The memory heap is not already mapped.</exception>
    /// <exception cref="ObjectDisposedException">The memory heap has been disposed.</exception>
    public void Unmap()
    {
        ThrowIfDisposed();
        UnmapUnsafe();
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        DisposeVkDeviceMemory(Device.VkDevice, _vkDeviceMemory);
        _vkDeviceMemory = VkDeviceMemory.NULL;

        _mappedAddress = null;
        _mappedCount = 0;
        _mappedMutex.Dispose();

        static void DisposeVkDeviceMemory(VkDevice vkDevice, VkDeviceMemory vkDeviceMemory)
        {
            if (vkDeviceMemory != VkDeviceMemory.NULL)
            {
                vkFreeMemory(vkDevice, vkDeviceMemory, pAllocator: null);
            }
        }
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        Device.SetVkObjectName(VK_OBJECT_TYPE_DEVICE_MEMORY, VkDeviceMemory, value);
    }

    private byte* MapNoMutex()
    {
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

    private byte* MapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        return MapNoMutex();
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
            vkUnmapMemory(Device.VkDevice, VkDeviceMemory);
        }
    }

    private void UnmapUnsafe()
    {
        using var mutex = new DisposableMutex(_mappedMutex, isExternallySynchronized: false);
        UnmapNoMutex();
    }
}
