// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("102CA951-311B-4B01-B11F-ECB83E061B37")]
    unsafe public struct ID3D12DebugCommandList1
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DebugCommandList1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate BOOL AssertResourceState(
            [In] ID3D12DebugCommandList1* This,
            [In] ID3D12Resource* pResource,
            [In] uint Subresource,
            [In] uint State
        );

        public /* static */ delegate HRESULT SetDebugParameter(
            [In] ID3D12DebugCommandList1* This,
            [In] D3D12_DEBUG_COMMAND_LIST_PARAMETER_TYPE Type,
            [In] void* pData,
            [In] uint DataSize
        );

        public /* static */ delegate HRESULT GetDebugParameter(
            [In] ID3D12DebugCommandList1* This,
            [In] D3D12_DEBUG_COMMAND_LIST_PARAMETER_TYPE Type,
            [Out] void* pData,
            [In] uint DataSize
        );
        #endregion

        #region Structs
        public struct Vtbl
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
