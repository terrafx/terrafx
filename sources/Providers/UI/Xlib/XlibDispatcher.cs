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
            : base(dispatchProvider, parentThread)
        {
        }

        /// <inheritdoc />
        public override event EventHandler? ExitRequested;

        /// <inheritdoc />
        public override void DispatchPending()
        {
            ThrowIfNotThread(ParentThread);

            var display = ((XlibDispatchProvider)DispatchProvider).Display;
            while (XPending(display) != 0)
            {
                XEvent xevent;
                _ = XNextEvent(display, &xevent);

                var isWmProtocolsEvent = (xevent.type == ClientMessage) && (xevent.xclient.format == 32) && (xevent.xclient.message_type == ((XlibDispatchProvider)DispatchProvider).WmProtocolsAtom);

                if (!isWmProtocolsEvent || (xevent.xclient.data.l[0] != (IntPtr)(void*)((XlibDispatchProvider)DispatchProvider).DispatcherExitRequestedAtom))
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

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
        }
    }
}
