// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("9D8E1289-D7B3-465F-8126-250E349AF85D")]
    unsafe public /* blittable */ struct IDXGIKeyedMutex
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AcquireSync(
            [In] IDXGIKeyedMutex* This,
            [In, ComAliasName("UINT64")] ulong Key,
            [In, ComAliasName("DWORD")] uint dwMilliseconds
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReleaseSync(
            [In] IDXGIKeyedMutex* This,
            [In, ComAliasName("UINT64")] ulong Key
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIDeviceSubObject.Vtbl BaseVtbl;

            public AcquireSync AcquireSync;

            public ReleaseSync ReleaseSync;
            #endregion
        }
        #endregion
    }
}
