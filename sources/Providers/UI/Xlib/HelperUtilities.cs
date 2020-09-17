// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.	

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TerraFX.Interop;
using TerraFX.Utilities;
using static TerraFX.Interop.Xlib;
using static TerraFX.Utilities.AssertionUtilities;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.UI.Providers.Xlib
{
    internal static unsafe partial class HelperUtilities
    {
        public static nuint CreateAtom(IntPtr display, ReadOnlySpan<byte> name)
        {
            var atom = XInternAtom(
                display,
                (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(name)),
                False
            );
            ThrowExternalExceptionIfZero(nameof(XInternAtom), atom);
            return atom;
        }

        public static void ThrowExternalExceptionIfFailed(string methodName, int value)
        {
            if (value != Success)
            {
                ThrowExternalException(methodName, value);
            }
        }

        public static void ThrowExternalExceptionIfZero(string methodName, int value)
        {
            if (value == 0)
            {
                ThrowExternalException(methodName, value);
            }
        }

        public static void ThrowExternalExceptionIfZero(string methodName, nint value)
        {
            if (value == 0)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }

        public static void ThrowExternalExceptionIfZero(string methodName, nuint value)
        {
            if (value == 0)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }

        public static void SendClientMessage(IntPtr display, nuint window, nuint messageType, nuint message, nint data = default)
        {
            var clientEvent = new XClientMessageEvent {
                type = ClientMessage,
                serial = 0,
                send_event = True,
                display = display,
                window = window,
                message_type = messageType,
                format = 32
            };

            if (Environment.Is64BitProcess)
            {
                clientEvent.data.l[0] = unchecked((nint)(uint)message);
                clientEvent.data.l[1] = (nint)(uint)(message >> 32);
                Assert(clientEvent.data.l[1] == 0, Resources.ArgumentOutOfRangeExceptionMessage, nameof(message), message);

                clientEvent.data.l[2] = unchecked((nint)(uint)data);
                clientEvent.data.l[3] = (nint)(uint)(data >> 32);
            }
            else
            {
                clientEvent.data.l[0] = (nint)message;
                clientEvent.data.l[1] = data;
            }

            ThrowExternalExceptionIfZero(nameof(XSendEvent), XSendEvent(
                clientEvent.display,
                clientEvent.window,
                False,
                NoEventMask,
                (XEvent*)&clientEvent
            ));
        }
    }
}
