// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public abstract unsafe class D3D12GraphicsTexture : GraphicsTexture
    {
        private ValueLazy<Pointer<ID3D12Resource>> _d3d12Resource;
        private ValueLazy<D3D12_RESOURCE_STATES> _d3d12ResourceState;

        private protected State _state;

        internal D3D12GraphicsTexture(GraphicsTextureKind kind, in GraphicsMemoryRegion<GraphicsMemoryBlock> blockRegion, GraphicsResourceCpuAccess cpuAccess, uint width, uint height, ushort depth)
            : base(kind, in blockRegion, cpuAccess, width, height, depth)
        {
            _d3d12Resource = new ValueLazy<Pointer<ID3D12Resource>>(CreateD3D12Resource);
            _d3d12ResourceState = new ValueLazy<D3D12_RESOURCE_STATES>(GetD3D12ResourceState);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTexture" /> class.</summary>
        ~D3D12GraphicsTexture()
            => Dispose(isDisposing: true);

        /// <inheritdoc cref="GraphicsResource.Allocator" />
        public new D3D12GraphicsMemoryAllocator Allocator => (D3D12GraphicsMemoryAllocator)base.Allocator;

        /// <inheritdoc cref="GraphicsResource.Block" />
        public new D3D12GraphicsMemoryBlock Block => (D3D12GraphicsMemoryBlock)base.Block;

        /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the texture.</summary>
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Device.CreateCommittedResource(D3D12_HEAP_PROPERTIES*, D3D12_HEAP_FLAGS, D3D12_RESOURCE_DESC*, D3D12_RESOURCE_STATES, D3D12_CLEAR_VALUE*, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public ID3D12Resource* D3D12Resource => _d3d12Resource.Value;

        /// <summary>Gets the default state of the underlying <see cref="ID3D12Resource" /> for the texture.</summary>
        public D3D12_RESOURCE_STATES D3D12ResourceState => _d3d12ResourceState.Value;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
        public override T* Map<T>()
        {
            var readRange = default(D3D12_RANGE);

            byte* pDestination;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, (void**)&pDestination), nameof(ID3D12Resource.Map));

            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
        public override T* Map<T>(nuint rangeOffset, nuint rangeLength)
        {
            var readRange = default(D3D12_RANGE);

            byte* pDestination;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, (void**)&pDestination), nameof(ID3D12Resource.Map));

            return (T*)(pDestination + rangeOffset);
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
        public override T* MapForRead<T>()
        {
            byte* pDestination;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, null, (void**)&pDestination), nameof(ID3D12Resource.Map));

            return (T*)pDestination;
        }

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
        public override T* MapForRead<T>(nuint readRangeOffset, nuint readRangeLength)
        {
            var readRange = new D3D12_RANGE {
                Begin = readRangeOffset,
                End = readRangeOffset + readRangeLength,
            };

            byte* pDestination;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, (void**)&pDestination), nameof(ID3D12Resource.Map));

            return (T*)(pDestination + readRange.Begin);
        }

        /// <inheritdoc />
        public override void Unmap()
        {
            var writtenRange = default(D3D12_RANGE);
            D3D12Resource->Unmap(Subresource: 0, &writtenRange);
        }

        /// <inheritdoc />
        public override void UnmapAndWrite() => D3D12Resource->Unmap(Subresource: 0, null);

        /// <inheritdoc />
        public override void UnmapAndWrite(nuint writtenRangeOffset, nuint writtenRangeLength)
        {
            var writtenRange = new D3D12_RANGE {
                Begin = writtenRangeOffset,
                End = writtenRangeOffset + writtenRangeLength,
            };
            D3D12Resource->Unmap(Subresource: 0, &writtenRange);
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _d3d12Resource.Dispose(ReleaseIfNotNull);
                BlockRegion.Collection.Free(in BlockRegion);
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12Resource> CreateD3D12Resource()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Resource* d3d12Resource;

            ref readonly var blockRegion = ref BlockRegion;

            var textureDesc = Kind switch {
                GraphicsTextureKind.OneDimensional => D3D12_RESOURCE_DESC.Tex1D(DXGI_FORMAT_R8G8B8A8_UNORM, Width, mipLevels: 1),
                GraphicsTextureKind.TwoDimensional => D3D12_RESOURCE_DESC.Tex2D(DXGI_FORMAT_R8G8B8A8_UNORM, Width, Height, mipLevels: 1),
                GraphicsTextureKind.ThreeDimensional => D3D12_RESOURCE_DESC.Tex3D(DXGI_FORMAT_R8G8B8A8_UNORM, Width, Height, Depth, mipLevels: 1),
                _ => default,
            };
            var iid = IID_ID3D12Resource;

            var device = Allocator.Device;
            var d3d12Device = device.D3D12Device;
            var d3d12Heap = Block.D3D12Heap;

            ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
                d3d12Heap,
                blockRegion.Offset,
                &textureDesc,
                D3D12ResourceState,
                pOptimizedClearValue: null,
                &iid,
                (void**)&d3d12Resource
            ), nameof(ID3D12Device.CreatePlacedResource));

            return d3d12Resource;
        }

        private D3D12_RESOURCE_STATES GetD3D12ResourceState() => CpuAccess switch {
            GraphicsResourceCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
            GraphicsResourceCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
            _ => D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
        };
    }
}
