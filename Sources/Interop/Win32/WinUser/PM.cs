// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\WinUser.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum PM : uint
    {
        NOREMOVE = 0x00000000,

        REMOVE = 0x00000001,

        NOYIELD = 0x00000002,

        QS_PAINT = (QS.PAINT << 16),

        QS_POSTMESSAGE  = ((QS.POSTMESSAGE | QS.TIMER | QS.HOTKEY) << 16),

        QS_SENDMESSAGE  = (QS.SENDMESSAGE << 16),

        QS_INPUT = (QS.INPUT << 16)
    }
}
