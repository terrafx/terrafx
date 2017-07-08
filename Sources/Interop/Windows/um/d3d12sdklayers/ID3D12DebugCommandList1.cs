// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("102CA951-311B-4B01-B11F-ECB83E061B37")]
    unsafe public /* blittable */ struct ID3D12DebugCommandList1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL AssertResourceState(
            [In] ID3D12DebugCommandList1* This,
            [In] ID3D12Resource* pResource,
            [In] UINT Subresource,
            [In] UINT State
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetDebugParameter(
            [In] ID3D12DebugCommandList1* This,
            [In] D3D12_DEBUG_COMMAND_LIST_PARAMETER_TYPE Type,
            [In] /* readonly */ void* pData,
            [In] UINT DataSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDebugParameter(
            [In] ID3D12DebugCommandList1* This,
            [In] D3D12_DEBUG_COMMAND_LIST_PARAMETER_TYPE Type,
            [Out] void* pData,
            [In] UINT DataSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public AssertResourceState AssertResourceState;

            public SetDebugParameter SetDebugParameter;

            public GetDebugParameter GetDebugParameter;
            #endregion
        }
        #endregion
    }
}
