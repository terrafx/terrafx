// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("1BC6EA02-EF36-464F-BF0C-21CA39E5168A")]
    unsafe public /* blittable */ struct IDXGIFactory4
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT EnumAdapterByLuid(
            [In] IDXGIFactory4* This,
            [In] LUID AdapterLuid,
            [In] REFIID riid,
            [Out] void** ppvAdapter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT EnumWarpAdapter(
            [In] IDXGIFactory4* This,
            [In] REFIID riid,
            [Out] void** ppvAdapter
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIFactory3.Vtbl BaseVtbl;

            public EnumAdapterByLuid EnumAdapterByLuid;

            public EnumWarpAdapter EnumWarpAdapter;
            #endregion
        }
        #endregion
    }
}
