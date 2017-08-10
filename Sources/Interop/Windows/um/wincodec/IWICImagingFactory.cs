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
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoderFromFilename(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("LPCWSTR")] /* readonly */ char* wzFilename,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess,
            [In] WICDecodeOptions metadataOptions,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoderFromStream(
            [In] IWICImagingFactory* This,
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoderFromFileHandle(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("ULONG_PTR")] nuint hFile,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateComponentInfo(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFCLSID")] /* readonly */ Guid* clsidComponent,
            [Out, Optional] IWICComponentInfo** ppIInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoder(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidContainerFormat,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [Out, Optional] IWICBitmapDecoder** ppIDecoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEncoder(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidContainerFormat,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [Out, Optional] IWICBitmapEncoder** ppIEncoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePalette(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICPalette** ppIPalette
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFormatConverter(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICFormatConverter** ppIFormatConverter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapScaler(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICBitmapScaler** ppIBitmapScaler
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapClipper(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICBitmapClipper** ppIBitmapClipper
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFlipRotator(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICBitmapFlipRotator** ppIBitmapFlipRotator
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStream(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICStream** ppIWICStream
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContext(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICColorContext** ppIWICColorContext
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorTransformer(
            [In] IWICImagingFactory* This,
            [Out, Optional] IWICColorTransform** ppIWICColorTransform
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmap(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] /* readonly */ Guid* pixelFormat,
            [In] WICBitmapCreateCacheOption option,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromSource(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] WICBitmapCreateCacheOption option,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromSourceRect(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In, ComAliasName("UINT")] uint x,
            [In, ComAliasName("UINT")] uint y,
            [In, ComAliasName("UINT")] uint width,
            [In, ComAliasName("UINT")] uint height,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromMemory(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] /* readonly */ Guid* pixelFormat,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [In, ComAliasName("BYTE")] byte* pbBuffer,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromHBITMAP(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("HBITMAP")] void* hBitmap,
            [In, Optional, ComAliasName("HPALETTE")] void* hPalette,
            [In] WICBitmapAlphaChannelOption options,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromHICON(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("HICON")] void* hIcon,
            [Out, Optional] IWICBitmap** ppIBitmap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateComponentEnumerator(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("DWORD")] uint componentTypes,
            [In, ComAliasName("DWORD")] uint options,
            [Out, Optional] IEnumUnknown** ppIEnumUnknown
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFastMetadataEncoderFromDecoder(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapDecoder* pIDecoder,
            [Out, Optional] IWICFastMetadataEncoder** ppIFastEncoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFastMetadataEncoderFromFrameDecode(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapFrameDecode* pIFrameDecoder,
            [Out, Optional] IWICFastMetadataEncoder** ppIFastEncoder
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryWriter(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFGUID")] /* readonly */ Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [Out, Optional] IWICMetadataQueryWriter** ppIQueryWriter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryWriterFromReader(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICMetadataQueryReader* pIQueryReader,
            [In, Optional, ComAliasName("GUID")] /* readonly */ Guid* pguidVendor,
            [Out, Optional] IWICMetadataQueryWriter** ppIQueryWriter
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr CreateDecoderFromFilename;

            public IntPtr CreateDecoderFromStream;

            public IntPtr CreateDecoderFromFileHandle;

            public IntPtr CreateComponentInfo;

            public IntPtr CreateDecoder;

            public IntPtr CreateEncoder;

            public IntPtr CreatePalette;

            public IntPtr CreateFormatConverter;

            public IntPtr CreateBitmapScaler;

            public IntPtr CreateBitmapClipper;

            public IntPtr CreateBitmapFlipRotator;

            public IntPtr CreateStream;

            public IntPtr CreateColorContext;

            public IntPtr CreateColorTransformer;

            public IntPtr CreateBitmap;

            public IntPtr CreateBitmapFromSource;

            public IntPtr CreateBitmapFromSourceRect;

            public IntPtr CreateBitmapFromMemory;

            public IntPtr CreateBitmapFromHBITMAP;

            public IntPtr CreateBitmapFromHICON;

            public IntPtr CreateComponentEnumerator;

            public IntPtr CreateFastMetadataEncoderFromDecoder;

            public IntPtr CreateFastMetadataEncoderFromFrameDecode;

            public IntPtr CreateQueryWriter;

            public IntPtr CreateQueryWriterFromReader;
            #endregion
        }
        #endregion
    }
}
