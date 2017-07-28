// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    unsafe public /* blittable */ struct EXCEPINFO
    {
        #region Fields
        [ComAliasName("WORD")]
        public ushort wCode;

        [ComAliasName("WORD")]
        public ushort wReserved;

        [ComAliasName("BSTR")]
        public char* bstrSource;

        [ComAliasName("BSTR")]
        public char* bstrDescription;

        [ComAliasName("BSTR")]
        public char* bstrHelpFile;

        [ComAliasName("DWORD")]
        public uint dwHelpContext;

        [ComAliasName("PVOID")]
        public void* pvReserved;

        public IntPtr /* pfnDeferredFillIn */ pfnDeferredFillIn;

        [ComAliasName("SCODE")]
        public int scode;
        #endregion
    }
}
