// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Threading;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.X11;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Provider.X11.UI
{
    /// <summary>Provides a means of dispatching events for a thread.</summary>
    public sealed unsafe class Dispatcher : IDispatcher
    {
        private const int False = 0;
        private const int NoExpose = 14;

        private readonly DispatchProvider _dispatchProvider;
        private readonly Thread _parentThread;
        private readonly Lazy<UIntPtr> _windowProviderProperty;

        internal Dispatcher(DispatchProvider dispatchProvider, Thread parentThread)
        {
            Assert(dispatchProvider != null, Resources.ArgumentNullExceptionMessage, nameof(dispatchProvider));
            Assert(parentThread != null, Resources.ArgumentNullExceptionMessage, nameof(parentThread));

            _dispatchProvider = dispatchProvider!;
            _parentThread = parentThread!;
            _windowProviderProperty = new Lazy<UIntPtr>(CreateWindowProviderProperty, isThreadSafe: true);
        }

        /// <summary>Occurs when an exit event is dispatched from the queue.</summary>
        public event EventHandler? ExitRequested;

        /// <summary>Gets the <see cref="IDispatchProvider" /> associated with the instance.</summary>
        public IDispatchProvider DispatchProvider => _dispatchProvider;

        /// <summary>Gets the handle for the instance.</summary>
        public IntPtr Handle => (IntPtr)(void*)_windowProviderProperty.Value;

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

            var display = (XDisplay*)_dispatchProvider.Display;

            while (XPending(display) != 0)
            {
                XEvent xevent;
                _ = XNextEvent(display, &xevent);

                if (xevent.type != NoExpose)
                {
                    WindowProvider.ForwardWindowEvent(_windowProviderProperty.Value, in xevent);
                }
            }
        }

        private UIntPtr CreateWindowProviderProperty()
        {
            var display = (XDisplay*)_dispatchProvider.Display;

            var name = stackalloc ulong[6];
            {
                name[0] = 0x2E58466172726554;   // TerraFX.
                name[1] = 0x72656469766F7250;   // Provider
                name[2] = 0x6E69572E3131582E;   // .X11.Win
                name[3] = 0x646E69572E776F64;   // dow.Wind
                name[4] = 0x6567616E614D776F;   // owManage
                name[5] = 0x0000000000000072;   // r
            };

            return XInternAtom(
                display,
                (sbyte*)name,
                False
            );
        }

        private void OnExitRequested() => ExitRequested?.Invoke(this, EventArgs.Empty);
    }
}
