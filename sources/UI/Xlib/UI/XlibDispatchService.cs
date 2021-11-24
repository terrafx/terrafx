// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
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

/// <summary>Provides access to an X11 based dispatch subsystem.</summary>
public sealed unsafe class XlibDispatchService : DispatchService
{
    private const uint AtomIdCount = (uint)ATOM_ID_COUNT;

    private static readonly XlibDispatchService s_instance = new XlibDispatchService();

    private readonly ConcurrentDictionary<Thread, XlibDispatcher> _dispatchers;

    private ValueLazy<Atom[]> _atoms;
    private ValueLazy<XWindow> _defaultRootWindow;
    private ValueLazy<Pointer<Screen>> _defaultScreen;
    private ValueLazy<Pointer<Display>> _display;
    private ValueLazy<Atom[]> _supportedAtoms;

    private VolatileState _state;

    private XlibDispatchService()
    {
        _dispatchers = new ConcurrentDictionary<Thread, XlibDispatcher>();

        _display = new ValueLazy<Pointer<Display>>(CreateDisplayHandle);
        _defaultRootWindow = new ValueLazy<XWindow>(GetDefaultRootWindow);
        _defaultScreen = new ValueLazy<Pointer<Screen>>(GetDefaultScreen);
        _atoms = new ValueLazy<Atom[]>(CreateAtoms);
        _supportedAtoms = new ValueLazy<Atom[]>(GetSupportedAtoms);

        _ = _state.Transition(to: Initialized);
    }

    /// <summary>Finalizes an instance of the <see cref="XlibDispatchService" /> class.</summary>
    ~XlibDispatchService() => Dispose(isDisposing: false);

    /// <summary>Gets the <see cref="XlibDispatchService" /> instance for the current program.</summary>
    public static XlibDispatchService Instance => s_instance;

