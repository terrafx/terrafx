// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("00675040-6908-45F8-86A3-49C7DFD6D9AD")]
    unsafe public /* blittable */ struct IWICPersistStream
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT LoadEx(
            [In] IWICPersistStream* This,
            [In, Optional] IStream* pIStream,
            [In, Optional] /* readonly */ GUID* pguidPreferredVendor,
            [In] DWORD dwPersistOptions
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SaveEx(
            [In] IWICPersistStream* This,
            [In, Optional] IStream* pIStream,
            [In] DWORD dwPersistOptions,
            [In] BOOL fClearDirty
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IPersistStream.Vtbl BaseVtbl;

            public LoadEx LoadEx;

            public SaveEx SaveEx;
            #endregion
        }
        #endregion
    }
}
