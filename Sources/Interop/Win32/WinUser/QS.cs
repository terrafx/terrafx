// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum QS : uint
    {
        KEY = 0x00000001,

        MOUSEMOVE = 0x00000002,

        MOUSEBUTTON = 0x00000004,

        MOUSE = (MOUSEMOVE | MOUSEBUTTON),

        POSTMESSAGE = 0x00000008,

        TIMER = 0x00000010,

        PAINT = 0x00000020,

        SENDMESSAGE = 0x00000040,

        HOTKEY = 0x00000080,

        ALLPOSTMESSAGE = 0x00000100,

        RAWINPUT = 0x00000400,

        TOUCH = 0x00000800,

        POINTER = 0x00001000,

        INPUT = (MOUSE | KEY | RAWINPUT | TOUCH | POINTER),

        ALLEVENTS = (POSTMESSAGE | TIMER | PAINT | HOTKEY | INPUT),

        ALLINPUT = (SENDMESSAGE | ALLEVENTS)
    }
}
