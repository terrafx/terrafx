// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgitype.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DXGI_JPEG_DC_HUFFMAN_TABLE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodeCounts;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] CodeValues;
    }
}
