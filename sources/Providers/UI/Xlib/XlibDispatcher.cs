// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI.Providers.Xlib
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class XlibDispatcher : IDispatcher
    {
        private readonly XlibDispatchProvider _dispatchProvider;
        private readonly Thread _parentThread;

        internal XlibDispatcher(XlibDispatchProvider dispatchProvider, Thread parentThread)
        {
            Assert(dispatchProvider != null, Resources.ArgumentNullExceptionMessage, nameof(dispatchProvider));
            Assert(parentThread != null, Resources.ArgumentNullExceptionMessage, nameof(parentThread));

            _dispatchProvider = dispatchProvider!;
            _parentThread = parentThread!;
        }

        /// <inheritdoc />
        public event EventHandler? ExitRequested;

        /// <inheritdoc />
        public IDispatchProvider DispatchProvider => _dispatchProvider;

        /// <inheritdoc />
        public Thread ParentThread => _parentThread;

        /// <inheritdoc />
        public void DispatchPending()
        {
            ThrowIfNotThread(_parentThread);

            var display = _dispatchProvider.Display;
            while (XPending(display) != 0)
            {
                XEvent xevent;
                _ = XNextEvent(display, &xevent);

                var isWmProtocolsEvent = (xevent.type == ClientMessage) && (xevent.xclient.format == 32) && (xevent.xclient.message_type == _dispatchProvider.WmProtocolsAtom);

                if (!isWmProtocolsEvent || (xevent.xclient.data.l[0] != (IntPtr)(void*)_dispatchProvider.DispatcherExitRequestedAtom))
                {
                    XlibWindowProvider.ForwardWindowEvent(&xevent, isWmProtocolsEvent);
                }
                else
                {
                    OnExitRequested();
                }
            }
        }

        private void OnExitRequested() => ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}
