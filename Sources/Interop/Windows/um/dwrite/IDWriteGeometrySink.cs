// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("2CD9069E-12E2-11DC-9FED-001143A055F9")]
    unsafe public /* blittable */ struct IDWriteGeometrySink
    {
        #region Fields
        internal ID2D1SimplifiedGeometrySink _value;
        #endregion

        #region ID2D1SimplifiedGeometrySink Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="IDWriteGeometrySink" /> struct.</summary>
        /// <param name="value">The <see cref="ID2D1SimplifiedGeometrySink" /> used to initialize the instance.</param>
        public IDWriteGeometrySink(ID2D1SimplifiedGeometrySink value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="IDWriteGeometrySink" /> value to a <see cref="ID2D1SimplifiedGeometrySink" /> value.</summary>
        /// <param name="value">The <see cref="IDWriteGeometrySink" /> value to convert.</param>
        public static implicit operator ID2D1SimplifiedGeometrySink(IDWriteGeometrySink value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="ID2D1SimplifiedGeometrySink" /> value to a <see cref="IDWriteGeometrySink" /> value.</summary>
        /// <param name="value">The <see cref="ID2D1SimplifiedGeometrySink" /> value to convert.</param>
        public static implicit operator IDWriteGeometrySink(ID2D1SimplifiedGeometrySink value)
        {
            return new IDWriteGeometrySink(value);
        }
        #endregion
    }
}
