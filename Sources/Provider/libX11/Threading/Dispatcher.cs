// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Diagnostics;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Provider.libX11.UI;
using TerraFX.Threading;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.libX11.Threading
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    unsafe public sealed class Dispatcher : IDispatcher
    {
        #region Fields
        /// <summary>The <see cref="DispatchManager" /> for the instance.</summary>
        internal readonly DispatchManager _dispatchManager;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Thread _parentThread;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Dispatcher" /> class.</summary>
        /// <param name="dispatchManager">The <see cref="DispatchManager" /> the instance is associated with.</param>
        /// <param name="parentThread">The <see cref="Thread" /> that was used to create the instance.</param>
        internal Dispatcher(DispatchManager dispatchManager, Thread parentThread)
        {
            Debug.Assert(dispatchManager != null);
            Debug.Assert(parentThread == Thread.CurrentThread);

            _dispatchManager = dispatchManager;
            _parentThread = parentThread;
        }
        #endregion

        #region TerraFX.Threading.IDispatcher Events
        /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
        public event EventHandler ExitRequested;
        #endregion

        #region Methods
        /// <summary>Raises the <see cref="ExitRequested" /> event.</summary>
        internal void OnExitRequested()
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Throws a <see cref="InvalidOperationException" /> if <see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        internal void ThrowIfNotParentThread()
        {
            var currentThread = Thread.CurrentThread;

            if (currentThread != _parentThread)
            {
                ThrowInvalidOperationException(nameof(Thread.CurrentThread), currentThread);
            }
        }
        #endregion

        #region TerraFX.Threading.IDispatcher Properties
        /// <summary>Gets the <see cref="IDispatchManager" /> associated with the instance.</summary>
        public IDispatchManager DispatchManager
        {
            get
            {
                return _dispatchManager;
            }
        }

        /// <summary>Gets the <see cref="Thread" /> that was used to create the instance.</summary>
        public Thread ParentThread
        {
            get
            {
                return _parentThread;
            }
        }
        #endregion

        #region TerraFX.Threading.IDispatcher Methods
        /// <summary>Dispatches all events currently pending in the queue.</summary>
        /// <exception cref="InvalidOperationException"><see cref="Thread.CurrentThread" /> is not <see cref="ParentThread" />.</exception>
        /// <remarks>
        ///     <para>This method does not wait for a new event to be raised if the queue is empty.</para>
        ///     <para>This method does not performing any translation or pre-processing on the dispatched events.</para>
        ///     <para>This method will continue dispatching pending events even after the <see cref="ExitRequested" /> event is raised.</para>
        /// </remarks>
        public void DispatchPending()
        {
            ThrowIfNotParentThread();

            var display = _dispatchManager.Display;
            var createdWindows = WindowManager.CreatedWindows;

            XEvent xevent;

            while (XPending(display) != 0)
            {
                XNextEvent(display, &xevent);
                Debug.Assert(xevent.xany.display == display);

                if (createdWindows.TryGetValue(xevent.xany.window, out var window))
                {
                    window.ProcessXEvent(ref xevent);
                }

                // TODO: There are likely some events we should process even if there is no associated window
            }
        }
        #endregion
    }
}
