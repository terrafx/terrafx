// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using TerraFX.Graphics.Advanced;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Graphics;

/// <summary>A graphics fence which can be used to synchronize the processor and a graphics context.</summary>
public abstract class GraphicsFence : GraphicsDeviceObject
{
    /// <summary>The information for the graphics fence.</summary>
    protected GraphicsFenceInfo FenceInfo;

    /// <summary>Initializes a new instance of the <see cref="GraphicsFence" /> class.</summary>
    /// <param name="device">The device for which the fence is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="device" /> is <c>null</c>.</exception>
    protected GraphicsFence(GraphicsDevice device) : base(device)
    {
    }

    /// <summary>Gets <c>true</c> if the fence is in the signalled state; otherwise, <c>false</c>.</summary>
    public bool IsSignalled => FenceInfo.IsSignalled;

    /// <summary>Resets the fence to the unsignalled state.</summary>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    public void Reset()
    {
        ThrowIfDisposed();

        if (FenceInfo.IsSignalled)
        {
            ResetUnsafe();
            FenceInfo.IsSignalled = false;
        }
    }

    /// <summary>Waits for the fence to transition to the signalled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signalled state before failing.</param>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    /// <exception cref="TimeoutException"><paramref name="millisecondsTimeout" /> was reached before the fence transitioned to the signalled state.</exception>
    /// <remarks>This method treats <see cref="uint.MaxValue" /> as having no timeout.</remarks>
    public void Wait(uint millisecondsTimeout = uint.MaxValue)
    {
        if (!TryWait(millisecondsTimeout))
        {
            ThrowTimeoutException(nameof(Wait), TimeSpan.FromMilliseconds(millisecondsTimeout));
        }
    }

    /// <summary>Waits for the fence to transition to the signalled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signalled state before failing.</param>
    /// <returns><c>true</c> if the fence transitioned to the signalled state; otherwise, <c>false</c>.</returns>
    /// <exception cref="ObjectDisposedException">The fence has been disposed.</exception>
    /// <remarks>This method treats <see cref="uint.MaxValue" /> as having no timeout.</remarks>
    public bool TryWait(uint millisecondsTimeout = uint.MaxValue)
    {
        ThrowIfDisposed();

        if (!FenceInfo.IsSignalled)
        {
            FenceInfo.IsSignalled = TryWaitUnsafe(millisecondsTimeout);
        }

        return FenceInfo.IsSignalled;
    }

    /// <summary>Resets the fence to the unsignalled state.</summary>
    /// <remarks>This method is unsafe because it does not perform most parameter or state validation.</remarks>
    protected abstract void ResetUnsafe();

    /// <summary>Waits for the fence to transition to the signalled state or the timeout to be reached, whichever occurs first.</summary>
    /// <param name="millisecondsTimeout">The amount of time, in milliseconds, to wait for the fence to transition to the signalled state before failing.</param>
    /// <returns><c>true</c> if the fence transitioned to the signalled state; otherwise, <c>false</c>.</returns>
    /// <remarks>
    ///     <para>This method treats <see cref="uint.MaxValue" /> as having no timeout.</para>
    ///     <para>This method is unsafe because it does not perform most parameter or state validation.</para>
    /// </remarks>
    protected abstract bool TryWaitUnsafe(uint millisecondsTimeout);
}
