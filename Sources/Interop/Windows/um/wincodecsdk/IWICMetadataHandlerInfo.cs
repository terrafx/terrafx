// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("ABA958BF-C672-44D1-8D61-CE6DF2E682C2")]
    unsafe public /* blittable */ struct IWICMetadataHandlerInfo
    {

        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMetadataFormat(
            [In] IWICMetadataHandlerInfo* This,
            [Out] GUID* pguidMetadataFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetContainerFormats(
            [In] IWICMetadataHandlerInfo* This,
            [In] UINT cContainerFormats,
            [In, Out, Optional] GUID* pguidContainerFormats,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDeviceManufacturer(
            [In] IWICMetadataHandlerInfo* This,
            [In] UINT cchDeviceManufacturer,
            [In, Out, Optional] WCHAR* wzDeviceManufacturer,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDeviceModels(
            [In] IWICMetadataHandlerInfo* This,
            [In] UINT cchDeviceModels,
            [In, Out, Optional] WCHAR* wzDeviceModels,
            [Out] UINT* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesRequireFullStream(
            [In] IWICMetadataHandlerInfo* This,
            [Out] BOOL* pfRequiresFullStream
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportPadding(
            [In] IWICMetadataHandlerInfo* This,
            [Out] BOOL* pfSupportsPadding
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesRequireFixedSize(
            [In] IWICMetadataHandlerInfo* This,
            [Out] BOOL* pfFixedSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICComponentInfo.Vtbl BaseVtbl;

            public GetMetadataFormat GetMetadataFormat;

            public GetContainerFormats GetContainerFormats;

            public GetDeviceManufacturer GetDeviceManufacturer;

            public GetDeviceModels GetDeviceModels;

            public DoesRequireFullStream DoesRequireFullStream;

            public DoesSupportPadding DoesSupportPadding;

            public DoesRequireFixedSize DoesRequireFixedSize;
            #endregion
        }
        #endregion
    }
}
