// Copyright © Tanner Gooding and Contributors.Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\objidlbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public /* unmanaged */ unsafe struct STATSTG
    {
        #region Fields
        [ComAliasName("LPOLESTR")]
        public char* pwcsName;

        [ComAliasName("DWORD")]
        public uint type;

        public ULARGE_INTEGER cbSize;

        public FILETIME mtime;

        public FILETIME ctime;

        public FILETIME atime;

        [ComAliasName("DWORD")]
        public uint grfMode;

        [ComAliasName("DWORD")]
        public uint grfLocksSupported;

        [ComAliasName("CLSID")]
        public Guid clsid;

        [ComAliasName("DWORD")]
        public uint grfStateBits;

        [ComAliasName("DWORD")]
        public uint reserved;
        #endregion
    }
}
