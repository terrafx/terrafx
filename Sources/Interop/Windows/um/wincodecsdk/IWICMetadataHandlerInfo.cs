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
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMetadataFormat(
            [In] IWICMetadataHandlerInfo* This,
            [Out, ComAliasName("GUID")] Guid* pguidMetadataFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetContainerFormats(
            [In] IWICMetadataHandlerInfo* This,
            [In, ComAliasName("UINT")] uint cContainerFormats,
            [In, Out, Optional, ComAliasName("GUID")] Guid* pguidContainerFormats,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDeviceManufacturer(
            [In] IWICMetadataHandlerInfo* This,
            [In, ComAliasName("UINT")] uint cchDeviceManufacturer,
            [In, Out, Optional, ComAliasName("WCHAR")] char* wzDeviceManufacturer,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDeviceModels(
            [In] IWICMetadataHandlerInfo* This,
            [In, ComAliasName("UINT")] uint cchDeviceModels,
            [In, Out, Optional, ComAliasName("WCHAR")] char* wzDeviceModels,
            [Out, ComAliasName("UINT")] uint* pcchActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesRequireFullStream(
            [In] IWICMetadataHandlerInfo* This,
            [Out, ComAliasName("BOOL")] int* pfRequiresFullStream
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportPadding(
            [In] IWICMetadataHandlerInfo* This,
            [Out, ComAliasName("BOOL")] int* pfSupportsPadding
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesRequireFixedSize(
            [In] IWICMetadataHandlerInfo* This,
            [Out, ComAliasName("BOOL")] int* pfFixedSize
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICComponentInfo.Vtbl BaseVtbl;

            public IntPtr GetMetadataFormat;

            public IntPtr GetContainerFormats;

            public IntPtr GetDeviceManufacturer;

            public IntPtr GetDeviceModels;

            public IntPtr DoesRequireFullStream;

            public IntPtr DoesSupportPadding;

            public IntPtr DoesRequireFixedSize;
            #endregion
        }
        #endregion
    }
}
