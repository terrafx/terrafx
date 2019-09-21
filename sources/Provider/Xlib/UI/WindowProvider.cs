// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Composition;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Xlib.UI
{
    /// <summary>Provides access to an X11 based window subsystem.</summary>
    [Export(typeof(IWindowProvider))]
    [Export(typeof(WindowProvider))]
    [Shared]
    public sealed unsafe class WindowProvider : IDisposable, IWindowProvider
    {
        private readonly ConcurrentDictionary<UIntPtr, Window> _windows;

        private ResettableLazy<GCHandle> _nativeHandle;
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="WindowProvider" /> class.</summary>
        [ImportingConstructor]
        public WindowProvider()
        {
            _nativeHandle = new ResettableLazy<GCHandle>(() => GCHandle.Alloc(this, GCHandleType.Normal));
            _windows = new ConcurrentDictionary<UIntPtr, Window>();
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="WindowProvider" /> class.</summary>
        ~WindowProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="IDispatchProvider" /> for the instance.</summary>
        public IDispatchProvider DispatchProvider => UI.DispatchProvider.Instance;

        /// <summary>Gets the <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        public GCHandle NativeHandle
        {
            get
            {
                _state.AssertNotDisposedOrDisposing();
                return _nativeHandle.Value;
            }
        }

        /// <summary>Gets the <see cref="IWindow" /> objects created by the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IEnumerable<IWindow> Windows
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windows.Values;
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Create a new <see cref="IWindow" /> instance.</summary>
        /// <returns>A new <see cref="IWindow" /> instance</returns>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var window = new Window(this);
            _ = _windows.TryAdd(window.Handle, window);

            return window;
        }

        internal static void ForwardWindowEvent(XEvent* xevent, bool isWmProtocolsEvent)
        {
            IntPtr userData;

            var dispatchProvider = UI.DispatchProvider.Instance;

            if (isWmProtocolsEvent && (xevent->xclient.data.l[0] == (IntPtr)(void*)dispatchProvider.WindowProviderCreateWindowAtom))
            {
                // We allow the WindowProviderCreateWindowAtom message to be forwarded to the Window instance
                // for xevent->xany.window. This allows some delayed initialization to occur since most of the
                // fields in Window are lazy.

                if (Environment.Is64BitProcess)
                {
                    var lowerBits = unchecked((uint)xevent->xclient.data.l[1].ToInt64());
                    var upperBits = unchecked((ulong)(uint)xevent->xclient.data.l[2].ToInt64());
                    userData = (IntPtr)((upperBits << 32) | lowerBits);
                }
                else
                {
                    var bits = xevent->xclient.data.l[1].ToInt32();
                    userData = (IntPtr)bits;
                }

                _ = XChangeProperty(
                    xevent->xany.display,
                    xevent->xany.window,
                    dispatchProvider.WindowWindowProviderAtom,
                    dispatchProvider.SystemIntPtrAtom,
                    format: 8,
                    PropModeReplace,
                    (byte*)&userData,
                    nelements: IntPtr.Size
                );
            }
            else
            {
                UIntPtr actualTypeReturn;
                int actualFormatReturn;
                UIntPtr nitemsReturn;
                UIntPtr bytesAfterReturn;
                IntPtr* propReturn;

                var status = XGetWindowProperty(
                    xevent->xany.display,
                    xevent->xany.window,
                    dispatchProvider.WindowWindowProviderAtom,
                    long_offset: IntPtr.Zero,
                    long_length: (IntPtr)IntPtr.Size,
                    delete: False,
                    dispatchProvider.SystemIntPtrAtom,
                    &actualTypeReturn,
                    &actualFormatReturn,
                    &nitemsReturn,
                    &bytesAfterReturn,
                    (byte**)&propReturn
                );

                if (status != Success)
                {
                    ThrowExternalException(nameof(XGetWindowProperty), status);
                }

                userData = *propReturn;
            }

            WindowProvider windowProvider = null!;
            var forwardMessage = false;
            Window? window = null;

            if (userData != IntPtr.Zero)
            {
                windowProvider = (WindowProvider)GCHandle.FromIntPtr(userData).Target!;
                forwardMessage = windowProvider._windows.TryGetValue(xevent->xany.window, out window);
            }

            if (forwardMessage)
            {
                if (isWmProtocolsEvent && (xevent->xclient.data.l[0] == (IntPtr)(void*)dispatchProvider.WmDeleteWindowAtom))
                {
                    // We forward the WM_DELETE_WINDOW message to the corresponding Window instance
                    // so that it can still be properly disposed of in the scenario that the
                    // xevent->xany.window was destroyed externally.

                    _ = windowProvider._windows.TryRemove(xevent->xany.window, out window);
                }

                window!.ProcessWindowEvent(xevent, isWmProtocolsEvent);
            }
        }

        private void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeWindows(isDisposing);
            }

            _state.EndDispose();
        }

        private void DisposeWindows(bool isDisposing)
        {
            _state.AssertDisposing();

            if (isDisposing)
            {
                foreach (var windowHandle in _windows.Keys)
                {
                    if (_windows.TryRemove(windowHandle, out var createdWindow))
                    {
                        createdWindow.Dispose();
                    }
                }
            }
            else
            {
                _windows.Clear();
            }

            Assert(_windows.IsEmpty, Resources.ArgumentOutOfRangeExceptionMessage, nameof(_windows.IsEmpty), _windows.IsEmpty);
        }
    }
}
