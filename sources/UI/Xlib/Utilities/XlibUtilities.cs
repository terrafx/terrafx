// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System;
using TerraFX.Interop.Xlib;
using TerraFX.UI;
using static TerraFX.Interop.Xlib.Xlib;
using static TerraFX.UI.XlibAtomId;
using static TerraFX.Utilities.ExceptionUtilities;
using XWindow = TerraFX.Interop.Xlib.Window;

namespace TerraFX.Utilities;

internal static unsafe partial class XlibUtilities
{
    public const int SourceApplication = 1;

#pragma warning disable IDE1006
    public const int _NET_WM_ORIENTATION_HORZ = 0;
    public const int _NET_WM_ORIENTATION_VERT = 1;

    public const int _NET_WM_TOPLEFT = 0;
    public const int _NET_WM_TOPRIGHT = 1;
    public const int _NET_WM_BOTTOMRIGHT = 2;
    public const int _NET_WM_BOTTOMLEFT = 3;

    public const int _NET_WM_MOVERESIZE_SIZE_TOPLEFT = 0;
    public const int _NET_WM_MOVERESIZE_SIZE_TOP = 1;
    public const int _NET_WM_MOVERESIZE_SIZE_TOPRIGHT = 2;
    public const int _NET_WM_MOVERESIZE_SIZE_RIGHT = 3;
    public const int _NET_WM_MOVERESIZE_SIZE_BOTTOMRIGHT = 4;
    public const int _NET_WM_MOVERESIZE_SIZE_BOTTOM = 5;
    public const int _NET_WM_MOVERESIZE_SIZE_BOTTOMLEFT = 6;
    public const int _NET_WM_MOVERESIZE_SIZE_LEFT = 7;
    public const int _NET_WM_MOVERESIZE_MOVE = 8;
    public const int _NET_WM_MOVERESIZE_SIZE_KEYBOARD = 9;
    public const int _NET_WM_MOVERESIZE_MOVE_KEYBOARD = 10;
    public const int _NET_WM_MOVERESIZE_CANCEL = 11;

    public const int _NET_WM_STATE_REMOVE = 0;
    public const int _NET_WM_STATE_ADD = 1;
    public const int _NET_WM_STATE_TOGGLE = 2;
#pragma warning restore IDE1006

    public static void SendClientMessage(Display* display, XWindow targetWindow, nint eventMask, XWindow eventWindow, Atom messageType, nint data0 = 0, nint data1 = 0, nint data2 = 0, nint data3 = 0, nint data4 = 0)
    {
        var clientEvent = new XClientMessageEvent {
            type = ClientMessage,
            send_event = True,
            display = display,
            window = eventWindow,
            message_type = messageType,
            format = 32
        };

        clientEvent.data.l[0] = data0;
        clientEvent.data.l[1] = data1;
        clientEvent.data.l[2] = data2;
        clientEvent.data.l[3] = data3;
        clientEvent.data.l[4] = data4;

        ThrowForLastErrorIfZero(XSendEvent(
            display,
            targetWindow,
            False,
            eventMask,
            (XEvent*)&clientEvent
        ));
    }

    public static void SetWindowTitle(XlibDispatchService dispatchService, Display* display, XWindow window, string value)
    {
        if (dispatchService.GetAtomIsSupported(_NET_WM_NAME))
        {
            var utf8Title = value.GetUtf8Span();

            fixed (sbyte* pUtf8Title = utf8Title)
            {
                _ = XChangeProperty(
                    display,
                    window,
                    dispatchService.GetAtom(_NET_WM_NAME),
                    dispatchService.GetAtom(UTF8_STRING),
                    8,
                    PropModeReplace,
                    (byte*)pUtf8Title,
                    utf8Title.Length
                );
            }
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    public static void ShowWindow(Display* display, XWindow window, int initialState = NormalState)
    {
        var wmHints = new XWMHints {
            flags = StateHint,
            initial_state = initialState,
        };
        _ = XSetWMHints(display, window, &wmHints);

        _ = XMapWindow(display, window);
    }
}
