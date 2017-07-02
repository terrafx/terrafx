// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\DCommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Represents a 4-by-3 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    unsafe public struct D2D_MATRIX_4X3_F
    {
        #region Fields
        [FieldOffset(0)]
        public _Anonymous_e__Union Anonymous;

        [FieldOffset(0)]
        public fixed float m[4 * 3];
        #endregion

        #region Structs
        public struct _Anonymous_e__Union
        {
            #region Fields
            public float _11, _12, _13;

            public float _21, _22, _23;

            public float _31, _32, _33;

            public float _41, _42, _43;
            #endregion
        }
        #endregion
    }
}
