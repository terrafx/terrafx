// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wingdi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe public /* blittable */ struct LOGFONT
    {
        #region Fields
        [ComAliasName("LONG")]
        public int lfHeight;

        [ComAliasName("LONG")]
        public int lfWidth;

        [ComAliasName("LONG")]
        public int lfEscapement;

        [ComAliasName("LONG")]
        public int lfOrientation;

        [ComAliasName("LONG")]
        public int lfWeight;

        [ComAliasName("BYTE")]
        public byte lfItalic;

        [ComAliasName("BYTE")]
        public byte lfUnderline;

        [ComAliasName("BYTE")]
        public byte lfStrikeOut;

        [ComAliasName("BYTE")]
        public byte lfCharSet;

        [ComAliasName("BYTE")]
        public byte lfOutPrecision;

        [ComAliasName("BYTE")]
        public byte lfClipPrecision;

        [ComAliasName("BYTE")]
        public byte lfQuality;

        [ComAliasName("BYTE")]
        public byte lfPitchAndFamily;

        [ComAliasName("WCHAR[32]")]
        public fixed char lfFaceName[32];
        #endregion
    }
}
