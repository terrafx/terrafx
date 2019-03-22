// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\minwinbase.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public unsafe struct SECURITY_ATTRIBUTES
    {
        #region Fields
        [ComAliasName("DWORD")]
        public uint nLength;

        [ComAliasName("LPVOID")]
        public void* lpSecurityDescriptor;

        [ComAliasName("BOOL")]
        public int bInheritHandle;
        #endregion
    }
}
