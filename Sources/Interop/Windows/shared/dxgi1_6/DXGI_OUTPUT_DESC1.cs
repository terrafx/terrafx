// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_6.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe public /* blittable */ struct DXGI_OUTPUT_DESC1
    {
        #region Fields
        [ComAliasName("WCHAR[32]")]
        public fixed char DeviceName[32];

        public RECT DesktopCoordinates;

        [ComAliasName("BOOL")]
        public int AttachedToDesktop;

        public DXGI_MODE_ROTATION Rotation;

        [ComAliasName("HMONITOR")]
        public IntPtr Monitor;

        [ComAliasName("UINT")]
        public uint BitsPerColor;

        public DXGI_COLOR_SPACE_TYPE ColorSpace;

        [ComAliasName("FLOAT[2]")]
        public fixed float RedPrimary[2];

        [ComAliasName("FLOAT[2]")]
        public fixed float GreenPrimary[2];

        [ComAliasName("FLOAT[2]")]
        public fixed float BluePrimary[2];

        [ComAliasName("FLOAT[2]")]
        public fixed float WhitePoint[2];

        [ComAliasName("FLOAT")]
        public float MinLuminance;

        [ComAliasName("FLOAT")]
        public float MaxLuminance;

        [ComAliasName("FLOAT")]
        public float MaxFullFrameLuminance;
        #endregion
    }
}
