// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("9EDDE9E7-8DEE-47EA-99DF-E6FAF2ED44BF")]
    unsafe public /* blittable */ struct IWICBitmapDecoder
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT QueryCapability(
            [In] IWICBitmapDecoder* This,
            [In, Optional] IStream* pIStream,
            [Out] DWORD* pdwCapability
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Initialize(
            [In] IWICBitmapDecoder* This,
            [In, Optional] IStream* pIStream,
            [In] WICDecodeOptions cacheOptions
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetContainerFormat(
            [In] IWICBitmapDecoder* This,
            [Out] GUID* pguidContainerFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDecoderInfo(
            [In] IWICBitmapDecoder* This,
            [Out, Optional] IWICBitmapDecoderInfo** ppIDecoderInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyPalette(
            [In] IWICBitmapDecoder* This,
            [In, Optional] IWICPalette* pIPalette
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMetadataQueryReader(
            [In] IWICBitmapDecoder* This,
            [Out, Optional] IWICMetadataQueryReader** ppIMetadataQueryReader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPreview(
            [In] IWICBitmapDecoder* This,
            [Out, Optional] IWICBitmapSource** ppIBitmapSource
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetColorContexts(
            [In] IWICBitmapDecoder* This,
            [In] UINT cCount,
            [In, Out, Optional]  IWICColorContext** ppIColorContexts,
            [Out] UINT* pcActualCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetThumbnail(
            [In] IWICBitmapDecoder* This,
            [Out, Optional] IWICBitmapSource** ppIThumbnail
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrameCount(
            [In] IWICBitmapDecoder* This,
            [Out] UINT* pCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrame(
            [In] IWICBitmapDecoder* This,
            [In] UINT index,
            [Out, Optional] IWICBitmapFrameDecode** ppIBitmapFrame
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public QueryCapability QueryCapability;

            public Initialize Initialize;

            public GetContainerFormat GetContainerFormat;

            public GetDecoderInfo GetDecoderInfo;

            public CopyPalette CopyPalette;

            public GetMetadataQueryReader GetMetadataQueryReader;

            public GetPreview GetPreview;

            public GetColorContexts GetColorContexts;

            public GetThumbnail GetThumbnail;

            public GetFrameCount GetFrameCount;

            public GetFrame GetFrame;
            #endregion
        }
        #endregion
    }
}
