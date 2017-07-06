// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_3.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public /* blittable */ struct DXGI_MATRIX_3X2_F
    {
        #region Fields
        public FLOAT _11;

        public FLOAT _12;

        public FLOAT _21;

        public FLOAT _22;

        public FLOAT _31;

        public FLOAT _32;
        #endregion
    }
}
