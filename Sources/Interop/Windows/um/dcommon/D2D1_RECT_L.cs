// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D1_RECT_L
    {
        #region Fields
        [FieldOffset(0)]
        internal D2D_RECT_L _value;
        #endregion

        #region D2D_RECT_L Fields
        [FieldOffset(0)]
        public LONG left;

        [FieldOffset(4)]
        public LONG top;

        [FieldOffset(8)]
        public LONG right;

        [FieldOffset(12)]
        public LONG bottom;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D2D1_RECT_L" /> struct.</summary>
        /// <param name="value">The <see cref="D2D_RECT_L" /> used to initialize the instance.</param>
        public D2D1_RECT_L(D2D_RECT_L value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="D2D1_RECT_L" /> value to a <see cref="D2D_RECT_L" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_RECT_L" /> value to convert.</param>
        public static implicit operator D2D_RECT_L(D2D1_RECT_L value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D1_RECT_L" /> value to a <see cref="RECT" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_RECT_L" /> value to convert.</param>
        public static implicit operator RECT(D2D1_RECT_L value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D_RECT_L" /> value to a <see cref="D2D1_RECT_L" /> value.</summary>
        /// <param name="value">The <see cref="D2D_RECT_L" /> value to convert.</param>
        public static implicit operator D2D1_RECT_L(D2D_RECT_L value)
        {
            return new D2D1_RECT_L(value);
        }

        /// <summary>Implicitly converts a <see cref="RECT" /> value to a <see cref="D2D1_RECT_L" /> value.</summary>
        /// <param name="value">The <see cref="RECT" /> value to convert.</param>
        public static implicit operator D2D1_RECT_L(RECT value)
        {
            return new D2D1_RECT_L(value);
        }
        #endregion
    }
}
