// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_DEBUG_RLO_FLAGS : uint
    {
        NONE = 0x00000000,

        SUMMARY = 0x00000001,

        DETAIL = 0x00000002,

        IGNORE_INTERNAL = 0x00000004,

        ALL = (SUMMARY | DETAIL | IGNORE_INTERNAL)
    }
}
