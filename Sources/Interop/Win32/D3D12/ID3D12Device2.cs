// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("30BAA41E-B15B-475C-A0BB-1AF5C5B64328")]
    unsafe public struct ID3D12Device2
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Device2).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CreatePipelineState(
            [In] ID3D12Device2* This,
            [In] D3D12_PIPELINE_STATE_STREAM_DESC* pDesc,
            [In] ref /* readonly */ Guid riid,
            [Out] void** ppPipelineState
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Device1.Vtbl BaseVtbl;

            public CreatePipelineState CreatePipelineState;
            #endregion
        }
        #endregion
    }
}
