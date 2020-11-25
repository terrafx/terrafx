// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.D3D12_SRV_DIMENSION;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsPrimitive : GraphicsPrimitive
    {
        private ValueLazy<Pointer<ID3D12DescriptorHeap>> _d3d12CbvSrvUavDescriptorHeap;
        private State _state;

        internal D3D12GraphicsPrimitive(D3D12GraphicsDevice device, D3D12GraphicsPipeline pipeline, in GraphicsBufferView vertexBufferView, in GraphicsBufferView indexBufferView, ReadOnlySpan<GraphicsResource> inputResources)
            : base(device, pipeline, in vertexBufferView, in indexBufferView, inputResources)
        {
            _d3d12CbvSrvUavDescriptorHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateD3D12CbvSrvUavDescriptorHeap);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPrimitive" /> class.</summary>
        ~D3D12GraphicsPrimitive() => Dispose(isDisposing: false);

        /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the primitive for constant buffer, shader resource, and unordered access views.</summary>
        /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
        public ID3D12DescriptorHeap* D3D12CbvSrvUavDescriptorHeap => _d3d12CbvSrvUavDescriptorHeap.Value;

        /// <inheritdoc cref="GraphicsPrimitive.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

        /// <inheritdoc cref="GraphicsPrimitive.Pipeline" />
        public new D3D12GraphicsPipeline Pipeline => (D3D12GraphicsPipeline)base.Pipeline;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _d3d12CbvSrvUavDescriptorHeap.Dispose(ReleaseIfNotNull);

                Pipeline?.Dispose();

                foreach (var inputResource in InputResources)
                {
                    inputResource?.Dispose();
                }

                VertexBufferView.Buffer?.Dispose();
                IndexBufferView.Buffer?.Dispose();
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12DescriptorHeap> CreateD3D12CbvSrvUavDescriptorHeap()
        {
            _state.ThrowIfDisposedOrDisposing();

            var d3d12Device = Device.D3D12Device;
            var inputResources = InputResources;
            var inputResourcesLength = inputResources.Length;
            var numCbvSrvUavDescriptors = 0u;

            for (var index = 0; index < inputResourcesLength; index++)
            {
                var inputResource = inputResources[index];

                if (inputResource is not D3D12GraphicsTexture)
                {
                    continue;
                }

                numCbvSrvUavDescriptors++;
            }

            ID3D12DescriptorHeap* cbvSrvUavDescriptorHeap;

            var cbvSrvUavDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV,
                NumDescriptors = numCbvSrvUavDescriptors,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE,
            };

            var iid = IID_ID3D12DescriptorHeap;
            ThrowExternalExceptionIfFailed(d3d12Device->CreateDescriptorHeap(&cbvSrvUavDescriptorHeapDesc, &iid, (void**)&cbvSrvUavDescriptorHeap), nameof(ID3D12Device.CreateDescriptorHeap));

            var cbvSrvUavDescriptorHandleIncrementSize = Device.CbvSrvUavDescriptorHandleIncrementSize;
            var cbvSrvUavDescriptorIndex = 0;

            for (var index = 0; index < inputResourcesLength; index++)
            {
                var inputResource = inputResources[index];

                if (inputResource is not D3D12GraphicsTexture d3d12GraphicsTexture)
                {
                    continue;
                }

                var shaderResourceViewDesc = new D3D12_SHADER_RESOURCE_VIEW_DESC {
                    Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                    ViewDimension = D3D12_SRV_DIMENSION_UNKNOWN,
                    Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING,
                };

                switch (d3d12GraphicsTexture.Kind)
                {
                    case GraphicsTextureKind.OneDimensional:
                    {
                        shaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE1D;
                        shaderResourceViewDesc.Texture1D.MipLevels = 1;
                        break;
                    }

                    case GraphicsTextureKind.TwoDimensional:
                    {
                        shaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE2D;
                        shaderResourceViewDesc.Texture2D.MipLevels = 1;
                        break;
                    }

                    case GraphicsTextureKind.ThreeDimensional:
                    {
                        shaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE3D;
                        shaderResourceViewDesc.Texture3D.MipLevels = 1;
                        break;
                    }
                }

                var cpuDescriptorHandleForHeapStart = cbvSrvUavDescriptorHeap->GetCPUDescriptorHandleForHeapStart();
                d3d12Device->CreateShaderResourceView(d3d12GraphicsTexture.D3D12Resource, &shaderResourceViewDesc, cpuDescriptorHandleForHeapStart.Offset(cbvSrvUavDescriptorIndex, cbvSrvUavDescriptorHandleIncrementSize));
                cbvSrvUavDescriptorIndex++;
            }

            return cbvSrvUavDescriptorHeap;
        }
    }
}
