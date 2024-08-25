// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_HEAP_TYPE;
using static TerraFX.Interop.DirectX.D3D12_SRV_DIMENSION;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A set of descriptors for a graphics pipeline.</summary>
public sealed unsafe class GraphicsPipelineDescriptorSet : GraphicsPipelineObject
{
    private ComPtr<ID3D12DescriptorHeap> _d3d12CbvSrvUavDescriptorHeap;
    private readonly uint _d3d12CbvSrvUavDescriptorHeapVersion;

    private ComPtr<ID3D12DescriptorHeap> _d3d12SamplerDescriptorHeap;
    private readonly uint _d3d12SamplerDescriptorHeapVersion;

    private readonly GraphicsResourceView[] _resourceViews;

    internal GraphicsPipelineDescriptorSet(GraphicsPipeline pipeline, in GraphicsPipelineDescriptorSetCreateOptions createOptions) : base(pipeline)
    {
        if (createOptions.TakeResourceViewsOwnership)
        {
            _resourceViews = createOptions.ResourceViews;
        }
        else
        {
            var resourceViews = createOptions.ResourceViews;
            _resourceViews = new GraphicsResourceView[resourceViews.Length];
            resourceViews.CopyTo(_resourceViews, 0);
        }

        var d3d12CbvSrvUavDescriptorHeap = CreateD3D12CbvSrvUavDescriptorHeap(out _d3d12CbvSrvUavDescriptorHeapVersion);
        _d3d12CbvSrvUavDescriptorHeap.Attach(d3d12CbvSrvUavDescriptorHeap);

        var d3d12SamplerDescriptorHeap = CreateD3D12SamplerDescriptorHeap(out _d3d12SamplerDescriptorHeapVersion);
        _d3d12SamplerDescriptorHeap.Attach(d3d12SamplerDescriptorHeap);

        SetNameUnsafe(Name);

        ID3D12DescriptorHeap* CreateD3D12CbvSrvUavDescriptorHeap(out uint d3d12CbvSrvUavDescriptorHeapVersion)
        {
            var device = pipeline.Device;
            var d3d12Device = device.D3D12Device;

            var d3d12CbvDescriptorCount = 0u;
            var d3d12SrvDescriptorCount = 0u;
            var d3d12UavDescriptorCount = 0u;

            var resourceViews = _resourceViews;

            for (var index = 0; index < resourceViews.Length; index++)
            {
                var resourceView = resourceViews[index];

                switch (resourceView.Kind)
                {
                    case GraphicsResourceKind.Buffer:
                    {
                        d3d12CbvDescriptorCount++;
                        break;
                    }

                    case GraphicsResourceKind.Texture:
                    {
                        d3d12SrvDescriptorCount++;
                        break;
                    }

                    default:
                    case GraphicsResourceKind.Unknown:
                    {
                        ThrowForInvalidKind(resourceView.Kind);
                        break;
                    }
                }
            }

            ID3D12DescriptorHeap* d3d12CbvSrvUavDescriptorHeap;

            var d3d12CbvSrvUavDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_CBV_SRV_UAV,
                NumDescriptors = d3d12CbvDescriptorCount + d3d12SrvDescriptorCount + d3d12UavDescriptorCount,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_SHADER_VISIBLE,
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(d3d12Device->CreateDescriptorHeap(&d3d12CbvSrvUavDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12CbvSrvUavDescriptorHeap));

            var d3d12CbvSrvUavDescriptorHandle = d3d12CbvSrvUavDescriptorHeap->GetCPUDescriptorHandleForHeapStart();
            var d3d12CbvSrvUavDescriptorHandleIncrementSize = device.D3D12CbvSrvUavDescriptorSize;

            for (var index = 0; index < resourceViews.Length; index++)
            {
                var resourceView = resourceViews[index];

                switch (resourceView.Kind)
                {
                    case GraphicsResourceKind.Buffer:
                    {
                        var bufferView = resourceView.As<GraphicsBufferView>();

                        var d3d12ConstantBufferViewDesc = new D3D12_CONSTANT_BUFFER_VIEW_DESC {
                            BufferLocation = bufferView.D3D12GpuVirtualAddress,
                            SizeInBytes = (uint)bufferView.ByteLength,
                        };

                        d3d12Device->CreateConstantBufferView(&d3d12ConstantBufferViewDesc, d3d12CbvSrvUavDescriptorHandle);
                        break;
                    }

                    case GraphicsResourceKind.Texture:
                    {
                        var textureView = resourceView.As<GraphicsTextureView>();

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
                            case GraphicsTextureKind.Unknown:
                            {
                                ThrowForInvalidKind(textureView.Kind);
                                break;
                            }
                        }

                        d3d12Device->CreateShaderResourceView(textureView.Resource.D3D12Resource, &d3d12ShaderResourceViewDesc, d3d12CbvSrvUavDescriptorHandle);
                        break;
                    }

                    default:
                    case GraphicsResourceKind.Unknown:
                    {
                        ThrowForInvalidKind(resourceView.Kind);
                        break;
                    }
                }

                _ = d3d12CbvSrvUavDescriptorHandle.Offset(1, d3d12CbvSrvUavDescriptorHandleIncrementSize);
            }

            return GetLatestD3D12DescriptorHeap(d3d12CbvSrvUavDescriptorHeap, out d3d12CbvSrvUavDescriptorHeapVersion);
        }

        ID3D12DescriptorHeap* CreateD3D12SamplerDescriptorHeap(out uint d3d12SamplerDescriptorHeapVersion)
        {
            ID3D12DescriptorHeap* d3d12SamplerDescriptorHeap;

            var d3d12SamplerDescriptorHeapDesc = new D3D12_DESCRIPTOR_HEAP_DESC {
                Type = D3D12_DESCRIPTOR_HEAP_TYPE_SAMPLER,
                NumDescriptors = 2,
                Flags = D3D12_DESCRIPTOR_HEAP_FLAG_NONE,
                NodeMask = 0,
            };
            ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateDescriptorHeap(&d3d12SamplerDescriptorHeapDesc, __uuidof<ID3D12DescriptorHeap>(), (void**)&d3d12SamplerDescriptorHeap));

            return GetLatestD3D12DescriptorHeap(d3d12SamplerDescriptorHeap, out d3d12SamplerDescriptorHeapVersion);
        }
    }

    /// <summary>Finalizes an instance of the <see cref="GraphicsPipelineDescriptorSet" /> class.</summary>
    ~GraphicsPipelineDescriptorSet() => Dispose(isDisposing: false);

    /// <summary>Gets the resource views for the pipeline descriptor.</summary>
    public ReadOnlySpan<GraphicsResourceView> ResourceViews => _resourceViews;

    internal ID3D12DescriptorHeap* D3D12CbvSrvUavDescriptorHeap => _d3d12CbvSrvUavDescriptorHeap;

    internal uint D3D12CbvSrvUavDescriptorHeapVersion => _d3d12CbvSrvUavDescriptorHeapVersion;

    internal ID3D12DescriptorHeap* D3D12SamplerDescriptorHeap => _d3d12SamplerDescriptorHeap;

    internal uint D3D12SamplerDescriptorHeapVersion => _d3d12SamplerDescriptorHeapVersion;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        _ = _d3d12CbvSrvUavDescriptorHeap.Reset();
        _ = _d3d12SamplerDescriptorHeap.Reset();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12CbvSrvUavDescriptorHeap->SetD3D12Name(value);
        D3D12SamplerDescriptorHeap->SetD3D12Name(value);
    }
}
