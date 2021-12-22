// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI;

/// <summary>An object which is created for a dispatcher.</summary>
public abstract class UIDispatcherObject : IUIDispatcherObject
{
    private readonly UIDispatcher _dispatcher;
    private readonly Thread _parentThread;
    private readonly UIService _service;

    /// <summary>Initializes a new instance of the <see cref="UIDispatcherObject" /> class.</summary>
    /// <param name="dispatcher">The dispatcher for which the object is being created.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dispatcher" /> is <c>null</c>.</exception>
    protected UIDispatcherObject(UIDispatcher dispatcher)
    {
        ThrowIfNull(dispatcher);

        _dispatcher = dispatcher;
        _parentThread = dispatcher.ParentThread;
        _service = dispatcher.Service;
    }

    /// <summary>Gets the dispatcher for which the object was created.</summary>
    public UIDispatcher Dispatcher => _dispatcher;

    /// <inheritdoc />
    public Thread ParentThread => _parentThread;

    /// <summary>Gets the underlying service for <see cref="Dispatcher" />.</summary>
    public UIService Service => _service;

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(isDisposing: true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="Dispose()" />
    /// <param name="isDisposing"><c>true</c> if the method was called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
    protected abstract void Dispose(bool isDisposing);
}
