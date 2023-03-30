// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using TerraFX.Interop.DirectX;
using TerraFX.Interop.Windows;
using static TerraFX.Interop.DirectX.D3D12_INDEX_BUFFER_STRIP_CUT_VALUE;
using static TerraFX.Interop.DirectX.D3D12_INPUT_CLASSIFICATION;
using static TerraFX.Interop.DirectX.D3D12_PIPELINE_STATE_FLAGS;
using static TerraFX.Interop.DirectX.D3D12_PRIMITIVE_TOPOLOGY_TYPE;
using static TerraFX.Interop.DirectX.DXGI_FORMAT;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPipeline : GraphicsPipeline
{
    private ComPtr<ID3D12PipelineState> _d3d12PipelineState;
    private readonly uint _d3d12PipelineStateVersion;

    internal D3D12GraphicsPipeline(D3D12GraphicsRenderPass renderPass, in GraphicsPipelineCreateOptions createOptions) : base(renderPass)
    {
        PipelineInfo.Signature = createOptions.Signature;
        PipelineInfo.PixelShader = createOptions.PixelShader;
        PipelineInfo.VertexShader = createOptions.VertexShader;

        _d3d12PipelineState = CreateD3D12PipelineState(out _d3d12PipelineStateVersion);

        SetNameUnsafe(Name);

        ID3D12PipelineState* CreateD3D12PipelineState(out uint d3d12PipelineStateVersion)
        {
            ID3D12PipelineState* d3d12PipelineState;

            var d3d12GraphicsPipelineStateDesc = new D3D12_GRAPHICS_PIPELINE_STATE_DESC {
                pRootSignature = Signature.D3D12RootSignature,
                VS = new D3D12_SHADER_BYTECODE(),
                PS = new D3D12_SHADER_BYTECODE(),
                DS = new D3D12_SHADER_BYTECODE(),
                HS = new D3D12_SHADER_BYTECODE(),
                GS = new D3D12_SHADER_BYTECODE(),
                StreamOutput = new D3D12_STREAM_OUTPUT_DESC(),
                BlendState = D3D12_BLEND_DESC.DEFAULT,
                SampleMask = uint.MaxValue,
                RasterizerState = D3D12_RASTERIZER_DESC.DEFAULT,
                DepthStencilState = D3D12_DEPTH_STENCIL_DESC.DEFAULT,
                InputLayout = new D3D12_INPUT_LAYOUT_DESC(),
                IBStripCutValue = D3D12_INDEX_BUFFER_STRIP_CUT_VALUE_DISABLED,
                PrimitiveTopologyType = D3D12_PRIMITIVE_TOPOLOGY_TYPE_TRIANGLE,
                NumRenderTargets = 1,
                DSVFormat = DXGI_FORMAT_UNKNOWN,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
                NodeMask = 0,
                CachedPSO = new D3D12_CACHED_PIPELINE_STATE(),
                Flags = D3D12_PIPELINE_STATE_FLAG_NONE,
            };

            d3d12GraphicsPipelineStateDesc.DepthStencilState.DepthEnable = FALSE;
            d3d12GraphicsPipelineStateDesc.RTVFormats[0] = renderPass.RenderTargetFormat.AsDxgiFormat();

            var d3d12InputElementDescs = UnmanagedArray<D3D12_INPUT_ELEMENT_DESC>.Empty;

            if (VertexShader is D3D12GraphicsShader vertexShader)
            {
                var inputs = Signature.Inputs;

                if (inputs.Length != 0)
                {
                    d3d12InputElementDescs = new UnmanagedArray<D3D12_INPUT_ELEMENT_DESC>(inputs.Length);

                    var alignedByteOffset = 0u;

                    for (nuint index = 0; index < inputs.Length; index++)
                    {
                        ref readonly var input = ref inputs[index];

                        var inputByteAlignment = input.ByteAlignment;
                        alignedByteOffset = AlignUp(alignedByteOffset, inputByteAlignment);

                        d3d12InputElementDescs[index] = new D3D12_INPUT_ELEMENT_DESC {
                            SemanticName = (sbyte*)GetSemanticName(input.Kind).GetPointerUnsafe(),
                            SemanticIndex = 0,
                            Format = input.Format.AsDxgiFormat(),
                            InputSlot = 0,
                            AlignedByteOffset = alignedByteOffset,
                            InputSlotClass = D3D12_INPUT_CLASSIFICATION_PER_VERTEX_DATA,
                            InstanceDataStepRate = 0,
                        };

                        alignedByteOffset += input.ByteLength;
                    }

                    d3d12GraphicsPipelineStateDesc.InputLayout = new D3D12_INPUT_LAYOUT_DESC {
                        pInputElementDescs = d3d12InputElementDescs.GetPointerUnsafe(0),
                        NumElements = (uint)d3d12InputElementDescs.Length,
                    };
                }

                var bytecode = vertexShader.Bytecode;

                d3d12GraphicsPipelineStateDesc.VS = new D3D12_SHADER_BYTECODE {
                    BytecodeLength = bytecode.Length,
                    pShaderBytecode = bytecode.GetPointerUnsafe(0),
                };
            }

            if (PixelShader is D3D12GraphicsShader pixelShader)
            {
                var bytecode = pixelShader.Bytecode;

                d3d12GraphicsPipelineStateDesc.PS = new D3D12_SHADER_BYTECODE {
                    BytecodeLength = bytecode.Length,
                    pShaderBytecode = bytecode.GetPointerUnsafe(0),
                };
            }

            var result = renderPass.Device.D3D12Device->CreateGraphicsPipelineState(&d3d12GraphicsPipelineStateDesc, __uuidof<ID3D12PipelineState>(), (void**)&d3d12PipelineState);

            d3d12InputElementDescs.Dispose();

            ThrowExternalExceptionIfFailed(result, nameof(ID3D12Device.CreateGraphicsPipelineState));

            return GetLatestD3D12PipelineState(d3d12PipelineState, out d3d12PipelineStateVersion);
        }

        static ReadOnlySpan<byte> GetSemanticName(GraphicsPipelineInputKind pipelineInputKind)
        {
            ReadOnlySpan<byte> inputElementSemanticName;

            switch (pipelineInputKind)
            {
                case GraphicsPipelineInputKind.Position:
                {
                    inputElementSemanticName = "POSITION"u8;
                    break;
                }

                case GraphicsPipelineInputKind.Color:
                {
                    inputElementSemanticName = "COLOR"u8;
                    break;
                }

                case GraphicsPipelineInputKind.Normal:
                {
                    inputElementSemanticName = "NORMAL"u8;
                    break;
                }

                case GraphicsPipelineInputKind.TextureCoordinate:
                {
                    inputElementSemanticName = "TEXCOORD"u8;
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

    /// <summary>Finalizes an instance of the <see cref="D3D12GraphicsPipeline" /> class.</summary>
    ~D3D12GraphicsPipeline() => Dispose(isDisposing: false);

    /// <inheritdoc cref="GraphicsAdapterObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12PipelineState" /> for the pipeline.</summary>
    public ID3D12PipelineState* D3D12PipelineState => _d3d12PipelineState;

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
    public new D3D12GraphicsShader? PixelShader => base.PixelShader.As<D3D12GraphicsShader>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new D3D12GraphicsRenderPass RenderPass => base.RenderPass.As<D3D12GraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsServiceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc cref="GraphicsPipeline.Signature" />
    public new D3D12GraphicsPipelineSignature Signature => base.Signature.As<D3D12GraphicsPipelineSignature>();

    /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
    public new D3D12GraphicsShader? VertexShader => base.VertexShader.As<D3D12GraphicsShader>();

    /// <inheritdoc />
    protected override D3D12GraphicsPipelineDescriptorSet CreateDescriptorSetUnsafe(in GraphicsPipelineDescriptorSetCreateOptions createOptions)
    {
        return new D3D12GraphicsPipelineDescriptorSet(this, in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            PipelineInfo.Signature = null!;
            PipelineInfo.PixelShader = null!;
            PipelineInfo.VertexShader = null!;
        }

        _ = _d3d12PipelineState.Reset();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value)
    {
        D3D12PipelineState->SetD3D12Name(value);
    }
}
