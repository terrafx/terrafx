// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    public /* blittable */ struct WICDdsParameters
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint Width;

        [ComAliasName("UINT")]
        public uint Height;

        [ComAliasName("UINT")]
        public uint Depth;

        [ComAliasName("UINT")]
        public uint MipLevels;

        [ComAliasName("UINT")]
        public uint ArraySize;

        public DXGI_FORMAT DxgiFormat;

        public WICDdsDimension Dimension;

        public WICDdsAlphaMode AlphaMode;
        #endregion
    }
}
