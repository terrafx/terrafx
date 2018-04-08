// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\windef.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct RECT
    {
        #region Fields
        [ComAliasName("LONG")]
        public int left;

        [ComAliasName("LONG")]
        public int top;

        [ComAliasName("LONG")]
        public int right;

        [ComAliasName("LONG")]
        public int bottom;
        #endregion
    }
}
