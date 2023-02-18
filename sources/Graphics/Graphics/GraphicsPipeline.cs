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
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics pipeline which defines how a graphics primitive should be rendered.</summary>
public sealed unsafe class GraphicsPipeline : GraphicsRenderPassObject
{
    private ComPtr<ID3D12PipelineState> _d3d12PipelineState;
    private readonly uint _d3d12PipelineStateVersion;

    private GraphicsShader? _pixelShader;
    private GraphicsPipelineSignature _signature;
    private GraphicsShader? _vertexShader;

    internal GraphicsPipeline(GraphicsRenderPass renderPass, in GraphicsPipelineCreateOptions createOptions) : base(renderPass)
    {
        _signature = createOptions.Signature;
        _pixelShader = createOptions.PixelShader;
        _vertexShader = createOptions.VertexShader;

        var d3d12PipelineState = CreateD3D12PipelineState(out _d3d12PipelineStateVersion);
        _d3d12PipelineState.Attach(d3d12PipelineState);

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

            if (VertexShader is GraphicsShader vertexShader)
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
                            SemanticName = GetSemanticName(input.Kind).GetPointerUnsafe(),
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

            if (PixelShader is GraphicsShader pixelShader)
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

        static ReadOnlySpan<sbyte> GetSemanticName(GraphicsPipelineInputKind pipelineInputKind)
        {
            ReadOnlySpan<sbyte> inputElementSemanticName;

            switch (pipelineInputKind)
            {
                case GraphicsPipelineInputKind.Position:
                {
                    // POSITION
                    inputElementSemanticName = new sbyte[] { 0x50, 0x4F, 0x53, 0x49, 0x54, 0x49, 0x4F, 0x4E, 0x00 };
                    break;
                }

                case GraphicsPipelineInputKind.Color:
                {
                    // COLOR
                    inputElementSemanticName = new sbyte[] { 0x43, 0x4F, 0x4C, 0x4F, 0x52, 0x00 };
                    break;
                }

                case GraphicsPipelineInputKind.Normal:
                {
                    // NORMAL
                    inputElementSemanticName = new sbyte[] { 0x4E, 0x4F, 0x52, 0x4D, 0x41, 0x4C, 0x00 };
                    break;
                }

                case GraphicsPipelineInputKind.TextureCoordinate:
                {
                    // TEXCOORD
                    inputElementSemanticName = new sbyte[] { 0x54, 0x45, 0x58, 0x43, 0x4F, 0x4F, 0x52, 0x44, 0x00 };
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

    /// <summary>Finalizes an instance of the <see cref="GraphicsPipeline" /> class.</summary>
    ~GraphicsPipeline() => Dispose(isDisposing: false);

    /// <summary>Gets <c>true</c> if the pipeline has a pixel shader; otherwise, <c>false</c>.</summary>
    public bool HasPixelShader => _pixelShader is not null;

    /// <summary>Gets <c>true</c> if the pipeline has a vertex shader; otherwise, <c>false</c>.</summary>
    public bool HasVertexShader => _vertexShader is not null;

    /// <summary>Gets the pixel shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? PixelShader => _pixelShader;

    /// <summary>Gets the signature of the pipeline.</summary>
    public GraphicsPipelineSignature Signature => _signature;

    /// <summary>Gets the vertex shader for the pipeline or <c>null</c> if none exists.</summary>
    public GraphicsShader? VertexShader => _vertexShader;

    internal ID3D12PipelineState* D3D12PipelineState => _d3d12PipelineState;

    internal uint D3D12PipelineStateVersion => _d3d12PipelineStateVersion;

    /// <summary>Creates a new descriptor set for the pipeline.</summary>
    /// <param name="resourceViews">The resource views for the pipeline descriptor set.</param>
    /// <returns>The created descriptor set.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>empty</c>.</exception>
    /// <exception cref="ObjectDisposedException">The pipeline has been disposed.</exception>
    public GraphicsPipelineDescriptorSet CreateDescriptorSet(ReadOnlySpan<GraphicsResourceView> resourceViews)
    {
        ThrowIfDisposed();
        ThrowIfZero(resourceViews.Length);

        var createOptions = new GraphicsPipelineDescriptorSetCreateOptions {
            ResourceViews = resourceViews.ToArray(),
            TakeResourceViewsOwnership = true,
        };
        return CreateDescriptorSetUnsafe(in createOptions);
    }

    /// <summary>Creates a new descriptor set for the pipeline.</summary>
    /// <param name="createOptions">The options to use when creating the descriptor set.</param>
    /// <returns>The created descriptor set.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineDescriptorSetCreateOptions.ResourceViews" /> is <c>empty</c>.</exception>
    /// <exception cref="ObjectDisposedException">The pipeline has been disposed.</exception>
    public GraphicsPipelineDescriptorSet CreateDescriptorSet(in GraphicsPipelineDescriptorSetCreateOptions createOptions)
    {
        ThrowIfDisposed();
        ThrowIfNull(createOptions.ResourceViews);
        ThrowIfZero(createOptions.ResourceViews.Length);

        return CreateDescriptorSetUnsafe(in createOptions);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _signature = null!;
            _pixelShader = null!;
            _vertexShader = null!;
        }

        _ = _d3d12PipelineState.Reset();
    }

    /// <inheritdoc />
    protected override void SetNameUnsafe(string value) => D3D12PipelineState->SetD3D12Name(value);

    private GraphicsPipelineDescriptorSet CreateDescriptorSetUnsafe(in GraphicsPipelineDescriptorSetCreateOptions createOptions) => new GraphicsPipelineDescriptorSet(this, in createOptions);
}
