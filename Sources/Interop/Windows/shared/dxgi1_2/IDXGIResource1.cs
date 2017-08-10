// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_2.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("30961379-4609-4A41-998E-54FE567EE0C1")]
    unsafe public /* blittable */ struct IDXGIResource1
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateSubresourceSurface(
            [In] IDXGIResource1* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] IDXGISurface2** ppSurface
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateSharedHandle(
            [In] IDXGIResource1* This,
            [In, Optional] /* readonly */ SECURITY_ATTRIBUTES* pAttributes,
            [In, ComAliasName("DWORD")] uint dwAccess,
            [In, Optional, ComAliasName("LPCWSTR")] /* readonly */ char* lpName,
            [Out, ComAliasName("HANDLE")] void* pHandle
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIResource.Vtbl BaseVtbl;

            public IntPtr CreateSubresourceSurface;

            public IntPtr CreateSharedHandle;
            #endregion
        }
        #endregion
    }
}
