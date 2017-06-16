// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_ADAPTER_FLAG : uint
    {
        NONE = 0x00000000,

        REMOTE = 0x00000001,

        SOFTWARE = 0x00000002
    }
}
