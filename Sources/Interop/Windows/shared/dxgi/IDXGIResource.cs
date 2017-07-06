// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("035F3AB4-482E-4E50-B41F-8A7F8BD8960B")]
    unsafe public /* blittable */ struct IDXGIResource
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSharedHandle(
            [In] IDXGIResource* This,
            [Out] HANDLE* pSharedHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetUsage(
            [In] IDXGIResource* This,
            [Out] DXGI_USAGE* pUsage
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetEvictionPriority(
            [In] IDXGIResource* This,
            [In] UINT EvictionPriority
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetEvictionPriority(
            [In] IDXGIResource* This,
            [Out] UINT* pEvictionPriority
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIDeviceSubObject.Vtbl BaseVtbl;

            public GetSharedHandle GetSharedHandle;

            public GetUsage GetUsage;

            public SetEvictionPriority SetEvictionPriority;

            public GetEvictionPriority GetEvictionPriority;
            #endregion
        }
        #endregion
    }
}
