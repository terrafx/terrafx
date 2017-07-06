// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("AEC22FB8-76F3-4639-9BE0-28EB43A67A2E")]
    unsafe public /* blittable */ struct IDXGIObject
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetPrivateData(
            [In] IDXGIObject* This,
            [In] REFGUID Name,
            [In] UINT DataSize,
            [In] /* readonly */ void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetPrivateDataInterface(
            [In] IDXGIObject* This,
            [In] REFGUID Name,
            [In, Optional] /* readonly */ IUnknown* pUnknown
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPrivateData(
            [In] IDXGIObject* This,
            [In] REFGUID Name,
            [In, Out] UINT* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetParent(
            [In] IDXGIObject* This,
            [In] REFIID riid,
            [Out] void** ppParent
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public SetPrivateData SetPrivateData;

            public SetPrivateDataInterface SetPrivateDataInterface;

            public GetPrivateData GetPrivateData;

            public GetParent GetParent;
            #endregion
        }
        #endregion
    }
}
