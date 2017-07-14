// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Desktop.User32;

namespace TerraFX.Provider.Win32.Threading
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
            MSG msg;

            while (PeekMessage(&msg, wMsgFilterMin: WM_NULL, wMsgFilterMax: WM_NULL, wRemoveMsg: PM_REMOVE))
            {
                TranslateMessage(&msg);
                DispatchMessage(&msg);
            }
        }
        #endregion
    }
}
