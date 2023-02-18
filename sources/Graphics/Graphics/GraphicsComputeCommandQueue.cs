// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using TerraFX.Collections;
using TerraFX.Threading;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics compute commands.</summary>
public sealed unsafe class GraphicsComputeCommandQueue : GraphicsCommandQueue
{
    private ValuePool<GraphicsComputeContext> _contexts;
    private readonly ValueMutex _contextsMutex;

    internal GraphicsComputeCommandQueue(GraphicsDevice device) : base(device, GraphicsContextKind.Compute)
    {
        _contexts = new ValuePool<GraphicsComputeContext>();
        _contextsMutex = new ValueMutex();
    }

    /// <inheritdoc cref="GraphicsCommandQueue.RentContext" />
    public new GraphicsComputeContext RentContext() => base.RentContext().As<GraphicsComputeContext>();

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

    internal override bool RemoveContext(GraphicsContext context) => IsDisposed || _contexts.Remove(context.As<GraphicsComputeContext>());

    private protected override GraphicsComputeContext RentContextUnsafe()
    {
        return _contexts.Rent(&CreateContext, this, _contextsMutex);

        static GraphicsComputeContext CreateContext(GraphicsComputeCommandQueue commandQueue)
        {
            return new GraphicsComputeContext(commandQueue);
        }
    }

    private protected override void ReturnContextUnsafe(GraphicsContext context) => _contexts.Return(context.As<GraphicsComputeContext>(), _contextsMutex);
}
