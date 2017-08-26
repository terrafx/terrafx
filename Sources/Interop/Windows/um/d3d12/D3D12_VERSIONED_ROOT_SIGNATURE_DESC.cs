// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Explicit)]
    public /* blittable */ struct D3D12_VERSIONED_ROOT_SIGNATURE_DESC
    {
        #region Fields
        [FieldOffset(0)]
        public D3D_ROOT_SIGNATURE_VERSION Version;

        #region union
        [FieldOffset(8)]
        public D3D12_ROOT_SIGNATURE_DESC Desc_1_0;

        [FieldOffset(8)]
        public D3D12_ROOT_SIGNATURE_DESC1 Desc_1_1;
        #endregion
        #endregion
    }
}
