// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct DXGI_JPEG_DC_HUFFMAN_TABLE
    {
        #region Fields
        [ComAliasName("BYTE[12]")]
        public fixed byte CodeCounts[12];

        [ComAliasName("BYTE[12]")]
        public fixed byte CodeValues[12];
        #endregion
    }
}
