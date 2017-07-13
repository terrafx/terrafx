// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct D3D12_TESSELLATOR_PARTITIONING
    {
        #region Fields
        internal D3D_TESSELLATOR_PARTITIONING _value;
        #endregion

        #region Constructors
        /// <summary>Initializes a new instance of the <see cref="D3D12_TESSELLATOR_PARTITIONING" /> struct.</summary>
        /// <param name="value">The <see cref="D3D_TESSELLATOR_PARTITIONING" /> used to initialize the instance.</param>
        public D3D12_TESSELLATOR_PARTITIONING(D3D_TESSELLATOR_PARTITIONING value)
        {
            _value = value;
        }
        #endregion

        #region Cast Operators
        /// <summary>Implicitly converts a <see cref="D3D12_TESSELLATOR_PARTITIONING" /> value to a <see cref="D3D_TESSELLATOR_PARTITIONING" /> value.</summary>
        /// <param name="value">The <see cref="D3D12_TESSELLATOR_PARTITIONING" /> value to convert.</param>
        public static implicit operator D3D_TESSELLATOR_PARTITIONING(D3D12_TESSELLATOR_PARTITIONING value)
        {
            return value._value;
        }

        /// <summary>Implicitly converts a <see cref="D3D_TESSELLATOR_PARTITIONING" /> value to a <see cref="D3D12_TESSELLATOR_PARTITIONING" /> value.</summary>
        /// <param name="value">The <see cref="D3D_TESSELLATOR_PARTITIONING" /> value to convert.</param>
        public static implicit operator D3D12_TESSELLATOR_PARTITIONING(D3D_TESSELLATOR_PARTITIONING value)
        {
            return new D3D12_TESSELLATOR_PARTITIONING(value);
        }
        #endregion
    }
}
