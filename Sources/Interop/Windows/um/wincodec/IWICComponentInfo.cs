// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("23BC3F0A-698B-4357-886B-F24D50671334")]
    unsafe public /* blittable */ struct IWICComponentInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetComponentType(
            [In] IWICComponentInfo* This,
            [Out] WICComponentType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetCLSID(
            [In] IWICComponentInfo* This,
            [Out] CLSID* pclsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSigningStatus(
            [In] IWICComponentInfo* This,
            [Out] DWORD* pStatus
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetAuthor(
            [In] IWICComponentInfo* This,
            [In] UINT cchAuthor,
            [In, Out, Optional] WCHAR* wzAuthor,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetVendorGUID(
            [In] IWICComponentInfo* This,
            [Out] GUID* pguidVendor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetVersion(
            [In] IWICComponentInfo* This,
            [In] UINT cchVersion,
            [In, Out, Optional] WCHAR* wzVersion,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetSpecVersion(
            [In] IWICComponentInfo* This,
            [In] UINT cchSpecVersion,
            [In, Out, Optional] WCHAR* wzSpecVersion,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFriendlyName(
            [In] IWICComponentInfo* This,
            [In] UINT cchFriendlyName,
            [In, Out, Optional] WCHAR* wzFriendlyName,
            [Out] UINT* pcchActual
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetComponentType GetComponentType;

            public GetCLSID GetCLSID;

            public GetSigningStatus GetSigningStatus;

            public GetAuthor GetAuthor;

            public GetVendorGUID GetVendorGUID;

            public GetVersion GetVersion;

            public GetSpecVersion GetSpecVersion;

            public GetFriendlyName GetFriendlyName;
            #endregion
        }
        #endregion
    }
}
