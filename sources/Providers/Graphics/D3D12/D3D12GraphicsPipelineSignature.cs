// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D_ROOT_SIGNATURE_VERSION;
using static TerraFX.Interop.D3D12_DESCRIPTOR_RANGE_TYPE;
using static TerraFX.Interop.D3D12_ROOT_SIGNATURE_FLAGS;
using static TerraFX.Interop.D3D12_SHADER_VISIBILITY;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsPipelineSignature : GraphicsPipelineSignature
    {
        private ValueLazy<Pointer<ID3D12RootSignature>> _d3d12RootSignature;

        private State _state;

        internal D3D12GraphicsPipelineSignature(D3D12GraphicsDevice device, ReadOnlySpan<GraphicsPipelineInput> inputs, ReadOnlySpan<GraphicsPipelineResource> resources)
            : base(device, inputs, resources)
        {
            _d3d12RootSignature = new ValueLazy<Pointer<ID3D12RootSignature>>(CreateD3D12RootSignature);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipelineSignature" /> class.</summary>
        ~D3D12GraphicsPipelineSignature() => Dispose(isDisposing: false);

        /// <summary>Gets the underlying <see cref="ID3D12RootSignature" /> for the pipeline.</summary>
        public ID3D12RootSignature* D3D12RootSignature => _d3d12RootSignature.Value;

        /// <inheritdoc cref="GraphicsPipeline.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _d3d12RootSignature.Dispose(ReleaseIfNotNull);
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12RootSignature> CreateD3D12RootSignature()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3DBlob* rootSignatureBlob = null;
            ID3DBlob* rootSignatureErrorBlob = null;

            try
            {
                ID3D12RootSignature* d3d12RootSignature;

                var rootSignatureDesc = new D3D12_ROOT_SIGNATURE_DESC {
                    Flags = D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT,
                };

                var resources = Resources;
                var resourcesLength = resources.Length;

                var rootParametersLength = 0;
                var staticSamplersLength = 0;

                var rootParametersIndex = 0;
                var constantShaderRegister = 0;
                var textureShaderRegister = 0;
                var staticSamplersIndex = 0;

                for (var inputIndex = 0; inputIndex < resourcesLength; inputIndex++)
                {
                    rootParametersLength++;

                    if (resources[inputIndex].Kind == GraphicsPipelineResourceKind.Texture)
                    {
                        staticSamplersLength++;
                    }
                }

                var rootParameters = stackalloc D3D12_ROOT_PARAMETER[rootParametersLength];
                var staticSamplers = stackalloc D3D12_STATIC_SAMPLER_DESC[staticSamplersLength];
                var descriptorRanges = stackalloc D3D12_DESCRIPTOR_RANGE[staticSamplersLength];

                for (var inputIndex = 0; inputIndex < resourcesLength; inputIndex++)
                {
                    var input = resources[inputIndex];

                    switch (input.Kind)
                    {
                        case GraphicsPipelineResourceKind.ConstantBuffer:
                        {
                            var shaderVisibility = GetD3D12ShaderVisiblity(input.ShaderVisibility);
                            rootParameters[rootParametersIndex].InitAsConstantBufferView(unchecked((uint)constantShaderRegister), registerSpace: 0, shaderVisibility);

                            constantShaderRegister++;
                            rootParametersIndex++;
                            break;
                        }

                        case GraphicsPipelineResourceKind.Texture:
                        {
                            descriptorRanges[staticSamplersIndex] = new D3D12_DESCRIPTOR_RANGE(D3D12_DESCRIPTOR_RANGE_TYPE_SRV, numDescriptors: 1, baseShaderRegister: unchecked((uint)textureShaderRegister));
                            var shaderVisibility = GetD3D12ShaderVisiblity(input.ShaderVisibility);

                            rootParameters[rootParametersIndex].InitAsDescriptorTable(1, &descriptorRanges[staticSamplersIndex], shaderVisibility);
                            staticSamplers[staticSamplersIndex] = new D3D12_STATIC_SAMPLER_DESC(
                                shaderRegister: unchecked((uint)staticSamplersIndex),
                                shaderVisibility: shaderVisibility
                            );

                            textureShaderRegister++;
                            rootParametersIndex++;
                            staticSamplersIndex++;
                            break;
                        }

                        default:
                        {
                            break;
                        }
                    }
                }

                rootSignatureDesc.NumParameters = unchecked((uint)rootParametersLength);
                rootSignatureDesc.pParameters = rootParameters;

                rootSignatureDesc.NumStaticSamplers = unchecked((uint)staticSamplersLength);
                rootSignatureDesc.pStaticSamplers = staticSamplers;

                ThrowExternalExceptionIfFailed(D3D12SerializeRootSignature(&rootSignatureDesc, D3D_ROOT_SIGNATURE_VERSION_1, &rootSignatureBlob, &rootSignatureErrorBlob), nameof(D3D12SerializeRootSignature));

                var iid = IID_ID3D12RootSignature;
                ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateRootSignature(0, rootSignatureBlob->GetBufferPointer(), rootSignatureBlob->GetBufferSize(), &iid, (void**)&d3d12RootSignature), nameof(ID3D12Device.CreateRootSignature));

                return d3d12RootSignature;
            }
            finally
            {
                ReleaseIfNotNull(rootSignatureErrorBlob);
                ReleaseIfNotNull(rootSignatureBlob);
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
    }
}
