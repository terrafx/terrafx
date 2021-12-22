// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI;

/// <summary>Provides a means of dispatching events for a thread.</summary>
public abstract class UIDispatcher : UIServiceObject
{
    private readonly Thread _parentThread;

    /// <summary>Initializes a new instance of the <see cref="UIDispatcher" /> class.</summary>
    /// <param name="service">The service for which the dispatcher is being created.</param>
    /// <param name="parentThread">The thread on which the dispatcher operates.</param>
    /// <exception cref="ArgumentNullException"><paramref name="service" /> is <c>null</c>.</exception>
    protected UIDispatcher(UIService service, Thread parentThread)
        : base(service)
    {
        ThrowIfNull(parentThread);
        _parentThread = parentThread;
    }

    /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
    public event EventHandler? ExitRequested;

    /// <summary>Gets the thread that was used to create the dispatcher.</summary>
    public Thread ParentThread => _parentThread;

    /// <summary>Gets the windows created for the dispatcher.</summary>
    public abstract IEnumerable<Window> Windows { get; }

    /// <summary>Create a new window which utilizes the dispatcher.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The dispatcher has been disposed.</exception>
    public abstract Window CreateWindow();

    /// <summary>Dispatches all events currently pending in the queue.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The dispatcher has been disposed.</exception>
    /// <remarks>
    ///   <para>This method does not wait for a new event to be raised if the queue is empty.</para>
    ///   <para>This method does not performing any translation or pre-processing on the dispatched events.</para>
    ///   <para>This method will continue dispatching pending events even after the <see cref="ExitRequested" /> event is raised.</para>
    /// </remarks>
    public abstract void DispatchPending();

    /// <summary>Requests that the dispatcher exit by posting the appropriate event to the dispatch queue.</summary>
    /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
    /// <exception cref="ObjectDisposedException">The dispatcher has been disposed.</exception>
    public abstract void RequestExit();

    /// <summary>Raises the <see cref="ExitRequested" /> event.</summary>
    protected void RaiseExitRequested()
    {
        AssertThread(ParentThread);
        ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}
