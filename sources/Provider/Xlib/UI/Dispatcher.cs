// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.Xlib.UI
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class Dispatcher : IDispatcher
    {
        private readonly DispatchProvider _dispatchProvider;
        private readonly Thread _parentThread;

        internal Dispatcher(DispatchProvider dispatchProvider, Thread parentThread)
        {
            Assert(dispatchProvider != null, Resources.ArgumentNullExceptionMessage, nameof(dispatchProvider));
            Assert(parentThread != null, Resources.ArgumentNullExceptionMessage, nameof(parentThread));

            _dispatchProvider = dispatchProvider!;
            _parentThread = parentThread!;
        }

        /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
        public event EventHandler? ExitRequested;

        /// <summary>Gets the <see cref="IDispatchProvider" /> associated with the instance.</summary>
        public IDispatchProvider DispatchProvider => _dispatchProvider;

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread => _parentThread;

        /// <summary>Dispatches all events currently pending in the queue.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <remarks>
        ///   <para>This method does not wait for a new event to be raised if the queue is empty.</para>
        ///   <para>This method does not performing any translation or pre-processing on the dispatched events.</para>
        ///   <para>This method will continue dispatching pending events even after the <see cref="ExitRequested" /> event is raised.</para>
        /// </remarks>
        public void DispatchPending()
        {
            ThrowIfNotThread(_parentThread);

            var display = _dispatchProvider.Display;
            var wmProtocolsAtom = _dispatchProvider.WmProtocolsAtom;
            var dispatcherExitRequestedAtom = (IntPtr)(void*)_dispatchProvider.DispatcherExitRequestedAtom;

            while (XPending(display) != 0)
            {
                XEvent xevent;
                _ = XNextEvent(display, &xevent);

                var isWmProtocolsEvent = (xevent.type == ClientMessage) && (xevent.xclient.format == 32) && (xevent.xclient.message_type == wmProtocolsAtom);

                if (!isWmProtocolsEvent || (xevent.xclient.data.l[0] != dispatcherExitRequestedAtom))
                {
                    WindowProvider.ForwardWindowEvent(_dispatchProvider, &xevent, isWmProtocolsEvent);
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
