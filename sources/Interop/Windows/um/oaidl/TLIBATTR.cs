// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\oaidl.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct TLIBATTR
    {
        #region Fields
        [ComAliasName("GUID")]
        public Guid guid;

        [ComAliasName("LCID")]
        public uint lcid;

        public SYSKIND syskind;

        [ComAliasName("WORD")]
        public ushort wMajorVerNum;

        [ComAliasName("WORD")]
        public ushort wMinorVerNum;

        [ComAliasName("WORD")]
        public ushort wLibFlags;
        #endregion
    }
}
