// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsBuffer : GraphicsBuffer
    {
        private ValueLazy<Pointer<ID3D12Resource>> _d3d12Resource;
        private ValueLazy<D3D12_RESOURCE_STATES> _d3d12ResourceState;
        private State _state;

        internal D3D12GraphicsBuffer(GraphicsBufferKind kind, D3D12GraphicsHeap graphicsHeap, ulong offset, ulong size, ulong stride)
            : base(kind, graphicsHeap, offset, size, stride)
        {
            _d3d12Resource = new ValueLazy<Pointer<ID3D12Resource>>(CreateD3D12Resource);
            _d3d12ResourceState = new ValueLazy<D3D12_RESOURCE_STATES>(GetD3D12ResourceState);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsBuffer" /> class.</summary>
        ~D3D12GraphicsBuffer()
        {
            Dispose(isDisposing: true);
        }

        /// <inheritdoc cref="GraphicsResource.GraphicsHeap" />
        public D3D12GraphicsHeap D3D12GraphicsHeap => (D3D12GraphicsHeap)GraphicsHeap;

        /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Device.CreateCommittedResource(D3D12_HEAP_PROPERTIES*, D3D12_HEAP_FLAGS, D3D12_RESOURCE_DESC*, D3D12_RESOURCE_STATES, D3D12_CLEAR_VALUE*, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public ID3D12Resource* D3D12Resource => _d3d12Resource.Value;

        /// <summary>Gets the default state of the underlying <see cref="ID3D12Resource" /> for the buffer.</summary>
        public D3D12_RESOURCE_STATES D3D12ResourceState => _d3d12ResourceState.Value;

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Resource.Map(uint, D3D12_RANGE*, void**)" /> failed.</exception>
        public override T* Map<T>(UIntPtr readRangeOffset, UIntPtr readRangeLength)
        {
            var readRange = new D3D12_RANGE {
                Begin = readRangeOffset,
                End = (UIntPtr)((ulong)readRangeOffset + (ulong)readRangeLength),
            };

            void* pDestination;
            ThrowExternalExceptionIfFailed(nameof(ID3D12Resource.Map), D3D12Resource->Map(Subresource: 0, &readRange, &pDestination));

            return (T*)pDestination;
        }

        /// <inheritdoc />
        public override void Unmap(UIntPtr writtenRangeOffset, UIntPtr writtenRangeLength)
        {
            var writtenRange = new D3D12_RANGE {
                Begin = writtenRangeOffset,
                End = (UIntPtr)((ulong)writtenRangeOffset + (ulong)writtenRangeLength),
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
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12Resource> CreateD3D12Resource()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Resource* d3d12Resource;

            var bufferDesc = D3D12_RESOURCE_DESC.Buffer(width: Size);
            var iid = IID_ID3D12Resource;

            var d3d12GraphicsHeap = D3D12GraphicsHeap;
            var d3d12Heap = d3d12GraphicsHeap.D3D12Heap;

            var d3d12GraphicsDevice = d3d12GraphicsHeap.D3D12GraphicsDevice;
            var d3d12Device = d3d12GraphicsDevice.D3D12Device;

            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreatePlacedResource), d3d12Device->CreatePlacedResource(
                d3d12Heap,
                Offset,
                &bufferDesc,
                D3D12ResourceState,
                pOptimizedClearValue: null,
                &iid,
                (void**)&d3d12Resource
            ));

            return d3d12Resource;
        }

        private D3D12_RESOURCE_STATES GetD3D12ResourceState() => D3D12GraphicsHeap.CpuAccess switch {
            GraphicsHeapCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
            GraphicsHeapCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
            _ => Kind switch {
                GraphicsBufferKind.Vertex => D3D12_RESOURCE_STATE_VERTEX_AND_CONSTANT_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                GraphicsBufferKind.Index => D3D12_RESOURCE_STATE_INDEX_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                GraphicsBufferKind.Constant => D3D12_RESOURCE_STATE_VERTEX_AND_CONSTANT_BUFFER | D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
                _ => default,
            },
        };
    }
}
