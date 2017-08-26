// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("A9B71770-D099-4A65-A698-3DEE10020F88")]
    unsafe public /* blittable */ struct ID3D12DebugDevice1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetDebugParameter(
            [In] ID3D12DebugDevice1* This,
            [In] D3D12_DEBUG_DEVICE_PARAMETER_TYPE Type,
            [In] /* readonly */ void* pData,
            [In, ComAliasName("UINT")] uint DataSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDebugParameter(
            [In] ID3D12DebugDevice1* This,
            [In] D3D12_DEBUG_DEVICE_PARAMETER_TYPE Type,
            [Out] void* pData,
            [In, ComAliasName("UINT")] uint DataSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReportLiveDeviceObjects(
            [In] ID3D12DebugDevice1* This,
            [In] D3D12_RLDO_FLAGS Flags
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr SetDebugParameter;

            public IntPtr GetDebugParameter;

            public IntPtr ReportLiveDeviceObjects;
            #endregion
        }
        #endregion
    }
}
