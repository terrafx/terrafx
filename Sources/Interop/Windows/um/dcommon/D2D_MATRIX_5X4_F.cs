// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Represents a 5-by-4 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ unsafe struct D2D_MATRIX_5X4_F
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        [ComAliasName("FLOAT")]
        public float _11;

        [FieldOffset(4)]
        [ComAliasName("FLOAT")]
        public float _12;

        [FieldOffset(8)]
        [ComAliasName("FLOAT")]
        public float _13;

        [FieldOffset(12)]
        [ComAliasName("FLOAT")]
        public float _14;

        [FieldOffset(16)]
        [ComAliasName("FLOAT")]
        public float _21;

        [FieldOffset(20)]
        [ComAliasName("FLOAT")]
        public float _22;

        [FieldOffset(24)]
        [ComAliasName("FLOAT")]
        public float _23;

        [FieldOffset(28)]
        [ComAliasName("FLOAT")]
        public float _24;

        [FieldOffset(32)]
        [ComAliasName("FLOAT")]
        public float _31;

        [FieldOffset(36)]
        [ComAliasName("FLOAT")]
        public float _32;

        [FieldOffset(40)]
        [ComAliasName("FLOAT")]
        public float _33;

        [FieldOffset(44)]
        [ComAliasName("FLOAT")]
        public float _34;

        [FieldOffset(48)]
        [ComAliasName("FLOAT")]
        public float _41;

        [FieldOffset(52)]
        [ComAliasName("FLOAT")]
        public float _42;

        [FieldOffset(56)]
        [ComAliasName("FLOAT")]
        public float _43;

        [FieldOffset(60)]
        [ComAliasName("FLOAT")]
        public float _44;

        [FieldOffset(64)]
        [ComAliasName("FLOAT")]
        public float _51;

        [FieldOffset(68)]
        [ComAliasName("FLOAT")]
        public float _52;

        [FieldOffset(72)]
        [ComAliasName("FLOAT")]
        public float _53;

        [FieldOffset(76)]
        [ComAliasName("FLOAT")]
        public float _54;
        #endregion

        [FieldOffset(0)]
        [ComAliasName("FLOAT[5][4]")]
        public fixed float m[5 * 4];
        #endregion
    }
}
