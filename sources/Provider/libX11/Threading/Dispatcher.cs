// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using TerraFX.Provider.libX11.UI;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.libX11.Threading
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class Dispatcher : IDispatcher
    {
        #region Fields
        /// <summary>The <see cref="DispatchManager" /> for the instance.</summary>
        internal readonly DispatchManager _dispatchManager;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        internal readonly Thread _parentThread;

        /// <summary>The <c>Atom</c> used to access the <c>Window</c> property containing the associated <see cref="WindowManager" />.</summary>
        internal readonly Lazy<nuint> _windowManagerProperty;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Dispatcher" /> class.</summary>
        /// <param name="dispatchManager">The <see cref="DispatchManager" /> the instance is associated with.</param>
        /// <param name="parentThread">The <see cref="Thread" /> that was used to create the instance.</param>
        internal Dispatcher(DispatchManager dispatchManager, Thread parentThread)
        {
            Assert(dispatchManager != null, Resources.ArgumentNullExceptionMessage, nameof(dispatchManager));
            Assert(parentThread != null, Resources.ArgumentNullExceptionMessage, nameof(parentThread));

            _dispatchManager = dispatchManager;
            _parentThread = parentThread;
            _windowManagerProperty = new Lazy<nuint>(CreateWindowManagerProperty, isThreadSafe: true);
        }
        #endregion

        #region TerraFX.Threading.IDispatcher Events
        /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
        public event EventHandler ExitRequested;
        #endregion

        #region Methods
        /// <summary>Creates an <c>Atom</c> for the window manager property.</summary>
        /// <returns>An <c>Atom</c> for the window manager property.</returns>
        internal nuint CreateWindowManagerProperty()
        {
            var display = _dispatchManager.Display;

            var name = stackalloc ulong[6]; {
                name[0] = 0x2E58466172726554;   // TerraFX.
                name[1] = 0x72656469766F7250;   // Provider
                name[2] = 0x2E31315862696C2E;   // .libX11.
                name[3] = 0x572E776F646E6957;   // Window.W
                name[4] = 0x6E614D776F646E69;   // indowMan
                name[5] = 0x0000000072656761;   // ager
            };

            return XInternAtom(
                display,
                (sbyte*)(name),
                only_if_exists: False
            );
        }

        /// <summary>Raises the <see cref="ExitRequested" /> event.</summary>
        internal void OnExitRequested()
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
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
            ThrowIfNotThread(_parentThread);

            var display = _dispatchManager.Display;
            XEvent xevent;

            while (XPending(display) != 0)
            {
                XNextEvent(display, &xevent);

                if (xevent.type != NoExpose)
                {
                    WindowManager.ForwardWindowEvent(_windowManagerProperty.Value, ref xevent);
                }
            }
        }
        #endregion
    }
}
