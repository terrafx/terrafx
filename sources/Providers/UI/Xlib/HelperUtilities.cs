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
        public static UIntPtr CreateAtom(UIntPtr display, ReadOnlySpan<byte> name)
        {
            var atom = XInternAtom(
                display,
                atom_name: (sbyte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(name)),
                only_if_exists: False
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

        public static void ThrowExternalExceptionIfZero(string methodName, UIntPtr value)
        {
            if (value == UIntPtr.Zero)
            {
                ThrowExternalExceptionForLastError(methodName);
            }
        }

        public static void SendClientMessage(UIntPtr display, UIntPtr window, UIntPtr messageType, UIntPtr message, IntPtr data = default)
        {
            var clientEvent = new XClientMessageEvent {
                type = ClientMessage,
                serial = UIntPtr.Zero,
                send_event = True,
                display = display,
                window = window,
                message_type = messageType,
                format = 32
            };

            if (Environment.Is64BitProcess)
            {
                var messageBits = message.ToUInt64();
                clientEvent.data.l[0] = unchecked((IntPtr)(uint)messageBits);
                clientEvent.data.l[1] = (IntPtr)(uint)(messageBits >> 32);
                Assert(clientEvent.data.l[1] == IntPtr.Zero, Resources.ArgumentOutOfRangeExceptionMessage, nameof(message), message);

                var dataBits = data.ToInt64();
                clientEvent.data.l[2] = unchecked((IntPtr)(uint)dataBits);
                clientEvent.data.l[3] = (IntPtr)(uint)(dataBits >> 32);
            }
            else
            {
                var messageBits = message.ToUInt32();
                clientEvent.data.l[0] = (IntPtr)messageBits;

                var dataBits = data.ToInt32();
                clientEvent.data.l[1] = (IntPtr)dataBits;
            }

            ThrowExternalExceptionIfZero(nameof(XSendEvent), XSendEvent(
                clientEvent.display,
                clientEvent.window,
                propagate: False,
                (IntPtr)NoEventMask,
                (XEvent*)&clientEvent
            ));
        }
    }
}
