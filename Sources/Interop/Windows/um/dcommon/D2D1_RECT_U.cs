// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D2D1_RECT_U
    {
        #region Fields
        [FieldOffset(0)]
        internal D2D_RECT_U _value;
        #endregion

        #region D2D_RECT_U Fields
        [FieldOffset(0)]
        public UINT32 left;

        [FieldOffset(4)]
        public UINT32 top;

        [FieldOffset(8)]
        public UINT32 right;

        [FieldOffset(12)]
        public UINT32 bottom;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D2D1_RECT_U" /> struct.</summary>
        /// <param name="value">The <see cref="D2D_RECT_U" /> used to initialize the instance.</param>
        public D2D1_RECT_U(D2D_RECT_U value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="D2D1_RECT_U" /> value to a <see cref="D2D_RECT_U" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_RECT_U" /> value to convert.</param>
        public static implicit operator D2D_RECT_U(D2D1_RECT_U value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D_RECT_U" /> value to a <see cref="D2D1_RECT_U" /> value.</summary>
        /// <param name="value">The <see cref="D2D_RECT_U" /> value to convert.</param>
        public static implicit operator D2D1_RECT_U(D2D_RECT_U value)
        {
            return new D2D1_RECT_U(value);
        }
        #endregion
    }
}
