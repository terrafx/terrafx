// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_SRV_DIMENSION;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPipelineDescriptorSet : GraphicsPipelineDescriptorSet
{
    private ID3D12DescriptorHeap* _d3d12CbvSrvUavDescriptorHeap;
    private readonly uint _d3d12CbvSrvUavDescriptorHeapVersion;

    internal D3D12GraphicsPipelineDescriptorSet(D3D12GraphicsPipeline pipeline, in GraphicsPipelineDescriptorSetCreateOptions createOptions) : base(pipeline)
    {
        if (createOptions.TakeResourceViewsOwnership)
        {
            PipelineDescriptorSetInfo.ResourceViews = createOptions.ResourceViews;
        }
        else
        {
            var resourceViews = createOptions.ResourceViews;
            PipelineDescriptorSetInfo.ResourceViews = new GraphicsResourceView[resourceViews.Length];
            resourceViews.CopyTo(PipelineDescriptorSetInfo.ResourceViews, 0);
        }

        _d3d12CbvSrvUavDescriptorHeap = CreateD3D12CbvSrvUavDescriptorHeap(out _d3d12CbvSrvUavDescriptorHeapVersion);

        SetNameUnsafe(Name);

        ID3D12DescriptorHeap* CreateD3D12CbvSrvUavDescriptorHeap(out uint d3d12CbvSrvUavDescriptorHeapVersion)
        {
            var device = pipeline.Device;
            var d3d12Device = device.D3D12Device;

            var d3d12CbvSrvUavDescriptorCount = 0u;

            var resourceViews = PipelineDescriptorSetInfo.ResourceViews;

            for (var index = 0; index < resourceViews.Length; index++)
            {
                var resourceView = resourceViews[index];

                if (resourceView is not D3D12GraphicsTextureView)
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
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(d3d12Device->CreateDescriptorHeap(&d3d12CbvSrvUavDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12CbvSrvUavDescriptorHeap));

            var d3d12CbvSrvUavDescriptorHandleIncrementSize = device.CbvSrvUavDescriptorSize;
            var d3d12CbvSrvUavDescriptorIndex = 0;

            for (var index = 0; index < resourceViews.Length; index++)
            {
                var resourceView = resourceViews[index];

                if (resourceView is not D3D12GraphicsTextureView textureView)
                {
                    continue;
                }

                var d3d12ShaderResourceViewDesc = new D3D12_SHADER_RESOURCE_VIEW_DESC {
                    Format = textureView.PixelFormat.AsDxgiFormat(),
                    ViewDimension = D3D12_SRV_DIMENSION_UNKNOWN,
                    Shader4ComponentMapping = D3D12_DEFAULT_SHADER_4_COMPONENT_MAPPING,
                };

                switch (textureView.Kind)
                {
                    case GraphicsTextureKind.OneDimensional:
                    {
                        d3d12ShaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE1D;
                        d3d12ShaderResourceViewDesc.Texture1D.MostDetailedMip = textureView.MipLevelIndex;
                        d3d12ShaderResourceViewDesc.Texture1D.MipLevels = textureView.MipLevelCount;
                        break;
                    }

                    case GraphicsTextureKind.TwoDimensional:
                    {
                        d3d12ShaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE2D;
                        d3d12ShaderResourceViewDesc.Texture2D.MostDetailedMip = textureView.MipLevelIndex;
                        d3d12ShaderResourceViewDesc.Texture2D.MipLevels = textureView.MipLevelCount;
                        break;
                    }

                    case GraphicsTextureKind.ThreeDimensional:
                    {
                        d3d12ShaderResourceViewDesc.ViewDimension = D3D12_SRV_DIMENSION_TEXTURE3D;
                        d3d12ShaderResourceViewDesc.Texture3D.MostDetailedMip = textureView.MipLevelIndex;
                        d3d12ShaderResourceViewDesc.Texture3D.MipLevels = textureView.MipLevelCount;
                        break;
                    }

                    default:
                    {
                        ThrowForInvalidKind(textureView.Kind);
                        break;
                    }
                }

                var d3d12CbvSrvUavDescriptorHeapStart = d3d12CbvSrvUavDescriptorHeap->GetCPUDescriptorHandleForHeapStart();
                d3d12Device->CreateShaderResourceView(textureView.Resource.D3D12Resource, &d3d12ShaderResourceViewDesc, d3d12CbvSrvUavDescriptorHeapStart.Offset(d3d12CbvSrvUavDescriptorIndex, d3d12CbvSrvUavDescriptorHandleIncrementSize));
                d3d12CbvSrvUavDescriptorIndex++;
            }

            return GetLatestD3D12DescriptorHeap(d3d12CbvSrvUavDescriptorHeap, out d3d12CbvSrvUavDescriptorHeapVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipelineDescriptorSet" /> class.</summary>
    ~D3D12GraphicsPipelineDescriptorSet() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the <see cref="ID3D12DescriptorHeap" /> used by the resource view set for constant buffer, shader resource, and unordered access views.</summary>
    public ID3D12DescriptorHeap* D3D12CbvSrvUavDescriptorHeap => _d3d12CbvSrvUavDescriptorHeap;

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
        _d3d12CbvSrvUavDescriptorHeap = null;
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CbvSrvUavDescriptorHeap->SetD3D12Name(value);
    }
}
