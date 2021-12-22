// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace TerraFX.UI;

/// <summary>Provides access to a UI subsystem.</summary>
public abstract class UIService : IDisposable
{
    /// <summary>Initializes a new instance of the <see cref="UIService" /> class.</summary>
    protected UIService() { }

    /// <summary>Gets the current timestamp for the service.</summary>
    public abstract Timestamp CurrentTimestamp { get; }

    /// <summary>Gets the dispatcher associated with <see cref="Thread.CurrentThread" />.</summary>
    /// <returns>The dispatcher associated with <see cref="Thread.CurrentThread" />.</returns>
    /// <remarks>This will create a new dispatcher if one does not already exist.</remarks>
    public abstract UIDispatcher DispatcherForCurrentThread { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Gets the dispatcher associated with a thread, creating one if it does not exist.</summary>
    /// <param name="thread">The thread for which the dispatcher should be retrieved.</param>
    /// <returns>The dispatcher associated with <paramref name="thread" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
    public abstract UIDispatcher GetDispatcher(Thread thread);

    /// <summary>Gets the dispatcher associated with a thread or <c>null</c> if one does not exist.</summary>
    /// <param name="thread">The thread for which the dispatcher should be retrieved.</param>
    /// <param name="dispatcher">The dispatcher associated with <paramref name="thread" />.</param>
    /// <returns><c>true</c> if a dispatcher was found for <paramref name="thread" />; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
    public abstract bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out UIDispatcher dispatcher);

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
