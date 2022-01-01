// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;

namespace TerraFX.Graphics;

/// <summary>Represents a graphics context, which can be used for executing commands.</summary>
public abstract class GraphicsContext<TSelf> : GraphicsCommandQueueObject<TSelf>
    where TSelf : GraphicsContext<TSelf>
{
    /// <summary>The information for the graphics context.</summary>
    protected GraphicsContextInfo ContextInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsContext{TSelf}" /> class.</summary>
    /// <param name="commandQueue">The command queue for which the context is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="commandQueue" /> is <c>null</c>.</exception>
    protected GraphicsContext(GraphicsCommandQueue<TSelf> commandQueue) : base(commandQueue)
    {
    }

    /// <summary>Gets the context kind.</summary>
    public GraphicsContextKind Kind => ContextInfo.Kind;

    /// <summary>Gets the fence used by the context for synchronization.</summary>
    public GraphicsFence Fence => ContextInfo.Fence;

    /// <summary>Closes the graphics context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void Close()
    {
        ThrowIfDisposed();
        CloseUnsafe();
    }

    /// <summary>Executes the graphics context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void Execute()
    {
        ThrowIfDisposed();
        ExecuteUnsafe();
    }

    /// <summary>Resets the graphics context.</summary>
    /// <exception cref="ObjectDisposedException">The context has been disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposed();
        ResetUnsafe();
    }

    /// <summary>Closes the graphics context.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void CloseUnsafe();

    /// <summary>Executes the graphics context.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void ExecuteUnsafe();

    /// <summary>Resets the graphics context.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void ResetUnsafe();
}
