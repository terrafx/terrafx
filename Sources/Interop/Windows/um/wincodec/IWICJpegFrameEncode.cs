// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("2F0C601F-D2C6-468C-ABFA-49495D983ED1")]
    unsafe public /* blittable */ struct IWICJpegFrameEncode
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetAcHuffmanTable(
            [In] IWICJpegFrameEncode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_AC_HUFFMAN_TABLE* pAcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDcHuffmanTable(
            [In] IWICJpegFrameEncode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_DC_HUFFMAN_TABLE* pDcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetQuantizationTable(
            [In] IWICJpegFrameEncode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_QUANTIZATION_TABLE* pQuantizationTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WriteScan(
            [In] IWICJpegFrameEncode* This,
            [In, ComAliasName("UINT")] uint cbScanData,
            [In, ComAliasName("BYTE")] /* readonly */ byte* pbScanData
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetAcHuffmanTable GetAcHuffmanTable;

            public GetDcHuffmanTable GetDcHuffmanTable;

            public GetQuantizationTable GetQuantizationTable;

            public WriteScan WriteScan;
            #endregion
        }
        #endregion
    }
}
