// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct DXGI_OUTPUT_DESC1
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public ushort[] DeviceName;
        public RECT DesktopCoordinates;
        public int AttachedToDesktop;
        public DXGI_MODE_ROTATION Rotation;
        public IntPtr Monitor;
        public uint BitsPerColor;
        public DXGI_COLOR_SPACE_TYPE ColorSpace;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] RedPrimary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] GreenPrimary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] BluePrimary;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public float[] WhitePoint;
        public float MinLuminance;
        public float MaxLuminance;
        public float MaxFullFrameLuminance;
    }
}
