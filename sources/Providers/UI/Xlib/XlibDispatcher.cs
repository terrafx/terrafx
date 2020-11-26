// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI.Providers.Xlib
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class XlibDispatcher : Dispatcher
    {
        internal XlibDispatcher(XlibDispatchProvider dispatchProvider, Thread parentThread)
            : base(dispatchProvider, parentThread) { }

        /// <inheritdoc />
        public override event EventHandler? ExitRequested;

        /// <inheritdoc cref="Dispatcher.DispatchProvider" />
        public new XlibDispatchProvider DispatchProvider => (XlibDispatchProvider)base.DispatchProvider;

        /// <inheritdoc />
        public override void DispatchPending()
        {
            ThrowIfNotThread(ParentThread);
            var display = DispatchProvider.Display;

            while (XPending(display) != 0)
            {
                XEvent xevent;
                _ = XNextEvent(display, &xevent);
                XlibWindowProvider.ForwardWindowEvent(&xevent);
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing) { }

        internal void OnExitRequested() => ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}
