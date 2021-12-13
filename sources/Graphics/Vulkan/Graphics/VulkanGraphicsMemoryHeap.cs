// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

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
public sealed unsafe class VulkanGraphicsMemoryHeap : GraphicsMemoryHeap
{
    private readonly VkDeviceMemory _vkDeviceMemory;

    private VolatileState _state;

    internal VulkanGraphicsMemoryHeap(VulkanGraphicsDevice device, VulkanGraphicsMemoryHeapCollection collection, ulong size)
        : base(device, collection, size)
    {
        _vkDeviceMemory = CreateVkDeviceMemory(device, collection, size);

        _ = _state.Transition(to: Initialized);

        static VkDeviceMemory CreateVkDeviceMemory(VulkanGraphicsDevice device, VulkanGraphicsMemoryHeapCollection collection, ulong size)
        {
            VkDeviceMemory vkDeviceMemory;

            var vkMemoryAllocateInfo = new VkMemoryAllocateInfo {
                sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
                allocationSize = size,
                memoryTypeIndex = collection.VkMemoryTypeIndex,
            };
            ThrowExternalExceptionIfNotSuccess(vkAllocateMemory(device.VkDevice, &vkMemoryAllocateInfo, pAllocator: null, &vkDeviceMemory));

            return vkDeviceMemory;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsMemoryHeap" /> class.</summary>
    ~VulkanGraphicsMemoryHeap() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsMemoryHeap.Collection" />
    public new VulkanGraphicsMemoryHeapCollection Collection => base.Collection.As<VulkanGraphicsMemoryHeapCollection>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

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
}
