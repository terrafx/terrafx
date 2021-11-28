// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the VmaBlockVector struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Reflection;
using TerraFX.Interop.Vulkan;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed class VulkanGraphicsMemoryHeapCollection : GraphicsMemoryHeapCollection
{
    private readonly uint _vulkanMemoryTypeIndex;

    internal VulkanGraphicsMemoryHeapCollection(VulkanGraphicsDevice device, VulkanGraphicsMemoryAllocator allocator, uint memoryTypeIndex)
        : base(device, allocator)
    {
        _vulkanMemoryTypeIndex = memoryTypeIndex;
    }

    /// <inheritdoc cref="GraphicsMemoryHeapCollection.Allocator" />
    public new VulkanGraphicsMemoryAllocator Allocator => (VulkanGraphicsMemoryAllocator)base.Allocator;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

    /// <summary>Gets the memory type index used when creating the <see cref="VkDeviceMemory" /> instance for a memory block.</summary>
    public uint VulkanMemoryTypeIndex => _vulkanMemoryTypeIndex;

    /// <inheritdoc />
    protected override VulkanGraphicsMemoryHeap CreateHeap(ulong size) => (VulkanGraphicsMemoryHeap)Activator.CreateInstance(
        typeof(VulkanGraphicsMemoryHeap<>).MakeGenericType(Allocator.Settings.RegionCollectionMetadataType!),
        bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.CreateInstance,
        binder: null,
        args: new object[] { Device, this, size },
        culture: null,
        activationAttributes: null
    )!;
}
