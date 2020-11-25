// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// This file includes code based on the MemoryBlock class from https://github.com/GPUOpen-LibrariesAndSDKs/D3D12MemoryAllocator/
// The original code is Copyright © Advanced Micro Devices, Inc. All rights reserved. Licensed under the MIT License (MIT).

using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12_HEAP_FLAGS;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsMemoryBlock<TMetadata> : GraphicsMemoryBlock<TMetadata>
        where TMetadata : struct, GraphicsMemoryBlock.IMetadata
    {
        private ValueLazy<Pointer<ID3D12Heap>> _d3d12Heap;
        private State _state;

        internal D3D12GraphicsMemoryBlock(D3D12GraphicsMemoryBlockCollection collection, ulong size, ulong marginSize, ulong minimumFreeRegionSizeToRegister)
            : base(collection, size, marginSize, minimumFreeRegionSizeToRegister)
        {
            _d3d12Heap = new ValueLazy<Pointer<ID3D12Heap>>(CreateD3D12Heap);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsMemoryBlock{TMetadata}" /> class.</summary>
        ~D3D12GraphicsMemoryBlock() => Dispose(isDisposing: true);

        /// <summary>Gets the <see cref="ID3D12Heap" /> for the memory block.</summary>
        public ID3D12Heap* D3D12Heap => _d3d12Heap.Value;

        /// <inheritdoc cref="GraphicsMemoryBlock.Collection" />
        public new D3D12GraphicsMemoryBlockCollection Collection => (D3D12GraphicsMemoryBlockCollection)base.Collection;

        /// <inheritdoc />
        public override T GetHandle<T>()
        {
            if (typeof(T) != typeof(Pointer<ID3D12Heap>))
            {
                ThrowArgumentExceptionForInvalidType(typeof(T), nameof(T));
            }
            return (T)(object)_d3d12Heap.Value;
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _d3d12Heap.Dispose(ReleaseIfNotNull);
            }

            _state.EndDispose();
        }

        private static ulong GetAlignment(D3D12_HEAP_FLAGS heapFlags)
        {
            const D3D12_HEAP_FLAGS DenyAllTexturesFlags = D3D12_HEAP_FLAG_DENY_NON_RT_DS_TEXTURES | D3D12_HEAP_FLAG_DENY_RT_DS_TEXTURES;
            var canContainAnyTextures = (heapFlags & DenyAllTexturesFlags) != DenyAllTexturesFlags;
            return canContainAnyTextures ? D3D12_DEFAULT_MSAA_RESOURCE_PLACEMENT_ALIGNMENT : D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT;
        }

        private Pointer<ID3D12Heap> CreateD3D12Heap()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Heap* d3d12Heap;

            var collection = Collection;
            var d3d12Device = collection.Allocator.Device.D3D12Device;

            var heapFlags = collection.D3D12HeapFlags;
            var heapType = collection.D3D12HeapType;

            var heapDesc = new D3D12_HEAP_DESC(Size, heapType, GetAlignment(heapFlags), heapFlags);

            var iid = IID_ID3D12Heap;
            ThrowExternalExceptionIfFailed(d3d12Device->CreateHeap(&heapDesc, &iid, (void**)&d3d12Heap), nameof(ID3D12Device.CreateHeap));

            return d3d12Heap;
        }
    }
}
