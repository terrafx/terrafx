// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    /// <summary>Represents a 5-by-4 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D_MATRIX_5X4_F
    {
        #region Fields
        #region struct
        [FieldOffset(0)]
        public FLOAT _11;

        [FieldOffset(4)]
        public FLOAT _12;

        [FieldOffset(8)]
        public FLOAT _13;

        [FieldOffset(12)]
        public FLOAT _14;

        [FieldOffset(16)]
        public FLOAT _21;

        [FieldOffset(20)]
        public FLOAT _22;

        [FieldOffset(24)]
        public FLOAT _23;

        [FieldOffset(28)]
        public FLOAT _24;

        [FieldOffset(32)]
        public FLOAT _31;

        [FieldOffset(36)]
        public FLOAT _32;

        [FieldOffset(40)]
        public FLOAT _33;

        [FieldOffset(44)]
        public FLOAT _34;

        [FieldOffset(48)]
        public FLOAT _41;

        [FieldOffset(52)]
        public FLOAT _42;

        [FieldOffset(56)]
        public FLOAT _43;

        [FieldOffset(60)]
        public FLOAT _44;

        [FieldOffset(64)]
        public FLOAT _51;

        [FieldOffset(68)]
        public FLOAT _52;

        [FieldOffset(72)]
        public FLOAT _53;

        [FieldOffset(76)]
        public FLOAT _54;
        #endregion

        [FieldOffset(0)]
        public _m_e__FixedBuffer m;
        #endregion

        #region Structs
        unsafe public /* blittable */ struct _m_e__FixedBuffer
        {
            #region Fields
            public FLOAT e0_0, e0_1, e0_2, e0_3;

            public FLOAT e1_0, e1_1, e1_2, e1_3;

            public FLOAT e2_0, e2_1, e2_2, e2_3;

            public FLOAT e3_0, e3_1, e3_2, e3_3;

            public FLOAT e4_0, e4_1, e4_2, e4_3;
            #endregion

            #region Properties
            public FLOAT this[int index1, int index2]
            {
                get
                {
                    if ((uint)(index1) > 4) // (index1 < 0) || (index1 > 4)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index1), index1);
                    }

                    if ((uint)(index2) > 3) // (index2 < 0) || (index2 > 3)
                    {
                        ExceptionUtilities.ThrowArgumentOutOfRangeException(nameof(index2), index2);
                    }

                    fixed (FLOAT* e = &e0_0)
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
