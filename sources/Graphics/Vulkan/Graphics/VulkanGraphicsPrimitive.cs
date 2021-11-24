// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Threading;
using static TerraFX.Threading.VolatileState;

namespace TerraFX.Graphics;

/// <inheritdoc />
public sealed unsafe class VulkanGraphicsPrimitive : GraphicsPrimitive
{
    private VolatileState _state;

    internal VulkanGraphicsPrimitive(VulkanGraphicsDevice device, VulkanGraphicsPipeline pipeline, in GraphicsMemoryRegion<GraphicsResource> vertexBufferView, uint vertexBufferStride, in GraphicsMemoryRegion<GraphicsResource> indexBufferView, uint indexBufferStride, ReadOnlySpan<GraphicsMemoryRegion<GraphicsResource>> inputResourceRegions = default)
        : base(device, pipeline, in vertexBufferView, vertexBufferStride, in indexBufferView, indexBufferStride, inputResourceRegions)
    {
        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="VulkanGraphicsPrimitive" /> class.</summary>
    ~VulkanGraphicsPrimitive() => Dispose(isDisposing: true);

    /// <inheritdoc cref="GraphicsDeviceObject.Device" />
    public new VulkanGraphicsDevice Device => (VulkanGraphicsDevice)base.Device;

    /// <inheritdoc cref="GraphicsPrimitive.Pipeline" />
    public new VulkanGraphicsPipeline Pipeline => (VulkanGraphicsPipeline)base.Pipeline;

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            Pipeline?.Dispose();

            // TODO: The primitive shouldn't dispose the collections, it
            // should be freeing the region and something else should control
            // resource disposal.

            foreach (var inputResourceRegion in InputResourceRegions)
            {
                inputResourceRegion.Collection?.Dispose();
            }

            VertexBufferRegion.Collection?.Dispose();
            IndexBufferRegion.Collection?.Dispose();
        }

        _state.EndDispose();
    }
}
