// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d2d1_1.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D1_VECTOR_2F
    {
        #region Fields
        [FieldOffset(0)]
        internal D2D_VECTOR_2F _value;
        #endregion

        #region D2D_VECTOR_2F Fields
        [FieldOffset(0)]
        public FLOAT x;

        [FieldOffset(4)]
        public FLOAT y;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D2D1_VECTOR_2F" /> struct.</summary>
        /// <param name="value">The <see cref="D2D_VECTOR_2F" /> used to initialize the instance.</param>
        public D2D1_VECTOR_2F(D2D_VECTOR_2F value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="D2D1_VECTOR_2F" /> value to a <see cref="D2D_VECTOR_2F" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_VECTOR_2F" /> value to convert.</param>
        public static implicit operator D2D_VECTOR_2F(D2D1_VECTOR_2F value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D_VECTOR_2F" /> value to a <see cref="D2D1_VECTOR_2F" /> value.</summary>
        /// <param name="value">The <see cref="D2D_VECTOR_2F" /> value to convert.</param>
        public static implicit operator D2D1_VECTOR_2F(D2D_VECTOR_2F value)
        {
            return new D2D1_VECTOR_2F(value);
        }
        #endregion
    }
}
