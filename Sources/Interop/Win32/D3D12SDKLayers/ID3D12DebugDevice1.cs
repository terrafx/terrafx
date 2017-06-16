// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("A9B71770-D099-4A65-A698-3DEE10020F88")]
    unsafe public struct ID3D12DebugDevice1
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DebugDevice1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetDebugParameter(
            [In] ID3D12DebugDevice1* This,
            [In] D3D12_DEBUG_DEVICE_PARAMETER_TYPE Type,
            [In] void* pData,
            [In] uint DataSize
        );

        public /* static */ delegate HRESULT GetDebugParameter(
            [In] ID3D12DebugDevice1* This,
            [In] D3D12_DEBUG_DEVICE_PARAMETER_TYPE Type,
            [Out] void* pData,
            [In] uint DataSize
        );

        public /* static */ delegate HRESULT ReportLiveDeviceObjects(
            [In] ID3D12DebugDevice1* This,
            [In] D3D12_RLDO_FLAGS Flags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public SetDebugParameter SetDebugParameter;

            public GetDebugParameter GetDebugParameter;

            public ReportLiveDeviceObjects ReportLiveDeviceObjects;
            #endregion
        }
        #endregion
    }
}
