// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using TerraFX.Utilities;

namespace TerraFX.Interop
{
    [Unmanaged]
    public struct DXGI_SWAP_CHAIN_DESC1
    {
        #region Fields
        [ComAliasName("UINT")]
        public uint Width;

        [ComAliasName("UINT")]
        public uint Height;

        public DXGI_FORMAT Format;

        [ComAliasName("BOOL")]
        public int Stereo;

        public DXGI_SAMPLE_DESC SampleDesc;

        [ComAliasName("DXGI_USAGE")]
        public uint BufferUsage;

        [ComAliasName("UINT")]
        public uint BufferCount;

        public DXGI_SCALING Scaling;

        public DXGI_SWAP_EFFECT SwapEffect;

        public DXGI_ALPHA_MODE AlphaMode;

        [ComAliasName("UINT")]
        public uint Flags;
        #endregion
    }
}
