// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_SRV_DIMENSION;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPipelineResourceViewSet : GraphicsPipelineResourceViewSet
{
    private readonly ID3D12DescriptorHeap* _d3d12CbvSrvUavDescriptorHeap;

    internal D3D12GraphicsPipelineResourceViewSet(D3D12GraphicsPipeline pipeline, ReadOnlySpan<GraphicsResourceView> resourceViews)
        : base(pipeline, resourceViews)
    {
        _d3d12CbvSrvUavDescriptorHeap = CreateD3D12CbvSrvUavDescriptorHeap(pipeline, resourceViews);

        static ID3D12DescriptorHeap* CreateD3D12CbvSrvUavDescriptorHeap(D3D12GraphicsPipeline pipeline, ReadOnlySpan<GraphicsResourceView> resourceViews)
        {
            var device = pipeline.Device;
            var d3d12Device = device.D3D12Device;

            var d3d12CbvSrvUavDescriptorCount = 0u;

            for (var index = 0; index < resourceViews.Length; index++)
            {
                var inputResourceView = resourceViews[index];

                if (inputResourceView is not D3D12GraphicsTextureView)
                {
                    continue;
                }

                d3d12CbvSrvUavDescriptorCount++;
            }

            ID3D12DescriptorHeap* d3d12CbvSrvUavDescriptorHeap;

            var d3d12CbvSrvUavDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV,
                NumDescriptors = Math.Max(1, d3d12CbvSrvUavDescriptorCount),
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE,
            };
            ThrowExternalExceptionIfFailed(d3d12Device->CreateDescriptorHeap(&d3d12CbvSrvUavDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12CbvSrvUavDescriptorHeap));

            var d3d12CbvSrvUavDescriptorHandleIncrementSize = device.D3D12CbvSrvUavDescriptorHandleIncrementSize;
            var d3d12CbvSrvUavDescriptorIndex = 0;

            for (var index = 0; index < resourceViews.Length; index++)
            {
                var inputResourceView = resourceViews[index];

                if (inputResourceView is not D3D12GraphicsTextureView graphicsTextureView)
                {
                    continue;
                }

                var d3d12ShaderResourceViewDesc = new D3D12_SHADER_RESOURCE_VIEW_DESC {
                    Format = graphicsTextureView.Format.AsDxgiFormat(),
                    ViewDimension = D3D12_SRV_DIMENSION_UNKNOWN,
                    Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING,
                };

                switch (graphicsTextureView.Kind)
                {
                    case GraphicsTextureKind.OneDimensional:
                    {
                        d3d12ShaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE1D;
                        d3d12ShaderResourceViewDesc.Texture1D.MostDetailedMip = graphicsTextureView.MipLevelIndex;
                        d3d12ShaderResourceViewDesc.Texture1D.MipLevels = graphicsTextureView.MipLevelCount;
                        break;
                    }

                    case GraphicsTextureKind.TwoDimensional:
                    {
                        d3d12ShaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE2D;
                        d3d12ShaderResourceViewDesc.Texture2D.MostDetailedMip = graphicsTextureView.MipLevelIndex;
                        d3d12ShaderResourceViewDesc.Texture2D.MipLevels = graphicsTextureView.MipLevelCount;
                        break;
                    }

                    case GraphicsTextureKind.ThreeDimensional:
                    {
                        d3d12ShaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE3D;
                        d3d12ShaderResourceViewDesc.Texture3D.MostDetailedMip = graphicsTextureView.MipLevelIndex;
                        d3d12ShaderResourceViewDesc.Texture3D.MipLevels = graphicsTextureView.MipLevelCount;
                        break;
                    }
                }

                var d3d12CbvSrvUavDescriptorHeapStart = d3d12CbvSrvUavDescriptorHeap->GetCPUDescriptorHandleForHeapStart();
                d3d12Device->CreateShaderResourceView(graphicsTextureView.Resource.D3D12Resource, &d3d12ShaderResourceViewDesc, d3d12CbvSrvUavDescriptorHeapStart.Offset(d3d12CbvSrvUavDescriptorIndex, d3d12CbvSrvUavDescriptorHandleIncrementSize));
                d3d12CbvSrvUavDescriptorIndex++;
            }

            return d3d12CbvSrvUavDescriptorHeap;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipelineResourceViewSet" /> class.</summary>
    ~D3D12GraphicsPipelineResourceViewSet() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the resource view set for constant buffer, shader resource, and unordered access views.</summary>
    public ID3D12DescriptorHeap* D3D12CbvSrvUavDescriptorHeap
    {
        get
        {
            AssertNotDisposed();
            return _d3d12CbvSrvUavDescriptorHeap;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsPipelineObject.Pipeline" />
    public new D3D12GraphicsPipeline Pipeline => base.Pipeline.As<D3D12GraphicsPipeline>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12CbvSrvUavDescriptorHeap);
    }

    /// <inheritdoc />
    protected override void SetNameInternal(string value)
    {
        D3D12CbvSrvUavDescriptorHeap->SetD3D12Name(value);
    }
}
