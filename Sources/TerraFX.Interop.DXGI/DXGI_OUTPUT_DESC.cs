// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_OUTPUT_DESC
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public ushort[] DeviceName;
        public RECT DesktopCoordinates;
        public int AttachedToDesktop;
        public DXGI_MODE_ROTATION Rotation;
        public IntPtr Monitor;
    }
}
