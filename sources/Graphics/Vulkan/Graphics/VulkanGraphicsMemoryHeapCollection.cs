// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop.Vulkan;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed class VulkanGraphicsMemoryHeapCollection : GraphicsMemoryHeapCollection
{
    private readonly uint _vkMemoryTypeIndex;

    internal VulkanGraphicsMemoryHeapCollection(VulkanGraphicsDevice device, VulkanGraphicsMemoryAllocator allocator, uint vkMemoryTypeIndex)
        : base(device, allocator)
    {
        _vkMemoryTypeIndex = vkMemoryTypeIndex;
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsMemoryHeapCollection.Allocator" />
    public new VulkanGraphicsMemoryAllocator Allocator => base.Allocator.As<VulkanGraphicsMemoryAllocator>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <summary>Gets the memory type index used when creating the <see cref="VkDeviceMemory" /> instance for a memory heap.</summary>
    public uint VkMemoryTypeIndex => _vkMemoryTypeIndex;

    /// <inheritdoc />
    protected override VulkanGraphicsMemoryHeap CreateHeap(ulong size) => new VulkanGraphicsMemoryHeap(Device, this, size);
}
