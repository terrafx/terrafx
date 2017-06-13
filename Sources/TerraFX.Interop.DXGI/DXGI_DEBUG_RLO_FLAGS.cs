// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum DXGI_DEBUG_RLO_FLAGS
    {
        NONE = 0,

        SUMMARY = 1,

        DETAIL = 2,

        IGNORE_INTERNAL = 4,

        ALL = 7
    }
}