    /// <inheritdoc />
    /// <exception cref="ExternalException">The call to <see cref="clock_gettime(int, timespec*)" /> failed.</exception>
    public override Timestamp CurrentTimestamp
    {
        get
        {
            timespec timespec;
            ThrowForLastErrorIfNotZero(clock_gettime(CLOCK_MONOTONIC, &timespec), nameof(clock_gettime));

            const long NanosecondsPerSecond = TimeSpan.TicksPerSecond * 100;
            Assert(AssertionsEnabled && (NanosecondsPerSecond == 1000000000));

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
    public override XlibDispatcher DispatcherForCurrentThread => GetDispatcher(Thread.CurrentThread);

    /// <summary>Gets the <c>Display</c> that was created for the instance.</summary>
    public Display* Display => _display.Value;

    /// <summary>Gets the default root window associated with <see cref="Display" />.</summary>
    public XWindow DefaultRootWindow => _defaultRootWindow.Value;

    /// <summary>Gets the default screen associated with <see cref="Display" />.</summary>
    public Screen* DefaultScreen => _defaultScreen.Value;

    internal Atom GetAtom(XlibAtomId id) => _atoms.Value[(nuint)id];

    internal bool GetAtomIsSupported(XlibAtomId id)
    {
        var (supportedAtomIndex, supportedAtomBitIndex) = DivRem((nuint)id, SizeOf<Atom>() * 8);
        return (_supportedAtoms.Value[supportedAtomIndex] & ((nuint)1 << (int)supportedAtomBitIndex)) != 0;
    }

    /// <inheritdoc />
    public override XlibDispatcher GetDispatcher(Thread thread)
    {
        ThrowIfNull(thread, nameof(thread));
        return _dispatchers.GetOrAdd(thread, (parentThread) => new XlibDispatcher(this, parentThread));
    }

    /// <inheritdoc />
    public override bool TryGetDispatcher(Thread thread, [MaybeNullWhen(false)] out Dispatcher dispatcher)
    {
        ThrowIfNull(thread, nameof(thread));
        Unsafe.SkipInit(out dispatcher);
        return _dispatchers.TryGetValue(thread, out Unsafe.As<Dispatcher, XlibDispatcher>(ref dispatcher)!);
    }

    private static Pointer<Display> CreateDisplayHandle()
    {
        var display = XOpenDisplay(null);
        ThrowForLastErrorIfNull(display, nameof(XOpenDisplay));

        _ = XSetErrorHandler(&HandleXlibError);
        _ = XSetIOErrorHandler(&HandleXlibIOError);

        return display;
    }

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

    private Atom[] CreateAtoms()
    {
        var atoms = new Atom[AtomIdCount];

        fixed (Atom* pAtoms = atoms)
        {
            var atomNames = stackalloc sbyte*[(int)AtomIdCount] {
                (sbyte*)XlibAtomName._NET_ACTIVE_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_CLIENT_LIST.GetPointer(),
                (sbyte*)XlibAtomName._NET_CLIENT_LIST_STACKING.GetPointer(),
                (sbyte*)XlibAtomName._NET_CLOSE_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_CURRENT_DESKTOP.GetPointer(),
                (sbyte*)XlibAtomName._NET_DESKTOP_GEOMETRY.GetPointer(),
                (sbyte*)XlibAtomName._NET_DESKTOP_LAYOUT.GetPointer(),
                (sbyte*)XlibAtomName._NET_DESKTOP_NAMES.GetPointer(),
                (sbyte*)XlibAtomName._NET_DESKTOP_VIEWPORT.GetPointer(),
                (sbyte*)XlibAtomName._NET_FRAME_EXTENTS.GetPointer(),
                (sbyte*)XlibAtomName._NET_MOVERESIZE_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_NUMBER_OF_DESKTOPS.GetPointer(),
                (sbyte*)XlibAtomName._NET_REQUEST_FRAME_EXTENTS.GetPointer(),
                (sbyte*)XlibAtomName._NET_RESTACK_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_SHOWING_DESKTOP.GetPointer(),
                (sbyte*)XlibAtomName._NET_SUPPORTED.GetPointer(),
                (sbyte*)XlibAtomName._NET_SUPPORTING_WM_CHECK.GetPointer(),
                (sbyte*)XlibAtomName._NET_VIRTUAL_ROOTS.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_ABOVE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_BELOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_CHANGE_DESKTOP.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_CLOSE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_FULLSCREEN.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MAXIMIZE_HORZ.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MAXIMIZE_VERT.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MINIMIZE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_MOVE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_RESIZE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_SHADE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ACTION_STICK.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ALLOWED_ACTIONS.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_BYPASS_COMPOSITOR.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_DESKTOP.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_FULL_PLACEMENT.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_FULLSCREEN_MONITORS.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_HANDLED_ICONS.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ICON.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ICON_GEOMETRY.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_ICON_NAME.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_MOVERESIZE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_NAME.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_OPAQUE_REGION.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_PID.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_PING.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_ABOVE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_BELOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_DEMANDS_ATTENTION.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_FOCUSED.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_FULLSCREEN.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_HIDDEN.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_MAXIMIZED_HORZ.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_MAXIMIZED_VERT.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_MODAL.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_SHADED.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_SKIP_PAGER.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_SKIP_TASKBAR.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STATE_STICKY.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STRUT.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_STRUT_PARTIAL.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_SYNC_REQUEST.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_USER_TIME.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_USER_TIME_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_VISIBLE_NAME.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_VISIBLE_ICON_NAME.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_COMBO.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DESKTOP.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DIALOG.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DND.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DOCK.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_DROPDOWN_MENU.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_MENU.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_NORMAL.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_NOTIFICATION.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_POPUP_MENU.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_SPLASH.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_TOOLBAR.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_TOOLTIP.GetPointer(),
                (sbyte*)XlibAtomName._NET_WM_WINDOW_TYPE_UTILITY.GetPointer(),
                (sbyte*)XlibAtomName._NET_WORKAREA.GetPointer(),
                (sbyte*)XlibAtomName._TERRAFX_CREATE_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._TERRAFX_DISPOSE_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName._TERRAFX_NATIVE_INT.GetPointer(),
                (sbyte*)XlibAtomName._TERRAFX_WINDOWSERVICE.GetPointer(),
                (sbyte*)XlibAtomName.UTF8_STRING.GetPointer(),
                (sbyte*)XlibAtomName.WM_DELETE_WINDOW.GetPointer(),
                (sbyte*)XlibAtomName.WM_PROTOCOLS.GetPointer(),
                (sbyte*)XlibAtomName.WM_STATE.GetPointer(),
            };

            ThrowForLastErrorIfZero(XInternAtoms(
                Display,
                atomNames,
                (int)AtomIdCount,
                False,
                pAtoms
            ), nameof(XInternAtoms));
        }

        return atoms;
    }

    private XWindow GetDefaultRootWindow() => XDefaultRootWindow(Display);

    private Pointer<Screen> GetDefaultScreen() => XDefaultScreenOfDisplay(Display);

    private Atom[] GetSupportedAtoms()
    {
        var supportedAtoms = new Atom[DivideRoundingUp(AtomIdCount, SizeOf<Atom>() * 8)];

        Atom actualType;
        int actualFormat;
        nuint itemCount;
        nuint bytesRemaining;
        Atom* pSupportedAtoms;

        _ = XGetWindowProperty(
            Display,
            DefaultRootWindow,
            GetAtom(_NET_SUPPORTED),
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

        if ((actualType == XA_ATOM) && (actualFormat == 32) && (bytesRemaining == 0))
        {
            for (nuint i = 0; i < itemCount; i++)
            {
                var supportedAtom = pSupportedAtoms[i];

                for (nuint n = 0; n < AtomIdCount; n++)
                {
                    if (_atoms.Value[n] != supportedAtom)
                    {
                        continue;
                    }

                    var (supportedAtomIndex, supportedAtomBitIndex) = DivRem(n, SizeOf<Atom>() * 8);
                    supportedAtoms[supportedAtomIndex] = (Atom)(supportedAtoms[supportedAtomIndex] | ((nuint)1 << (int)supportedAtomBitIndex));
                    break;
                }
            }
        }

        return supportedAtoms;
    }

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
        AssertDisposing(_state);

        if (_display.IsValueCreated)
        {
            _ = XSetIOErrorHandler(null);
            _ = XSetErrorHandler(null);
            _ = XCloseDisplay(_display.Value);
        }
    }
}
