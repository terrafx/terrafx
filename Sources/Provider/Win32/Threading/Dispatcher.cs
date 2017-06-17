// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System.Threading;
using TerraFX.Interop;
using TerraFX.Interop.Desktop;
using TerraFX.Threading;

namespace TerraFX.Provider.Win32.Threading
{
    /// <summary>Provides a means of dispatching messages for a thread.</summary>
    public sealed class Dispatcher : IDispatcher
    {
        #region Fields
        private Thread _parentThread;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Dispatcher" /> class.</summary>
        /// <param name="parentThread">The <see cref="Thread" /> the instance is associated with.</param>
        internal Dispatcher(Thread parentThread)
        {
            _parentThread = parentThread;
        }
        #endregion

        #region TerraFX.Threading.IDispatcher
        /// <summary>Gets the <see cref="Thread" /> associated with the instance.</summary>
        public Thread ParentThread
        {
            get
            {
                return _parentThread;
            }
        }

        /// <summary>Dispatches all messages currently pending in the queue.</summary>
        /// <remarks>This method does not wait for a new event to be raised if the queue is empty.</remarks>
        public void DispatchPending()
        {
            while (User32.PeekMessage(out var lpMsg, wMsgFilterMin: WM.NULL, wMsgFilterMax: WM.NULL, wRemoveMsg: PM.REMOVE).Value != 0)
            {
                User32.TranslateMessage(ref lpMsg);
                User32.DispatchMessage(ref lpMsg);
            }
        }
        #endregion
    }
}
