// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Libc;
using static TerraFX.Interop.Xlib;
using static TerraFX.UI.Providers.Xlib.HelperUtilities;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.UI.Providers.Xlib
{
    /// <summary>Provides access to an X11 based dispatch subsystem.</summary>
    public sealed unsafe class XlibDispatchProvider : DispatchProvider
    {
        private static readonly delegate* unmanaged<IntPtr, XErrorEvent*, int> s_errorHandler = (delegate* unmanaged<IntPtr, XErrorEvent*, int> )(delegate*<IntPtr, XErrorEvent*, int> )&HandleXlibError;
        private static ValueLazy<XlibDispatchProvider> s_instance = new ValueLazy<XlibDispatchProvider>(CreateDispatchProvider);

        private readonly ConcurrentDictionary<Thread, Dispatcher> _dispatchers;

        private ValueLazy<nuint> _dispatcherExitRequestedAtom;
        private ValueLazy<IntPtr> _display;
        private ValueLazy<nuint> _systemIntPtrAtom;
        private ValueLazy<nuint> _windowProviderCreateWindowAtom;
        private ValueLazy<nuint> _windowWindowProviderAtom;
        private ValueLazy<nuint> _wmProtocolsAtom;
        private ValueLazy<nuint> _wmDeleteWindowAtom;

        private State _state;

        private XlibDispatchProvider()
        {
            _dispatchers = new ConcurrentDictionary<Thread, Dispatcher>();

            _dispatcherExitRequestedAtom = new ValueLazy<nuint>(CreateDispatcherExitRequestedAtom);
            _display = new ValueLazy<IntPtr>(CreateDisplay);
            _systemIntPtrAtom = new ValueLazy<nuint>(CreateSystemIntPtrAtom);
            _windowProviderCreateWindowAtom = new ValueLazy<nuint>(CreateWindowProviderCreateWindowAtom);
            _windowWindowProviderAtom = new ValueLazy<nuint>(CreateWindowWindowProviderAtom);
            _wmProtocolsAtom = new ValueLazy<nuint>(CreateWmProtocolsAtom);
            _wmDeleteWindowAtom = new ValueLazy<nuint>(CreateWmDeleteWindowAtom);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="XlibDispatchProvider" /> class.</summary>
        ~XlibDispatchProvider() => Dispose(isDisposing: false);

        /// <summary>Gets the <see cref="XlibDispatchProvider" /> instance for the current program.</summary>
        public static XlibDispatchProvider Instance => s_instance.Value;

        // TerraFX.Provider.Xlib.UI.Dispatcher.ExitRequested
        private static ReadOnlySpan<byte> DispatcherExitRequestedAtomName => new byte[] {
            0x54,
            0x65,
            0x72,
            0x72,
            0x61,
            0x46,
            0x58,
            0x2E,
            0x50,
            0x72,
            0x6F,
            0x76,
            0x69,
            0x64,
            0x65,
            0x72,
            0x2E,
            0x58,
            0x6C,
            0x69,
            0x62,
            0x2E,
            0x55,
            0x49,
            0x2E,
            0x44,
            0x69,
            0x73,
            0x70,
            0x61,
            0x74,
            0x63,
            0x68,
            0x65,
            0x72,
            0x2E,
            0x45,
            0x78,
            0x69,
            0x74,
            0x52,
            0x65,
            0x71,
            0x75,
            0x65,
            0x73,
            0x74,
            0x65,
            0x64,
            0x00
        };

        // System.IntPtr
        private static ReadOnlySpan<byte> SystemIntPtrAtomName => new byte[] {
            0x53,
            0x79,
            0x73,
            0x74,
            0x65,
            0x6D,
            0x2E,
            0x49,
            0x6E,
            0x74,
            0x50,
            0x74,
            0x72,
            0x00
        };

        // TerraFX.Provider.Xlib.UI.WindowProvider.CreateWindow
        private static ReadOnlySpan<byte> WindowProviderCreateWindowAtomName => new byte[] {
            0x54,
            0x65,
            0x72,
            0x72,
            0x61,
            0x46,
            0x58,
            0x2E,
            0x50,
            0x72,
            0x6F,
            0x76,
            0x69,
            0x64,
            0x65,
            0x72,
            0x2E,
            0x58,
            0x6C,
            0x69,
            0x62,
            0x2E,
            0x55,
            0x49,
            0x2E,
            0x57,
            0x69,
            0x6E,
            0x64,
            0x6F,
            0x77,
            0x50,
            0x72,
            0x6F,
            0x76,
            0x69,
            0x64,
            0x65,
            0x72,
            0x2E,
            0x43,
            0x72,
            0x65,
            0x61,
            0x74,
            0x65,
            0x57,
            0x69,
            0x6E,
            0x64,
            0x6F,
            0x77,
            0x00
        };

        // TerraFX.Provider.Xlib.UI.Window.WindowProvider
        private static ReadOnlySpan<byte> WindowWindowProviderAtomName => new byte[] {
            0x54,
            0x65,
            0x72,
            0x72,
            0x61,
            0x46,
            0x58,
            0x2E,
            0x50,
            0x72,
            0x6F,
            0x76,
            0x69,
            0x64,
            0x65,
            0x72,
            0x2E,
            0x58,
            0x6C,
            0x69,
            0x62,
            0x2E,
            0x55,
            0x49,
            0x2E,
            0x57,
            0x69,
            0x6E,
            0x64,
            0x6F,
            0x77,
            0x2E,
            0x57,
            0x69,
            0x6E,
            0x64,
            0x6F,
            0x77,
            0x50,
            0x72,
            0x6F,
            0x76,
            0x69,
            0x64,
            0x65,
            0x72,
            0x00
        };

        // WM_PROTOCOLS
        private static ReadOnlySpan<byte> WmProtocolsAtomName => new byte[] {
            0x57,
            0x4D,
            0x5F,
            0x50,
            0x52,
            0x4F,
            0x54,
            0x4F,
            0x43,
            0x4F,
            0x4C,
            0x53,
            0x00
        };

        // WM_DELETE_WINDOW
        private static ReadOnlySpan<byte> WmDeleteWindowAtomName => new byte[] {
            0x57,
            0x4D,
            0x5F,
            0x44,
            0x45,
            0x4C,
            0x45,
            0x54,
            0x45,
            0x5F,
            0x57,
            0x49,
            0x4E,
            0x44,
            0x4F,
            0x57,
            0x00
        };

        /// <inheritdoc />
        /// <exception cref="ExternalException">The call to <see cref="clock_gettime(int, timespec*)" /> failed.</exception>
        public override Timestamp CurrentTimestamp
        {
            get
            {
                timespec timespec;
                var result = clock_gettime(CLOCK_MONOTONIC, &timespec);

                if (result != 0)
                {
                    ThrowExternalExceptionForLastError(nameof(clock_gettime));
                }

                const long NanosecondsPerSecond = TimeSpan.TicksPerSecond * 100;
                Assert(NanosecondsPerSecond == 1000000000, Resources.ArgumentOutOfRangeExceptionMessage, nameof(NanosecondsPerSecond), NanosecondsPerSecond);

                var ticks = (long)timespec.tv_sec;
                {
                    ticks *= NanosecondsPerSecond;
                    ticks += (long)timespec.tv_nsec;
                    ticks /= 100;
                }
                return new Timestamp(ticks);
            }
        }

        /// <summary>Gets the atom created to track the <see cref="XlibDispatcher.ExitRequested" /> event.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public nuint DispatcherExitRequestedAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _dispatcherExitRequestedAtom.Value;
            }
        }

        /// <inheritdoc />
        public override Dispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

        /// <summary>Gets the <c>Display</c> that was created for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public IntPtr Display
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _display.Value;
            }
        }

        /// <summary>Gets the atom created to track the <see cref="IntPtr" /> type.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public nuint SystemIntPtrAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _systemIntPtrAtom.Value;
            }
        }

        /// <summary>Gets the atom created to track the <see cref="XlibWindowProvider.CreateWindow" /> method.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public nuint WindowProviderCreateWindowAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windowProviderCreateWindowAtom.Value;
            }
        }

        /// <summary>Gets the atom created to track the <see cref="Window.WindowProvider" /> property.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public nuint WindowWindowProviderAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windowWindowProviderAtom.Value;
            }
        }

        /// <summary>Gets the atom created for the <c>WM_PROTOCOLS</c> client message.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public nuint WmProtocolsAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _wmProtocolsAtom.Value;
            }
        }

        /// <summary>Gets the atom created for the <c>WM_DELETE_WINDOW</c> client message.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public nuint WmDeleteWindowAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _wmDeleteWindowAtom.Value;
            }
        }

        /// <inheritdoc />
        public override Dispatcher GetDispatcher(Thread thread)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.GetOrAdd(thread, (parentThread) => new XlibDispatcher(this, parentThread));
        }

        /// <inheritdoc />
        public override bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out Dispatcher dispatcher)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.TryGetValue(thread, out dispatcher!);
        }

        private static XlibDispatchProvider CreateDispatchProvider() => new XlibDispatchProvider();

        private static int HandleXlibError(IntPtr display, XErrorEvent* errorEvent)
        {
            // Due to the asynchronous nature of Xlib, there can be a race between
            // the window being deleted and it being unmapped. This ignores the warning
            // raised by the unmap event in that scenario, as the call to XGetWindowProperty
            // will fail.

            if ((errorEvent->error_code != BadWindow) || (errorEvent->request_code != X_GetProperty))
            {
                ThrowExternalException(nameof(HandleXlibError), errorEvent->error_code);
            }

            return 0;
        }

        private static IntPtr CreateDisplay()
        {
            var display = XOpenDisplay(null);
            ThrowExternalExceptionIfZero(nameof(XOpenDisplay), display);

            _ = XSetErrorHandler(s_errorHandler);
            return display;
        }

        private nuint CreateDispatcherExitRequestedAtom() => CreateAtom(Display, DispatcherExitRequestedAtomName);

        private nuint CreateSystemIntPtrAtom() => CreateAtom(Display, SystemIntPtrAtomName);

        private nuint CreateWindowProviderCreateWindowAtom() => CreateAtom(Display, WindowProviderCreateWindowAtomName);

        private nuint CreateWindowWindowProviderAtom() => CreateAtom(Display, WindowWindowProviderAtomName);

        private nuint CreateWmProtocolsAtom() => CreateAtom(Display, WmProtocolsAtomName);

        private nuint CreateWmDeleteWindowAtom() => CreateAtom(Display, WmDeleteWindowAtomName);

        /// <inheritdoc />
        protected override void Dispose(bool isDisposing)
        {
            var priorState = _state.BeginDispose();

            if (priorState < Disposing)
            {
                DisposeDisplay();
            }

            _state.EndDispose();
        }

        private void DisposeDisplay()
        {
            _state.AssertDisposing();

            if (_display.IsCreated)
            {
                _ = XSetErrorHandler(null);
                _ = XCloseDisplay(_display.Value);
            }
        }
    }
}
