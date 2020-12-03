// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

// This file includes code based on the VmaAllocator_T struct from https://github.com/GPUOpen-LibrariesAndSDKs/VulkanMemoryAllocator
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;

namespace TerraFX.Graphics
{
    /// <summary>A memory allocator which manages memory for a graphics device.</summary>
    public abstract class GraphicsMemoryAllocator : IDisposable
    {
        /// <summary>The name of the data value that controls the value of <see cref="BlockMarginSize" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on allocators that have already been created.</para>
        ///     <para>This data value is interpreted as a ulong.</para>
        /// </remarks>
        public const string BlockMarginSizeDataName = "TerraFX.Graphics.GraphicsMemoryAllocator.BlockMarginSize";

        /// <summary>The name of the data value that controls the value of <see cref="BlockMinimumFreeRegionSizeToRegister" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on allocators that have already been created.</para>
        ///     <para>This data value is interpreted as a ulong.</para>
        /// </remarks>
        public const string BlockMinimumFreeRegionSizeToRegisterDataName = "TerraFX.Graphics.GraphicsMemoryAllocator.BlockMinimumFreeRegionSizeToRegister";

        /// <summary>The name of the data value that controls the fallback value of <see cref="BlockPreferredSize" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on allocators that have already been created.</para>
        ///     <para>This data value is interpreted as a ulong.</para>
        /// </remarks>
        public const string BlockPreferredSizeDataName = "TerraFX.Graphics.GraphicsMemoryAllocator.BlockPreferredSize";

        /// <summary>The name of the data value that controls the fallback value of <see cref="BlockPreferredSize" />.</summary>
        /// <remarks>
        ///     <para>This name is meant to be used with <see cref="AppDomain.SetData(string, object)" />.</para>
        ///     <para>Setting this data value has no affect on allocators that have already been created.</para>
        ///     <para>This data value is interpreted as a ulong.</para>
        /// </remarks>
        public const string IsExternallySynchronizedDataName = "TerraFX.Graphics.GraphicsMemoryAllocator.IsExternallySynchronized";

        // Default to no margins as they are mainly meant for debugging scenarios
        private const ulong DefaultBlockMarginSize = 0;

        // Default to 16 bytes as the smallest useful amount of free space
        private const ulong DefaultBlockMinimumFreeRegionSizeToRegister = 16;

        // Default to 256MB blocks, causing anything larger to get its own block
        private const ulong DefaultBlockPreferredSize = 256 * 1024 * 1024;

        private readonly ulong _blockMarginSize;
        private readonly ulong _blockMinimumFreeRegionSizeToRegister;
        private readonly ulong _blockPreferredSize;
        private readonly GraphicsDevice _device;
        private readonly bool _isExternallySynchronized;

        /// <summary>Initializes a new instance of the <see cref="GraphicsMemoryAllocator" /> class.</summary>
        /// <param name="device">The device for which the allocator will manage memory.</param>
        /// <param name="blockPreferredSize">The preferred size of an allocated block or <c>zero</c> to use the system default.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
        protected GraphicsMemoryAllocator(GraphicsDevice device, ulong blockPreferredSize)
        {
            _blockMarginSize = GetDataNameValue(BlockMarginSizeDataName, DefaultBlockMarginSize);
            _blockMinimumFreeRegionSizeToRegister = GetDataNameValue(BlockMinimumFreeRegionSizeToRegisterDataName, DefaultBlockMinimumFreeRegionSizeToRegister);
            _blockPreferredSize = (blockPreferredSize == 0) ? GetDataNameValue(BlockPreferredSizeDataName, DefaultBlockPreferredSize) : blockPreferredSize;
            _device = device;
            _isExternallySynchronized = GetDataNameValue(IsExternallySynchronizedDataName, false);

            static T GetDataNameValue<T>(string dataName, T defaultValue)
            {
                return AppContext.GetData(dataName) is T value ? value : defaultValue;
            }
        }

        /// <summary>Gets the minimum size of free regions to keep on either side of an allocated region, in bytes.</summary>
        public ulong BlockMarginSize => _blockMarginSize;

