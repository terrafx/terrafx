// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct VARIANTARG
    {
        #region Fields
        internal VARIANT _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="VARIANTARG" /> struct.</summary>
        /// <param name="value">The <see cref="VARIANT" /> used to initialize the instance.</param>
        public VARIANTARG(VARIANT value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Implicitly converts a <see cref="VARIANTARG" /> value to a <see cref="VARIANT" /> value.</summary>
        /// <param name="value">The <see cref="VARIANTARG" /> value to convert.</param>
        public static implicit operator VARIANT(VARIANTARG value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="VARIANT" /> value to a <see cref="VARIANTARG" /> value.</summary>
        /// <param name="value">The <see cref="VARIANT" /> value to convert.</param>
        public static implicit operator VARIANTARG(VARIANT value)
        {
            return new VARIANTARG(value);
        }
        #endregion
    }
}
