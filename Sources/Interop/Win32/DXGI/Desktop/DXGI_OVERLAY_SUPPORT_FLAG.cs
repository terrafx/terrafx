// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.Desktop
{
    [Flags]
    public enum DXGI_OVERLAY_SUPPORT_FLAG : uint
    {
        NONE = 0x00000000,

        DIRECT = 0x00000001,

        SCALING = 0x00000002
    }
}
