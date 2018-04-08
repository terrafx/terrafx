// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System.Runtime.InteropServices;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    public /* unmanaged */ struct D3D12_BLEND_DESC
    {
        #region Fields
        [ComAliasName("BOOL")]
        public int AlphaToCoverageEnable;

        [ComAliasName("BOOL")]
        public int IndependentBlendEnable;

        [ComAliasName("D3D12_RENDER_TARGET_BLEND_DESC[8]")]
        public _RenderTarget_e__FixedBuffer RenderTarget;
        #endregion

        #region Structs
        public /* unmanaged */ unsafe struct _RenderTarget_e__FixedBuffer
        {
            #region Fields
            public D3D12_RENDER_TARGET_BLEND_DESC e0;

            public D3D12_RENDER_TARGET_BLEND_DESC e1;

            public D3D12_RENDER_TARGET_BLEND_DESC e2;

            public D3D12_RENDER_TARGET_BLEND_DESC e3;

            public D3D12_RENDER_TARGET_BLEND_DESC e4;

            public D3D12_RENDER_TARGET_BLEND_DESC e5;

            public D3D12_RENDER_TARGET_BLEND_DESC e6;

            public D3D12_RENDER_TARGET_BLEND_DESC e7;
            #endregion

            #region Properties
            public ref D3D12_RENDER_TARGET_BLEND_DESC this[int index]
            {
                get
                {
                    fixed (D3D12_RENDER_TARGET_BLEND_DESC* e = &e0)
                    {
                        return ref AsRef<D3D12_RENDER_TARGET_BLEND_DESC>(e + index);
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
