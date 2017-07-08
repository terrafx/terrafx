// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("77ACCE80-638E-4E65-8895-C1F23386863E")]
    unsafe public /* blittable */ struct ID3D12Device1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreatePipelineLibrary(
            [In] ID3D12Device1* This,
            [In] /* readonly */ void* pLibraryBlob,
            [In] SIZE_T BlobLength,
            [In] REFIID riid,
            [Out] void** ppPipelineLibrary
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetEventOnMultipleFenceCompletion(
            [In] ID3D12Device1* This,
            [In] /* readonly */ ID3D12Fence** ppFences,
            [In] /* readonly */ UINT64* pFenceValues,
            [In] UINT NumFences,
            [In] D3D12_MULTIPLE_FENCE_WAIT_FLAGS Flags,
            [In] HANDLE hEvent
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetResidencyPriority(
            [In] ID3D12Device1* This,
            [In] UINT NumObjects,
            [In] /* readonly */ ID3D12Pageable** ppObjects,
            [In] /* readonly */ D3D12_RESIDENCY_PRIORITY* pPriorities
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Device.Vtbl BaseVtbl;

            public CreatePipelineLibrary CreatePipelineLibrary;

            public SetEventOnMultipleFenceCompletion SetEventOnMultipleFenceCompletion;

            public SetResidencyPriority SetResidencyPriority;
            #endregion
        }
        #endregion
    }
}
