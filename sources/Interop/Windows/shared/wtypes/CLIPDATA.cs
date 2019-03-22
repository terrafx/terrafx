// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\wtypes.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct CLIPDATA
    {
        #region Fields
        [ComAliasName("ULONG")]
        public uint cbSize;

        [ComAliasName("LONG")]
        public int ulClipFmt;

        [ComAliasName("BYTE[]")]
        public byte* pClipDAta;
        #endregion
    }
}