        /// <summary>Gets the minimum size of a free region for it to be registered as available, in bytes.</summary>
        public ulong BlockMinimumFreeRegionSizeToRegister => _blockMinimumFreeRegionSizeToRegister;

        /// <summary>Gets the preferred size of an allocated block.</summary>
        public ulong BlockPreferredSize => _blockPreferredSize;

        /// <summary>Gets the device for which the allocator will manage memory.</summary>
        public GraphicsDevice Device => _device;

        /// <summary>Gets <c>true</c> if the allocator is externally synchronized; otherwise, <c>false</c>.</summary>
        public bool IsExternallySynchronized => _isExternallySynchronized;

        /// <summary>Creates a new graphics buffer.</summary>
        /// <param name="kind">The kind of graphics buffer to create.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
        /// <param name="size">The size, in bytes, of the graphics buffer.</param>
        /// <param name="alignment">The alignment of the buffer, in bytes.</param>
        /// <param name="allocationFlags">Additional flags used when allocating the backing memory for the buffer.</param>
        /// <returns>A created graphics buffer.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="kind" /> is unsupported.</exception>
        /// <exception cref="ObjectDisposedException">The allocator has been disposed.</exception>
        public GraphicsBuffer<IGraphicsMemoryRegionCollection<IGraphicsResource>.DefaultMetadata> CreateBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None)
            => CreateBuffer<IGraphicsMemoryRegionCollection<IGraphicsResource>.DefaultMetadata>(kind, cpuAccess, size, alignment, allocationFlags);

        /// <inheritdoc cref="CreateBuffer(GraphicsBufferKind, GraphicsResourceCpuAccess, ulong, ulong, GraphicsMemoryAllocationFlags)" />
        /// <typeparam name="TMetadata">The type used for metadata in the resource.</typeparam>
        public abstract GraphicsBuffer<TMetadata> CreateBuffer<TMetadata>(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None)
            where TMetadata : struct, IGraphicsMemoryRegionCollection<IGraphicsResource>.IMetadata;

        /// <summary>Creates a new graphics texture.</summary>
        /// <param name="kind">The kind of graphics texture to create.</param>
        /// <param name="cpuAccess">The CPU access capabilities of the buffer.</param>
        /// <param name="width">The width, in pixels, of the graphics texture.</param>
        /// <param name="height">The height, in pixels, of the graphics texture.</param>
        /// <param name="depth">The depth, in pixels, of the graphics texture.</param>
        /// <param name="alignment">The alignment of the buffer, in bytes.</param>
        /// <param name="allocationFlags">Additional flags used when allocating the backing memory for the buffer.</param>
        /// <param name="texelFormat">Optional parameter to specify the texel format.</param>
        /// <returns>A created graphics texture.</returns>
        /// <exception cref="ObjectDisposedException">The allocator has been disposed.</exception>
        public GraphicsTexture<IGraphicsMemoryRegionCollection<IGraphicsResource>.DefaultMetadata> CreateTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None, TexelFormat texelFormat = default)
            => CreateTexture<IGraphicsMemoryRegionCollection<IGraphicsResource>.DefaultMetadata>(kind, cpuAccess, width, height, depth, alignment, allocationFlags, texelFormat);

        /// <inheritdoc cref="CreateTexture(GraphicsTextureKind, GraphicsResourceCpuAccess, uint, uint, ushort, ulong, GraphicsMemoryAllocationFlags, TexelFormat)" />
        /// <typeparam name="TMetadata">The type used for metadata in the resource.</typeparam>
        public abstract GraphicsTexture<TMetadata> CreateTexture<TMetadata>(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None, TexelFormat texelFormat = default)
            where TMetadata : struct, IGraphicsMemoryRegionCollection<IGraphicsResource>.IMetadata;

        /// <summary>Gets the budget for a block collection.</summary>
        /// <param name="blockCollection">The block collection for which the budget should be retrieved.</param>
        /// <param name="budget">On return, contains the budget for <paramref name="blockCollection" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="blockCollection" /> is <c>null</c>.</exception>
        public abstract void GetBudget(GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc cref="Dispose()" />
        /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        protected abstract void Dispose(bool isDisposing);
    }
}
