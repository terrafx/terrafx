// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("412D0C3A-9650-44FA-AF5B-DD2A06C8E8FB")]
    unsafe public /* blittable */ struct IWICComponentFactory
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateMetadataReader(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwOptions,
            [In, Optional] IStream* pIStream,
            [Out, Optional] IWICMetadataReader** ppIReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateMetadataReaderFromContainer(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidContainerFormat,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwOptions,
            [In, Optional] IStream* pIStream,
            [Out, Optional] IWICMetadataReader** ppIReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateMetadataWriter(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwMetadataOptions,
            [Out, Optional] IWICMetadataWriter** ppIWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateMetadataWriterFromReader(
            [In] IWICComponentFactory* This,
            [In, Optional] IWICMetadataReader* pIReader,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [Out, Optional] IWICMetadataWriter** ppIWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryReaderFromBlockReader(
            [In] IWICComponentFactory* This,
            [In, Optional] IWICMetadataBlockReader* pIBlockReader,
            [Out, Optional] IWICMetadataQueryReader** ppIQueryReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryWriterFromBlockWriter(
            [In] IWICComponentFactory* This,
            [In, Optional] IWICMetadataBlockWriter* pIBlockWriter,
            [Out, Optional] IWICMetadataQueryWriter** ppIQueryWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEncoderPropertyBag(
            [In] IWICComponentFactory* This,
            [In, Optional] PROPBAG2* ppropOptions,
            [In, ComAliasName("UINT")] uint cCount,
            [Out, Optional] IPropertyBag2** ppIPropertyBag
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICImagingFactory.Vtbl BaseVtbl;

            public IntPtr CreateMetadataReader;

            public IntPtr CreateMetadataReaderFromContainer;

            public IntPtr CreateMetadataWriter;

            public IntPtr CreateMetadataWriterFromReader;

            public IntPtr CreateQueryReaderFromBlockReader;

            public IntPtr CreateQueryWriterFromBlockWriter;

            public IntPtr CreateEncoderPropertyBag;
            #endregion
        }
        #endregion
    }
}
