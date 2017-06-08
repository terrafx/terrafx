// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h and shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.DXGI
{
    [Flags]
    public enum DXGI_ENUM_MODES : uint
    {
        NONE = 0,

        INTERLACED = 1,

        SCALING = 2,

        STEREO = 4,

        DISABLED_STEREO = 8
    }
}
