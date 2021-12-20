// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using static TerraFX.Interop.DirectX.D3D12_DESCRIPTOR_RANGE_TYPE;
using static TerraFX.Interop.DirectX.D3D12_ROOT_SIGNATURE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_SHADER_VISIBILITY;
using static TerraFX.Interop.DirectX.D3D_ROOT_SIGNATURE_VERSION;
using static TerraFX.Interop.DirectX.DirectX;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.UnsafeUtilities;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPipelineSignature : GraphicsPipelineSignature
{
    private readonly ID3D12RootSignature* _d3d12RootSignature;

    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsPipelineSignature(D3D12GraphicsDevice device, ReadOnlySpan<GraphicsPipelineInput> inputs, ReadOnlySpan<GraphicsPipelineResourceInfo> resources)
        : base(device, inputs, resources)
    {
        _d3d12RootSignature = CreateD3D12RootSignature(device, resources);

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsPipelineSignature);

        static ID3D12RootSignature* CreateD3D12RootSignature(D3D12GraphicsDevice device, ReadOnlySpan<GraphicsPipelineResourceInfo> resources)
        {
            ID3DBlob* d3dRootSignatureBlob = null;
            ID3DBlob* d3dRootSignatureErrorBlob = null;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return CreateD3D12RootSignatureInternal(device, resources, &d3dRootSignatureBlob, &d3dRootSignatureErrorBlob);
            }
            finally
            {
                ReleaseIfNotNull(d3dRootSignatureErrorBlob);
                ReleaseIfNotNull(d3dRootSignatureBlob);
            }
        }

        static ID3D12RootSignature* CreateD3D12RootSignatureInternal(D3D12GraphicsDevice device, ReadOnlySpan<GraphicsPipelineResourceInfo> resources, ID3DBlob** pD3DRootSignatureBlob, ID3DBlob** pD3DRootSignatureErrorBlob)
        {
            ID3D12RootSignature* d3d12RootSignature;

            var d3d12RootSignatureDesc = new D3D12_ROOT_SIGNATURE_DESC {
                Flags = D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT,
            };

            var d3d12RootParameterCount = 0;
            var d3d12StaticSamplerDescCount = 0;

            var d3d12RootParametersIndex = 0;
            var d3d12StaticSamplerDescsIndex = 0;

            var constantShaderRegister = 0;
            var textureShaderRegister = 0;

            for (var inputIndex = 0; inputIndex < resources.Length; inputIndex++)
            {
                d3d12RootParameterCount++;

                if (resources[inputIndex].Kind == GraphicsPipelineResourceKind.Texture)
                {
                    d3d12StaticSamplerDescCount++;
                }
            }

            var d3d12RootParameters = stackalloc D3D12_ROOT_PARAMETER[d3d12RootParameterCount];
            var d3d12StaticSamplerDescs = stackalloc D3D12_STATIC_SAMPLER_DESC[d3d12StaticSamplerDescCount];
            var d3d12DescriptorRanges = stackalloc D3D12_DESCRIPTOR_RANGE[d3d12StaticSamplerDescCount];

            for (var inputIndex = 0; inputIndex < resources.Length; inputIndex++)
            {
                var input = resources[inputIndex];

                switch (input.Kind)
                {
                    case GraphicsPipelineResourceKind.ConstantBuffer:
                    {
                        var d3d12ShaderVisibility = GetD3D12ShaderVisiblity(input.ShaderVisibility);
                        d3d12RootParameters[d3d12RootParametersIndex].InitAsConstantBufferView(unchecked((uint)constantShaderRegister), registerSpace: 0, d3d12ShaderVisibility);

                        constantShaderRegister++;
                        d3d12RootParametersIndex++;
                        break;
                    }

                    case GraphicsPipelineResourceKind.Texture:
                    {
                        d3d12DescriptorRanges[d3d12StaticSamplerDescsIndex] = new D3D12_DESCRIPTOR_RANGE(D3D12_DESCRIPTOR_RANGE_TYPE_SRV, numDescriptors: 1, baseShaderRegister: unchecked((uint)textureShaderRegister));
                        var shaderVisibility = GetD3D12ShaderVisiblity(input.ShaderVisibility);

                        d3d12RootParameters[d3d12RootParametersIndex].InitAsDescriptorTable(1, &d3d12DescriptorRanges[d3d12StaticSamplerDescsIndex], shaderVisibility);
                        d3d12StaticSamplerDescs[d3d12StaticSamplerDescsIndex] = new D3D12_STATIC_SAMPLER_DESC(
                            shaderRegister: unchecked((uint)d3d12StaticSamplerDescsIndex),
                            shaderVisibility: shaderVisibility
                        );

                        textureShaderRegister++;
                        d3d12RootParametersIndex++;
                        d3d12StaticSamplerDescsIndex++;
                        break;
                    }

                    default:
                    {
                        break;
                    }
                }
            }

            d3d12RootSignatureDesc.NumParameters = unchecked((uint)d3d12RootParameterCount);
            d3d12RootSignatureDesc.pParameters = d3d12RootParameters;

            d3d12RootSignatureDesc.NumStaticSamplers = unchecked((uint)d3d12StaticSamplerDescCount);
            d3d12RootSignatureDesc.pStaticSamplers = d3d12StaticSamplerDescs;

            ThrowExternalExceptionIfFailed(D3D12SerializeRootSignature(&d3d12RootSignatureDesc, D3D_ROOT_SIGNATURE_VERSION_1, pD3DRootSignatureBlob, pD3DRootSignatureErrorBlob));
            ThrowExternalExceptionIfFailed(device.D3D12Device->CreateRootSignature(0, pD3DRootSignatureBlob[0]->GetBufferPointer(), pD3DRootSignatureBlob[0]->GetBufferSize(), __uuidof<ID3D12RootSignature>(), (void**)&d3d12RootSignature));

            return d3d12RootSignature;
        }

        static D3D12_SHADER_VISIBILITY GetD3D12ShaderVisiblity(GraphicsShaderVisibility shaderVisibility)
        {
            D3D12_SHADER_VISIBILITY d3d12ShaderVisibility;

            switch (shaderVisibility)
            {
                case GraphicsShaderVisibility.Vertex:
                {
                    d3d12ShaderVisibility = D3D12_SHADER_VISIBILITY_VERTEX;
                    break;
                }

                case GraphicsShaderVisibility.Pixel:
                {
                    d3d12ShaderVisibility = D3D12_SHADER_VISIBILITY_PIXEL;
                    break;
                }

                default:
                {
                    d3d12ShaderVisibility = D3D12_SHADER_VISIBILITY_ALL;
                    break;
                }
            }

            return d3d12ShaderVisibility;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipelineSignature" /> class.</summary>
    ~D3D12GraphicsPipelineSignature() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12RootSignature" /> for the pipeline.</summary>
    public ID3D12RootSignature* D3D12RootSignature
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12RootSignature;
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets or sets the name for the pipeline signature.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = D3D12RootSignature->UpdateD3D12Name(value);
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12RootSignature);
        }

        _state.EndDispose();
    }
}
