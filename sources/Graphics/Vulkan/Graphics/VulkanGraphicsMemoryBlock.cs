// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop.Vulkan;
using TerraFX.Threading;
using static TerraFX.Interop.Vulkan.VkStructureType;
using static TerraFX.Interop.Vulkan.Vulkan;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.VulkanUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public abstract unsafe class VulkanGraphicsMemoryBlock : GraphicsMemoryBlock
{
    private ValueLazy<VkDeviceMemory> _vulkanDeviceMemory;
    private protected VolatileState _state;

    private protected VulkanGraphicsMemoryBlock(VulkanGraphicsDevice device, VulkanGraphicsMemoryBlockCollection collection)
        : base(device, collection)
    {
        _vulkanDeviceMemory = new ValueLazy<VkDeviceMemory>(CreateVulkanDeviceMemory);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsMemoryBlock{TMetadata}" /> class.</summary>
    ~VulkanGraphicsMemoryBlock() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsMemoryBlock.Collection" />
    public new VulkanGraphicsMemoryBlockCollection Collection => (VulkanGraphicsMemoryBlockCollection)base.Collection;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

    /// <summary>Gets the <see cref="VkDeviceMemory" /> for the memory block.</summary>
    public VkDeviceMemory VulkanDeviceMemory => _vulkanDeviceMemory.Value;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            _vulkanDeviceMemory.Dispose(DisposeVulkanDeviceMemory);
        }

        _state.EndDispose();
    }

    private VkDeviceMemory CreateVulkanDeviceMemory()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(VulkanGraphicsMemoryBlock));

        VkDeviceMemory vulkanDeviceMemory;

        var collection = Collection;
        var vulkanDevice = collection.Allocator.Device.VulkanDevice;

        var memoryAllocateInfo = new VkMemoryAllocateInfo {
            sType = VK_STRUCTURE_TYPE_MEMORY_ALLOCATE_INFO,
            allocationSize = Size,
            memoryTypeIndex = collection.VulkanMemoryTypeIndex,
        };
        ThrowExternalExceptionIfNotSuccess(vkAllocateMemory(vulkanDevice, &memoryAllocateInfo, pAllocator: null, &vulkanDeviceMemory));

        return vulkanDeviceMemory;
    }

    private void DisposeVulkanDeviceMemory(VkDeviceMemory vulkanDeviceMemory)
    {
        if (vulkanDeviceMemory != VkDeviceMemory.NULL)
        {
            vkFreeMemory(Collection.Allocator.Device.VulkanDevice, vulkanDeviceMemory, pAllocator: null);
        }
    }
}
