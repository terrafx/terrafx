// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaAllocator_T struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using System.Collections;
using System.Collections.Generic;

namespace TerraFX.Graphics;

/// <summary>A memory allocator which manages memory for a graphics device.</summary>
public abstract class GraphicsMemoryAllocator : GraphicsDeviceObject, IReadOnlyCollection<GraphicsMemoryBlockCollection>
{
    private readonly GraphicsMemoryAllocatorSettings _settings;

    /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryAllocator" /> class.</summary>
    /// <param name="device">The device for which the memory allocator is being created.</param>
    /// <param name="settings">The settings used by the allocator.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsMemoryAllocator(GraphicsDevice device, in GraphicsMemoryAllocatorSettings settings)
        : base(device)
    {
        // Default to no external synchronization
        var isExternallySynchronized = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.IsExternallySynchronizedDataName,
            settings.IsExternallySynchronized.GetValueOrDefault()
        );

        // Default to effectively no maximum block count
        var maximumBlockCountPerCollection = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.MaximumBlockCountPerCollectionDataName,
            settings.MaximumBlockCountPerCollection > 0 ? settings.MaximumBlockCountPerCollection : int.MaxValue
        );

        // Default to a 256MB max shared block size so anything larger gets a dedicated block
        // Given D3D12, this will be up to:
        //  * 64k small textures
        //  * 4k  buffers
        //  * 64  MSAA textures
        var maximumSharedBlockSize = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.MaximumSharedBlockSizeDataName,
            settings.MaximumSharedBlockSize.HasValue ? settings.MaximumSharedBlockSize.GetValueOrDefault() : 256 * 1024 * 1024
        );

        // Default to no minimum block count
        var minimumBlockCountPerCollection = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.MinimumBlockCountPerCollectionDataName,
            settings.MinimumBlockCountPerCollection >= 0 ? settings.MinimumBlockCountPerCollection : 0
        );

        // Default to 1/8th the maximum shared block size or 4096 if the user specified some really small max block size
        // Given the other defaults, this will be typically be a 32MB minimum block size
        var minimumBlockSize = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.MinimumBlockSizeDataName,
            settings.MinimumBlockSize != 0 ? settings.MinimumBlockSize : Math.Max(4096, maximumSharedBlockSize / 8)
        );

        // Default to no margins
        var minimumAllocatedRegionMarginSize = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.MinimumAllocatedRegionMarginSizeDataName,
            settings.MinimumAllocatedRegionMarginSize.HasValue ? settings.MinimumAllocatedRegionMarginSize.GetValueOrDefault() : 0
        );

        // Default to registering 4096 byte or larger free regions
        // This default exists due to the small resource textures and would otherwise be 64k 
        var minimumFreeRegionSizeToRegister = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.MinimumAllocatedRegionMarginSizeDataName,
            settings.MinimumFreeRegionSizeToRegister != 0 ? settings.MinimumFreeRegionSizeToRegister : 4096
        );

        // Default to IGraphicsMemoryRegionCoollection<GraphicsMemoryBlock>.DefaultMetadata
        var regionCollectionMetadataType = GetDataNameValue(
            GraphicsMemoryAllocatorSettings.RegionCollectionMetadataTypeDataName,
            settings.RegionCollectionMetadataType ?? typeof(IGraphicsMemoryRegionCollection<GraphicsMemoryBlock>.DefaultMetadata)
        );

        _settings = new GraphicsMemoryAllocatorSettings {
            IsExternallySynchronized = isExternallySynchronized,
            MaximumBlockCountPerCollection = maximumBlockCountPerCollection,
            MaximumSharedBlockSize = maximumSharedBlockSize,
            MinimumBlockCountPerCollection = minimumBlockCountPerCollection,
            MinimumBlockSize = minimumBlockSize,
            MinimumAllocatedRegionMarginSize = minimumAllocatedRegionMarginSize,
            MinimumFreeRegionSizeToRegister = minimumFreeRegionSizeToRegister,
            RegionCollectionMetadataType = regionCollectionMetadataType,
        };

        static T GetDataNameValue<T>(string dataName, T defaultValue)
        {
            return AppContext.GetData(dataName) is T value ? value : defaultValue;
        }
    }

    /// <summary>Gets the number of block collections in the allocator.</summary>
    public abstract int Count { get; }

    /// <summary>Gets a <c>true</c> if the memory allocator should be externally synchronized; otherwise, <c>false</c>.</summary>
    public bool IsExternallySynchronized => _settings.IsExternallySynchronized.GetValueOrDefault();

    /// <summary>Gets the settings used by the allocator.</summary>
    public ref readonly GraphicsMemoryAllocatorSettings Settings => ref _settings;

    /// <summary>Creates a new graphics buffer.</summary>
    /// <param name="kind">The kind of graphics buffer to create.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <param name="size">The size, in bytes, of the graphics buffer.</param>
    /// <param name="allocationFlags">Additional flags used when allocating the backing memory for the buffer.</param>
    /// <returns>A created graphics buffer.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="size" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The allocator has been disposed.</exception>
    public GraphicsBuffer CreateBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, GraphicsMemoryRegionAllocationFlags allocationFlags = GraphicsMemoryRegionAllocationFlags.None)
        => CreateBuffer<IGraphicsMemoryRegionCollection<GraphicsResource>.DefaultMetadata>(kind, cpuAccess, size, allocationFlags);

    /// <inheritdoc cref="CreateBuffer(GraphicsBufferKind, GraphicsResourceCpuAccess, ulong, GraphicsMemoryRegionAllocationFlags)" />
    /// <typeparam name="TMetadata">The type used for metadata in the resource.</typeparam>
    public abstract GraphicsBuffer CreateBuffer<TMetadata>(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, GraphicsMemoryRegionAllocationFlags allocationFlags = GraphicsMemoryRegionAllocationFlags.None)
        where TMetadata : struct, IGraphicsMemoryRegionCollection<GraphicsResource>.IMetadata;

    /// <summary>Creates a new graphics texture.</summary>
    /// <param name="kind">The kind of graphics texture to create.</param>
    /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
    /// <param name="width">The width, in pixels, of the graphics texture.</param>
    /// <param name="height">The height, in pixels, of the graphics texture.</param>
    /// <param name="depth">The depth, in pixels, of the graphics texture.</param>
    /// <param name="allocationFlags">Additional flags used when allocating the backing memory for the buffer.</param>
    /// <param name="texelFormat">Optional parameter to specify the texel format.</param>
    /// <returns>A created graphics texture.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="width" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="height" /> is <c>zero</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="depth" /> is <c>zero</c>.</exception>
    /// <exception cref="ObjectDisposedException">The allocator has been disposed.</exception>
    public GraphicsTexture CreateTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, GraphicsMemoryRegionAllocationFlags allocationFlags = GraphicsMemoryRegionAllocationFlags.None, TexelFormat texelFormat = default)
        => CreateTexture<IGraphicsMemoryRegionCollection<GraphicsResource>.DefaultMetadata>(kind, cpuAccess, width, height, depth, allocationFlags, texelFormat);

    /// <inheritdoc cref="CreateTexture(GraphicsTextureKind, GraphicsResourceCpuAccess, uint, uint, ushort, GraphicsMemoryRegionAllocationFlags, TexelFormat)" />
    /// <typeparam name="TMetadata">The type used for metadata in the resource.</typeparam>
    public abstract GraphicsTexture CreateTexture<TMetadata>(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, GraphicsMemoryRegionAllocationFlags allocationFlags = GraphicsMemoryRegionAllocationFlags.None, TexelFormat texelFormat = default)
        where TMetadata : struct, IGraphicsMemoryRegionCollection<GraphicsResource>.IMetadata;

    /// <summary>Gets the budget for a block collection.</summary>
    /// <param name="blockCollection">The block collection for which the budget should be retrieved.</param>
    /// <param name="budget">On return, contains the budget for <paramref name="blockCollection" />.</param>
    /// <exception cref="ArgumentNullException"><paramref name="blockCollection" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="blockCollection" /> is not managed by the allocator.</exception>
    public abstract void GetBudget(GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget);

    /// <summary>Gets an enumerator that can be used to iterate through the block collections of the allocator.</summary>
    /// <returns>An enumerator that can be used to iterate through the block collections of the allocator.</returns>
    public abstract IEnumerator<GraphicsMemoryBlockCollection> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
