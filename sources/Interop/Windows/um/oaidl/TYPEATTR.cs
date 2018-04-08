// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public /* unmanaged */ unsafe struct TYPEATTR
    {
        #region Fields
        [ComAliasName("GUID")]
        public Guid guid;

        [ComAliasName("LCID")]
        public uint lcid;

        [ComAliasName("DWORD")]
        public uint dwReserved;

        [ComAliasName("MEMBERID")]
        public int memidConstructor;

        [ComAliasName("MEMBERID")]
        public int memidDestructor;

        [ComAliasName("LPOLESTR")]
        public char* lpstrSchema;

        [ComAliasName("ULONG")]
        public uint cbSizeInstance;

        public TYPEKIND typekind;

        [ComAliasName("WORD")]
        public ushort cFuncs;

        [ComAliasName("WORD")]
        public ushort cVars;

        [ComAliasName("WORD")]
        public ushort cImplTypes;

        [ComAliasName("WORD")]
        public ushort cbSizeVft;

        [ComAliasName("WORD")]
        public ushort cbAlignment;

        [ComAliasName("WORD")]
        public ushort wTypeFlags;

        [ComAliasName("WORD")]
        public ushort wMajorVerNum;

        [ComAliasName("WORD")]
        public ushort wMinorVerNum;

        public TYPEDESC tdescAlias;

        public IDLDESC idldescType;
        #endregion
    }
}
