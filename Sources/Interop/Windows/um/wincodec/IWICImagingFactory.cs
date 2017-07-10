// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("EC5EC8A9-C395-4314-9C77-54D7A935FF70")]
    unsafe public /* blittable */ struct IWICImagingFactory
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateDecoderFromFilename(
            [In] IWICImagingFactory* This,
            [In] LPCWSTR wzFilename,
            [In, Optional] GUID* pguidVendor,
            [In] DWORD dwDesiredAccess,
            [In] WICDecodeOptions metadataOptions,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateDecoderFromStream(
            [In] IWICImagingFactory* This,
            [In, Optional] IStream* pIStream,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateDecoderFromFileHandle(
            [In] IWICImagingFactory* This,
            [In] ULONG_PTR hFile,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateComponentInfo(
            [In] IWICImagingFactory* This,
            [In] REFCLSID clsidComponent,
            [Out, Optional] IWICComponentInfo** ppIInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateDecoder(
            [In] IWICImagingFactory* This,
            [In] REFGUID guidContainerFormat,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateEncoder(
            [In] IWICImagingFactory* This,
            [In] REFGUID guidContainerFormat,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [Out, Optional] IWICBitmapEncoder** ppIEncoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreatePalette(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICPalette** ppIPalette
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateFormatConverter(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICFormatConverter** ppIFormatConverter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapScaler(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICBitmapScaler** ppIBitmapScaler
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapClipper(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICBitmapClipper** ppIBitmapClipper
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapFlipRotator(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICBitmapFlipRotator** ppIBitmapFlipRotator
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateStream(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICStream** ppIWICStream
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateColorContext(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICColorContext** ppIWICColorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateColorTransformer(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICColorTransform** ppIWICColorTransform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmap(
            [In] IWICImagingFactory* This,
            [In] UINT uiWidth,
            [In] UINT uiHeight,
            [In] REFWICPixelFormatGUID pixelFormat,
            [In] WICBitmapCreateCacheOption option,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapFromSource(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] WICBitmapCreateCacheOption option,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapFromSourceRect(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] UINT x,
            [In] UINT y,
            [In] UINT width,
            [In] UINT height,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapFromMemory(
            [In] IWICImagingFactory* This,
            [In] UINT uiWidth,
            [In] UINT uiHeight,
            [In] REFWICPixelFormatGUID pixelFormat,
            [In] UINT cbStride,
            [In] UINT cbBufferSize,
            [In] BYTE* pbBuffer,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapFromHBITMAP(
            [In] IWICImagingFactory* This,
            [In] HBITMAP hBitmap,
            [In, Optional] HPALETTE hPalette,
            [In] WICBitmapAlphaChannelOption options,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateBitmapFromHICON(
            [In] IWICImagingFactory* This,
            [In] HICON hIcon,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateComponentEnumerator(
            [In] IWICImagingFactory* This,
            [In] DWORD componentTypes,
            [In] DWORD options,
            [Out, Optional] IEnumUnknown** ppIEnumUnknown
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateFastMetadataEncoderFromDecoder(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapDecoder* pIDecoder,
            [Out, Optional] IWICFastMetadataEncoder** ppIFastEncoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateFastMetadataEncoderFromFrameDecode(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapFrameDecode* pIFrameDecoder,
            [Out, Optional] IWICFastMetadataEncoder** ppIFastEncoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateQueryWriter(
            [In] IWICImagingFactory* This,
            [In] REFGUID guidMetadataFormat,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [Out, Optional] IWICMetadataQueryWriter** ppIQueryWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CreateQueryWriterFromReader(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICMetadataQueryReader* pIQueryReader,
            [In, Optional] /* readonly */ GUID* pguidVendor,
            [Out, Optional] IWICMetadataQueryWriter** ppIQueryWriter
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public CreateDecoderFromFilename CreateDecoderFromFilename;

            public CreateDecoderFromStream CreateDecoderFromStream;

            public CreateDecoderFromFileHandle CreateDecoderFromFileHandle;

            public CreateComponentInfo CreateComponentInfo;

            public CreateDecoder CreateDecoder;

            public CreateEncoder CreateEncoder;

            public CreatePalette CreatePalette;

            public CreateFormatConverter CreateFormatConverter;

            public CreateBitmapScaler CreateBitmapScaler;

            public CreateBitmapClipper CreateBitmapClipper;

            public CreateBitmapFlipRotator CreateBitmapFlipRotator;

            public CreateStream CreateStream;

            public CreateColorContext CreateColorContext;

            public CreateColorTransformer CreateColorTransformer;

            public CreateBitmap CreateBitmap;

            public CreateBitmapFromSource CreateBitmapFromSource;

            public CreateBitmapFromSourceRect CreateBitmapFromSourceRect;

            public CreateBitmapFromMemory CreateBitmapFromMemory;

            public CreateBitmapFromHBITMAP CreateBitmapFromHBITMAP;

            public CreateBitmapFromHICON CreateBitmapFromHICON;

            public CreateComponentEnumerator CreateComponentEnumerator;

            public CreateFastMetadataEncoderFromDecoder CreateFastMetadataEncoderFromDecoder;

            public CreateFastMetadataEncoderFromFrameDecode CreateFastMetadataEncoderFromFrameDecode;

            public CreateQueryWriter CreateQueryWriter;

            public CreateQueryWriterFromReader CreateQueryWriterFromReader;
            #endregion
        }
        #endregion
    }
}
