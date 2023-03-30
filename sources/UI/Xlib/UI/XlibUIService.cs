// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Threading;
using TerraFX.Interop.LibC;
using TerraFX.Interop.Xlib;
using TerraFX.Threading;
using TerraFX.Utilities;
using static TerraFX.Interop.LibC.LibC;
using static TerraFX.Interop.Xlib.Xlib;
using static TerraFX.Runtime.Configuration;
using static TerraFX.Threading.VolatileState;
using static TerraFX.UI.XlibAtomId;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;
using static TerraFX.Utilities.MathUtilities;
using static TerraFX.Utilities.UnsafeUtilities;
using XWindow = TerraFX.Interop.Xlib.Window;

namespace TerraFX.UI;

/// <summary>Provides access to an X11 based window subsystem.</summary>
public sealed unsafe class XlibUIService : UIService
{
    private const string VulkanRequiredExtensionNamesDataName = "TerraFX.Graphics.VulkanGraphicsService.RequiredExtensionNames";

    private static XlibUIService? s_instance;

    /// <summary>Gets the X11 based UI service.</summary>
    public static XlibUIService Instance
    {
        get
        {
            var instance = s_instance;

            if (instance is null)
            {
                // This might unnecessarily create a service in the case of a race
                // between two threads, but that is an extremely exceptional case
                // given the typical usage pattern and is acceptable to keep this
                // simple.

                var service = new XlibUIService();
                instance = Interlocked.CompareExchange(ref s_instance, service, null) ?? service;
            }

            AssertNotNull(instance);
            return instance;
        }
    }

    private readonly Dictionary<Thread, XlibUIDispatcher> _dispatchers;
    private readonly ValueReaderWriterLock _dispatchersLock;

    private readonly UnmanagedArray<Atom> _atoms;
    private readonly XWindow _defaultRootWindow;
    private readonly Screen* _defaultScreen;
    private readonly Display* _display;
    private readonly UnmanagedArray<nuint> _supportedAtoms;

    private VolatileState _state;

