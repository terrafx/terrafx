// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPrimitive : GraphicsPrimitive
{
    private string _name = null!;
    private VolatileState _state;

    internal VulkanGraphicsPrimitive(VulkanGraphicsDevice device, VulkanGraphicsPipeline pipeline, VulkanGraphicsBufferView vertexBufferView, VulkanGraphicsBufferView? indexBufferView, ReadOnlySpan<GraphicsResourceView> inputResourceViews)
        : base(device, pipeline, vertexBufferView, indexBufferView, inputResourceViews)
    {
        _ = _state.Transition(to: Initialized);
        Name = nameof(VulkanGraphicsPrimitive);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPrimitive" /> class.</summary>
    ~VulkanGraphicsPrimitive() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsPrimitive.IndexBufferView" />
    public new VulkanGraphicsBufferView? IndexBufferView => base.IndexBufferView.As<VulkanGraphicsBufferView>();

    /// <inheritdoc />
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

    /// <inheritdoc cref="GraphicsPrimitive.Pipeline" />
    public new VulkanGraphicsPipeline Pipeline => base.Pipeline.As<VulkanGraphicsPipeline>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc cref="GraphicsPrimitive.VertexBufferView" />
    public new VulkanGraphicsBufferView VertexBufferView => base.VertexBufferView.As<VulkanGraphicsBufferView>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                Pipeline?.Dispose();
                IndexBufferView?.Dispose();

                foreach (var inputResourceView in InputResourceViews)
                {
                    inputResourceView?.Dispose();
                }

                VertexBufferView?.Dispose();
            }
        }

        _state.EndDispose();
    }
}
