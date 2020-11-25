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
    [Export(typeof(WindowProvider))]
    [Shared]
    public sealed unsafe class XlibWindowProvider : WindowProvider
    {
        private const string VulkanRequiredExtensionNamesDataName = "TerraFX.Graphics.Providers.Vulkan.GraphicsProvider.RequiredExtensionNames";

        private readonly ThreadLocal<Dictionary<nuint, XlibWindow>> _windows;

        private ValueLazy<GCHandle> _nativeHandle;
        private State _state;

        /// <summary>Initializes a new instance of the <see cref="XlibWindowProvider" /> class.</summary>
        [ImportingConstructor]
        public XlibWindowProvider()
        {
            var vulkanRequiredExtensionNamesDataName = AppContext.GetData(VulkanRequiredExtensionNamesDataName) as string;
            vulkanRequiredExtensionNamesDataName += ";VK_KHR_surface;VK_KHR_xlib_surface";
            AppDomain.CurrentDomain.SetData(VulkanRequiredExtensionNamesDataName, vulkanRequiredExtensionNamesDataName);

            _nativeHandle = new ValueLazy<GCHandle>(() => GCHandle.Alloc(this, GCHandleType.Normal));
            _windows = new ThreadLocal<Dictionary<nuint, XlibWindow>>(trackAllValues: true);
            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="XlibWindowProvider" /> class.</summary>
        ~XlibWindowProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <inheritdoc />
        public override DispatchProvider DispatchProvider => XlibDispatchProvider.Instance;

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
        public override IEnumerable<Window> WindowsForCurrentThread
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windows.Value?.Values ?? Enumerable.Empty<XlibWindow>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override Window CreateWindow()
        {
            _state.ThrowIfDisposedOrDisposing();

            var windows = _windows.Value;

            if (windows is null)
            {
                windows = new Dictionary<nuint, XlibWindow>(capacity: 4);
                _windows.Value = windows;
            }

            var window = new XlibWindow(this);
            _ = windows.TryAdd(window.Handle, window);

            return window;
        }

        internal static void ForwardWindowEvent(XEvent* xevent, bool isWmProtocolsEvent)
        {
            nint userData;

            var dispatchProvider = Xlib.XlibDispatchProvider.Instance;

            if (isWmProtocolsEvent && (xevent->xclient.data.l[0] == (nint)dispatchProvider.WindowProviderCreateWindowAtom))
            {
                // We allow the WindowProviderCreateWindowAtom message to be forwarded to the Window instance
                // for xevent->xany.window. This allows some delayed initialization to occur since most of the
                // fields in Window are lazy.

                userData = Environment.Is64BitProcess
                         ? (xevent->xclient.data.l[3] << 32) | xevent->xclient.data.l[2]
                         : xevent->xclient.data.l[1];

                _ = XChangeProperty(
                    xevent->xany.display,
                    xevent->xany.window,
                    dispatchProvider.WindowWindowProviderAtom,
                    dispatchProvider.SystemIntPtrAtom,
                    8,
                    PropModeReplace,
                    (byte*)&userData,
                    sizeof(nint)
                );
            }
            else
            {
                nuint actualTypeReturn;
                int actualFormatReturn;
                nuint nitemsReturn;
                nuint bytesAfterReturn;
                IntPtr* propReturn;

                ThrowExternalExceptionIfFailed(XGetWindowProperty(
                    xevent->xany.display,
                    xevent->xany.window,
                    dispatchProvider.WindowWindowProviderAtom,
                    0,
                    sizeof(nint),
                    False,
                    dispatchProvider.SystemIntPtrAtom,
                    &actualTypeReturn,
                    &actualFormatReturn,
                    &nitemsReturn,
                    &bytesAfterReturn,
                    (byte**)&propReturn
                ), nameof(XGetWindowProperty));

                userData = *propReturn;
            }

            XlibWindowProvider windowProvider = null!;
            Dictionary<nuint, XlibWindow>? windows = null;
            var forwardMessage = false;
            XlibWindow? window = null;

            if (userData != 0)
            {
                windowProvider = (XlibWindowProvider)GCHandle.FromIntPtr(userData).Target!;
                windows = windowProvider._windows.Value;
                forwardMessage = (windows?.TryGetValue(xevent->xany.window, out window)).GetValueOrDefault();
            }

            if (forwardMessage)
            {
                Assert(windows != null, Resources.ArgumentNullExceptionMessage, nameof(windows));
                Assert(window != null, Resources.ArgumentNullExceptionMessage, nameof(window));

                window.ProcessWindowEvent(xevent, isWmProtocolsEvent);

                if (isWmProtocolsEvent && (xevent->xclient.data.l[0] == (nint)dispatchProvider.WmDeleteWindowAtom))
                {
                    // We forward the WM_DELETE_WINDOW message to the corresponding Window instance
                    // so that it can still be properly disposed of in the scenario that the
                    // xevent->xany.window was destroyed externally.

                    _ = RemoveWindow(windows, xevent->xany.display, xevent->xany.window, dispatchProvider);
                }
            }
        }

        private static XlibWindow RemoveWindow(Dictionary<nuint, XlibWindow> windows, IntPtr display, nuint windowHandle, XlibDispatchProvider dispatchProvider)
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

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
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
                            var dispatchProvider = Xlib.XlibDispatchProvider.Instance;
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
