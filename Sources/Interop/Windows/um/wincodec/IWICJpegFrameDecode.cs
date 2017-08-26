// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("8939F66E-C46A-4C21-A9D1-98B327CE1679")]
    public /* blittable */ unsafe struct IWICJpegFrameDecode
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int DoesSupportIndexing(
            [In] IWICJpegFrameDecode* This,
            [Out, ComAliasName("BOOL")] int* pfIndexingSupported
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetIndexing(
            [In] IWICJpegFrameDecode* This,
            [In] WICJpegIndexingOptions options,
            [In, ComAliasName("UINT")] uint horizontalIntervalSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ClearIndexing(
            [In] IWICJpegFrameDecode* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetAcHuffmanTable(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_AC_HUFFMAN_TABLE* pAcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDcHuffmanTable(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_DC_HUFFMAN_TABLE* pDcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetQuantizationTable(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_QUANTIZATION_TABLE* pQuantizationTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetFrameHeader(
            [In] IWICJpegFrameDecode* This,
            [Out] WICJpegFrameHeader* pFrameHeader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetScanHeader(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [Out] WICJpegScanHeader* pScanHeader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CopyScan(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint scanOffset,
            [In, ComAliasName("UINT")] uint cbScanData,
            [Out, ComAliasName("BYTE")] byte* pbScanData,
            [Out, ComAliasName("UINT")] uint* pcbScanDataActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CopyMinimalStream(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint streamOffset,
            [In, ComAliasName("UINT")] uint cbStreamData,
            [Out, ComAliasName("BYTE")] byte* pbStreamData,
            [Out, ComAliasName("UINT")] uint* pcbStreamDataActual
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr DoesSupportIndexing;

            public IntPtr SetIndexing;

            public IntPtr ClearIndexing;

            public IntPtr GetAcHuffmanTable;

            public IntPtr GetDcHuffmanTable;

            public IntPtr GetQuantizationTable;

            public IntPtr GetFrameHeader;

            public IntPtr GetScanHeader;

            public IntPtr CopyScan;

            public IntPtr CopyMinimalStream;
            #endregion
        }
        #endregion
    }
}
