// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("7B816B45-1996-4476-B132-DE9E247C8AF0")]
    public /* blittable */ unsafe struct IWICImagingFactory2
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IWICImagingFactory2* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IWICImagingFactory2* This
        );
        #endregion

        #region IWICImagingFactory Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoderFromFilename(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("LPCWSTR")] char* wzFilename,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoderFromStream(
            [In] IWICImagingFactory2* This,
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoderFromFileHandle(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("ULONG_PTR")] nuint hFile,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateComponentInfo(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("REFCLSID")] Guid* clsidComponent,
            [Out] IWICComponentInfo** ppIInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDecoder(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateEncoder(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapEncoder** ppIEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePalette(
            [In] IWICImagingFactory2* This,
            [Out] IWICPalette** ppIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFormatConverter(
            [In] IWICImagingFactory2* This,
            [Out] IWICFormatConverter** ppIFormatConverter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapScaler(
            [In] IWICImagingFactory2* This,
            [Out] IWICBitmapScaler** ppIBitmapScaler = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapClipper(
            [In] IWICImagingFactory2* This,
            [Out] IWICBitmapClipper** ppIBitmapClipper = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFlipRotator(
            [In] IWICImagingFactory2* This,
            [Out] IWICBitmapFlipRotator** ppIBitmapFlipRotator = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateStream(
            [In] IWICImagingFactory2* This,
            [Out] IWICStream** ppIWICStream = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorContext(
            [In] IWICImagingFactory2* This,
            [Out] IWICColorContext** ppIWICColorContext = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateColorTransformer(
            [In] IWICImagingFactory2* This,
            [Out] IWICColorTransform** ppIWICColorTransform = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmap(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] Guid* pixelFormat,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromSource(
            [In] IWICImagingFactory2* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromSourceRect(
            [In] IWICImagingFactory2* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In, ComAliasName("UINT")] uint x,
            [In, ComAliasName("UINT")] uint y,
            [In, ComAliasName("UINT")] uint width,
            [In, ComAliasName("UINT")] uint height,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromMemory(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] Guid* pixelFormat,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [In, ComAliasName("BYTE[]")] byte* pbBuffer,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromHBITMAP(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("HBITMAP")] IntPtr hBitmap,
            [In, Optional, ComAliasName("HPALETTE")] IntPtr hPalette,
            [In] WICBitmapAlphaChannelOption options,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateBitmapFromHICON(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("HICON")] IntPtr hIcon,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateComponentEnumerator(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("DWORD")] uint componentTypes,
            [In, ComAliasName("DWORD")] uint options,
            [Out] IEnumUnknown** ppIEnumUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFastMetadataEncoderFromDecoder(
            [In] IWICImagingFactory2* This,
            [In] IWICBitmapDecoder* pIDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFastMetadataEncoderFromFrameDecode(
            [In] IWICImagingFactory2* This,
            [In] IWICBitmapFrameDecode* pIFrameDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryWriter(
            [In] IWICImagingFactory2* This,
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryWriterFromReader(
            [In] IWICImagingFactory2* This,
            [In] IWICMetadataQueryReader* pIQueryReader = null,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateImageEncoder(
            [In] IWICImagingFactory2* This,
            [In] ID2D1Device* pD2DDevice,
            [Out] IWICImageEncoder** ppWICImageEncoder
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

            #region IWICImagingFactory Fields
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

            #region Fields
            public IntPtr CreateImageEncoder;
            #endregion
        }
        #endregion
    }
}
