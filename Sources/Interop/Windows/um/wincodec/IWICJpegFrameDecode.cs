// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodec.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("8939F66E-C46A-4C21-A9D1-98B327CE1679")]
    public /* blittable */ unsafe struct IWICJpegFrameDecode
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICJpegFrameDecode* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICJpegFrameDecode* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _DoesSupportIndexing(
            [In] IWICJpegFrameDecode* This,
            [Out, ComAliasName("BOOL")] int* pfIndexingSupported
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetIndexing(
            [In] IWICJpegFrameDecode* This,
            [In] WICJpegIndexingOptions options,
            [In, ComAliasName("UINT")] uint horizontalIntervalSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ClearIndexing(
            [In] IWICJpegFrameDecode* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetAcHuffmanTable(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_AC_HUFFMAN_TABLE* pAcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDcHuffmanTable(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_DC_HUFFMAN_TABLE* pDcHuffmanTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetQuantizationTable(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_QUANTIZATION_TABLE* pQuantizationTable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetFrameHeader(
            [In] IWICJpegFrameDecode* This,
            [Out] WICJpegFrameHeader* pFrameHeader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetScanHeader(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [Out] WICJpegScanHeader* pScanHeader
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyScan(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint scanOffset,
            [In, ComAliasName("UINT")] uint cbScanData,
            [Out, ComAliasName("BYTE[]")] byte* pbScanData,
            [Out, ComAliasName("UINT")] uint* pcbScanDataActual
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CopyMinimalStream(
            [In] IWICJpegFrameDecode* This,
            [In, ComAliasName("UINT")] uint streamOffset,
            [In, ComAliasName("UINT")] uint cbStreamData,
            [Out, ComAliasName("BYTE[]")] byte* pbStreamData,
            [Out, ComAliasName("UINT")] uint* pcbStreamDataActual
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
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
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int DoesSupportIndexing(
            [Out, ComAliasName("BOOL")] int* pfIndexingSupported
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_DoesSupportIndexing>(lpVtbl->DoesSupportIndexing)(
                    This,
                    pfIndexingSupported
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetIndexing(
            [In] WICJpegIndexingOptions options,
            [In, ComAliasName("UINT")] uint horizontalIntervalSize
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_SetIndexing>(lpVtbl->SetIndexing)(
                    This,
                    options,
                    horizontalIntervalSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ClearIndexing()
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_ClearIndexing>(lpVtbl->ClearIndexing)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetAcHuffmanTable(
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_AC_HUFFMAN_TABLE* pAcHuffmanTable
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_GetAcHuffmanTable>(lpVtbl->GetAcHuffmanTable)(
                    This,
                    scanIndex,
                    tableIndex,
                    pAcHuffmanTable
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDcHuffmanTable(
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_DC_HUFFMAN_TABLE* pDcHuffmanTable
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_GetDcHuffmanTable>(lpVtbl->GetDcHuffmanTable)(
                    This,
                    scanIndex,
                    tableIndex,
                    pDcHuffmanTable
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetQuantizationTable(
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint tableIndex,
            [Out] DXGI_JPEG_QUANTIZATION_TABLE* pQuantizationTable
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_GetQuantizationTable>(lpVtbl->GetQuantizationTable)(
                    This,
                    scanIndex,
                    tableIndex,
                    pQuantizationTable
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetFrameHeader(
            [Out] WICJpegFrameHeader* pFrameHeader
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_GetFrameHeader>(lpVtbl->GetFrameHeader)(
                    This,
                    pFrameHeader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetScanHeader(
            [In, ComAliasName("UINT")] uint scanIndex,
            [Out] WICJpegScanHeader* pScanHeader
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_GetScanHeader>(lpVtbl->GetScanHeader)(
                    This,
                    scanIndex,
                    pScanHeader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyScan(
            [In, ComAliasName("UINT")] uint scanIndex,
            [In, ComAliasName("UINT")] uint scanOffset,
            [In, ComAliasName("UINT")] uint cbScanData,
            [Out, ComAliasName("BYTE[]")] byte* pbScanData,
            [Out, ComAliasName("UINT")] uint* pcbScanDataActual
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_CopyScan>(lpVtbl->CopyScan)(
                    This,
                    scanIndex,
                    scanOffset,
                    cbScanData,
                    pbScanData,
                    pcbScanDataActual
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CopyMinimalStream(
            [In, ComAliasName("UINT")] uint streamOffset,
            [In, ComAliasName("UINT")] uint cbStreamData,
            [Out, ComAliasName("BYTE[]")] byte* pbStreamData,
            [Out, ComAliasName("UINT")] uint* pcbStreamDataActual
        )
        {
            fixed (IWICJpegFrameDecode* This = &this)
            {
                return MarshalFunction<_CopyMinimalStream>(lpVtbl->CopyMinimalStream)(
                    This,
                    streamOffset,
                    cbStreamData,
                    pbStreamData,
                    pcbStreamDataActual
                );
            }
        }
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region Fields
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

