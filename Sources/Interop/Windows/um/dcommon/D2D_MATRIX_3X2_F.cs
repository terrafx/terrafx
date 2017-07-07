// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    /// <summary>Represents a 3-by-2 matrix.</summary>
    [StructLayout(LayoutKind.Explicit)]
    unsafe public /* blittable */ struct D2D_MATRIX_3X2_F
    {
        #region Fields
        #region struct
        /// <summary>Horizontal scaling / cosine of rotation</summary>
        [FieldOffset(0)]
        public float m11;

        /// <summary>Vertical shear / sine of rotation</summary>
        [FieldOffset(4)]
        public float m12;

        /// <summary>Horizontal shear / negative sine of rotation</summary>
        [FieldOffset(8)]
        public float m21;

        /// <summary>Vertical scaling / cosine of rotation</summary>
        [FieldOffset(12)]
        public float m22;

        /// <summary>Horizontal shift (always orthogonal regardless of rotation)</summary>
        [FieldOffset(16)]
        public float dx;

        /// <summary>Vertical shift (always orthogonal regardless of rotation)</summary>
        [FieldOffset(20)]
        public float dy;
        #endregion

        #region struct
        [FieldOffset(0)]
        public FLOAT _11;

        [FieldOffset(4)]
        public FLOAT _12;

        [FieldOffset(8)]
        public FLOAT _21;

        [FieldOffset(12)]
        public FLOAT _22;

        [FieldOffset(16)]
        public FLOAT _31;

        [FieldOffset(20)]
        public FLOAT _32;
        #endregion

        [FieldOffset(0)]
        public _m_e__FixedBuffer m;
        #endregion

        #region Structs
        public /* blittable */ struct _m_e__FixedBuffer
        {
            #region Fields
            #region 0
            public FLOAT _0_0;

            public FLOAT _0_1;
            #endregion

            #region 1
            public FLOAT _1_0;

            public FLOAT _1_1;
            #endregion

            #region 2
            public FLOAT _2_0;

            public FLOAT _2_1;
            #endregion
            #endregion
        }
        #endregion
    }
}
