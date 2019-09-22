// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop;
using TerraFX.UI;
using TerraFX.Utilities;
using static TerraFX.Interop.Libc;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.State;

namespace TerraFX.Provider.Xlib.UI
{
    /// <summary>Provides access to an X11 based dispatch subsystem.</summary>
    public sealed unsafe class DispatchProvider : IDisposable, IDispatchProvider
    {
        private static readonly NativeDelegate<XErrorHandler> s_errorHandler = new NativeDelegate<XErrorHandler>(HandleXlibError);
        private static ResettableLazy<DispatchProvider> s_instance = new ResettableLazy<DispatchProvider>(CreateDispatchProvider);

        private readonly ConcurrentDictionary<Thread, IDispatcher> _dispatchers;

        private ResettableLazy<UIntPtr> _dispatcherExitRequestedAtom;
        private ResettableLazy<UIntPtr> _display;
        private ResettableLazy<UIntPtr> _systemIntPtrAtom;
        private ResettableLazy<UIntPtr> _windowProviderCreateWindowAtom;
        private ResettableLazy<UIntPtr> _windowWindowProviderAtom;
        private ResettableLazy<UIntPtr> _wmProtocolsAtom;
        private ResettableLazy<UIntPtr> _wmDeleteWindowAtom;

        private State _state;

        private DispatchProvider()
        {
            _dispatchers = new ConcurrentDictionary<Thread, IDispatcher>();

            _dispatcherExitRequestedAtom = new ResettableLazy<UIntPtr>(CreateDispatcherExitRequestedAtom);
            _display = new ResettableLazy<UIntPtr>(CreateDisplay);
            _systemIntPtrAtom = new ResettableLazy<UIntPtr>(CreateSystemIntPtrAtom);
            _windowProviderCreateWindowAtom = new ResettableLazy<UIntPtr>(CreateWindowProviderCreateWindowAtom);
            _windowWindowProviderAtom = new ResettableLazy<UIntPtr>(CreateWindowWindowProviderAtom);
            _wmProtocolsAtom = new ResettableLazy<UIntPtr>(CreateWmProtocolsAtom);
            _wmDeleteWindowAtom = new ResettableLazy<UIntPtr>(CreateWmDeleteWindowAtom);

            _ = _state.Transition(to: Initialized);
        }

        /// <summary>Finalizes an instance of the <see cref="DispatchProvider" /> class.</summary>
        ~DispatchProvider()
        {
            Dispose(isDisposing: false);
        }

        /// <summary>Gets the <see cref="DispatchProvider" /> instance for the current program.</summary>
        public static DispatchProvider Instance => s_instance.Value;

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

        /// <summary>Gets the current <see cref="Timestamp" /> for the instance.</summary>
        /// <exception cref="ExternalException">The call to <see cref="clock_gettime(int, timespec*)" /> failed.</exception>
        public Timestamp CurrentTimestamp
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

        /// <summary>Gets the atom created to track the <see cref="Dispatcher.ExitRequested" /> event.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr DispatcherExitRequestedAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _dispatcherExitRequestedAtom.Value;
            }
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</summary>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <see cref="Thread.CurrentThread" />.</returns>
        /// <remarks>This will create a new <see cref="IDispatcher" /> instance if one does not already exist.</remarks>
        public IDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

