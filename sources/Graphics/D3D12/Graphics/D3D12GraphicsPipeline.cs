// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Interop.DirectX;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.DirectX.D3D12_PRIMITIVE_TOPOLOGY_TYPE;
using static TerraFX.Interop.Windows.Windows;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.D3D12Utilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsPipeline : GraphicsPipeline
{
    private readonly ID3D12PipelineState* _d3d12PipelineState;

    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsPipeline(D3D12GraphicsRenderPass renderPass, D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader)
        : base(renderPass, signature, vertexShader, pixelShader)
    {
        _d3d12PipelineState = CreateD3D12GraphicsPipelineState(renderPass, signature, vertexShader, pixelShader);

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsPipeline);

        static ID3D12PipelineState* CreateD3D12GraphicsPipelineState(D3D12GraphicsRenderPass renderPass, D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader)
        {
            var d3d12InputElementDescs = UnmanagedArray<D3D12_INPUT_ELEMENT_DESC>.Empty;

            try
            {
                // We split this into two methods so the JIT can still optimize the "core" part
                return CreateD3D12GraphicsPipelineStateInternal(renderPass, signature, vertexShader, pixelShader, ref d3d12InputElementDescs);
            }
            finally
            {
                d3d12InputElementDescs.Dispose();
            }
        }

        static ID3D12PipelineState* CreateD3D12GraphicsPipelineStateInternal(D3D12GraphicsRenderPass renderPass, D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader, ref UnmanagedArray<D3D12_INPUT_ELEMENT_DESC> d3d12InputElementDescs)
        {
            ID3D12PipelineState* d3d12GraphicsPipelineState;

            var d3d12GraphicsPipelineStateDesc = new D3D12_GRAPHICS_PIPELINE_STATE_DESC {
                pRootSignature = signature.D3D12RootSignature,
                RasterizerState = D3D12_RASTERIZER_DESC.DEFAULT,
                BlendState = D3D12_BLEND_DESC.DEFAULT,
                DepthStencilState = D3D12_DEPTH_STENCIL_DESC.DEFAULT,
                SampleMask = uint.MaxValue,
                PrimitiveTopologyType = D3D12_PRIMITIVE_TOPOLOGY_TYPE_TRIANGLE,
                NumRenderTargets = 1,
                SampleDesc = new DXGI_SAMPLE_DESC(count: 1, quality: 0),
            };
            d3d12GraphicsPipelineStateDesc.DepthStencilState.DepthEnable = FALSE;
            d3d12GraphicsPipelineStateDesc.RTVFormats[0] = renderPass.RenderTargetFormat.AsDxgiFormat();

            if (vertexShader is not null)
            {
                var inputs = signature.Inputs;

                var inputElementsCount = GetInputElementCount(inputs);
                nuint inputElementsIndex = 0;

                if (inputElementsCount != 0)
                {
                    d3d12InputElementDescs = new UnmanagedArray<D3D12_INPUT_ELEMENT_DESC>(inputElementsCount);

                    for (nuint inputIndex = 0; inputIndex < inputs.Length; inputIndex++)
                    {
                        var input = inputs[inputIndex];
                        var inputElements = input.Elements;

                        var inputLayoutStride = 0u;
                        var maxAlignment = 0u;

                        for (nuint inputElementIndex = 0; inputElementIndex < inputElements.Length; inputElementIndex++)
                        {
                            var inputElement = inputElements[inputElementIndex];

                            var inputElementAlignment = inputElement.Alignment;
                            inputLayoutStride = AlignUp(inputLayoutStride, inputElementAlignment);

                            maxAlignment = Max(maxAlignment, inputElementAlignment);

                            d3d12InputElementDescs[inputElementsIndex] = new D3D12_INPUT_ELEMENT_DESC {
                                SemanticName = GetInputElementSemanticName(inputElement.Kind).GetPointer(),
                                Format = inputElement.Format.AsDxgiFormat(),
                                InputSlot = unchecked((uint)inputIndex),
                                AlignedByteOffset = inputLayoutStride,
                            };

                            inputLayoutStride += inputElement.Size;
                            inputElementsIndex++;
                        }

                        inputLayoutStride = AlignUp(inputLayoutStride, maxAlignment);
                    }
                }

                d3d12GraphicsPipelineStateDesc.VS = vertexShader.D3D12ShaderBytecode;
            }

            if (pixelShader is not null)
            {
                d3d12GraphicsPipelineStateDesc.PS = pixelShader.D3D12ShaderBytecode;
            }

            d3d12GraphicsPipelineStateDesc.InputLayout = new D3D12_INPUT_LAYOUT_DESC {
                pInputElementDescs = d3d12InputElementDescs.GetPointerUnsafe(0),
                NumElements = (uint)d3d12InputElementDescs.Length,
            };
            ThrowExternalExceptionIfFailed(renderPass.Device.D3D12Device->CreateGraphicsPipelineState(&d3d12GraphicsPipelineStateDesc, __uuidof<ID3D12PipelineState>(), (void**)&d3d12GraphicsPipelineState));

            return d3d12GraphicsPipelineState;
        }

        static nuint GetInputElementCount(UnmanagedReadOnlySpan<GraphicsPipelineInput> inputs)
        {
            nuint inputElementCount = 0;

            for (nuint i = 0; i < inputs.Length; i++)
            {
                inputElementCount += inputs[i].Elements.Length;
            }

            return inputElementCount;
        }

        static ReadOnlySpan<sbyte> GetInputElementSemanticName(GraphicsPipelineInputElementKind inputElementKind)
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

    /// <inheritdoc cref="GraphicsRenderPassObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

    /// <summary>Gets the underlying <see cref="ID3D12PipelineState" /> for the pipeline.</summary>
    public ID3D12PipelineState* D3D12PipelineState
    {
        get
        {
            AssertNotDisposedOrDisposing(_state);
            return _d3d12PipelineState;
        }
    }

    /// <inheritdoc cref="GraphicsRenderPassObject.Device" />
    public new D3D12GraphicsDevice Device => base.Device.As<D3D12GraphicsDevice>();

    /// <summary>Gets or sets the name for the pipeline.</summary>
    public override string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = D3D12PipelineState->UpdateD3D12Name(value);
        }
    }

    /// <inheritdoc cref="GraphicsPipeline.PixelShader" />
    public new D3D12GraphicsShader? PixelShader => base.PixelShader.As<D3D12GraphicsShader>();

    /// <inheritdoc cref="GraphicsRenderPassObject.RenderPass" />
    public new D3D12GraphicsRenderPass RenderPass => base.RenderPass.As<D3D12GraphicsRenderPass>();

    /// <inheritdoc cref="GraphicsRenderPassObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc cref="GraphicsPipeline.Signature" />
    public new D3D12GraphicsPipelineSignature Signature => base.Signature.As<D3D12GraphicsPipelineSignature>();

    /// <inheritdoc cref="GraphicsPipeline.VertexShader" />
    public new D3D12GraphicsShader? VertexShader => base.VertexShader.As<D3D12GraphicsShader>();

    /// <inheritdoc />
    public override D3D12GraphicsPipelineResourceViewSet CreateResourceViews(ReadOnlySpan<GraphicsResourceView> resourceViews)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsPipeline));
        ThrowIfZero(resourceViews.Length);

        return new D3D12GraphicsPipelineResourceViewSet(this, resourceViews);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            ReleaseIfNotNull(_d3d12PipelineState);

            if (isDisposing)
            {
                Signature?.Dispose();
                PixelShader?.Dispose();
                VertexShader?.Dispose();
            }
        }

        _state.EndDispose();
    }
}
