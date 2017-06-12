// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop.D3D12
{
    [Flags]
    public enum D3D12_COLOR_WRITE_ENABLE
    {
        UNDEFINED = 0,

        RED = 1,

        GREEN = 2,

        BLUE = 4,

        ALPHA = 8,

        ALL = (RED | GREEN | BLUE | ALPHA),
    }
}
