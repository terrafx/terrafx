// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3dcommon.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("8BA5FB08-5195-40E2-AC58-0D989C3A0102")]
    [StructLayout(LayoutKind.Explicit)]
    unsafe public /* blittable */ struct ID3DBlob
    {
        #region Fields
        [FieldOffset(0)]
        internal ID3D10Blob _value;
        #endregion

        #region ID3D10Blob Fields
        [FieldOffset(0)]
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="ID3DBlob" /> struct.</summary>
        /// <param name="value">The <see cref="ID3D10Blob" /> used to initialize the instance.</param>
        public ID3DBlob(ID3D10Blob value) : this()
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="ID3DBlob" /> value to a <see cref="ID3D10Blob" /> value.</summary>
        /// <param name="value">The <see cref="ID3DBlob" /> value to convert.</param>
        public static implicit operator ID3D10Blob(ID3DBlob value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="ID3D10Blob" /> value to a <see cref="ID3DBlob" /> value.</summary>
        /// <param name="value">The <see cref="ID3D10Blob" /> value to convert.</param>
        public static implicit operator ID3DBlob(ID3D10Blob value)
        {
            return new ID3DBlob(value);
        }
        #endregion
    }
}
