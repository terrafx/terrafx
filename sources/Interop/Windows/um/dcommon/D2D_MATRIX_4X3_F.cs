// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Represents a 4-by-3 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    [Unmanaged]
    public unsafe struct D2D_MATRIX_4X3_F
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        [NativeTypeName("FLOAT")]
        public float _11;

        [FieldOffset(4)]
        [NativeTypeName("FLOAT")]
        public float _12;

        [FieldOffset(8)]
        [NativeTypeName("FLOAT")]
        public float _13;

        [FieldOffset(12)]
        [NativeTypeName("FLOAT")]
        public float _21;

        [FieldOffset(16)]
        [NativeTypeName("FLOAT")]
        public float _22;

        [FieldOffset(20)]
        [NativeTypeName("FLOAT")]
        public float _23;

        [FieldOffset(24)]
        [NativeTypeName("FLOAT")]
        public float _31;

        [FieldOffset(28)]
        [NativeTypeName("FLOAT")]
        public float _32;

        [FieldOffset(32)]
        [NativeTypeName("FLOAT")]
        public float _33;

        [FieldOffset(36)]
        [NativeTypeName("FLOAT")]
        public float _41;

        [FieldOffset(40)]
        [NativeTypeName("FLOAT")]
        public float _42;

        [FieldOffset(44)]
        [NativeTypeName("FLOAT")]
        public float _43;
        #endregion

        [FieldOffset(0)]
        [NativeTypeName("FLOAT[4][3]")]
        public fixed float m[4 * 3];
        #endregion
    }
}
