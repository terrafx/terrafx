// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("EC5EC8A9-C395-4314-9C77-54D7A935FF70")]
    public /* unmanaged */ unsafe struct IWICImagingFactory
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICImagingFactory* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICImagingFactory* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoderFromFilename(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("LPCWSTR")] char* wzFilename,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoderFromStream(
            [In] IWICImagingFactory* This,
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoderFromFileHandle(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("ULONG_PTR")] nuint hFile,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateComponentInfo(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFCLSID")] Guid* clsidComponent,
            [Out] IWICComponentInfo** ppIInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoder(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateEncoder(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapEncoder** ppIEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreatePalette(
            [In] IWICImagingFactory* This,
            [Out] IWICPalette** ppIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFormatConverter(
            [In] IWICImagingFactory* This,
            [Out] IWICFormatConverter** ppIFormatConverter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapScaler(
            [In] IWICImagingFactory* This,
            [Out] IWICBitmapScaler** ppIBitmapScaler = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapClipper(
            [In] IWICImagingFactory* This,
            [Out] IWICBitmapClipper** ppIBitmapClipper = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFlipRotator(
            [In] IWICImagingFactory* This,
            [Out] IWICBitmapFlipRotator** ppIBitmapFlipRotator = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateStream(
            [In] IWICImagingFactory* This,
            [Out] IWICStream** ppIWICStream = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateColorContext(
            [In] IWICImagingFactory* This,
            [Out] IWICColorContext** ppIWICColorContext = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateColorTransformer(
            [In] IWICImagingFactory* This,
            [Out] IWICColorTransform** ppIWICColorTransform = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmap(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] Guid* pixelFormat,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFromSource(
            [In] IWICImagingFactory* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFromSourceRect(
            [In] IWICImagingFactory* This,
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
        public /* static */ delegate int _CreateBitmapFromMemory(
            [In] IWICImagingFactory* This,
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
        public /* static */ delegate int _CreateBitmapFromHBITMAP(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("HBITMAP")] IntPtr hBitmap,
            [In, Optional, ComAliasName("HPALETTE")] IntPtr hPalette,
            [In] WICBitmapAlphaChannelOption options,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFromHICON(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("HICON")] IntPtr hIcon,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateComponentEnumerator(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("DWORD")] uint componentTypes,
            [In, ComAliasName("DWORD")] uint options,
            [Out] IEnumUnknown** ppIEnumUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFastMetadataEncoderFromDecoder(
            [In] IWICImagingFactory* This,
            [In] IWICBitmapDecoder* pIDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFastMetadataEncoderFromFrameDecode(
            [In] IWICImagingFactory* This,
            [In] IWICBitmapFrameDecode* pIFrameDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateQueryWriter(
            [In] IWICImagingFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateQueryWriterFromReader(
            [In] IWICImagingFactory* This,
            [In] IWICMetadataQueryReader* pIQueryReader = null,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_QueryInterface>(lpVtbl->QueryInterface)(
                    This,
                    riid,
                    ppvObject
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint AddRef()
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreateDecoderFromFilename(
            [In, ComAliasName("LPCWSTR")] char* wzFilename,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateDecoderFromFilename>(lpVtbl->CreateDecoderFromFilename)(
                    This,
                    wzFilename,
                    pguidVendor,
                    dwDesiredAccess,
                    metadataOptions,
                    ppIDecoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateDecoderFromStream(
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateDecoderFromStream>(lpVtbl->CreateDecoderFromStream)(
                    This,
                    pIStream,
                    pguidVendor,
                    metadataOptions,
                    ppIDecoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateDecoderFromFileHandle(
            [In, ComAliasName("ULONG_PTR")] nuint hFile,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateDecoderFromFileHandle>(lpVtbl->CreateDecoderFromFileHandle)(
                    This,
                    hFile,
                    pguidVendor,
                    metadataOptions,
                    ppIDecoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateComponentInfo(
            [In, ComAliasName("REFCLSID")] Guid* clsidComponent,
            [Out] IWICComponentInfo** ppIInfo = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateComponentInfo>(lpVtbl->CreateComponentInfo)(
                    This,
                    clsidComponent,
                    ppIInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateDecoder(
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateDecoder>(lpVtbl->CreateDecoder)(
                    This,
                    guidContainerFormat,
                    pguidVendor,
                    ppIDecoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateEncoder(
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapEncoder** ppIEncoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateEncoder>(lpVtbl->CreateEncoder)(
                    This,
                    guidContainerFormat,
                    pguidVendor,
                    ppIEncoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreatePalette(
            [Out] IWICPalette** ppIPalette = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreatePalette>(lpVtbl->CreatePalette)(
                    This,
                    ppIPalette
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateFormatConverter(
            [Out] IWICFormatConverter** ppIFormatConverter = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateFormatConverter>(lpVtbl->CreateFormatConverter)(
                    This,
                    ppIFormatConverter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapScaler(
            [Out] IWICBitmapScaler** ppIBitmapScaler = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapScaler>(lpVtbl->CreateBitmapScaler)(
                    This,
                    ppIBitmapScaler
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapClipper(
            [Out] IWICBitmapClipper** ppIBitmapClipper = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapClipper>(lpVtbl->CreateBitmapClipper)(
                    This,
                    ppIBitmapClipper
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapFlipRotator(
            [Out] IWICBitmapFlipRotator** ppIBitmapFlipRotator = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapFlipRotator>(lpVtbl->CreateBitmapFlipRotator)(
                    This,
                    ppIBitmapFlipRotator
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateStream(
            [Out] IWICStream** ppIWICStream = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateStream>(lpVtbl->CreateStream)(
                    This,
                    ppIWICStream
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateColorContext(
            [Out] IWICColorContext** ppIWICColorContext = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateColorContext>(lpVtbl->CreateColorContext)(
                    This,
                    ppIWICColorContext
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateColorTransformer(
            [Out] IWICColorTransform** ppIWICColorTransform = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateColorTransformer>(lpVtbl->CreateColorTransformer)(
                    This,
                    ppIWICColorTransform
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmap(
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] Guid* pixelFormat,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmap>(lpVtbl->CreateBitmap)(
                    This,
                    uiWidth,
                    uiHeight,
                    pixelFormat,
                    option,
                    ppIBitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapFromSource(
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapFromSource>(lpVtbl->CreateBitmapFromSource)(
                    This,
                    pIBitmapSource,
                    option,
                    ppIBitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapFromSourceRect(
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In, ComAliasName("UINT")] uint x,
            [In, ComAliasName("UINT")] uint y,
            [In, ComAliasName("UINT")] uint width,
            [In, ComAliasName("UINT")] uint height,
            [Out] IWICBitmap** ppIBitmap = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapFromSourceRect>(lpVtbl->CreateBitmapFromSourceRect)(
                    This,
                    pIBitmapSource,
                    x,
                    y,
                    width,
                    height,
                    ppIBitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapFromMemory(
            [In, ComAliasName("UINT")] uint uiWidth,
            [In, ComAliasName("UINT")] uint uiHeight,
            [In, ComAliasName("REFWICPixelFormatGUID")] Guid* pixelFormat,
            [In, ComAliasName("UINT")] uint cbStride,
            [In, ComAliasName("UINT")] uint cbBufferSize,
            [In, ComAliasName("BYTE[]")] byte* pbBuffer,
            [Out] IWICBitmap** ppIBitmap = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapFromMemory>(lpVtbl->CreateBitmapFromMemory)(
                    This,
                    uiWidth,
                    uiHeight,
                    pixelFormat,
                    cbStride,
                    cbBufferSize,
                    pbBuffer,
                    ppIBitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapFromHBITMAP(
            [In, ComAliasName("HBITMAP")] IntPtr hBitmap,
            [In, Optional, ComAliasName("HPALETTE")] IntPtr hPalette,
            [In] WICBitmapAlphaChannelOption options,
            [Out] IWICBitmap** ppIBitmap = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapFromHBITMAP>(lpVtbl->CreateBitmapFromHBITMAP)(
                    This,
                    hBitmap,
                    hPalette,
                    options,
                    ppIBitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateBitmapFromHICON(
            [In, ComAliasName("HICON")] IntPtr hIcon,
            [Out] IWICBitmap** ppIBitmap = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateBitmapFromHICON>(lpVtbl->CreateBitmapFromHICON)(
                    This,
                    hIcon,
                    ppIBitmap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateComponentEnumerator(
            [In, ComAliasName("DWORD")] uint componentTypes,
            [In, ComAliasName("DWORD")] uint options,
            [Out] IEnumUnknown** ppIEnumUnknown = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateComponentEnumerator>(lpVtbl->CreateComponentEnumerator)(
                    This,
                    componentTypes,
                    options,
                    ppIEnumUnknown
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateFastMetadataEncoderFromDecoder(
            [In] IWICBitmapDecoder* pIDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateFastMetadataEncoderFromDecoder>(lpVtbl->CreateFastMetadataEncoderFromDecoder)(
                    This,
                    pIDecoder,
                    ppIFastEncoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateFastMetadataEncoderFromFrameDecode(
            [In] IWICBitmapFrameDecode* pIFrameDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateFastMetadataEncoderFromFrameDecode>(lpVtbl->CreateFastMetadataEncoderFromFrameDecode)(
                    This,
                    pIFrameDecoder,
                    ppIFastEncoder
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateQueryWriter(
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateQueryWriter>(lpVtbl->CreateQueryWriter)(
                    This,
                    guidMetadataFormat,
                    pguidVendor,
                    ppIQueryWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateQueryWriterFromReader(
            [In] IWICMetadataQueryReader* pIQueryReader = null,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        )
        {
            fixed (IWICImagingFactory* This = &this)
            {
                return MarshalFunction<_CreateQueryWriterFromReader>(lpVtbl->CreateQueryWriterFromReader)(
                    This,
                    pIQueryReader,
                    pguidVendor,
                    ppIQueryWriter
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
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

