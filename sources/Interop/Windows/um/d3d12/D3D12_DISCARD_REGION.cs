// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* unmanaged */ unsafe struct D3D12_DISCARD_REGION
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint NumRects;

        [ComAliasName("D3D12_RECT[]")]
        public RECT* pRects;

        [ComAliasName("UINT")]
        public uint FirstSubresource;

        [ComAliasName("UINT")]
        public uint NumSubresources;
        #endregion
    }
}