    private XlibUIService()
    {
        var vulkanRequiredExtensionNamesDataName = AppContext.GetData(VulkanRequiredExtensionNamesDataName) as string;
        vulkanRequiredExtensionNamesDataName += ";VK_KHR_surface;VK_KHR_xlib_surface";
        AppDomain.CurrentDomain.SetData(VulkanRequiredExtensionNamesDataName, vulkanRequiredExtensionNamesDataName);

        ThrowForLastErrorIfZero(XInitThreads());

        var display = OpenDisplay();
        _display = display;

        var atoms = CreateAtoms(display);
        _atoms = atoms;

        var defaultRootWindow = XDefaultRootWindow(display);
        _defaultRootWindow = defaultRootWindow;

        _defaultScreen = XDefaultScreenOfDisplay(display);
        _supportedAtoms = GetSupportedAtoms(display, atoms, defaultRootWindow);

        _dispatchers = new Dictionary<Thread, XlibUIDispatcher>();
        _dispatchersLock = new ValueReaderWriterLock();

        _ = _state.Transition(to: Initialized);

        static UnmanagedArray<Atom> CreateAtoms(Display* display)
        {
            var atoms = new UnmanagedArray<Atom>((nuint)ATOM_ID_COUNT);

            var atomNames = stackalloc sbyte*[(int)ATOM_ID_COUNT] {
                (sbyte*)XlibAtomName._NET_ACTIVE_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_CLIENT_LIST.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_CLIENT_LIST_STACKING.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_CLOSE_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_CURRENT_DESKTOP.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_DESKTOP_GEOMETRY.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_DESKTOP_LAYOUT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_DESKTOP_NAMES.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_DESKTOP_VIEWPORT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_FRAME_EXTENTS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_MOVERESIZE_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_NUMBER_OF_DESKTOPS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_REQUEST_FRAME_EXTENTS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_RESTACK_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_SHOWING_DESKTOP.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_SUPPORTED.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_SUPPORTING_WM_CHECK.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_VIRTUAL_ROOTS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_ABOVE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_BELOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_CHANGE_DESKTOP.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_CLOSE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_FULLSCREEN.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MAXIMIZE_HORZ.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MAXIMIZE_VERT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MINIMIZE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MOVE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_RESIZE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_SHADE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_STICK.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ALLOWED_ACTIONS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_BYPASS_COMPOSITOR.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_DESKTOP.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_FULL_PLACEMENT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_FULLSCREEN_MONITORS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_HANDLED_ICONS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ICON.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ICON_GEOMETRY.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_ICON_NAME.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_MOVERESIZE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_NAME.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_OPAQUE_REGION.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_PID.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_PING.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_ABOVE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_BELOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_DEMANDS_ATTENTION.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_FOCUSED.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_FULLSCREEN.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_HIDDEN.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_MAXIMIZED_HORZ.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_MAXIMIZED_VERT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_MODAL.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_SHADED.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_SKIP_PAGER.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_SKIP_TASKBAR.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STATE_STICKY.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STRUT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_STRUT_PARTIAL.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_SYNC_REQUEST.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_USER_TIME.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_USER_TIME_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_VISIBLE_NAME.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_VISIBLE_ICON_NAME.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_COMBO.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DESKTOP.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DIALOG.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DND.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DOCK.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DROPDOWN_MENU.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_MENU.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_NORMAL.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_NOTIFICATION.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_POPUP_MENU.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_SPLASH.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_TOOLBAR.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_TOOLTIP.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_UTILITY.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._NET_WORKAREA.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._TERRAFX_CREATE_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._TERRAFX_DISPOSE_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._TERRAFX_QUIT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName._TERRAFX_NATIVE_INT.GetPointerUnsafe(),
                (sbyte*)XlibAtomName.UTF8_STRING.GetPointerUnsafe(),
                (sbyte*)XlibAtomName.WM_DELETE_WINDOW.GetPointerUnsafe(),
                (sbyte*)XlibAtomName.WM_PROTOCOLS.GetPointerUnsafe(),
                (sbyte*)XlibAtomName.WM_STATE.GetPointerUnsafe(),
            };

            XLockDisplay(display);

            try
            {
                ThrowForLastErrorIfZero(XInternAtoms(
                    display,
                    atomNames,
                    (int)ATOM_ID_COUNT,
                    False,
                    atoms.GetPointerUnsafe(0)
                ));
            }
            finally
            {
                XUnlockDisplay(display);
            }

            return atoms;
        }

        static UnmanagedArray<nuint> GetSupportedAtoms(Display* display, UnmanagedArray<Atom> atoms, XWindow defaultRootWindow)
        {
            var supportedAtoms = new UnmanagedArray<nuint>(DivideRoundingUp((nuint)ATOM_ID_COUNT, SizeOf<nuint>() * 8));

            Atom actualType;
            int actualFormat;
            nuint itemCount;
            nuint bytesRemaining;
            Atom* pSupportedAtoms;

            XLockDisplay(display);

            try
            {
                _ = XGetWindowProperty(
                    display,
                    defaultRootWindow,
                    atoms[(nuint)_NET_SUPPORTED],
                    0,
                    nint.MaxValue,
                    False,
                    XA_ATOM,
                    &actualType,
                    &actualFormat,
                    &itemCount,
                    &bytesRemaining,
                    (byte**)&pSupportedAtoms
                );
            }
            finally
            {
                XUnlockDisplay(display);
            }

            if ((actualType == XA_ATOM) && (actualFormat == 32) && (bytesRemaining == 0))
            {
                for (nuint i = 0; i < itemCount; i++)
                {
                    var supportedAtom = pSupportedAtoms[i];

                    for (nuint n = 0; n < (nuint)ATOM_ID_COUNT; n++)
                    {
                        if (atoms[n] != supportedAtom)
                        {
                            continue;
                        }

                        var (supportedAtomIndex, supportedAtomBitIndex) = DivRem(n, SizeOf<nuint>() * 8);
                        supportedAtoms[supportedAtomIndex] = supportedAtoms[supportedAtomIndex] | ((nuint)1 << (int)supportedAtomBitIndex);
                        break;
                    }
                }
            }

            return supportedAtoms;
        }

        static Display* OpenDisplay()
        {
            Display* display;
            ThrowForLastErrorIfNull(display = XOpenDisplay(null));

            _ = XSetErrorHandler(&HandleXlibError);
            _ = XSetIOErrorHandler(&HandleXlibIOError);

            return display;
        }
    }