        /// <summary>Gets the <c>Display</c> that was created for the instance.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr Display
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _display.Value;
            }
        }

        /// <summary>Gets the atom created to track the <see cref="System.IntPtr" /> type.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr SystemIntPtrAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _systemIntPtrAtom.Value;
            }
        }

        /// <summary>Gets the atom created to track the <see cref="WindowProvider.CreateWindow" /> method.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr WindowProviderCreateWindowAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windowProviderCreateWindowAtom.Value;
            }
        }

        /// <summary>Gets the atom created to track the <see cref="Window.WindowProvider" /> property.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr WindowWindowProviderAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _windowWindowProviderAtom.Value;
            }
        }

        /// <summary>Gets the atom created for the <c>WM_PROTOCOLS</c> client message.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr WmProtocolsAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _wmProtocolsAtom.Value;
            }
        }

        /// <summary>Gets the atom created for the <c>WM_DELETE_WINDOW</c> client message.</summary>
        /// <exception cref="ObjectDisposedException">The instance has already been disposed.</exception>
        public UIntPtr WmDeleteWindowAtom
        {
            get
            {
                _state.ThrowIfDisposedOrDisposing();
                return _wmDeleteWindowAtom.Value;
            }
        }

        /// <summary>Disposes of any unmanaged resources tracked by the instance.</summary>
        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" />, creating one if it does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <returns>The <see cref="IDispatcher" /> instance associated with <paramref name="thread" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">A <see cref="IDispatcher" /> instance for <paramref name="thread" /> could not be found.</exception>
        public IDispatcher GetDispatcher(Thread thread)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.GetOrAdd(thread, (parentThread) => new Dispatcher(this, parentThread));
        }

        /// <summary>Gets the <see cref="IDispatcher" /> instance associated with a <see cref="Thread" /> or <c>null</c> if one does not exist.</summary>
        /// <param name="thread">The <see cref="Thread" /> for which the <see cref="IDispatcher" /> instance should be retrieved.</param>
        /// <param name="dispatcher">The <see cref="IDispatcher" /> instance associated with <paramref name="thread" />.</param>
        /// <returns><c>true</c> if a <see cref="IDispatcher" /> instance was found for <paramref name="thread" />; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="thread" /> is <c>null</c>.</exception>
        public bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out IDispatcher dispatcher)
        {
            ThrowIfNull(thread, nameof(thread));
            return _dispatchers.TryGetValue(thread, out dispatcher!);
        }

        private static DispatchProvider CreateDispatchProvider() => new DispatchProvider();

        private static int HandleXlibError(UIntPtr display, XErrorEvent* errorEvent)
        {
            // Due to the asynchronous nature of Xlib, there can be a race between
            // the window being deleted and it being unmapped. This ignores the warning
            // raised by the unmap event in that scenario, as the call to XGetWindowProperty
            // will fail.

            if ((errorEvent->error_code != BadWindow) || (errorEvent->request_code != X_GetProperty))
            {
                ThrowExternalException(nameof(XErrorHandler), errorEvent->error_code);
            }

            return 0;
        }

        private static UIntPtr CreateDisplay()
        {
            var display = XOpenDisplay(display_name: null);

            if (display == UIntPtr.Zero)
            {
                ThrowExternalExceptionForLastError(nameof(XOpenDisplay));
            }
            _ = XSetErrorHandler(s_errorHandler);

            return display;
        }

        private UIntPtr CreateDispatcherExitRequestedAtom()
        {
            var atom = XInternAtom(
                Display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in DispatcherExitRequestedAtomName[0])),
                only_if_exists: False
            );

            if (atom == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XInternAtom));
            }

            return atom;
        }

        private UIntPtr CreateSystemIntPtrAtom()
        {
            var atom = XInternAtom(
                Display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in SystemIntPtrAtomName[0])),
                only_if_exists: False
            );

            if (atom == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XInternAtom));
            }

            return atom;
        }

        private UIntPtr CreateWindowProviderCreateWindowAtom()
        {
            var atom = XInternAtom(
                Display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in WindowProviderCreateWindowAtomName[0])),
                only_if_exists: False
            );

            if (atom == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XInternAtom));
            }

            return atom;
        }

        private UIntPtr CreateWindowWindowProviderAtom()
        {
            var atom = XInternAtom(
                Display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in WindowWindowProviderAtomName[0])),
                only_if_exists: False
            );

            if (atom == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XInternAtom));
            }

            return atom;
        }

        private UIntPtr CreateWmProtocolsAtom()
        {
            var atom = XInternAtom(
                Display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in WmProtocolsAtomName[0])),
                only_if_exists: False
            );

            if (atom == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XInternAtom));
            }

            return atom;
        }

        private UIntPtr CreateWmDeleteWindowAtom()
        {
            var atom = XInternAtom(
                Display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref Unsafe.AsRef(in WmDeleteWindowAtomName[0])),
                only_if_exists: False
            );

            if (atom == (UIntPtr)None)
            {
                ThrowExternalExceptionForLastError(nameof(XInternAtom));
            }

            return atom;
        }

        private void Dispose(bool isDisposing)
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
                _ = XSetErrorHandler(IntPtr.Zero);
                _ = XCloseDisplay(_display.Value);
            }
        }
    }
}
