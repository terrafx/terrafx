// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using TerraFX.Graphics.Advanced;
using TerraFX.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics render pass.</summary>
public sealed class GraphicsRenderPass : IDisposable, INameable
{
    private readonly GraphicsAdapter _adapter;
    private readonly GraphicsDevice _device;
    private readonly GraphicsService _service;

    private readonly GraphicsFormat _renderTargetFormat;

    private readonly IGraphicsSurface _surface;

    private GraphicsSwapchain _swapchain;

    private string _name;
    private VolatileState _state;

    internal GraphicsRenderPass(GraphicsDevice device, in GraphicsRenderPassCreateOptions createOptions)
    {
        AssertNotNull(device);
        _device = device;

        var adapter = device.Adapter;
        _adapter = adapter;

        var service = adapter.Service;
        _service = service;

        device.AddRenderPass(this);

        _renderTargetFormat = createOptions.RenderTargetFormat;
        _surface = createOptions.Surface;

        var swapchainCreateOptions = new GraphicsSwapchainCreateOptions {
            MinimumRenderTargetCount = createOptions.MinimumRenderTargetCount,
            RenderTargetFormat = createOptions.RenderTargetFormat,
            Surface = createOptions.Surface,
        };
        _swapchain = new GraphicsSwapchain(this, in swapchainCreateOptions);

        _name = GetType().Name;
        _ = _state.Transition(VolatileState.Initialized);
    }

    /// <summary>Gets the adapter for which the object was created.</summary>
    public GraphicsAdapter Adapter => _adapter;

    /// <summary>Gets the device for which the object was created.</summary>
    public GraphicsDevice Device => _device;

    /// <summary>Gets <c>true</c> if the object has been disposed; otherwise, <c>false</c>.</summary>
    public bool IsDisposed => _state.IsDisposedOrDisposing;

    /// <inheritdoc />
    [AllowNull]
    public string Name
    {
        get
        {
            return _name;
        }

        set
        {
            _name = value ?? GetType().Name;
        }
    }

    /// <summary>Gets the format of render targets used by the render pass.</summary>
    public GraphicsFormat RenderTargetFormat => _renderTargetFormat;

    /// <summary>Gets the service for which the object was created.</summary>
    public GraphicsService Service => _service;

    /// <summary>Gets the surface used by the render pass.</summary>
    public IGraphicsSurface Surface => _surface;

    /// <summary>Gets the swapchain for the render pass.</summary>
    public GraphicsSwapchain Swapchain => _swapchain;

    /// <summary>Creates a new graphics pipeline for the device.</summary>
    /// <param name="createOptions">The options to use when creating the pipeline.</param>
    /// <returns>A new graphics pipeline created for the device.</returns>
    /// <exception cref="ArgumentNullException"><see cref="GraphicsPipelineCreateOptions.Signature" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.Signature" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.VertexShader" /> is not <see cref="GraphicsShaderKind.Vertex"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.VertexShader" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.PixelShader" /> is not <see cref="GraphicsShaderKind.Pixel"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="GraphicsPipelineCreateOptions.PixelShader" /> was not created for the same device as this render pass.</exception>
    /// <exception cref="ObjectDisposedException">The device has been disposed.</exception>
    public GraphicsPipeline CreatePipeline(in GraphicsPipelineCreateOptions createOptions)
    {
        ThrowIfNull(createOptions.Signature);

        if (createOptions.Signature.Device != Device)
        {
            ThrowForInvalidParent(createOptions.Signature.Device);
        }

        if (createOptions.VertexShader is GraphicsShader vertexShader)
        {
            if (vertexShader.Device != Device)
            {
                ThrowForInvalidParent(vertexShader.Device);
            }

            if (vertexShader.Kind != GraphicsShaderKind.Vertex)
            {
                ThrowForInvalidKind(vertexShader.Kind);
            }
        }

        if (createOptions.PixelShader is GraphicsShader pixelShader)
        {
            if (pixelShader.Device != Device)
            {
                ThrowForInvalidParent(pixelShader.Device);
            }

            if (pixelShader.Kind != GraphicsShaderKind.Pixel)
            {
                ThrowForInvalidKind(pixelShader.Kind);
            }
        }

        return CreatePipelineUnsafe(in createOptions);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _ = _state.BeginDispose();
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        _state.EndDispose();
    }

    /// <inheritdoc />
    public override string ToString() => _name;

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            _swapchain.Dispose();
            _swapchain = null!;
        }

        _ = Device.RemoveRenderPass(this);
    }

    private GraphicsPipeline CreatePipelineUnsafe(in GraphicsPipelineCreateOptions createOptions) => new GraphicsPipeline(this, in createOptions);
}
