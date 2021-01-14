// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop;
using TerraFX.Numerics;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Graphics.Providers.D3D12.HelperUtilities;
using static TerraFX.Interop.D3D12_PRIMITIVE_TOPOLOGY_TYPE;
using static TerraFX.Interop.DXGI_FORMAT;
using static TerraFX.Interop.Windows;
using static TerraFX.Threading.VolatileState;

namespace TerraFX.Graphics.Providers.D3D12
{
    /// <inheritdoc />
    public sealed unsafe class D3D12GraphicsPipeline : GraphicsPipeline
    {
        private ValueLazy<Pointer<ID3D12PipelineState>> _d3d12PipelineState;
        private VolatileState _state;

        internal D3D12GraphicsPipeline(D3D12GraphicsDevice device, D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader)
            : base(device, signature, vertexShader, pixelShader)
        {
            _d3d12PipelineState = new ValueLazy<Pointer<ID3D12PipelineState>>(CreateD3D12GraphicsPipelineState);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipeline" /> class.</summary>
        ~D3D12GraphicsPipeline() => Dispose(isDisposing: false);

        // COLOR
        private static ReadOnlySpan<sbyte> COLOR_SEMANTIC_NAME => new sbyte[] { 0x43, 0x4F, 0x4C, 0x4F, 0x52, 0x00 };

        // NORMAL
        private static ReadOnlySpan<sbyte> NORMAL_SEMANTIC_NAME => new sbyte[] { 0x4E, 0x4F, 0x52, 0x4D, 0x41, 0x4C, 0x00 };

        // POSITION
        private static ReadOnlySpan<sbyte> POSITION_SEMANTIC_NAME => new sbyte[] { 0x50, 0x4F, 0x53, 0x49, 0x54, 0x49, 0x4F, 0x4E, 0x00 };

        // TEXCOORD
        private static ReadOnlySpan<sbyte> TEXCOORD_SEMANTIC_NAME => new sbyte[] { 0x54, 0x45, 0x58, 0x43, 0x4F, 0x4F, 0x52, 0x44, 0x00 };

        /// <inheritdoc cref="GraphicsPipeline.Device" />
        public new D3D12GraphicsDevice Device => (D3D12GraphicsDevice)base.Device;

        /// <summary>Gets the underlying <see cref="ID3D12PipelineState" /> for the pipeline.</summary>
        public ID3D12PipelineState* D3D12PipelineState => _d3d12PipelineState.Value;

        /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
        public new D3D12GraphicsShader? PixelShader => (D3D12GraphicsShader?)base.PixelShader;

        /// <inheritdoc cref="GraphicsPipeline.Signature" />
        public new D3D12GraphicsPipelineSignature Signature => (D3D12GraphicsPipelineSignature)base.Signature;

        /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
        public new D3D12GraphicsShader? VertexShader => (D3D12GraphicsShader?)base.VertexShader;

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                _d3d12PipelineState.Dispose(ReleaseIfNotNull);

                Signature?.Dispose();
                PixelShader?.Dispose();
                VertexShader?.Dispose();
            }

            _state.EndDispose();
        }

