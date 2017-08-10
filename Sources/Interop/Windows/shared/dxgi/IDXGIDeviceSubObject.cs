// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("3D3E0379-F9DE-4D58-BB6C-18D62992F1A6")]
    unsafe public /* blittable */ struct IDXGIDeviceSubObject
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDevice(
            [In] IDXGIDeviceSubObject* This,
            [In, ComAliasName("REFIID")] /* readonly */ Guid* riid,
            [Out] void** ppDevice
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IDXGIObject.Vtbl BaseVtbl;

            public IntPtr GetDevice;
            #endregion
        }
        #endregion
    }
}
