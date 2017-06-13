// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

namespace TerraFX.Interop
{
    public struct D3D12_RT_FORMAT_ARRAY
    {
        #region Fields
        public _RTFormats_e__FixedBuffer RTFormats;

        public uint NumRenderTargets;
        #endregion

        #region Structs
        public struct _RTFormats_e__FixedBuffer
        {
            #region Fields
            public DXGI_FORMAT _0;

            public DXGI_FORMAT _1;

            public DXGI_FORMAT _2;

            public DXGI_FORMAT _3;

            public DXGI_FORMAT _4;

            public DXGI_FORMAT _5;

            public DXGI_FORMAT _6;

            public DXGI_FORMAT _7;
            #endregion
        }
        #endregion
    }
}
