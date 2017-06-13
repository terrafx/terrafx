// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_MWA_FLAG : uint
    {
        NONE = 0x00000000,

        NO_WINDOW_CHANGES = 0x00000001,

        NO_ALT_ENTER = 0x00000002,

        NO_PRINT_SCREEN = 0x00000004,

        VALID = 0x00000007
    }
}
