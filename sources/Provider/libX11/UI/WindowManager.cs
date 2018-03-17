// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Provider.libX11.Threading;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.libX11;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.libX11.UI
{
    /// <summary>Provides a means of managing the windows created for an application.</summary>
    [Export(typeof(IWindowManager))]
    [Export(typeof(WindowManager))]
    [Shared]
    public sealed unsafe class WindowManager : IDisposable, IWindowManager
    {
        #region Fields
        /// <summary>The <see cref="DispatchManager" /> for the instance.</summary>
        internal readonly Lazy<DispatchManager> _dispatchManager;

        /// <summary>A map of <c>Window</c> to <see cref="Window" /> objects created for the instance.</summary>
        internal readonly ConcurrentDictionary<IntPtr, Window> _windows;

        /// <summary>The <see cref="State" /> of the instance.</summary>
        internal readonly State _state;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="WindowManager" /> class.</summary>
        [ImportingConstructor]
        public WindowManager(
            [Import] Lazy<DispatchManager> dispatchManager
        )
        {
            _dispatchManager = dispatchManager;
            _windows = new ConcurrentDictionary<IntPtr, Window>();
            _state.Transition(to: Initialized);
        }
        #endregion

        #region Destructors
        /// <summary>Finalizes an instance of the <see cref="WindowManager" /> class.</summary>
        ~WindowManager()
        {
            Dispose(isDisposing: false);
        }
        #endregion

        #region Properties
        /// <summary>Gets the <see cref="DispatchManager" /> for the instance.</summary>
        public DispatchManager DispatchManager
        {
            get
            {
                return _dispatchManager.Value;
            }
        }
        #endregion

        #region TerraFX.UI.IWindowManager Properties
        /// <summary>Gets the <see cref="IWindow" /> objects created by the instance.</summary>
        public IEnumerable<IWindow> Windows
        {
            get
            {
                return (IEnumerable<IWindow>)(_windows);
            }
        }
        #endregion

        #region Static Methods
        /// <summary>Forwards native window messages to the appropriate <see cref="Window" /> instance for processing.</summary>
        /// <param name="windowManagerProperty">The property used to get the <see cref="WindowManager" /> associated with the event.</param>
        /// <param name="xevent">The event to be processed.</param>
        /// <exception cref="ExternalException">The call to <see cref="XGetWindowProperty(IntPtr, nuint, nuint, nint, nint, int, nuint, nuint*, int*, nuint*, nuint*, byte**)" /> failed.</exception>
        internal static void ForwardWindowEvent(nuint windowManagerProperty, ref /* readonly */ XEvent xevent)
        {
            nuint actualType, nitems, bytesAfter;
            int actualFormat;
            IntPtr* prop;

            var result = XGetWindowProperty(
                xevent.xany.display,
                xevent.xany.window,
                windowManagerProperty,
                long_offset: 0,
                long_length: (IntPtr.Size >> 2),
                delete: False,
                req_type: 32,
                actual_type_return: &actualType,
                actual_format_return: &actualFormat,
                nitems_return: &nitems,
                bytes_after_return: &bytesAfter,
                prop_return: (byte**)(&prop)
            );

            if (result != Success)
            {
                ThrowExternalException(nameof(XGetWindowProperty), result);
            }

            var windowManager = (WindowManager)(GCHandle.FromIntPtr(*prop).Target);

            if (windowManager._windows.TryGetValue((IntPtr)(xevent.xany.window), out var window))
            {
                window.ProcessWindowEvent(ref xevent);
            }
        }
        #endregion

        #region Methods
        /// <summary>Disposes of any unmanaged resources associated with the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeWindows(isDisposing);
            }

            _state.EndDispose();
        }

        /// <summary>Disposes of all <see cref="Window" /> objects that were created by the instance.</summary>
        /// <param name="isDisposing"><c>true</c> if called from <see cref="Dispose()" />; otherwise, <c>false</c>.</param>
        internal void DisposeWindows(bool isDisposing)
        {
            if (isDisposing)
            {
                foreach (var createdWindowHandle in _windows.Keys)
                {
                    if (_windows.TryRemove(createdWindowHandle, out var createdWindow))
                    {
                        createdWindow.Dispose();
                    }
                }
            }
            else
            {
                _windows.Clear();
            }
        }
        #endregion

        #region System.IDisposable Methods
        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region TerraFX.UI.IWindowManager Methods
        /// <summary>Create a new <see cref="IWindow"/> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var window = new Window(this);
            _windows.TryAdd(window.Handle, window);

            return window;
        }
        #endregion
    }
}
