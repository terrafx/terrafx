// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)] // Size = 8 or 16
    unsafe public struct DXGI_MAPPED_RECT
    {
        #region Fields
        public int Pitch;

        public byte* pBits;
        #endregion
    }
}
