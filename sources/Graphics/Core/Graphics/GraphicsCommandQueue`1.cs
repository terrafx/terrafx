// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>Represents a queue of graphics commands.</summary>
public abstract class GraphicsCommandQueue<TGraphicsContext> : GraphicsDeviceObject
    where TGraphicsContext : GraphicsContext<TGraphicsContext>
{
    /// <summary>The information for the graphics command queue.</summary>
    protected GraphicsCommandQueueInfo CommandQueueInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsCommandQueue{TGraphicsContext}" /> class.</summary>
    /// <param name="device">The device for which the command queue was created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsCommandQueue(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets the kind of contexts in the command queue.</summary>
    public GraphicsContextKind Kind => CommandQueueInfo.Kind;

    /// <summary>Executes a graphics context.</summary>
    /// <param name="context">The graphics context to execute.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The context kind for <paramref name="context" /> is not the same as <see cref="Kind" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="context" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void ExecuteContext(TGraphicsContext context)
    {
        ThrowIfDisposed();
        ThrowIfNull(context);

        if (context.Kind != Kind)
        {
            ThrowForInvalidKind(context.Kind);
        }

        if (context.Device != Device)
        {
            ThrowForInvalidParent(context.Device);
        }

        ExecuteContextUnsafe(context);
    }

    /// <summary>Rents a graphics context from the command queue, creating a new context if none are available.</summary>
    /// <returns>A rented graphics context.</returns>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public TGraphicsContext RentContext()
    {
        ThrowIfDisposed();
        return RentContextUnsafe();
    }

    /// <summary>Returns a graphics context to the command queue.</summary>
    /// <param name="context">The graphics context that should be returned to the command queue.</param>
    /// <exception cref="ArgumentNullException"><paramref name="context" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The context kind for <paramref name="context" /> is not the same as <see cref="Kind" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="context" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void ReturnContext(TGraphicsContext context)
    {
        ThrowIfDisposed();
        ThrowIfNull(context);

        if (context.Kind != Kind)
        {
            ThrowForInvalidKind(context.Kind);
        }

        if (context.Device != Device)
        {
            ThrowForInvalidParent(context.Device);
        }

        ReturnContextUnsafe(context);
    }

    /// <summary>Enqueues a GPU side signal for a graphics fence.</summary>
    /// <param name="fence">The graphics fence to signal.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fence" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fence" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void SignalFence(GraphicsFence fence)
    {
        ThrowIfDisposed();
        ThrowIfNull(fence);

        if (fence.Device != Device)
        {
            ThrowForInvalidParent(fence.Device);
        }

        SignalFenceUnsafe(fence);
    }

    /// <summary>Enqueues a GPU side wait for a graphics fence.</summary>
    /// <param name="fence">The graphics fence for which to wait.</param>
    /// <exception cref="ArgumentNullException"><paramref name="fence" /> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fence" /> was not created for the same device as the command queue.</exception>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void WaitForFence(GraphicsFence fence)
    {
        ThrowIfDisposed();
        ThrowIfNull(fence);

        if (fence.Device != Device)
        {
            ThrowForInvalidParent(fence.Device);
        }

        WaitForFenceUnsafe(fence);
    }

    /// <summary>Waits for the command queue to become idle.</summary>
    /// <exception cref="ObjectDisposedException">The command queue has been disposed.</exception>
    public void WaitForIdle()
    {
        ThrowIfDisposed();
        WaitForIdleUnsafe();
    }

    /// <summary>Executes a graphics context.</summary>
    /// <param name="context">The graphics context to execute.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void ExecuteContextUnsafe(TGraphicsContext context);

    /// <summary>Rents a graphics context from the command queue, creating a new context if none are available.</summary>
    /// <returns>A rented graphics context.</returns>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract TGraphicsContext RentContextUnsafe();

    /// <summary>Returns a graphics context to the command queue.</summary>
    /// <param name="context">The graphics context that should be returned to the command queue.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void ReturnContextUnsafe(TGraphicsContext context);

    /// <summary>Enqueues a GPU side signal for a graphics fence.</summary>
    /// <param name="fence">The graphics fence to signal.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void SignalFenceUnsafe(GraphicsFence fence);

    /// <summary>Enqueues a GPU side wait for a graphics fence.</summary>
    /// <param name="fence">The graphics fence for which to wait.</param>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void WaitForFenceUnsafe(GraphicsFence fence);

    /// <summary>Waits for the command queue to become idle.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void WaitForIdleUnsafe();
}
