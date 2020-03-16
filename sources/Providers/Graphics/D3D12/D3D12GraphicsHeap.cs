// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_HEAP_TYPE;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsHeap : GraphicsHeap
    {
        private ulong _offset;
        private ValueLazy<Pointer<ID3D12Heap>> _d3d12Heap;
        private State _state;

        internal D3D12GraphicsHeap(D3D12GraphicsDevice graphicsDevice, ulong size, GraphicsHeapCpuAccess cpuAccess)
            : base(graphicsDevice, size, cpuAccess)
        {
            _d3d12Heap = new ValueLazy<Pointer<ID3D12Heap>>(CreateD3D12Heap);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsHeap" /> class.</summary>
        ~D3D12GraphicsHeap()
        {
            Dispose(isDisposing: true);
        }

        /// <inheritdoc cref="GraphicsHeap.GraphicsDevice" />
        public D3D12GraphicsDevice D3D12GraphicsDevice => (D3D12GraphicsDevice)GraphicsDevice;

        /// <summary>Gets the underlying <see cref="ID3D12Heap" /> for the heap.</summary>
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Device.CreateHeap(D3D12_HEAP_DESC*, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The heap has been disposed.</exception>
        public ID3D12Heap* D3D12Heap => _d3d12Heap.Value;

        /// <inheritdoc cref="CreateGraphicsBuffer(GraphicsBufferKind, ulong, ulong)" />
        public D3D12GraphicsBuffer CreateD3D12GraphicsBuffer(GraphicsBufferKind kind, ulong size, ulong stride)
        {
            _state.ThrowIfDisposedOrDisposing();

            var offset = _offset;
            size = (size + D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT - 1) & ~(D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT - 1);
            _offset = offset + size;

            return new D3D12GraphicsBuffer(kind, this, offset, size, stride);
        }

        /// <inheritdoc cref="CreateGraphicsTexture(GraphicsTextureKind, ulong, uint, ushort)" />
        public D3D12GraphicsTexture CreateD3D12GraphicsTexture(GraphicsTextureKind kind, ulong width, uint height, ushort depth)
        {
            _state.ThrowIfDisposedOrDisposing();

            var size = width * height * depth * sizeof(uint);
            var offset = _offset;

            size = (size + D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT - 1) & ~(D3D12_DEFAULT_RESOURCE_PLACEMENT_ALIGNMENT - 1);
            _offset = offset + size;

            return new D3D12GraphicsTexture(kind, this, offset, size, width, height, depth);
        }

        /// <inheritdoc />
        public override GraphicsBuffer CreateGraphicsBuffer(GraphicsBufferKind kind, ulong size, ulong stride) => CreateD3D12GraphicsBuffer(kind, size, stride);

        /// <inheritdoc />
        public override GraphicsTexture CreateGraphicsTexture(GraphicsTextureKind kind, ulong width, uint height, ushort depth) => CreateD3D12GraphicsTexture(kind, width, height, depth);

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

        private Pointer<ID3D12Heap> CreateD3D12Heap()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12Heap* d3d12Heap;

            var heapType = CpuAccess switch {
                GraphicsHeapCpuAccess.Read => D3D12_HEAP_TYPE_READBACK,
                GraphicsHeapCpuAccess.Write => D3D12_HEAP_TYPE_UPLOAD,
                _ => D3D12_HEAP_TYPE_DEFAULT,
            };
            
            var heapDesc = new D3D12_HEAP_DESC(Size, heapType);
            var iid = IID_ID3D12Heap;

            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateHeap), D3D12GraphicsDevice.D3D12Device->CreateHeap(
                &heapDesc,
                &iid,
                (void**)&d3d12Heap
            ));

            return d3d12Heap;
        }
    }
}
