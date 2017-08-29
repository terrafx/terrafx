// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("D8CD007F-D08F-4191-9BFC-236EA7F0E4B5")]
    public /* blittable */ unsafe struct IWICBitmapDecoderInfo
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IWICBitmapDecoderInfo* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IWICBitmapDecoderInfo* This
        );
        #endregion

        #region IWICComponentInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetComponentType(
            [In] IWICBitmapDecoderInfo* This,
            [Out] WICComponentType* pType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCLSID(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("CLSID")] Guid* pclsid
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSigningStatus(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("DWORD")] uint* pStatus
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetAuthor(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchAuthor,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzAuthor,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVendorGUID(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidVendor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetVersion(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetSpecVersion(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchSpecVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzSpecVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFriendlyName(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchFriendlyName,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFriendlyName,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );
        #endregion

        #region IWICBitmapCodecInfo Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetContainerFormat(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPixelFormats(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cFormats,
            [In, Out, Optional, ComAliasName("GUID[]")] Guid* pguidPixelFormats,
            [Out, ComAliasName("UINT")] uint* pcActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetColorManagementVersion(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchColorManagementVersion,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzColorManagementVersion,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDeviceManufacturer(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchDeviceManufacturer,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzDeviceManufacturer,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDeviceModels(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchDeviceModels,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzDeviceModels,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMimeTypes(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchMimeTypes,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzMimeTypes,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFileExtensions(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cchFileExtensions,
            [In, Out, Optional, ComAliasName("WCHAR[]")] char* wzFileExtensions,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportAnimation(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportAnimation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportChromakey(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportChromakey
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportLossless(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportLossless
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportMultiframe(
            [In] IWICBitmapDecoderInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportMultiframe
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MatchesMimeType(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("LPCWSTR")] char* wzMimeType,
            [Out, ComAliasName("BOOL")] int* pfMatches
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPatterns(
            [In] IWICBitmapDecoderInfo* This,
            [In, ComAliasName("UINT")] uint cbSizePatterns,
            [Out, Optional, ComAliasName("WICBitmapPattern[]")] WICBitmapPattern* pPatterns,
            [Out, Optional, ComAliasName("UINT")] uint* pcPatterns,
            [Out, ComAliasName("UINT")] uint* pcbPatternsActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MatchesPattern(
            [In] IWICBitmapDecoderInfo* This,
            [In, Optional] IStream* pIStream,
            [Out, ComAliasName("BOOL")] int* pfMatches
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateInstance(
            [In] IWICBitmapDecoderInfo* This,
            [Out] IWICBitmapDecoder** ppIBitmapDecoder = null
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

            #region IWICBitmapCodecInfo Fields
            public IntPtr GetContainerFormat;

            public IntPtr GetPixelFormats;

            public IntPtr GetColorManagementVersion;

            public IntPtr GetDeviceManufacturer;

            public IntPtr GetDeviceModels;

            public IntPtr GetMimeTypes;

            public IntPtr GetFileExtensions;

            public IntPtr DoesSupportAnimation;

            public IntPtr DoesSupportChromakey;

            public IntPtr DoesSupportLossless;

            public IntPtr DoesSupportMultiframe;

            public IntPtr MatchesMimeType;
            #endregion

            #region Fields
            public IntPtr GetPatterns;

            public IntPtr MatchesPattern;

            public IntPtr CreateInstance;
            #endregion
        }
        #endregion
    }
}
