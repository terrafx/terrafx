// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Graphics.Advanced;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class D3D12GraphicsRenderPass : GraphicsRenderPass
{
    private readonly D3D12GraphicsSwapchain _swapchain;

    private string _name = null!;
    private VolatileState _state;

    internal D3D12GraphicsRenderPass(D3D12GraphicsDevice device, IGraphicsSurface surface, GraphicsFormat renderTargetFormat, uint minimumRenderTargetCount = 0)
        : base(device, surface, renderTargetFormat)
    {
        _swapchain = new D3D12GraphicsSwapchain(this, surface, renderTargetFormat, minimumRenderTargetCount);

        _ = _state.Transition(to: Initialized);
        Name = nameof(D3D12GraphicsRenderPass);
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new D3D12GraphicsAdapter Adapter => base.Adapter.As<D3D12GraphicsAdapter>();

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
            _name = value ?? "";
        }
    }

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new D3D12GraphicsService Service => base.Service.As<D3D12GraphicsService>();

    /// <inheritdoc />
    public override D3D12GraphicsSwapchain Swapchain => _swapchain;

    /// <inheritdoc />
    public override D3D12GraphicsPipeline CreatePipeline(GraphicsPipelineSignature signature, GraphicsShader? vertexShader = null, GraphicsShader? pixelShader = null)
        => CreatePipeline((D3D12GraphicsPipelineSignature)signature, (D3D12GraphicsShader?)vertexShader, (D3D12GraphicsShader?)pixelShader);

    private D3D12GraphicsPipeline CreatePipeline(D3D12GraphicsPipelineSignature signature, D3D12GraphicsShader? vertexShader, D3D12GraphicsShader? pixelShader)
    {
        ThrowIfDisposedOrDisposing(_state, nameof(D3D12GraphicsDevice));
        return new D3D12GraphicsPipeline(this, signature, vertexShader, pixelShader);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                _swapchain?.Dispose();
            }
        }

        _state.EndDispose();
    }
}
