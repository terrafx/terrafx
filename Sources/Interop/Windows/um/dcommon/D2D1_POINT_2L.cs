// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D2D1_POINT_2L
    {
        #region Fields
        internal D2D_POINT_2L _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D2D1_POINT_2L" /> struct.</summary>
        /// <param name="value">The <see cref="D2D_POINT_2L" /> used to initialize the instance.</param>
        public D2D1_POINT_2L(D2D_POINT_2L value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="D2D1_POINT_2L" /> value to a <see cref="D2D_POINT_2L" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_POINT_2L" /> value to convert.</param>
        public static implicit operator D2D_POINT_2L(D2D1_POINT_2L value)
        {
            return value._value;
        }

        /// <summary>Explicitly converts a <see cref="D2D1_POINT_2L" /> value to a <see cref="POINT" /> value.</summary>
        /// <param name="value">The <see cref="D2D1_POINT_2L" /> value to convert.</param>
        public static implicit operator POINT(D2D1_POINT_2L value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D2D_POINT_2L" /> value to a <see cref="D2D1_POINT_2L" /> value.</summary>
        /// <param name="value">The <see cref="D2D_POINT_2L" /> value to convert.</param>
        public static implicit operator D2D1_POINT_2L(D2D_POINT_2L value)
        {
            return new D2D1_POINT_2L(value);
        }

        /// <summary>Implicitly converts a <see cref="POINT" /> value to a <see cref="D2D1_POINT_2L" /> value.</summary>
        /// <param name="value">The <see cref="POINT" /> value to convert.</param>
        public static implicit operator D2D1_POINT_2L(POINT value)
        {
            return new D2D1_POINT_2L(value);
        }
        #endregion
    }
}
