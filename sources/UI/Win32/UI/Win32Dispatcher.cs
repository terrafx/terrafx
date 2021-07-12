// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using static TerraFX.Interop.Windows;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class Win32Dispatcher : Dispatcher
    {
        internal Win32Dispatcher(Win32DispatchService dispatchService, Thread parentThread)
            : base(dispatchService, parentThread) { }

        /// <inheritdoc />
        public override event EventHandler? ExitRequested;

        /// <inheritdoc />
        public override void DispatchPending()
        {
            ThrowIfNotThread(ParentThread);

            MSG msg;
            while (PeekMessageW(&msg, hWnd: IntPtr.Zero, wMsgFilterMin: WM_NULL, wMsgFilterMax: WM_NULL, wRemoveMsg: PM_REMOVE) != FALSE)
            {
                if (msg.message != WM_QUIT)
                {
                    _ = DispatchMessageW(&msg);
                }
                else
                {
                    OnExitRequested();
                }
            }
        }

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing) { }

        private void OnExitRequested() => ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}
