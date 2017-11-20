// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop.Desktop
{
    public /* blittable */ struct DXGI_OUTDUPL_POINTER_SHAPE_INFO
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint Type;

        [ComAliasName("UINT")]
        public uint Width;

        [ComAliasName("UINT")]
        public uint Height;

        [ComAliasName("UINT")]
        public uint Pitch;

        public POINT HotSpot;
        #endregion
    }
}
