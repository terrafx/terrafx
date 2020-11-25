// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the Allocator class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_HEAP_TYPE;
using static TerraFX.Interop.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_HEAP_TIER;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsMemoryAllocator : GraphicsMemoryAllocator
    {
        private readonly D3D12GraphicsMemoryBlockCollection[] _blockCollections;
        private readonly bool _supportsResourceHeapTier2;

        private State _state;

        internal D3D12GraphicsMemoryAllocator(D3D12GraphicsDevice device, ulong blockPreferredSize)
            : base(device, blockPreferredSize)
        {
            blockPreferredSize = BlockPreferredSize;

            var supportsResourceHeapTier2 = Device.D3D12Options.ResourceHeapTier >= D3D12_RESOURCE_HEAP_TIER_2;
            var blockMinimumSize = blockPreferredSize >> 3;

            if (blockMinimumSize == 0)
            {
                blockMinimumSize = blockPreferredSize;
            }

            _blockCollections = supportsResourceHeapTier2
                ? new D3D12GraphicsMemoryBlockCollection[3] {
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_NONE, D3D12_HEAP_TYPE_READBACK),
                }
                : new D3D12GraphicsMemoryBlockCollection[9] {
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_DEFAULT),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_UPLOAD),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                    new D3D12GraphicsMemoryBlockCollection(blockMinimumSize, blockPreferredSize, BlockMarginSize, BlockMinimumFreeRegionSizeToRegister, this, minimumBlockCount: 0, maximumBlockCount: nuint.MaxValue, D3D12_HEAP_FLAG_DENY_BUFFERS | D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES, D3D12_HEAP_TYPE_READBACK),
                };

            _supportsResourceHeapTier2 = supportsResourceHeapTier2;

            // TODO: UpdateBudget
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsMemoryAllocator" /> class.</summary>
        ~D3D12GraphicsMemoryAllocator() => Dispose(isDisposing: true);

        /// <inheritdoc cref="GraphicsMemoryAllocator.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

        /// <summary>Gets <c>true</c> if <see cref="Device" /> supports <see cref="D3D12_RESOURCE_HEAP_TIER_2" />; otherwise, <c>false</c>.</summary>
        public bool SupportsResourceHeapTier2 => _supportsResourceHeapTier2;

        /// <inheritdoc />
        public override D3D12GraphicsBuffer CreateBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, ulong size, ulong alignment = 0, GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None)
        {
            var index = GetBlockCollectionIndex(cpuAccess, 0);

            var resourceDesc = D3D12_RESOURCE_DESC.Buffer(size, D3D12_RESOURCE_FLAG_NONE, alignment);
            var resourceAllocationInfo = Device.D3D12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &resourceDesc);
            ref readonly var blockCollection = ref _blockCollections[index];

            return !blockCollection.TryAllocate(resourceAllocationInfo.SizeInBytes, resourceAllocationInfo.Alignment, allocationFlags, out var region)
                 ? new D3D12GraphicsBuffer(kind, cpuAccess, in region)
                 : throw new OutOfMemoryException();
        }

        /// <inheritdoc />
        public override D3D12GraphicsTexture CreateTexture(GraphicsTextureKind kind, GraphicsResourceCpuAccess cpuAccess, uint width, uint height = 1, ushort depth = 1, ulong alignment = 0,
            GraphicsMemoryAllocationFlags allocationFlags = GraphicsMemoryAllocationFlags.None,
            TexelFormat texelFormat = default)
        {
            var dxgiFormat = D3D12GraphicsMemoryTexelMapper.Map(texelFormat);
            var index = GetBlockCollectionIndex(cpuAccess, 1);

            var resourceDesc = kind switch {
                GraphicsTextureKind.OneDimensional => D3D12_RESOURCE_DESC.Tex1D(dxgiFormat, width, mipLevels: 1, alignment: alignment),
                GraphicsTextureKind.TwoDimensional => D3D12_RESOURCE_DESC.Tex2D(dxgiFormat, width, height, mipLevels: 1, alignment: alignment),
                GraphicsTextureKind.ThreeDimensional => D3D12_RESOURCE_DESC.Tex3D(dxgiFormat, width, height, depth, mipLevels: 1, alignment: alignment),
                _ => default,
            };

            var resourceAllocationInfo = Device.D3D12Device->GetResourceAllocationInfo(visibleMask: 0, numResourceDescs: 1, &resourceDesc);
            ref readonly var blockCollection = ref _blockCollections[index];

            return blockCollection.TryAllocate(resourceAllocationInfo.SizeInBytes, resourceAllocationInfo.Alignment, allocationFlags, out var region)
                 ? new D3D12GraphicsTexture(kind, cpuAccess, in region, width, height, depth)
                 : throw new OutOfMemoryException();
        }

        /// <inheritdoc />
        public override void GetBudget(GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget) => GetBudget((D3D12GraphicsMemoryBlockCollection)blockCollection, out budget);

        /// <inheritdoc cref="GetBudget(GraphicsMemoryBlockCollection, out GraphicsMemoryBudget)" />
        public void GetBudget(D3D12GraphicsMemoryBlockCollection blockCollection, out GraphicsMemoryBudget budget) => budget = new GraphicsMemoryBudget(estimatedBudget: ulong.MaxValue, estimatedUsage: 0, totalAllocatedRegionSize: 0, totalBlockSize: 0);

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                foreach (var blockCollection in _blockCollections)
                {
                    blockCollection?.Dispose();
                }
            }

            _state.EndDispose();
        }

        private int GetBlockCollectionIndex(GraphicsResourceCpuAccess cpuAccess, int kind)
        {
            var index = cpuAccess switch
            {
                GraphicsResourceCpuAccess.None => 0,        // DEFAULT
                GraphicsResourceCpuAccess.Read => 2,        // READBACK
                GraphicsResourceCpuAccess.Write => 1,       // UPLOAD
                _ => -1,
            };

            if (index < 0)
            {
                ThrowArgumentOutOfRangeException(cpuAccess, nameof(cpuAccess));
            }
            else if (!_supportsResourceHeapTier2)
            {
                // Scale to account for resource kind
                index *= 3;
                index += kind;
            }

            return index;
        }
    }
}
