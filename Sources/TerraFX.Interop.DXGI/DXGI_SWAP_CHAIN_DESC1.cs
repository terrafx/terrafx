// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    public struct DXGI_SWAP_CHAIN_DESC1
    {
        #region Fields
        public uint Width;

        public uint Height;

        public DXGI_FORMAT Format;

        public BOOL Stereo;

        public DXGI_SAMPLE_DESC SampleDesc;

        public DXGI_USAGE BufferUsage;

        public uint BufferCount;

        public DXGI_SCALING Scaling;

        public DXGI_SWAP_EFFECT SwapEffect;

        public DXGI_ALPHA_MODE AlphaMode;

        public DXGI_SWAP_CHAIN_FLAG Flags;
        #endregion
    }
}
