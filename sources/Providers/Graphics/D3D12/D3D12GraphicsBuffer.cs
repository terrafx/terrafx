// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12_RESOURCE_FLAGS;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsBuffer : GraphicsBuffer
    {
        private ValueLazy<Pointer<ID3D12Resource>> _d3d12Resource;
        private ValueLazy<D3D12_RESOURCE_STATES> _d3d12ResourceState;

        private State _state;

        internal D3D12GraphicsBuffer(GraphicsBufferKind kind, GraphicsResourceCpuAccess cpuAccess, in GraphicsMemoryBlockRegion memoryBlockRegion)
            : base(kind, cpuAccess, in memoryBlockRegion)
        {
            _d3d12Resource = new ValueLazy<Pointer<ID3D12Resource>>(CreateD3D12Resource);
            _d3d12ResourceState = new ValueLazy<D3D12_RESOURCE_STATES>(GetD3D12ResourceState);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBuffer" /> class.</summary>
        ~D3D12GraphicsBuffer() => Dispose(isDisposing: true);

        /// <inheritdoc cref="GraphicsResource.Allocator" />
        public new D3D12GraphicsMemoryAllocator Allocator => (D3D12GraphicsMemoryAllocator)base.Allocator;

        /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Device.CreateCommittedResource(D3D12_HEAP_PROPERTIES*, D3D12_HEAP_FLAGS, D3D12_RESOURCE_DESC*, D3D12_RESOURCE_STATES, D3D12_CLEAR_VALUE*, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public ID3D12Resource* D3D12Resource => _d3d12Resource.Value;

        /// <summary>Gets the default state of the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
        public D3D12_RESOURCE_STATES D3D12ResourceState => _d3d12ResourceState.Value;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
        public override T* Map<T>(nuint readRangeOffset, nuint readRangeLength)
        {
            var readRange = new D3D12_RANGE {
                Begin = readRangeOffset,
                End = readRangeOffset + readRangeLength,
            };

            void* pDestination;
            ThrowExternalExceptionIfFailed(D3D12Resource->Map(Subresource: 0, &readRange, &pDestination), nameof(ID3D12Resource.Map));

            return (T*)pDestination;
        }

        /// <inheritdoc />
        public override void Unmap(nuint writtenRangeOffset, nuint writtenRangeLength)
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
                MemoryBlockRegion.Block.Free(in MemoryBlockRegion);
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12Resource> CreateD3D12Resource()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Resource* d3d12Resource;

            ref readonly var memoryBlockRegion = ref MemoryBlockRegion;

            var bufferDesc = D3D12_RESOURCE_DESC.Buffer(memoryBlockRegion.Size, D3D12_RESOURCE_FLAG_NONE, Alignment);
            var iid = IID_ID3D12Resource;

            var d3d12Device = Allocator.Device.D3D12Device;
            var d3d12Heap = Block.GetHandle<Pointer<ID3D12Heap>>();

            ThrowExternalExceptionIfFailed(d3d12Device->CreatePlacedResource(
                d3d12Heap,
                memoryBlockRegion.Offset,
                &bufferDesc,
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
            _ => Kind switch {
                GraphicsBufferKind.Vertex => D3D12_RESOURCE_STATE_VERTEX_AND_CONSTANT_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                GraphicsBufferKind.Index => D3D12_RESOURCE_STATE_INDEX_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                GraphicsBufferKind.Constant => D3D12_RESOURCE_STATE_VERTEX_AND_CONSTANT_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                _ => default,
            },
        };
    }
}
