// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12_RESOURCE_STATES;
using static TerraFX.Interop.D3D12_SRV_DIMENSION;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsTexture : GraphicsTexture
    {
        private ValueLazy<Pointer<ID3D12Resource>> _d3d12Resource;
        private ValueLazy<D3D12_RESOURCE_STATES> _d3d12ResourceState;
        private State _state;

        internal D3D12GraphicsTexture(GraphicsTextureKind kind, D3D12GraphicsHeap graphicsHeap, ulong offset, ulong size, ulong width, uint height, ushort depth)
            : base(kind, graphicsHeap, offset, size, width, height, depth)
        {
            _d3d12Resource = new ValueLazy<Pointer<ID3D12Resource>>(CreateD3D12Resource);
            _d3d12ResourceState = new ValueLazy<D3D12_RESOURCE_STATES>(GetD3D12ResourceState);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsTexture" /> class.</summary>
        ~D3D12GraphicsTexture()
        {
            Dispose(isDisposing: true);
        }

        /// <inheritdoc cref="GraphicsResource.GraphicsHeap" />
        public D3D12GraphicsHeap D3D12GraphicsHeap => (D3D12GraphicsHeap)GraphicsHeap;

        /// <summary>Gets the underlying <see cref="ID3D12Resource" /> for the texture.</summary>
        /// <exception cref="ExternalException">The call to <see cref="ID3D12Device.CreateCommittedResource(D3D12_HEAP_PROPERTIES*, D3D12_HEAP_FLAGS, D3D12_RESOURCE_DESC*, D3D12_RESOURCE_STATES, D3D12_CLEAR_VALUE*, Guid*, void**)" /> failed.</exception>
        /// <exception cref="ObjectDisposedException">The buffer has been disposed.</exception>
        public ID3D12Resource* D3D12Resource => _d3d12Resource.Value;

        /// <summary>Gets the default state of the underlying <see cref="ID3D12Resource" /> for the texture.</summary>
        public D3D12_RESOURCE_STATES D3D12ResourceState => _d3d12ResourceState.Value;

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

            var textureDesc = Kind switch {
                GraphicsTextureKind.OneDimensional => D3D12_RESOURCE_DESC.Tex1D(DXGI_FORMAT_R8G8B8A8_UNORM, Width, mipLevels: 1),
                GraphicsTextureKind.TwoDimensional => D3D12_RESOURCE_DESC.Tex2D(DXGI_FORMAT_R8G8B8A8_UNORM, Width, Height, mipLevels: 1),
                GraphicsTextureKind.ThreeDimensional => D3D12_RESOURCE_DESC.Tex3D(DXGI_FORMAT_R8G8B8A8_UNORM, Width, Height, Depth, mipLevels: 1),
                _ => default,
            };
            var iid = IID_ID3D12Resource;

            var d3d12GraphicsHeap = D3D12GraphicsHeap;
            var d3d12Heap = d3d12GraphicsHeap.D3D12Heap;

            var d3d12GraphicsDevice = d3d12GraphicsHeap.D3D12GraphicsDevice;
            var d3d12Device = d3d12GraphicsDevice.D3D12Device;

            ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreatePlacedResource), d3d12Device->CreatePlacedResource(
                d3d12Heap,
                Offset,
                &textureDesc,
                D3D12ResourceState,
                pOptimizedClearValue: null,
                &iid,
                (void**)&d3d12Resource
            ));

            var shaderResourceViewDesc = new D3D12_SHADER_RESOURCE_VIEW_DESC {
                Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                ViewDimension = D3D12_SRV_DIMENSION_TEXTURE2D,
                Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING,
            };
            shaderResourceViewDesc.Anonymous.Texture2D.MipLevels = 1;

            d3d12Device->CreateShaderResourceView(d3d12Resource, &shaderResourceViewDesc, d3d12GraphicsDevice.D3D12ShaderResourceDescriptorHeap->GetCPUDescriptorHandleForHeapStart());

            return d3d12Resource;
        }

        private D3D12_RESOURCE_STATES GetD3D12ResourceState() => D3D12GraphicsHeap.CpuAccess switch {
            GraphicsHeapCpuAccess.Read => D3D12_RESOURCE_STATE_COPY_DEST,
            GraphicsHeapCpuAccess.Write => D3D12_RESOURCE_STATE_GENERIC_READ,
            _ => D3D12_RESOURCE_STATE_NON_PIXEL_SHADER_RESOURCE | D3D12_RESOURCE_STATE_PIXEL_SHADER_RESOURCE,
        };
    }
}
