// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    unsafe public /* blittable */ struct DXGI_JPEG_QUANTIZATION_TABLE
    {
        #region Fields
        [ComAliasName("BYTE[64]")]
        public fixed byte Elements[64];
        #endregion
    }
}
