// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("9F34FB65-13F4-4F15-BC57-3726B5E53D9F")]
    public /* blittable */ unsafe struct IWICFormatConverterInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IWICFormatConverterInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IWICFormatConverterInfo* This
        );
        #endregion

        #region IWICComponentInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetComponentType(
            [In] IWICFormatConverterInfo* This,
            [Out] WICComponentType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCLSID(
            [In] IWICFormatConverterInfo* This,
            [Out, ComAliasName("CLSID")] Guid* pclsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSigningStatus(
            [In] IWICFormatConverterInfo* This,
            [Out, ComAliasName("DWORD")] uint* pStatus
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetAuthor(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchAuthor,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzAuthor,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVendorGUID(
            [In] IWICFormatConverterInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidVendor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVersion(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSpecVersion(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchSpecVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzSpecVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFriendlyName(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cchFriendlyName,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFriendlyName,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPixelFormats(
            [In] IWICFormatConverterInfo* This,
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("WICPixelFormatGUID[]")] Guid* pPixelFormatGUIDs,
            [Out, ComAliasName("UINT")] uint* pcActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInstance(
            [In] IWICFormatConverterInfo* This,
            [Out] IWICFormatConverter** ppIConverter = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IWICComponentInfo Fields
            public IntPtr GetComponentType;

            public IntPtr GetCLSID;

            public IntPtr GetSigningStatus;

            public IntPtr GetAuthor;

            public IntPtr GetVendorGUID;

            public IntPtr GetVersion;

            public IntPtr GetSpecVersion;

            public IntPtr GetFriendlyName;
            #endregion

            #region Fields
            public IntPtr GetPixelFormats;

            public IntPtr CreateInstance;
            #endregion
        }
        #endregion
    }
}
