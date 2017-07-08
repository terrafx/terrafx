// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("C4FEC28F-7966-4E95-9F94-F431CB56C3B8")]
    unsafe public /* blittable */ struct ID3D12Object
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPrivateData(
            [In] ID3D12Object* This,
            [In] REFGUID guid,
            [In, Out] UINT* pDataSize,
            [Out, Optional] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetPrivateData(
            [In] ID3D12Object* This,
            [In] REFGUID guid,
            [In] UINT DataSize,
            [In, Optional] /* readonly */ void* pData);


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetPrivateDataInterface(
            [In] ID3D12Object* This,
            [In] REFGUID guid,
            [In, Optional] /* readonly */ IUnknown* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetName(
            [In] ID3D12Object* This,
            [In] LPCWSTR Name
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetPrivateData GetPrivateData;

            public SetPrivateData SetPrivateData;

            public SetPrivateDataInterface SetPrivateDataInterface;

            public SetName SetName;
            #endregion
        }
        #endregion
    }
}
