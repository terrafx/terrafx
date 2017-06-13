// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("77ACCE80-638E-4E65-8895-C1F23386863E")]
    unsafe public struct ID3D12Device1
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Device1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT CreatePipelineLibrary(
            [In] ID3D12Device1* This,
            [In] void* pLibraryBlob,
            [In] UIntPtr BlobLength,
            [In] Guid* riid,
            [Out] void** ppPipelineLibrary
        );

        public /* static */ delegate HRESULT SetEventOnMultipleFenceCompletion(
            [In] ID3D12Device1* This,
            [In] ID3D12Fence** ppFences,
            [In] ulong* pFenceValues,
            [In] uint NumFences,
            [In] D3D12_MULTIPLE_FENCE_WAIT_FLAGS Flags,
            [In] HANDLE hEvent
        );

        public /* static */ delegate HRESULT SetResidencyPriority(
            [In] ID3D12Device1* This,
            [In] uint NumObjects,
            [In] ID3D12Pageable** ppObjects,
            [In] D3D12_RESIDENCY_PRIORITY* pPriorities
        );
        #endregion

        #region Structs
        public struct Vtbl
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
