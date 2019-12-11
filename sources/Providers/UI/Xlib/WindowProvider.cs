// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib;
using static TerraFX.UI.Providers.Xlib.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.UI.Providers.Xlib
{
    /// <summary>Provides access to an X11 based window subsystem.</summary>
    [Export(typeof(IWindowProvider))]
    [Shared]
    public sealed unsafe class WindowProvider : IDisposable, IWindowProvider
    {
        private readonly ThreadLocal<Dictionary<UIntPtr, Window>> _windows;

        private ValueLazy<GCHandle> _nativeHandle;
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="WindowProvider" /> class.</summary>
        [ImportingConstructor]
        public WindowProvider()
        {
            _nativeHandle = new ValueLazy<GCHandle>(() => GCHandle.Alloc(this, GCHandleType.Normal));
            _windows = new ThreadLocal<Dictionary<UIntPtr, Window>>(trackAllValues: true);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="WindowProvider" /> class.</summary>
        ~WindowProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        public IDispatchProvider DispatchProvider => Xlib.DispatchProvider.Instance;

        /// <summary>Gets the <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        public GCHandle NativeHandle
        {
            get
            {
                _state.AssertNotDisposedOrDisposing();
                return _nativeHandle.Value;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IEnumerable<IWindow> WindowsForCurrentThread
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windows.Value?.Values ?? Enumerable.Empty<Window>();
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IWindow CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var windows = _windows.Value;

            if (windows is null)
            {
                windows = new Dictionary<UIntPtr, Window>(capacity: 4);
                _windows.Value = windows;
            }

            var window = new Window(this);
            _ = windows.TryAdd(window.Handle, window);

            return window;
        }

        internal static void ForwardWindowEvent(XEvent* xevent, bool isWmProtocolsEvent)
        {
            IntPtr userData;

            var dispatchProvider = Xlib.DispatchProvider.Instance;

            if (isWmProtocolsEvent && (xevent->xclient.data.l[0] == (IntPtr)(void*)dispatchProvider.WindowProviderCreateWindowAtom))
            {
                // We allow the WindowProviderCreateWindowAtom message to be forwarded to the Window instance
                // for xevent->xany.window. This allows some delayed initialization to occur since most of the
                // fields in Window are lazy.

                if (Environment.Is64BitProcess)
                {
                    var lowerBits = unchecked((uint)xevent->xclient.data.l[2].ToInt64());
                    var upperBits = unchecked((ulong)(uint)xevent->xclient.data.l[3].ToInt64());
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

                ThrowExternalExceptionIfFailed(nameof(XGetWindowProperty), XGetWindowProperty(
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
                ));

                userData = *propReturn;
            }

            WindowProvider windowProvider = null!;
            Dictionary<UIntPtr, Window>? windows = null;
            var forwardMessage = false;
            Window? window = null;

            if (userData != IntPtr.Zero)
            {
                windowProvider = (WindowProvider)GCHandle.FromIntPtr(userData).Target!;
                windows = windowProvider._windows.Value;
                forwardMessage = (windows?.TryGetValue(xevent->xany.window, out window)).GetValueOrDefault();
            }

            if (forwardMessage)
            {
                Assert(windows != null, Resources.ArgumentNullExceptionMessage, nameof(windows));
                Assert(window != null, Resources.ArgumentNullExceptionMessage, nameof(window));

                window.ProcessWindowEvent(xevent, isWmProtocolsEvent);

                if (isWmProtocolsEvent && (xevent->xclient.data.l[0] == (IntPtr)(void*)dispatchProvider.WmDeleteWindowAtom))
                {
                    // We forward the WM_DELETE_WINDOW message to the corresponding Window instance
                    // so that it can still be properly disposed of in the scenario that the
                    // xevent->xany.window was destroyed externally.

                    _ = RemoveWindow(windows, xevent->xany.display, xevent->xany.window, dispatchProvider);
                }
            }
        }

        private static Window RemoveWindow(Dictionary<UIntPtr, Window> windows, UIntPtr display, UIntPtr windowHandle, DispatchProvider dispatchProvider)
        {
            _ = windows.Remove(windowHandle, out var window);
            Assert(window != null, Resources.ArgumentNullExceptionMessage, nameof(window));

            if (windows.Count == 0)
            {
                SendClientMessage(
                    display,
                    windowHandle,
                    messageType: dispatchProvider.WmProtocolsAtom,
                    message: dispatchProvider.DispatcherExitRequestedAtom
                );
            }

            return window;
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
                var threadWindows = _windows.Values;

                for (var i = 0; i < threadWindows.Count; i++)
                {
                    var windows = threadWindows[i];

                    if (windows != null)
                    {
                        var windowHandles = windows.Keys;

                        foreach (var windowHandle in windowHandles)
                        {
                            var dispatchProvider = Xlib.DispatchProvider.Instance;
                            var window = RemoveWindow(windows, dispatchProvider.Display, windowHandle, dispatchProvider);
                            window.Dispose();
                        }

                        Assert(windows.Count == 0, Resources.ArgumentOutOfRangeExceptionMessage, nameof(windows.Count), windows.Count);
                    }
                }

                _windows.Dispose();
            }
        }
    }
}
