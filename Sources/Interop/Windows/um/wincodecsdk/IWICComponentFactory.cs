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
        public /* static */ delegate HRESULT CreateMetadataReader(
            [In] IWICComponentFactory* This,
            [In] REFGUID guidMetadataFormat,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [In] DWORD dwOptions,
            [In, Optional] IStream* pIStream,
            [Out, Optional] IWICMetadataReader** ppIReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateMetadataReaderFromContainer(
            [In] IWICComponentFactory* This,
            [In] REFGUID guidContainerFormat,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [In] DWORD dwOptions,
            [In, Optional] IStream* pIStream,
            [Out, Optional] IWICMetadataReader** ppIReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateMetadataWriter(
            [In] IWICComponentFactory* This,
            [In] REFGUID guidMetadataFormat,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [In] DWORD dwMetadataOptions,
            [Out, Optional] IWICMetadataWriter** ppIWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateMetadataWriterFromReader(
            [In] IWICComponentFactory* This,
            [In, Optional] IWICMetadataReader* pIReader,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [Out, Optional] IWICMetadataWriter** ppIWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateQueryReaderFromBlockReader(
            [In] IWICComponentFactory* This,
            [In, Optional] IWICMetadataBlockReader* pIBlockReader,
            [Out, Optional] IWICMetadataQueryReader** ppIQueryReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateQueryWriterFromBlockWriter(
            [In] IWICComponentFactory* This,
            [In, Optional] IWICMetadataBlockWriter* pIBlockWriter,
            [Out, Optional] IWICMetadataQueryWriter** ppIQueryWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateEncoderPropertyBag(
            [In] IWICComponentFactory* This,
            [In, Optional] PROPBAG2* ppropOptions,
            [In] UINT cCount,
            [Out, Optional] IPropertyBag2** ppIPropertyBag
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IWICImagingFactory.Vtbl BaseVtbl;

            public CreateMetadataReader CreateMetadataReader;

            public CreateMetadataReaderFromContainer CreateMetadataReaderFromContainer;

            public CreateMetadataWriter CreateMetadataWriter;

            public CreateMetadataWriterFromReader CreateMetadataWriterFromReader;

            public CreateQueryReaderFromBlockReader CreateQueryReaderFromBlockReader;

            public CreateQueryWriterFromBlockWriter CreateQueryWriterFromBlockWriter;

            public CreateEncoderPropertyBag CreateEncoderPropertyBag;
            #endregion
        }
        #endregion
    }
}
