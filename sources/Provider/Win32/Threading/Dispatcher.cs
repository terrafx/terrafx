// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Threading;
using static TerraFX.Interop.User32;
using static TerraFX.Interop.Windows;
using static TerraFX.Interop.Desktop.User32;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.Win32.Threading
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class Dispatcher : IDispatcher
    {
        #region Fields
        /// <summary>The <see cref="DispatchManager" /> for the instance.</summary>
        private readonly DispatchManager _dispatchManager;

        /// <summary>The <see cref="Thread" /> that was used to create the instance.</summary>
        private readonly Thread _parentThread;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="Dispatcher" /> class.</summary>
        /// <param name="dispatchManager">The <see cref="DispatchManager" /> for the instance.</param>
        /// <param name="parentThread">The <see cref="Thread" /> that was used to create the instance.</param>
        internal Dispatcher(DispatchManager dispatchManager, Thread parentThread)
        {
            Assert(dispatchManager != null, Resources.ArgumentNullExceptionMessage, nameof(dispatchManager));
            Assert(parentThread != null, Resources.ArgumentNullExceptionMessage, nameof(parentThread));

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
        private void OnExitRequested()
        {
            ExitRequested?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region TerraFX.Threading.IDispatcher Properties
        /// <summary>Gets the <see cref="IDispatchManager" /> for the instance.</summary>
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

            while (PeekMessage(out var msg, wMsgFilterMin: WM_NULL, wMsgFilterMax: WM_NULL, wRemoveMsg: PM_REMOVE) != FALSE)
            {
                if (msg.message != WM_QUIT)
                {
                    DispatchMessage(in msg);
                }
                else
                {
                    OnExitRequested();
                }
            }
        }
        #endregion
    }
}
