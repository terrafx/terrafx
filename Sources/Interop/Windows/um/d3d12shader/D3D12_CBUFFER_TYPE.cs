// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_CBUFFER_TYPE
    {
        #region Fields
        internal D3D_CBUFFER_TYPE _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D3D12_CBUFFER_TYPE" /> struct.</summary>
        /// <param name="value">The <see cref="D3D_CBUFFER_TYPE" /> used to initialize the instance.</param>
        public D3D12_CBUFFER_TYPE(D3D_CBUFFER_TYPE value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Implicitly converts a <see cref="D3D12_CBUFFER_TYPE" /> value to a <see cref="D3D_CBUFFER_TYPE" /> value.</summary>
        /// <param name="value">The <see cref="D3D12_CBUFFER_TYPE" /> value to convert.</param>
        public static implicit operator D3D_CBUFFER_TYPE(D3D12_CBUFFER_TYPE value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D3D_CBUFFER_TYPE" /> value to a <see cref="D3D12_CBUFFER_TYPE" /> value.</summary>
        /// <param name="value">The <see cref="D3D_CBUFFER_TYPE" /> value to convert.</param>
        public static implicit operator D3D12_CBUFFER_TYPE(D3D_CBUFFER_TYPE value)
        {
            return new D3D12_CBUFFER_TYPE(value);
        }
        #endregion
    }
}
