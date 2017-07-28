// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.ExceptionUtilities;

namespace TerraFX.Interop
{
    /// <summary>Represents a 4-by-3 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D_MATRIX_4X3_F
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
        public float _21;

        [FieldOffset(16)]
        [ComAliasName("FLOAT")]
        public float _22;

        [FieldOffset(20)]
        [ComAliasName("FLOAT")]
        public float _23;

        [FieldOffset(24)]
        [ComAliasName("FLOAT")]
        public float _31;

        [FieldOffset(28)]
        [ComAliasName("FLOAT")]
        public float _32;

        [FieldOffset(32)]
        [ComAliasName("FLOAT")]
        public float _33;

        [FieldOffset(36)]
        [ComAliasName("FLOAT")]
        public float _41;

        [FieldOffset(40)]
        [ComAliasName("FLOAT")]
        public float _42;

        [FieldOffset(44)]
        [ComAliasName("FLOAT")]
        public float _43;
        #endregion

        [FieldOffset(0)]
        [ComAliasName("FLOAT[4][3]")]
        public _m_e__FixedBuffer m;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _m_e__FixedBuffer
        {
            #region Fields
            public float e0_0, e0_1, e0_2;

            public float e1_0, e1_1, e1_2;

            public float e2_0, e2_1, e2_2;

            public float e3_0, e3_1, e3_2;
            #endregion

            #region Properties
            public float this[int index1, int index2]
            {
                get
                {
                    if ((uint)(index1) > 3) // (index1 < 0) || (index1 > 3)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index1), index1);
                    }

                    if ((uint)(index2) > 2) // (index2 < 0) || (index2 > 2)
                    {
                        ThrowArgumentOutOfRangeException(nameof(index2), index2);
                    }

                    fixed (float* e = &e0_0)
                    {
                        return e[(index1 * 2) + index2];
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
