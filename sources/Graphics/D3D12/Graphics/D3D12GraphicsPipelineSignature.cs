// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Interop.DirectX;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_RANGE_TYPE;
using static TerraFX.Interop.DirectX.D3D12_FEATURE;
using static TerraFX.Interop.DirectX.D3D12_ROOT_SIGNATURE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_SHADER_VISIBILITY;
using static TerraFX.Interop.DirectX.D3D_ROOT_SIGNATURE_VERSION;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPipelineSignature : GraphicsPipelineSignature
{
    private readonly ID3D12RootSignature* _d3d12RootSignature;

    internal D3D12GraphicsPipelineSignature(D3D12GraphicsDevice device, in GraphicsPipelineSignatureCreateOptions createOptions) : base(device)
    {
        device.AddPipelineSignature(this);

        if (createOptions.TakeInputsOwnership)
        {
            PipelineSignatureInfo.Inputs = createOptions.Inputs;
        }
        else
        {
            var inputs = createOptions.Inputs;
            PipelineSignatureInfo.Inputs = new UnmanagedArray<GraphicsPipelineInput>(inputs.Length);
            inputs.CopyTo(PipelineSignatureInfo.Inputs);
        }

        if (createOptions.TakeResourcesOwnership)
        {
            PipelineSignatureInfo.Resources = createOptions.Resources;
        }
        else
        {
            var resources = createOptions.Resources;
            PipelineSignatureInfo.Resources = new UnmanagedArray<GraphicsPipelineResource>(resources.Length);
            resources.CopyTo(PipelineSignatureInfo.Resources);
        }

        _d3d12RootSignature = CreateD3D12RootSignature(device, in createOptions);

        SetNameUnsafe(Name);

        static ID3D12RootSignature* CreateD3D12RootSignature(D3D12GraphicsDevice device, in GraphicsPipelineSignatureCreateOptions createOptions)
        {
            ID3D12RootSignature* d3d12RootSignature = null;

            var d3d12Device = device.D3D12Device;

            var d3d12FeatureDataRootSignature = new D3D12_FEATURE_DATA_ROOT_SIGNATURE {
                HighestVersion = D3D_ROOT_SIGNATURE_VERSION_1_1,
            };

            if (d3d12Device->CheckFeatureSupport(D3D12_FEATURE_ROOT_SIGNATURE, &d3d12FeatureDataRootSignature, SizeOf<D3D12_FEATURE_DATA_ROOT_SIGNATURE>()).FAILED)
            {
                d3d12FeatureDataRootSignature.HighestVersion = D3D_ROOT_SIGNATURE_VERSION_1_0;
            }

            var d3d12DescriptorRanges = UnmanagedArray<D3D12_DESCRIPTOR_RANGE1>.Empty;
            var d3d12RootParameters = UnmanagedArray<D3D12_ROOT_PARAMETER1>.Empty;
            var d3d12StaticSamplerDescs = UnmanagedArray<D3D12_STATIC_SAMPLER_DESC>.Empty;

            var resources = createOptions.Resources;

            if (resources.Length != 0)
            {
                var d3d12DescriptorRangeCount = 0u;
                var d3d12RootParameterCount = 0u;
                var d3d12StaticSamplerDescCount = 0u;

                for (nuint index = 0; index < resources.Length; index++)
                {
                    ref readonly var resource = ref resources[index];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            d3d12DescriptorRangeCount++;
                            d3d12RootParameterCount++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            d3d12DescriptorRangeCount++;
                            d3d12RootParameterCount++;
                            d3d12StaticSamplerDescCount++;
                            break;
                        }

                        default:
                        {
                            ThrowForInvalidKind(resource.Kind);
                            break;
                        }
                    }
                }

                d3d12DescriptorRanges = new UnmanagedArray<D3D12_DESCRIPTOR_RANGE1>(d3d12DescriptorRangeCount);
                d3d12RootParameters = new UnmanagedArray<D3D12_ROOT_PARAMETER1>(d3d12RootParameterCount);
                d3d12StaticSamplerDescs = new UnmanagedArray<D3D12_STATIC_SAMPLER_DESC>(d3d12StaticSamplerDescCount);

                var d3d12DescriptorRangeIndex = 0u;
                var d3d12RootParametersIndex = 0u;
                var d3d12StaticSamplerDescsIndex = 0u;

                for (nuint index = 0; index < resources.Length; index++)
                {
                    ref readonly var resource = ref resources[index];

                    switch (resource.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            d3d12DescriptorRanges[d3d12DescriptorRangeIndex].Init(
                                D3D12_DESCRIPTOR_RANGE_TYPE_CBV,
                                numDescriptors: 1,
                                baseShaderRegister: resource.BindingIndex
                            );

                            d3d12RootParameters[d3d12RootParametersIndex].InitAsDescriptorTable(
                                numDescriptorRanges: 1,
                                d3d12DescriptorRanges.GetPointerUnsafe(d3d12DescriptorRangeIndex),
                                GetD3D12ShaderVisiblity(resource.ShaderVisibility)
                            );

                            d3d12DescriptorRangeIndex++;
                            d3d12RootParametersIndex++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            var shaderVisibility = GetD3D12ShaderVisiblity(resource.ShaderVisibility);

                            d3d12DescriptorRanges[d3d12DescriptorRangeIndex].Init(
                                D3D12_DESCRIPTOR_RANGE_TYPE_SRV,
                                numDescriptors: 1,
                                baseShaderRegister: resource.BindingIndex
                            );

                            d3d12RootParameters[d3d12RootParametersIndex].InitAsDescriptorTable(
                                numDescriptorRanges: 1,
                                d3d12DescriptorRanges.GetPointerUnsafe(d3d12DescriptorRangeIndex),
                                shaderVisibility
                            );

                            d3d12StaticSamplerDescs[d3d12StaticSamplerDescsIndex].Init(
                                shaderRegister: resource.BindingIndex,
                                shaderVisibility: shaderVisibility
                            );

                            d3d12DescriptorRangeIndex++;
                            d3d12RootParametersIndex++;
                            d3d12StaticSamplerDescsIndex++;
                            break;
                        }

                        default:
                        {
                            ThrowForInvalidKind(resource.Kind);
                            break;
                        }
                    }
                }
            }

            D3D12_VERSIONED_ROOT_SIGNATURE_DESC.Init_1_1(
                out var d3d12VersionedRootSignatureDesc,
                (uint)d3d12RootParameters.Length,
                d3d12RootParameters.GetPointerUnsafe(0),
                (uint)d3d12StaticSamplerDescs.Length,
                d3d12StaticSamplerDescs.GetPointerUnsafe(0),
                D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT
            );

            ID3DBlob* d3dRootSignatureBlob;
            ID3DBlob* d3dRootSignatureErrorBlob;

            ThrowExternalExceptionIfFailed(D3D12SerializeVersionedRootSignature(&d3d12VersionedRootSignatureDesc, d3d12FeatureDataRootSignature.HighestVersion, &d3dRootSignatureBlob, &d3dRootSignatureErrorBlob));

            var result = d3d12Device->CreateRootSignature(0, d3dRootSignatureBlob->GetBufferPointer(), d3dRootSignatureBlob->GetBufferSize(), __uuidof<ID3D12RootSignature>(), (void**)&d3d12RootSignature);

            d3d12DescriptorRanges.Dispose();
            d3d12RootParameters.Dispose();
            d3d12StaticSamplerDescs.Dispose();

            ReleaseIfNotNull(d3dRootSignatureBlob);
            ReleaseIfNotNull(d3dRootSignatureErrorBlob);

            if (result.FAILED)
            {
                ThrowExternalException(nameof(ID3D12Device.CreateRootSignature), result);
            }

            return d3d12RootSignature;
        }

        static D3D12_SHADER_VISIBILITY GetD3D12ShaderVisiblity(GraphicsShaderVisibility shaderVisibility)
        {
            D3D12_SHADER_VISIBILITY d3d12ShaderVisibility = 0;

            if (shaderVisibility == GraphicsShaderVisibility.All)
            {
                d3d12ShaderVisibility = D3D12_SHADER_VISIBILITY_ALL;
            }
            else
            {
                if (shaderVisibility.HasFlag(GraphicsShaderVisibility.Vertex))
                {
                    d3d12ShaderVisibility |= D3D12_SHADER_VISIBILITY_VERTEX;
                }

                if (shaderVisibility.HasFlag(GraphicsShaderVisibility.Pixel))
                {
                    d3d12ShaderVisibility |= D3D12_SHADER_VISIBILITY_PIXEL;
                }
            }

            return d3d12ShaderVisibility;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipelineSignature" /> class.</summary>
    ~D3D12GraphicsPipelineSignature() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12RootSignature" /> for the pipeline.</summary>
    public ID3D12RootSignature* D3D12RootSignature => _d3d12RootSignature;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        ReleaseIfNotNull(_d3d12RootSignature);

        _ = Device.RemovePipelineSignature(this);
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12RootSignature->SetD3D12Name(value);
    }
}
