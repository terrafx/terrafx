// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Threading;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics render commands.</summary>
public sealed unsafe class GraphicsRenderCommandQueue : GraphicsCommandQueue
{
    private ValuePool<GraphicsRenderContext> _contexts;
    private readonly ValueMutex _contextsMutex;

    internal GraphicsRenderCommandQueue(GraphicsDevice device) : base(device, GraphicsContextKind.Render)
    {
        _contexts = new ValuePool<GraphicsRenderContext>();
        _contextsMutex = new ValueMutex();
    }

    /// <inheritdoc cref="GraphicsCommandQueue.RentContext" />
    public new GraphicsRenderContext RentContext() => base.RentContext().As<GraphicsRenderContext>();

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

    internal override bool RemoveContext(GraphicsContext context) => IsDisposed || _contexts.Remove(context.As<GraphicsRenderContext>());

    private protected override GraphicsRenderContext RentContextUnsafe()
    {
        return _contexts.Rent(&CreateContext, this, _contextsMutex);

        static GraphicsRenderContext CreateContext(GraphicsRenderCommandQueue commandQueue)
        {
            return new GraphicsRenderContext(commandQueue);
        }
    }

    private protected override void ReturnContextUnsafe(GraphicsContext context) => _contexts.Return(context.As<GraphicsRenderContext>(), _contextsMutex);
}
