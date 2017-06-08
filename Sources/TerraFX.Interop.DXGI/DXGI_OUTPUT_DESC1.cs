// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.DXGI
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]  // Size = 144 or 148
    unsafe public struct DXGI_OUTPUT_DESC1
    {
        #region Fields
        public fixed ushort DeviceName[32];

        public RECT DesktopCoordinates;

        public int /* BOOL */ AttachedToDesktop;

        public DXGI_MODE_ROTATION Rotation;

        public IntPtr /* HMONITOR */ Monitor;

        public uint BitsPerColor;

        public DXGI_COLOR_SPACE_TYPE ColorSpace;

        public fixed float RedPrimary[2];

        public fixed float GreenPrimary[2];

        public fixed float BluePrimary[2];

        public fixed float WhitePoint[2];

        public float MinLuminance;

        public float MaxLuminance;

        public float MaxFullFrameLuminance;
        #endregion
    }
}
