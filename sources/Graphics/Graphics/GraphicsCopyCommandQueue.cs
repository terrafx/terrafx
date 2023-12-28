// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Threading;
using static TerraFX.Utilities.UnsafeUtilities;

#pragma warning disable CA1711 // Identifiers should not have incorrect suffix

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics copy commands.</summary>
public sealed unsafe class GraphicsCopyCommandQueue : GraphicsCommandQueue
{
    private ValuePool<GraphicsCopyContext> _contexts;
    private readonly ValueMutex _contextsMutex;

    internal GraphicsCopyCommandQueue(GraphicsDevice device) : base(device, GraphicsContextKind.Copy)
    {
        _contexts = new ValuePool<GraphicsCopyContext>();
        _contextsMutex = new ValueMutex();
    }

    /// <inheritdoc cref="GraphicsCommandQueue.RentContext" />
    public new GraphicsCopyContext RentContext() => base.RentContext().As<GraphicsCopyContext>();

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            foreach (var context in _contexts)
            {
                context.Dispose();
            }
            _contexts.Clear();
        }
        _contextsMutex.Dispose();

        base.Dispose(isDisposing);
    }

    internal override bool RemoveContext(GraphicsContext context) => IsDisposed || _contexts.Remove(context.As<GraphicsCopyContext>());

    private protected override GraphicsCopyContext RentContextUnsafe()
    {
        return _contexts.Rent(&CreateContext, this, _contextsMutex);

        static GraphicsCopyContext CreateContext(GraphicsCopyCommandQueue commandQueue)
        {
            return new GraphicsCopyContext(commandQueue);
        }
    }

    private protected override void ReturnContextUnsafe(GraphicsContext context) => _contexts.Return(context.As<GraphicsCopyContext>(), _contextsMutex);
}