        private Pointer<ID3D12PipelineState> CreateD3D12GraphicsPipelineState()
        {
            _state.ThrowIfDisposedOrDisposing();

            ID3D12PipelineState* d3d12GraphicsPipelineState;

            var inputElementDescs = Array.Empty<D3D12_INPUT_ELEMENT_DESC>();

            var pipelineStateDesc = new D3D12_GRAPHICS_PIPELINE_STATE_DESC {
                pRootSignature = Signature.D3D12RootSignature,
                RasterizerState = D3D12_RASTERIZER_DESC.DEFAULT,
                BlendState = D3D12_BLEND_DESC.DEFAULT,
                DepthStencilState = D3D12_DEPTH_STENCIL_DESC.DEFAULT,
                SampleMask = uint.MaxValue,
                PrimitiveTopologyType = D3D12_PRIMITIVE_TOPOLOGY_TYPE_TRIANGLE,
                NumRenderTargets = 1,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
            };
            pipelineStateDesc.DepthStencilState.DepthEnable = FALSE;
            pipelineStateDesc.RTVFormats[0] = Device.SwapChainFormat;

            var vertexShader = VertexShader;

            if (vertexShader is not null)
            {
                var inputs = Signature.Inputs;
                var inputsLength = inputs.Length;

                var inputElementsCount = GetInputElementsCount(inputs);
                var inputElementsIndex = 0;

                var inputSlotIndex = 0;

                if (inputElementsCount != 0)
                {
                    inputElementDescs = new D3D12_INPUT_ELEMENT_DESC[inputElementsCount];

                    for (var inputIndex = 0; inputIndex < inputsLength; inputIndex++)
                    {
                        var input = inputs[inputIndex];

                        var inputElements = input.Elements;
                        var inputElementsLength = inputElements.Length;

                        uint inputLayoutStride = 0;

                        for (var inputElementIndex = 0; inputElementIndex < inputElementsLength; inputElementIndex++)
                        {
                            var inputElement = inputElements[inputElementIndex];

                            inputElementDescs[inputElementsIndex] = new D3D12_INPUT_ELEMENT_DESC {
                                SemanticName = GetInputElementSemanticName(inputElement.Kind).AsPointer(),
                                Format = GetInputElementFormat(inputElement.Type),
                                InputSlot = unchecked((uint)inputSlotIndex),
                                AlignedByteOffset = inputLayoutStride,
                            };

                            inputLayoutStride += inputElement.Size;
                            inputElementsIndex++;
                        }
                        inputSlotIndex++;
                    }
                }

                pipelineStateDesc.VS = vertexShader.D3D12ShaderBytecode;
            }

            var pixelShader = PixelShader;

            if (pixelShader is not null)
            {
                pipelineStateDesc.PS = pixelShader.D3D12ShaderBytecode;
            }

            fixed (D3D12_INPUT_ELEMENT_DESC* pInputElementDescs = inputElementDescs)
            {
                pipelineStateDesc.InputLayout = new D3D12_INPUT_LAYOUT_DESC {
                    pInputElementDescs = pInputElementDescs,
                    NumElements = unchecked((uint)inputElementDescs.Length),
                };

                var iid = IID_ID3D12PipelineState;
                ThrowExternalExceptionIfFailed(Device.D3D12Device->CreateGraphicsPipelineState(&pipelineStateDesc, &iid, (void**)&d3d12GraphicsPipelineState), nameof(ID3D12Device.CreateGraphicsPipelineState));
            }
            return d3d12GraphicsPipelineState;

            static int GetInputElementsCount(ReadOnlySpan<GraphicsPipelineInput> inputs)
            {
                var inputElementsCount = 0;

                foreach (var input in inputs)
                {
                    inputElementsCount += input.Elements.Length;
                }

                return inputElementsCount;
            }
        }

        private static DXGI_FORMAT GetInputElementFormat(Type type)
        {
            var inputElementFormat = DXGI_FORMAT_UNKNOWN;

            if (type == typeof(Vector2))
            {
                inputElementFormat = DXGI_FORMAT_R32G32_FLOAT;
            }
            else if (type == typeof(Vector3))
            {
                inputElementFormat = DXGI_FORMAT_R32G32B32_FLOAT;
            }
            else if (type == typeof(Vector4))
            {
                inputElementFormat = DXGI_FORMAT_R32G32B32A32_FLOAT;
            }

            return inputElementFormat;
        }

        private static ReadOnlySpan<sbyte> GetInputElementSemanticName(GraphicsPipelineInputElementKind inputElementKind)
        {
            ReadOnlySpan<sbyte> inputElementSemanticName;

            switch (inputElementKind)
            {
                case GraphicsPipelineInputElementKind.Position:
                {
                    inputElementSemanticName = POSITION_SEMANTIC_NAME;
                    break;
                }

                case GraphicsPipelineInputElementKind.Color:
                {
                    inputElementSemanticName = COLOR_SEMANTIC_NAME;
                    break;
                }

                case GraphicsPipelineInputElementKind.Normal:
                {
                    inputElementSemanticName = NORMAL_SEMANTIC_NAME;
                    break;
                }

                case GraphicsPipelineInputElementKind.TextureCoordinate:
                {
                    inputElementSemanticName = TEXCOORD_SEMANTIC_NAME;
                    break;
                }

                default:
                {
                    inputElementSemanticName = default;
                    break;
                }
            }

            return inputElementSemanticName;
        }
    }
}
