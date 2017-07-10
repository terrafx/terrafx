// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("8939F66E-C46A-4C21-A9D1-98B327CE1679")]
    unsafe public /* blittable */ struct IWICJpegFrameDecode
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT DoesSupportIndexing(
            [In] IWICJpegFrameDecode* This,
            [Out] BOOL* pfIndexingSupported
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetIndexing(
            [In] IWICJpegFrameDecode* This,
            [In] WICJpegIndexingOptions options,
            [In] UINT horizontalIntervalSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ClearIndexing(
            [In] IWICJpegFrameDecode* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetAcHuffmanTable(
            [In] IWICJpegFrameDecode* This,
            [In] UINT scanIndex,
            [In] UINT tableIndex,
            [Out] DXGI_JPEG_AC_HUFFMAN_TABLE* pAcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDcHuffmanTable(
            [In] IWICJpegFrameDecode* This,
            [In] UINT scanIndex,
            [In] UINT tableIndex,
            [Out] DXGI_JPEG_DC_HUFFMAN_TABLE* pDcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetQuantizationTable(
            [In] IWICJpegFrameDecode* This,
            [In] UINT scanIndex,
            [In] UINT tableIndex,
            [Out] DXGI_JPEG_QUANTIZATION_TABLE* pQuantizationTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetFrameHeader(
            [In] IWICJpegFrameDecode* This,
            [Out] WICJpegFrameHeader* pFrameHeader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetScanHeader(
            [In] IWICJpegFrameDecode* This,
            [In] UINT scanIndex,
            [Out] WICJpegScanHeader* pScanHeader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyScan(
            [In] IWICJpegFrameDecode* This,
            [In] UINT scanIndex,
            [In] UINT scanOffset,
            [In] UINT cbScanData,
            [Out] BYTE* pbScanData,
            [Out] UINT* pcbScanDataActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT CopyMinimalStream(
            [In] IWICJpegFrameDecode* This,
            [In] UINT streamOffset,
            [In] UINT cbStreamData,
            [Out] BYTE* pbStreamData,
            [Out] UINT* pcbStreamDataActual
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public DoesSupportIndexing DoesSupportIndexing;

            public SetIndexing SetIndexing;

            public ClearIndexing ClearIndexing;

            public GetAcHuffmanTable GetAcHuffmanTable;

            public GetDcHuffmanTable GetDcHuffmanTable;

            public GetQuantizationTable GetQuantizationTable;

            public GetFrameHeader GetFrameHeader;

            public GetScanHeader GetScanHeader;

            public CopyScan CopyScan;

            public CopyMinimalStream CopyMinimalStream;
            #endregion
        }
        #endregion
    }
}
