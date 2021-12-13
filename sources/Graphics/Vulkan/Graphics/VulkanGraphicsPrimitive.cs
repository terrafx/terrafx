// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPrimitive : GraphicsPrimitive
{
    private VolatileState _state;

    internal VulkanGraphicsPrimitive(VulkanGraphicsDevice device, VulkanGraphicsPipeline pipeline, in GraphicsResourceView vertexBufferView, in GraphicsResourceView indexBufferView, ReadOnlySpan<GraphicsResourceView> inputResourceViews)
        : base(device, pipeline, in vertexBufferView, in indexBufferView, inputResourceViews)
    {
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPrimitive" /> class.</summary>
    ~VulkanGraphicsPrimitive() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Adapter" />
    public new VulkanGraphicsAdapter Adapter => base.Adapter.As<VulkanGraphicsAdapter>();

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => base.Device.As<VulkanGraphicsDevice>();

    /// <inheritdoc cref="GraphicsPrimitive.Pipeline" />
    public new VulkanGraphicsPipeline Pipeline => base.Pipeline.As<VulkanGraphicsPipeline>();

    /// <inheritdoc cref="GraphicsDeviceObject.Service" />
    public new VulkanGraphicsService Service => base.Service.As<VulkanGraphicsService>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                Pipeline?.Dispose();

                // TODO: The primitive shouldn't dispose the collections, it
                // should be freeing the region and something else should control
                // resource disposal.

                foreach (var inputResourceView in InputResourceViews)
                {
                    inputResourceView.Resource?.Dispose();
                }

                VertexBufferView.Resource?.Dispose();
                IndexBufferView.Resource?.Dispose();
            }
        }

        _state.EndDispose();
    }
}
