// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\D3DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;

namespace TerraFX.Interop
{
    [Flags]
    public enum D3D_SHADER_VARIABLE_FLAGS : uint
    {
        NONE = 0x00000000,

        USERPACKED = 0x00000001,

        USED = 0x00000002,

        INTERFACE_POINTER = 0x00000004,

        INTERFACE_PARAMETER = 0x00000008
    }
}
