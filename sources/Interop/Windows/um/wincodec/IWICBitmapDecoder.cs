// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("9EDDE9E7-8DEE-47EA-99DF-E6FAF2ED44BF")]
    [Unmanaged]
    public unsafe struct IWICBitmapDecoder
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICBitmapDecoder* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICBitmapDecoder* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICBitmapDecoder* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryCapability(
            [In] IWICBitmapDecoder* This,
            [In, Optional] IStream* pIStream,
            [Out, ComAliasName("DWORD")] uint* pdwCapability
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Initialize(
            [In] IWICBitmapDecoder* This,
            [In, Optional] IStream* pIStream,
            [In] WICDecodeOptions cacheOptions
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetContainerFormat(
            [In] IWICBitmapDecoder* This,
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDecoderInfo(
            [In] IWICBitmapDecoder* This,
            [Out] IWICBitmapDecoderInfo** ppIDecoderInfo = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyPalette(
            [In] IWICBitmapDecoder* This,
            [In] IWICPalette* pIPalette = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMetadataQueryReader(
            [In] IWICBitmapDecoder* This,
            [Out] IWICMetadataQueryReader** ppIMetadataQueryReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPreview(
            [In] IWICBitmapDecoder* This,
            [Out] IWICBitmapSource** ppIBitmapSource = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetColorContexts(
            [In] IWICBitmapDecoder* This,
            [In, ComAliasName("UINT")] uint cCount,
            [In, Out, Optional, ComAliasName("IWICColorContext*[]")] IWICColorContext** ppIColorContexts,
            [Out, ComAliasName("UINT")] uint* pcActualCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetThumbnail(
            [In] IWICBitmapDecoder* This,
            [Out] IWICBitmapSource** ppIThumbnail = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameCount(
            [In] IWICBitmapDecoder* This,
            [Out, ComAliasName("UINT")] uint* pCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrame(
            [In] IWICBitmapDecoder* This,
            [In, ComAliasName("UINT")] uint index,
            [Out] IWICBitmapFrameDecode** ppIBitmapFrame = null
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
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
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int QueryCapability(
            [In, Optional] IStream* pIStream,
            [Out, ComAliasName("DWORD")] uint* pdwCapability
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_QueryCapability>(lpVtbl->QueryCapability)(
                    This,
                    pIStream,
                    pdwCapability
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Initialize(
            [In, Optional] IStream* pIStream,
            [In] WICDecodeOptions cacheOptions
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_Initialize>(lpVtbl->Initialize)(
                    This,
                    pIStream,
                    cacheOptions
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetContainerFormat(
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetContainerFormat>(lpVtbl->GetContainerFormat)(
                    This,
                    pguidContainerFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDecoderInfo(
            [Out] IWICBitmapDecoderInfo** ppIDecoderInfo = null
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetDecoderInfo>(lpVtbl->GetDecoderInfo)(
                    This,
                    ppIDecoderInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyPalette(
            [In] IWICPalette* pIPalette = null
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_CopyPalette>(lpVtbl->CopyPalette)(
                    This,
                    pIPalette
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMetadataQueryReader(
            [Out] IWICMetadataQueryReader** ppIMetadataQueryReader = null
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetMetadataQueryReader>(lpVtbl->GetMetadataQueryReader)(
                    This,
                    ppIMetadataQueryReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPreview(
            [Out] IWICBitmapSource** ppIBitmapSource = null
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetPreview>(lpVtbl->GetPreview)(
                    This,
                    ppIBitmapSource
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetColorContexts(
            [In, ComAliasName("UINT")] uint cCount,
            [In, Out, Optional, ComAliasName("IWICColorContext*[]")] IWICColorContext** ppIColorContexts,
            [Out, ComAliasName("UINT")] uint* pcActualCount
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetColorContexts>(lpVtbl->GetColorContexts)(
                    This,
                    cCount,
                    ppIColorContexts,
                    pcActualCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetThumbnail(
            [Out] IWICBitmapSource** ppIThumbnail = null
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetThumbnail>(lpVtbl->GetThumbnail)(
                    This,
                    ppIThumbnail
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrameCount(
            [Out, ComAliasName("UINT")] uint* pCount
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetFrameCount>(lpVtbl->GetFrameCount)(
                    This,
                    pCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrame(
            [In, ComAliasName("UINT")] uint index,
            [Out] IWICBitmapFrameDecode** ppIBitmapFrame = null
        )
        {
            fixed (IWICBitmapDecoder* This = &this)
            {
                return MarshalFunction<_GetFrame>(lpVtbl->GetFrame)(
                    This,
                    index,
                    ppIBitmapFrame
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

            #region Fields
            public IntPtr QueryCapability;

            public IntPtr Initialize;

            public IntPtr GetContainerFormat;

            public IntPtr GetDecoderInfo;

            public IntPtr CopyPalette;

            public IntPtr GetMetadataQueryReader;

            public IntPtr GetPreview;

            public IntPtr GetColorContexts;

            public IntPtr GetThumbnail;

            public IntPtr GetFrameCount;

            public IntPtr GetFrame;
            #endregion
        }
        #endregion
    }
}
