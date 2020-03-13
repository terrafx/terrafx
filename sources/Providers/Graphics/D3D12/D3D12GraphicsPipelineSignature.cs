// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D_ROOT_SIGNATURE_VERSION;
using static TerraFX.Interop.D3D12;
using static TerraFX.Interop.D3D12_ROOT_SIGNATURE_FLAGS;
using static TerraFX.Interop.D3D12_SHADER_VISIBILITY;
using static TerraFX.Utilities.State;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsPipelineSignature : GraphicsPipelineSignature
    {
        private ValueLazy<Pointer<ID3D12RootSignature>> _d3d12RootSignature;

        private State _state;

        internal D3D12GraphicsPipelineSignature(D3D12GraphicsDevice graphicsDevice, ReadOnlySpan<GraphicsPipelineInput> inputs, ReadOnlySpan<GraphicsPipelineResource> resources)
            : base(graphicsDevice, inputs, resources)
        {
            _d3d12RootSignature = new ValueLazy<Pointer<ID3D12RootSignature>>(CreateD3D12RootSignature);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipelineSignature" /> class.</summary>
        ~D3D12GraphicsPipelineSignature()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc cref="GraphicsPipeline.GraphicsDevice" />
        public D3D12GraphicsDevice D3D12GraphicsDevice => (D3D12GraphicsDevice)GraphicsDevice;

        /// <summary>Gets the underlying <see cref="ID3D12RootSignature" /> for the pipeline.</summary>
        public ID3D12RootSignature* D3D12RootSignature => _d3d12RootSignature.Value;

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

                var rootParameters = Array.Empty<D3D12_ROOT_PARAMETER>();

                var rootSignatureDesc = new D3D12_ROOT_SIGNATURE_DESC {
                    Flags = D3D12_ROOT_SIGNATURE_FLAG_ALLOW_INPUT_ASSEMBLER_INPUT_LAYOUT,
                };

                var resources = Resources;
                var resourcesLength = resources.Length;

                var rootParametersIndex = 0;
                var constantShaderRegister = 0;

                if (resourcesLength != 0)
                {
                    rootParameters = new D3D12_ROOT_PARAMETER[resourcesLength];

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

                            default:
                            {
                                break;
                            }
                        }
                    }
                }

                fixed (D3D12_ROOT_PARAMETER* pRootParameters = rootParameters)
                {
                    rootSignatureDesc.NumParameters = unchecked((uint)rootParameters.Length);
                    rootSignatureDesc.pParameters = pRootParameters;

                    ThrowExternalExceptionIfFailed(nameof(D3D12SerializeRootSignature), D3D12SerializeRootSignature(&rootSignatureDesc, D3D_ROOT_SIGNATURE_VERSION_1, &rootSignatureBlob, &rootSignatureErrorBlob));
                }

                var iid = IID_ID3D12RootSignature;
                ThrowExternalExceptionIfFailed(nameof(ID3D12Device.CreateRootSignature), D3D12GraphicsDevice.D3D12Device->CreateRootSignature(0, rootSignatureBlob->GetBufferPointer(), rootSignatureBlob->GetBufferSize(), &iid, (void**)&d3d12RootSignature));

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
