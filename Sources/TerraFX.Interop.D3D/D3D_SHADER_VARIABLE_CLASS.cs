// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop.D3D
{
    public enum D3D_SHADER_VARIABLE_CLASS
    {
        SCALAR = 0,

        VECTOR = 1,

        MATRIX_ROWS = 2,

        MATRIX_COLUMNS = 3,

        OBJECT = 4,

        STRUCT = 5,

        INTERFACE_CLASS = 6,

        INTERFACE_POINTER = 7,

        FORCE_DWORD = 0x7FFFFFFF
    }
}
