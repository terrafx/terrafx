// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Diagnostics;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Provider.libX11.UI;
using TerraFX.Threading;
using static TerraFX.Interop.libX11;

namespace TerraFX.Provider.libX11.Threading
{
    /// <summary>Provides a means of dispatching messages for a thread.</summary>
    unsafe public sealed class Dispatcher : IDispatcher
    {
        #region Fields
        internal readonly DispatchManager _dispatchManager;

        internal readonly Thread _parentThread;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Dispatcher" /> class.</summary>
        /// <param name="dispatchManager">The <see cref="DispatchManager" /> the instance is associated with.</param>
        /// <param name="parentThread">The <see cref="Thread" /> the instance is associated with.</param>
        internal Dispatcher(DispatchManager dispatchManager, Thread parentThread)
        {
            _dispatchManager = dispatchManager;
            _parentThread = parentThread;
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

        /// <summary>Gets the <see cref="Thread" /> associated with the instance.</summary>
        public Thread ParentThread
        {
            get
            {
                return _parentThread;
            }
        }
        #endregion

        #region TerraFX.Threading.IDispatcher Methods
        /// <summary>Dispatches all messages currently pending in the queue.</summary>
        /// <remarks>This method does not wait for a new event to be raised if the queue is empty.</remarks>
        public void DispatchPending()
        {
            var display = _dispatchManager.Display;
            var createdWindows = WindowManager._createdWindows;

            while (XPending(display) != 0)
            {
                XEvent xevent;
                XNextEvent(display, &xevent);
                Debug.Assert(xevent.xany.display == display);

                var xwindow = xevent.xany.window;

                if (createdWindows.TryGetValue(xwindow, out var window) == false)
                {
                    // Don't process the event if there is not a window to send it to
                    // TODO: There are likely some events we should process anyways
                    continue;
                }

                window.ProcessXEvent(ref xevent);
            }
        }
        #endregion
    }
}
