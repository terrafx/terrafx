// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h and shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_ENUM_MODES : uint
    {
        NONE = 0x00000000,

        INTERLACED = 0x00000001,

        SCALING = 0x00000002,

        STEREO = 0x00000004,

        DISABLED_STEREO = 0x00000008
    }
}
