// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct WICRect
    {
        #region Fields
        [ComAliasName("INT")]
        public int X;

        [ComAliasName("INT")]
        public int Y;

        [ComAliasName("INT")]
        public int Width;

        [ComAliasName("INT")]
        public int Height;
        #endregion
    }
}