    /// <summary>Finalizes an instance of the <see cref="XlibUIService" /> class.</summary>
    ~XlibUIService() => Dispose(isDisposing: false);

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="clock_gettime(clockid_t, timespec*)" /> failed.</exception>
    public override Timestamp CurrentTimestamp
    {
        get
        {
            timespec timespec;
            ThrowForLastErrorIfNotZero(clock_gettime(CLOCK_MONOTONIC, &timespec));

            const long NanosecondsPerSecond = TimeSpan.TicksPerSecond * 100;
            Assert((NanosecondsPerSecond == 1000000000));

            var ticks = (long)timespec.tv_sec;
            {
                ticks *= NanosecondsPerSecond;
                ticks += timespec.tv_nsec;
                ticks /= 100;
            }
            return new Timestamp(ticks);
        }
    }

    /// <inheritdoc />
    public override XlibUIDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

    /// <summary>Gets the underlying display for the service.</summary>
    public Display* Display => _display;

    /// <summary>Gets the default root window associated with <see cref="Display" />.</summary>
    public XWindow DefaultRootWindow => _defaultRootWindow;

    /// <summary>Gets the default screen associated with <see cref="Display" />.</summary>
    public Screen* DefaultScreen => _defaultScreen;

    [UnmanagedCallersOnly]
    private static int HandleXlibError(Display* display, XErrorEvent* errorEvent)
    {
        // Due to the asynchronous nature of Xlib, there can be a race between
        // the window being deleted and it being unmapped. This ignores the warning
        // raised by the unmap event in that scenario, as the call to XGetWindowProperty
        // will fail.

        var errorCode = (XlibErrorCode)errorEvent->error_code;
        var requestCode = (XlibRequestCode)errorEvent->request_code;

        if ((errorCode != XlibErrorCode.BadWindow) || (requestCode != XlibRequestCode.GetProperty))
        {
            ThrowExternalException(requestCode.ToString(), (int)errorCode);
        }

        return 0;
    }

    [UnmanagedCallersOnly]
    private static int HandleXlibIOError(Display* display) => 0;

    /// <inheritdoc />
    public override XlibUIDispatcher GetDispatcher(Thread thread)
    {
        if (!TryGetDispatcher(thread, out var dispatcher))
        {
            dispatcher = CreateDispatcher(this, thread);
        }
        return dispatcher;

        static XlibUIDispatcher CreateDispatcher(XlibUIService service, Thread thread)
        {
            var dispatcher = new XlibUIDispatcher(service, thread);

            using var writerLock = new DisposableWriterLock(service._dispatchersLock, isExternallySynchronized: false);
            service._dispatchers.Add(thread, dispatcher);

            return dispatcher;
        }
    }

    /// <inheritdoc />
    public override bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out UIDispatcher dispatcher)
    {
        var result = TryGetDispatcher(thread, out var win32Dispatcher);
        dispatcher = win32Dispatcher;
        return result;
    }

    /// <inheritdoc cref="TryGetDispatcher(Thread, out UIDispatcher)" />
    public bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out XlibUIDispatcher dispatcher)
    {
        ThrowIfNull(thread);
        using var readerLock = new DisposableReaderLock(_dispatchersLock, isExternallySynchronized: false);
        return _dispatchers.TryGetValue(thread, out dispatcher);
    }

    /// <inheritdoc />
    protected override void Dispose(bool isDisposing)
    {
        var priorState = _state.BeginDispose();

        if (priorState < Disposing)
        {
            if (isDisposing)
            {
                DisposeDispatchers(_dispatchers);
            }
            DisposeDisplay(_display);
        }

        _state.EndDispose();


        static void DisposeDispatchers(Dictionary<Thread, XlibUIDispatcher> dispatchers)
        {
            foreach (var dispatcher in dispatchers)
            {
                dispatcher.Value.Dispose();
            }
            dispatchers.Clear();
        }

        static void DisposeDisplay(Display* display)
        {
            if (display != null)
            {
                _ = XSetIOErrorHandler(null);
                _ = XSetErrorHandler(null);
                _ = XCloseDisplay(display);
            }
        }
    }

    internal Atom GetAtom(XlibAtomId id) => _atoms[(nuint)id];

    internal bool GetAtomIsSupported(XlibAtomId id)
    {
        var (supportedAtomIndex, supportedAtomBitIndex) = DivRem((nuint)id, SizeOf<nuint>() * 8);
        return (_supportedAtoms[supportedAtomIndex] & ((nuint)1 << (int)supportedAtomBitIndex)) != 0;
    }
}
