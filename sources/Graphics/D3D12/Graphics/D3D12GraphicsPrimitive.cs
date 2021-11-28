// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_SRV_DIMENSION;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPrimitive : GraphicsPrimitive
{
    private ValueLazy<Pointer<ID3D12DescriptorHeap>> _d3d12CbvSrvUavDescriptorHeap;
    private VolatileState _state;

    internal D3D12GraphicsPrimitive(D3D12GraphicsDevice device, D3D12GraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView, ReadOnlySpan<GraphicsResourceView> inputResourceViews)
        : base(device, pipeline, in vertexBufferView, in indexBufferView, inputResourceViews)
    {
        _d3d12CbvSrvUavDescriptorHeap = new ValueLazy<Pointer<ID3D12DescriptorHeap>>(CreateD3D12CbvSrvUavDescriptorHeap);
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPrimitive" /> class.</summary>
    ~D3D12GraphicsPrimitive() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the primitive for constant buffer, shader resource, and unordered access views.</summary>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public ID3D12DescriptorHeap* D3D12CbvSrvUavDescriptorHeap => _d3d12CbvSrvUavDescriptorHeap.Value;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
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

            // TODO: The primitive shouldn't dispose the collections, it
            // should be freeing the region and something else should control
            // resource disposal.

            foreach (var inputResourceRegion in InputResourceViews)
            {
                inputResourceRegion.Resource?.Dispose();
            }

            VertexBufferView.Resource?.Dispose();
            IndexBufferView.Resource?.Dispose();
        }

        _state.EndDispose();
    }

    private Pointer<ID3D12DescriptorHeap> CreateD3D12CbvSrvUavDescriptorHeap()
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsPrimitive));

        var d3d12Device = Device.D3D12Device;
        var inputResourceRegions = InputResourceViews;
        var inputResourceRegionsLength = inputResourceRegions.Length;
        var numCbvSrvUavDescriptors = 0u;

        for (var index = 0; index < inputResourceRegionsLength; index++)
        {
            var inputResource = inputResourceRegions[index];

            if (inputResource.Resource is not D3D12GraphicsTexture)
            {
                continue;
            }

            numCbvSrvUavDescriptors++;
        }

        ID3D12DescriptorHeap* cbvSrvUavDescriptorHeap;

        var cbvSrvUavDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
            Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV,
            NumDescriptors = Math.Max(1, numCbvSrvUavDescriptors),
            Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE,
        };
        ThrowExternalExceptionIfFailed(d3d12Device->CreateDescriptorHeap(&cbvSrvUavDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&cbvSrvUavDescriptorHeap));

        var cbvSrvUavDescriptorHandleIncrementSize = Device.D3D12CbvSrvUavDescriptorHandleIncrementSize;
        var cbvSrvUavDescriptorIndex = 0;

        for (var index = 0; index < inputResourceRegionsLength; index++)
        {
            var inputResourceRegion = inputResourceRegions[index];

            if (inputResourceRegion.Resource is not D3D12GraphicsTexture graphicsTexture)
            {
                continue;
            }

            var shaderResourceViewDesc = new D3D12_SHADER_RESOURCE_VIEW_DESC {
                Format = DXGI_FORMAT_R8G8B8A8_UNORM,
                ViewDimension = D3D12_SRV_DIMENSION_UNKNOWN,
                Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING,
            };

            switch (graphicsTexture.Kind)
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
            d3d12Device->CreateShaderResourceView(graphicsTexture.D3D12Resource, &shaderResourceViewDesc, cpuDescriptorHandleForHeapStart.Offset(cbvSrvUavDescriptorIndex, cbvSrvUavDescriptorHandleIncrementSize));
            cbvSrvUavDescriptorIndex++;
        }

        return cbvSrvUavDescriptorHeap;
    }
}
