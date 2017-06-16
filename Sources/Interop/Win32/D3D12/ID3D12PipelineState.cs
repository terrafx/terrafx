// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("765A30F3-F624-4C6F-A828-ACE948622445")]
    unsafe public struct ID3D12PipelineState
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12PipelineState).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetCachedBlob(
            [In] ID3D12PipelineState* This,
            [Out] ID3DBlob** ppBlob
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public GetCachedBlob GetCachedBlob;
            #endregion
        }
        #endregion
    }
}
