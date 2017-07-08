// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_TESSELLATOR_DOMAIN
    {
        #region Fields
        internal D3D_TESSELLATOR_DOMAIN _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D3D12_TESSELLATOR_DOMAIN" /> struct.</summary>
        /// <param name="value">The <see cref="D3D_TESSELLATOR_DOMAIN" /> used to initialize the instance.</param>
        public D3D12_TESSELLATOR_DOMAIN(D3D_TESSELLATOR_DOMAIN value)
        {
            _value = value;
        }
        #endregion

        #region Operators
        /// <summary>Explicitly converts a <see cref="D3D12_TESSELLATOR_DOMAIN" /> value to a <see cref="D3D_TESSELLATOR_DOMAIN" /> value.</summary>
        /// <param name="value">The <see cref="D3D12_TESSELLATOR_DOMAIN" /> value to convert.</param>
        public static implicit operator D3D_TESSELLATOR_DOMAIN(D3D12_TESSELLATOR_DOMAIN value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D3D_TESSELLATOR_DOMAIN" /> value to a <see cref="D3D12_TESSELLATOR_DOMAIN" /> value.</summary>
        /// <param name="value">The <see cref="D3D_TESSELLATOR_DOMAIN" /> value to convert.</param>
        public static implicit operator D3D12_TESSELLATOR_DOMAIN(D3D_TESSELLATOR_DOMAIN value)
        {
            return new D3D12_TESSELLATOR_DOMAIN(value);
        }
        #endregion
    }
}
