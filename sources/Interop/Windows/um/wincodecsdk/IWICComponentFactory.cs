// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("412D0C3A-9650-44FA-AF5B-DD2A06C8E8FB")]
    [Unmanaged]
    public unsafe struct IWICComponentFactory
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICComponentFactory* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICComponentFactory* This
        );
        #endregion

        #region IWICImagingFactory Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoderFromFilename(
            [In] IWICComponentFactory* This,
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
            [In] IWICComponentFactory* This,
            [In, Optional] IStream* pIStream,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoderFromFileHandle(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("ULONG_PTR")] nuint hFile,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateComponentInfo(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFCLSID")] Guid* clsidComponent,
            [Out] IWICComponentInfo** ppIInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDecoder(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateEncoder(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICBitmapEncoder** ppIEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreatePalette(
            [In] IWICComponentFactory* This,
            [Out] IWICPalette** ppIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFormatConverter(
            [In] IWICComponentFactory* This,
            [Out] IWICFormatConverter** ppIFormatConverter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapScaler(
            [In] IWICComponentFactory* This,
            [Out] IWICBitmapScaler** ppIBitmapScaler = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapClipper(
            [In] IWICComponentFactory* This,
            [Out] IWICBitmapClipper** ppIBitmapClipper = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFlipRotator(
            [In] IWICComponentFactory* This,
            [Out] IWICBitmapFlipRotator** ppIBitmapFlipRotator = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateStream(
            [In] IWICComponentFactory* This,
            [Out] IWICStream** ppIWICStream = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateColorContext(
            [In] IWICComponentFactory* This,
            [Out] IWICColorContext** ppIWICColorContext = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateColorTransformer(
            [In] IWICComponentFactory* This,
            [Out] IWICColorTransform** ppIWICColorTransform = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmap(
            [In] IWICComponentFactory* This,
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
            [In] IWICComponentFactory* This,
            [In, Optional] IWICBitmapSource* pIBitmapSource,
            [In] WICBitmapCreateCacheOption option,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFromSourceRect(
            [In] IWICComponentFactory* This,
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
            [In] IWICComponentFactory* This,
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
            [In] IWICComponentFactory* This,
            [In, ComAliasName("HBITMAP")] IntPtr hBitmap,
            [In, Optional, ComAliasName("HPALETTE")] IntPtr hPalette,
            [In] WICBitmapAlphaChannelOption options,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateBitmapFromHICON(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("HICON")] IntPtr hIcon,
            [Out] IWICBitmap** ppIBitmap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateComponentEnumerator(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("DWORD")] uint componentTypes,
            [In, ComAliasName("DWORD")] uint options,
            [Out] IEnumUnknown** ppIEnumUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFastMetadataEncoderFromDecoder(
            [In] IWICComponentFactory* This,
            [In] IWICBitmapDecoder* pIDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFastMetadataEncoderFromFrameDecode(
            [In] IWICComponentFactory* This,
            [In] IWICBitmapFrameDecode* pIFrameDecoder = null,
            [Out] IWICFastMetadataEncoder** ppIFastEncoder = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateQueryWriter(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateQueryWriterFromReader(
            [In] IWICComponentFactory* This,
            [In] IWICMetadataQueryReader* pIQueryReader = null,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateMetadataReader(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwOptions,
            [In] IStream* pIStream = null,
            [Out] IWICMetadataReader** ppIReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateMetadataReaderFromContainer(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwOptions,
            [In] IStream* pIStream = null,
            [Out] IWICMetadataReader** ppIReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateMetadataWriter(
            [In] IWICComponentFactory* This,
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwMetadataOptions,
            [Out] IWICMetadataWriter** ppIWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateMetadataWriterFromReader(
            [In] IWICComponentFactory* This,
            [In] IWICMetadataReader* pIReader = null,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataWriter** ppIWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateQueryReaderFromBlockReader(
            [In] IWICComponentFactory* This,
            [In] IWICMetadataBlockReader* pIBlockReader = null,
            [Out] IWICMetadataQueryReader** ppIQueryReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateQueryWriterFromBlockWriter(
            [In] IWICComponentFactory* This,
            [In] IWICMetadataBlockWriter* pIBlockWriter = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateEncoderPropertyBag(
            [In] IWICComponentFactory* This,
            [In, Optional] PROPBAG2* ppropOptions,
            [In, ComAliasName("UINT")] uint cCount,
            [Out] IPropertyBag2** ppIPropertyBag = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IWICImagingFactory Methods
        [return: ComAliasName("HRESULT")]
        public int CreateDecoderFromFilename(
            [In, ComAliasName("LPCWSTR")] char* wzFilename,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwDesiredAccess,
            [In] WICDecodeOptions metadataOptions,
            [Out] IWICBitmapDecoder** ppIDecoder = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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
            fixed (IWICComponentFactory* This = &this)
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

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreateMetadataReader(
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwOptions,
            [In] IStream* pIStream = null,
            [Out] IWICMetadataReader** ppIReader = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateMetadataReader>(lpVtbl->CreateMetadataReader)(
                    This,
                    guidMetadataFormat,
                    pguidVendor,
                    dwOptions,
                    pIStream,
                    ppIReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateMetadataReaderFromContainer(
            [In, ComAliasName("REFGUID")] Guid* guidContainerFormat,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwOptions,
            [In] IStream* pIStream = null,
            [Out] IWICMetadataReader** ppIReader = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateMetadataReaderFromContainer>(lpVtbl->CreateMetadataReaderFromContainer)(
                    This,
                    guidContainerFormat,
                    pguidVendor,
                    dwOptions,
                    pIStream,
                    ppIReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateMetadataWriter(
            [In, ComAliasName("REFGUID")] Guid* guidMetadataFormat,
            [In, Optional, ComAliasName("GUID")] Guid* pguidVendor,
            [In, ComAliasName("DWORD")] uint dwMetadataOptions,
            [Out] IWICMetadataWriter** ppIWriter = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateMetadataWriter>(lpVtbl->CreateMetadataWriter)(
                    This,
                    guidMetadataFormat,
                    pguidVendor,
                    dwMetadataOptions,
                    ppIWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateMetadataWriterFromReader(
            [In] IWICMetadataReader* pIReader = null,
            [In, ComAliasName("GUID")] Guid* pguidVendor = null,
            [Out] IWICMetadataWriter** ppIWriter = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateMetadataWriterFromReader>(lpVtbl->CreateMetadataWriterFromReader)(
                    This,
                    pIReader,
                    pguidVendor,
                    ppIWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateQueryReaderFromBlockReader(
            [In] IWICMetadataBlockReader* pIBlockReader = null,
            [Out] IWICMetadataQueryReader** ppIQueryReader = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateQueryReaderFromBlockReader>(lpVtbl->CreateQueryReaderFromBlockReader)(
                    This,
                    pIBlockReader,
                    ppIQueryReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateQueryWriterFromBlockWriter(
            [In] IWICMetadataBlockWriter* pIBlockWriter = null,
            [Out] IWICMetadataQueryWriter** ppIQueryWriter = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateQueryWriterFromBlockWriter>(lpVtbl->CreateQueryWriterFromBlockWriter)(
                    This,
                    pIBlockWriter,
                    ppIQueryWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateEncoderPropertyBag(
            [In, Optional] PROPBAG2* ppropOptions,
            [In, ComAliasName("UINT")] uint cCount,
            [Out] IPropertyBag2** ppIPropertyBag = null
        )
        {
            fixed (IWICComponentFactory* This = &this)
            {
                return MarshalFunction<_CreateEncoderPropertyBag>(lpVtbl->CreateEncoderPropertyBag)(
                    This,
                    ppropOptions,
                    cCount,
                    ppIPropertyBag
                );
            }
        }
        #endregion

        #region Structs
        [Unmanaged]
        public struct Vtbl
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
