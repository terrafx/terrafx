// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Threading;
using static TerraFX.Interop.Xlib;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.UI.XlibAtomId;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.UnsafeUtilities;

namespace TerraFX.UI
{
    /// <summary>Provides access to an X11 based window subsystem.</summary>
    public sealed unsafe class XlibWindowService : WindowService
    {
        private const string VulkanRequiredExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.RequiredExtensionNames";

        private readonly ThreadLocal<Dictionary<nuint, XlibWindow>> _windows;

        private ValueLazy<GCHandle> _nativeHandle;
        private VolatileState _state;

        /// <summary>Initializes a new instance of the <see cref="XlibWindowService" /> class.</summary>
        public XlibWindowService()
        {
            var vulkanRequiredExtensionNamesDataName = AppContext.GetData(VulkanRequiredExtensionNamesDataName) as string;
            vulkanRequiredExtensionNamesDataName += ";VK_KHR_surface;VK_KHR_xlib_surface";
            AppDomain.CurrentDomain.SetData(VulkanRequiredExtensionNamesDataName, vulkanRequiredExtensionNamesDataName);

            _nativeHandle = new ValueLazy<GCHandle>(() => GCHandle.Alloc(this, GCHandleType.Normal));
            _windows = new ThreadLocal<Dictionary<nuint, XlibWindow>>(trackAllValues: true);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="XlibWindowService" /> class.</summary>
        ~XlibWindowService() => Dispose(isDisposing: false);

        /// <inheritdoc />
        public override DispatchService DispatchService => XlibDispatchService.Instance;

        /// <summary>Gets the <see cref="GCHandle" /> containing the native handle for the instance.</summary>
        public GCHandle NativeHandle
        {
            get
            {
                AssertNotDisposedOrDisposing(_state);
                return _nativeHandle.Value;
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override IEnumerable<Window> WindowsForCurrentThread
        {
            get
            {
                ThrowIfDisposedOrDisposing(_state, nameof(XlibWindowService));
                return _windows.Value?.Values ?? Enumerable.Empty<XlibWindow>();
            }
        }

        /// <inheritdoc />
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public override Window CreateWindow()
        {
            ThrowIfDisposedOrDisposing(_state, nameof(XlibWindowService));
            return new XlibWindow(this);
        }

        internal static void ForwardWindowEvent(XEvent* xevent)
        {
            nint userData;
            GCHandle gcHandle;
            XlibWindowService windowService;
            Dictionary<nuint, XlibWindow>? windows;
            XlibWindow? window;
            bool forwardMessage;

            var dispatchService = XlibDispatchService.Instance;

            if ((xevent->type == ClientMessage) && (xevent->xclient.format == 32) && ((xevent->xclient.message_type == dispatchService.GetAtom(_TERRAFX_CREATE_WINDOW)) || (xevent->xclient.message_type == dispatchService.GetAtom(_TERRAFX_DISPOSE_WINDOW))))
            {
                // We allow the create and dispose message to be forwarded to the Window instance for xevent->xany.window.
                // This allows some delayed initialization to occur since most of the fields in Window are lazy.

                userData = Environment.Is64BitProcess
                         ? (xevent->xclient.data.l[1] << 32) | unchecked((nint)(uint)xevent->xclient.data.l[0])
                         : xevent->xclient.data.l[0];

                // Unlike the WindowService GCHandle, the Window GCHandle is short lived and
                // we want to free it after we add the relevant entries to the window map.

                gcHandle = GCHandle.FromIntPtr(userData);
                {
                    window = (XlibWindow)gcHandle.Target!;
                    windowService = window.WindowService;
                    windows = windowService._windows.Value!;
                }
                gcHandle.Free();

                if (xevent->xclient.message_type == dispatchService.GetAtom(_TERRAFX_CREATE_WINDOW))
                {
                    if (windows is null)
                    {
                        windows = new Dictionary<nuint, XlibWindow>(capacity: 4);
                        windowService._windows.Value = windows;
                    }
                    windows.Add(xevent->xany.window, window);

                    // We then want to ensure the window service is registered as a property for fast
                    // subsequent lookups. This proocess also allows everything to be lazily initialized.

                    gcHandle = windowService.NativeHandle;
                    userData = GCHandle.ToIntPtr(gcHandle);

                    _ = XChangeProperty(
                        xevent->xany.display,
                        xevent->xany.window,
                        dispatchService.GetAtom(_TERRAFX_WINDOWSERVICE),
                        dispatchService.GetAtom(_TERRAFX_NATIVE_INT),
                        8,
                        PropModeReplace,
                        (byte*)&userData,
                        sizeof(nint)
                    );
                }
                else
                {
                    Assert(AssertionsEnabled && (xevent->xclient.message_type == dispatchService.GetAtom(_TERRAFX_DISPOSE_WINDOW)));
                    _ = RemoveWindow(windows, xevent->xany.display, xevent->xany.window, dispatchService);
                }

                forwardMessage = true;
            }
            else
            {
                nuint actualType;
                int actualFormat;
                nuint itemCount;
                nuint bytesRemaining;
                nint* pUserData;

                // We don't check the result as there are various cases where this might fail
                // For example, if the property doesn't exist or has already been deleted

                _ = XGetWindowProperty(
                    xevent->xany.display,
                    xevent->xany.window,
                    dispatchService.GetAtom(_TERRAFX_WINDOWSERVICE),
                    0,
                    sizeof(nint) / sizeof(int),
                    False,
                    dispatchService.GetAtom(_TERRAFX_NATIVE_INT),
                    &actualType,
                    &actualFormat,
                    &itemCount,
                    &bytesRemaining,
                    (byte**)&pUserData
                );

                if ((actualType == dispatchService.GetAtom(_TERRAFX_NATIVE_INT)) && (actualFormat == 8) && (itemCount == SizeOf<nuint>()) && (bytesRemaining == 0))
                {
                    userData = pUserData[0];
                    gcHandle = GCHandle.FromIntPtr(userData);

                    windowService = (XlibWindowService)gcHandle.Target!;
                    windows = windowService._windows.Value!;

                    forwardMessage = windows.TryGetValue(xevent->xany.window, out window);
                }
                else
                {
                    windows = null;
                    window = null;
                    forwardMessage = false;
                }
            }

            if (forwardMessage)
            {
                AssertNotNull(windows);
                AssertNotNull(window);

                window.ProcessWindowEvent(xevent);

                if (xevent->type == DestroyNotify)
                {
                    _ = RemoveWindow(windows, xevent->xany.display, xevent->xany.window, dispatchService);
                }
            }
        }

        private static XlibWindow RemoveWindow(Dictionary<nuint, XlibWindow> windows, IntPtr display, nuint windowHandle, XlibDispatchService dispatchService)
        {
            _ = windows.Remove(windowHandle, out var window);
            AssertNotNull(window);

            if (windows.Count == 0)
            {
                dispatchService.DispatcherForCurrentThread.OnExitRequested();
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
            AssertDisposing(_state);

            if (isDisposing)
            {
                var threadWindows = _windows.Values;

                for (var i = 0; i < threadWindows.Count; i++)
                {
                    var windows = threadWindows[i];

                    if (windows is not null)
                    {
                        var windowHandles = windows.Keys;

                        foreach (var windowHandle in windowHandles)
                        {
                            var dispatchService = XlibDispatchService.Instance;
                            var window = RemoveWindow(windows, dispatchService.Display, windowHandle, dispatchService);
                            window.Dispose();
                        }

                        Assert(AssertionsEnabled && (windows.Count == 0));
                    }
                }

                _windows.Dispose();
            }
        }
    }
}
