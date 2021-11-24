// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop.Xlib;
using static TerraFX.Interop.Xlib.Xlib;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI;

/// <summary>Provides a means of dispatching events for a thread.</summary>
public sealed unsafe class XlibDispatcher : Dispatcher
{
    internal XlibDispatcher(XlibDispatchService dispatchService, Thread parentThread)
        : base(dispatchService, parentThread) { }

    /// <inheritdoc />
    public override event EventHandler? ExitRequested;

    /// <inheritdoc cref="Dispatcher.DispatchService" />
    public new XlibDispatchService DispatchService => (XlibDispatchService)base.DispatchService;

    /// <inheritdoc />
    public override void DispatchPending()
    {
        ThrowIfNotThread(ParentThread);
        var display = DispatchService.Display;

        while (XPending(display) != 0)
        {
            XEvent xevent;
            _ = XNextEvent(display, &xevent);
            XlibWindowService.ForwardWindowEvent(&xevent);
        }
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing) { }

    internal void OnExitRequested() => ExitRequested?.Invoke(this, EventArgs.Empty);
}
