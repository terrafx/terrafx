// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_DISPLAY_COLOR_SPACE
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public float[] PrimaryCoordinates;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public float[] WhitePoints;
    }
}
