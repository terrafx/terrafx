// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("30BAA41E-B15B-475C-A0BB-1AF5C5B64328")]
    unsafe public /* blittable */ struct ID3D12Device2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreatePipelineState(
            [In] ID3D12Device2* This,
            [In] /* readonly */ D3D12_PIPELINE_STATE_STREAM_DESC* pDesc,
            [In] REFIID riid,
            [Out] void** ppPipelineState
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Device1.Vtbl BaseVtbl;

            public CreatePipelineState CreatePipelineState;
            #endregion
        }
        #endregion
    }
}
