// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D1_MATRIX_4X3_F
    {
        #region Fields
        [FieldOffset(0)]
        internal D2D_MATRIX_4X3_F _value;
        #endregion

        #region D2D_MATRIX_4X3_F Fields
        #region struct
        [FieldOffset(0)]
        public FLOAT _11;

        [FieldOffset(4)]
        public FLOAT _12;

        [FieldOffset(8)]
        public FLOAT _13;

        [FieldOffset(12)]
        public FLOAT _21;

        [FieldOffset(16)]
        public FLOAT _22;

        [FieldOffset(20)]
        public FLOAT _23;

        [FieldOffset(24)]
        public FLOAT _31;

        [FieldOffset(28)]
        public FLOAT _32;

        [FieldOffset(32)]
        public FLOAT _33;

        [FieldOffset(36)]
        public FLOAT _41;

        [FieldOffset(40)]
        public FLOAT _42;

        [FieldOffset(44)]
        public FLOAT _43;
        #endregion

        [FieldOffset(0)]
        public D2D_MATRIX_4X3_F._m_e__FixedBuffer m;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D2D1_MATRIX_4X3_F" /> struct.</summary>
        /// <param name="value">The <see cref="D2D_MATRIX_4X3_F" /> used to initialize the instance.</param>
        public D2D1_MATRIX_4X3_F(D2D_MATRIX_4X3_F value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="D2D1_MATRIX_4X3_F" /> value to a <see cref="D2D_MATRIX_4X3_F" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_MATRIX_4X3_F" /> value to convert.</param>
        public static implicit operator D2D_MATRIX_4X3_F(D2D1_MATRIX_4X3_F value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D_MATRIX_4X3_F" /> value to a <see cref="D2D1_MATRIX_4X3_F" /> value.</summary>
        /// <param name="value">The <see cref="D2D_MATRIX_4X3_F" /> value to convert.</param>
        public static implicit operator D2D1_MATRIX_4X3_F(D2D_MATRIX_4X3_F value)
        {
            return new D2D1_MATRIX_4X3_F(value);
        }
        #endregion
    }
}
