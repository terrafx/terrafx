// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D_PARAMETER_FLAGS : uint
    {
        NONE = 0x00000000,

        IN = 0x00000001,

        OUT = 0x00000002
    }
}
