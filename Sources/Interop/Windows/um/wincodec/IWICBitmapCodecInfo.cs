// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("E87A44C4-B76E-4C47-8B09-298EB12A2714")]
    unsafe public /* blittable */ struct IWICBitmapCodecInfo
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetContainerFormat(
            [In] IWICBitmapCodecInfo* This,
            [Out] GUID* pguidContainerFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPixelFormats(
            [In] IWICBitmapCodecInfo* This,
            [In] UINT cFormats,
            [In, Out, Optional] GUID* pguidPixelFormats,
            [Out] UINT* pcActual);

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetColorManagementVersion(
            [In] IWICBitmapCodecInfo* This,
            [In] UINT cchColorManagementVersion,
            [In, Out, Optional] WCHAR* wzColorManagementVersion,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDeviceManufacturer(
            [In] IWICBitmapCodecInfo* This,
            [In] UINT cchDeviceManufacturer,
            [In, Out, Optional] WCHAR* wzDeviceManufacturer,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDeviceModels(
            [In] IWICBitmapCodecInfo* This,
            [In] UINT cchDeviceModels,
            [In, Out, Optional] WCHAR* wzDeviceModels,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMimeTypes(
            [In] IWICBitmapCodecInfo* This,
            [In] UINT cchMimeTypes,
            [In, Out, Optional] WCHAR* wzMimeTypes,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFileExtensions(
            [In] IWICBitmapCodecInfo* This,
            [In] UINT cchFileExtensions,
            [In, Out, Optional] WCHAR* wzFileExtensions,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportAnimation(
            [In] IWICBitmapCodecInfo* This,
            [Out] BOOL* pfSupportAnimation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportChromakey(
            [In] IWICBitmapCodecInfo* This,
            [Out] BOOL* pfSupportChromakey
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportLossless(
            [In] IWICBitmapCodecInfo* This,
            [Out] BOOL* pfSupportLossless
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportMultiframe(
            [In] IWICBitmapCodecInfo* This,
            [Out] BOOL* pfSupportMultiframe
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT MatchesMimeType(
            [In] IWICBitmapCodecInfo* This,
            [In] LPCWSTR wzMimeType,
            [Out] BOOL* pfMatches
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICComponentInfo.Vtbl BaseVtbl;

            public GetContainerFormat GetContainerFormat;

            public GetPixelFormats GetPixelFormats;

            public GetColorManagementVersion GetColorManagementVersion;

            public GetDeviceManufacturer GetDeviceManufacturer;

            public GetDeviceModels GetDeviceModels;

            public GetMimeTypes GetMimeTypes;

            public GetFileExtensions GetFileExtensions;

            public DoesSupportAnimation DoesSupportAnimation;

            public DoesSupportChromakey DoesSupportChromakey;

            public DoesSupportLossless DoesSupportLossless;

            public DoesSupportMultiframe DoesSupportMultiframe;

            public MatchesMimeType MatchesMimeType;
            #endregion
        }
        #endregion
    }
}
