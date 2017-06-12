// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    public struct D3D12_BLEND_DESC
    {
        #region Fields
        public BOOL AlphaToCoverageEnable;

        public BOOL IndependentBlendEnable;

        public _RenderTarget_e__FixedBuffer RenderTarget;
        #endregion

        #region Structs
        public struct _RenderTarget_e__FixedBuffer
        {
            #region Fields
            public D3D12_RENDER_TARGET_BLEND_DESC _0;

            public D3D12_RENDER_TARGET_BLEND_DESC _1;

            public D3D12_RENDER_TARGET_BLEND_DESC _2;

            public D3D12_RENDER_TARGET_BLEND_DESC _3;

            public D3D12_RENDER_TARGET_BLEND_DESC _4;

            public D3D12_RENDER_TARGET_BLEND_DESC _5;

            public D3D12_RENDER_TARGET_BLEND_DESC _6;

            public D3D12_RENDER_TARGET_BLEND_DESC _7;
            #endregion
        }
        #endregion
    }
}
