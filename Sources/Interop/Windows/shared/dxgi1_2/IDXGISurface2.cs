// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("ABA496DD-B617-4CB8-A866-BC44D7EB1FA2")]
    unsafe public /* blittable */ struct IDXGISurface2
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetResource(
            [In] IDXGISurface2* This,
            [In] REFIID riid,
            [Out] void** ppParentResource,
            [Out] UINT* pSubresourceIndex
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGISurface1.Vtbl BaseVtbl;

            public GetResource GetResource;
            #endregion
        }
        #endregion
    }
}
