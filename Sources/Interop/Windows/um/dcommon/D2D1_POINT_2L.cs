// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D1_POINT_2L
    {
        #region Fields
        [FieldOffset(0)]
        internal D2D_POINT_2L _value;
        #endregion

        #region D2D_POINT_2L Fields
        [FieldOffset(0)]
        public LONG x;

        [FieldOffset(4)]
        public LONG y;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D2D1_POINT_2L" /> struct.</summary>
        /// <param name="value">The <see cref="D2D_POINT_2L" /> used to initialize the instance.</param>
        public D2D1_POINT_2L(D2D_POINT_2L value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="D2D1_POINT_2L" /> value to a <see cref="D2D_POINT_2L" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_POINT_2L" /> value to convert.</param>
        public static implicit operator D2D_POINT_2L(D2D1_POINT_2L value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D_POINT_2L" /> value to a <see cref="D2D1_POINT_2L" /> value.</summary>
        /// <param name="value">The <see cref="D2D_POINT_2L" /> value to convert.</param>
        public static implicit operator D2D1_POINT_2L(D2D_POINT_2L value)
        {
            return new D2D1_POINT_2L(value);
        }
        #endregion
    }
}
