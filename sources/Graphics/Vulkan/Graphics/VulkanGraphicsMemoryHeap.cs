// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the VmaDeviceMemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkObjectType;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsMemoryHeap : GraphicsDeviceObject
{
    private readonly VkDeviceMemory _vkDeviceMemory;
    private readonly ulong _size;

    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsMemoryHeap(VulkanGraphicsDevice device, ulong size, uint vkMemoryTypeIndex)
        : base(device)
    {
        _vkDeviceMemory = CreateVkDeviceMemory(device, size, vkMemoryTypeIndex);
        _size = size;

        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsMemoryHeap);

        static VkDeviceMemory CreateVkDeviceMemory(VulkanGraphicsDevice device, ulong size, uint vkMemoryTypeIndex)
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
