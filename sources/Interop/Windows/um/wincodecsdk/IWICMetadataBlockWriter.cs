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
    [Guid("08FB9676-B444-41E8-8DBE-6A53A542BFF1")]
    [Unmanaged]
    public unsafe struct IWICMetadataBlockWriter
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IWICMetadataBlockWriter* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IWICMetadataBlockWriter* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IWICMetadataBlockWriter* This
        );
        #endregion

        #region IWICMetadataBlockReader Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetContainerFormat(
            [In] IWICMetadataBlockWriter* This,
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetCount(
            [In] IWICMetadataBlockWriter* This,
            [Out, ComAliasName("UINT")] uint* pcCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetReaderByIndex(
            [In] IWICMetadataBlockWriter* This,
            [In, ComAliasName("UINT")] uint nIndex,
            [Out] IWICMetadataReader** ppIMetadataReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetEnumerator(
            [In] IWICMetadataBlockWriter* This,
            [Out] IEnumUnknown** ppIEnumMetadata = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _InitializeFromBlockReader(
            [In] IWICMetadataBlockWriter* This,
            [In] IWICMetadataBlockReader* pIMDBlockReader = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetWriterByIndex(
            [In] IWICMetadataBlockWriter* This,
            [In, ComAliasName("UINT")] uint nIndex,
            [Out] IWICMetadataWriter** ppIMetadataWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AddWriter(
            [In] IWICMetadataBlockWriter* This,
            [In] IWICMetadataWriter* pIMetadataWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetWriterByIndex(
            [In] IWICMetadataBlockWriter* This,
            [In, ComAliasName("UINT")] uint nIndex,
            [In] IWICMetadataWriter* pIMetadataWriter = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RemoveWriterByIndex(
            [In] IWICMetadataBlockWriter* This,
            [In, ComAliasName("UINT")] uint nIndex
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
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
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IWICMetadataBlockReader Methods
        [return: ComAliasName("HRESULT")]
        public int GetContainerFormat(
            [Out, ComAliasName("GUID")] Guid* pguidContainerFormat
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_GetContainerFormat>(lpVtbl->GetContainerFormat)(
                    This,
                    pguidContainerFormat
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetCount(
            [Out, ComAliasName("UINT")] uint* pcCount
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_GetCount>(lpVtbl->GetCount)(
                    This,
                    pcCount
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetReaderByIndex(
            [In, ComAliasName("UINT")] uint nIndex,
            [Out] IWICMetadataReader** ppIMetadataReader = null
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_GetReaderByIndex>(lpVtbl->GetReaderByIndex)(
                    This,
                    nIndex,
                    ppIMetadataReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetEnumerator(
            [Out] IEnumUnknown** ppIEnumMetadata = null
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_GetEnumerator>(lpVtbl->GetEnumerator)(
                    This,
                    ppIEnumMetadata
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int InitializeFromBlockReader(
            [In] IWICMetadataBlockReader* pIMDBlockReader = null
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_InitializeFromBlockReader>(lpVtbl->InitializeFromBlockReader)(
                    This,
                    pIMDBlockReader
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetWriterByIndex(
            [In, ComAliasName("UINT")] uint nIndex,
            [Out] IWICMetadataWriter** ppIMetadataWriter = null
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_GetWriterByIndex>(lpVtbl->GetWriterByIndex)(
                    This,
                    nIndex,
                    ppIMetadataWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AddWriter(
            [In] IWICMetadataWriter* pIMetadataWriter = null
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_AddWriter>(lpVtbl->AddWriter)(
                    This,
                    pIMetadataWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetWriterByIndex(
            [In, ComAliasName("UINT")] uint nIndex,
            [In] IWICMetadataWriter* pIMetadataWriter = null
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_SetWriterByIndex>(lpVtbl->SetWriterByIndex)(
                    This,
                    nIndex,
                    pIMetadataWriter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RemoveWriterByIndex(
            [In, ComAliasName("UINT")] uint nIndex
        )
        {
            fixed (IWICMetadataBlockWriter* This = &this)
            {
                return MarshalFunction<_RemoveWriterByIndex>(lpVtbl->RemoveWriterByIndex)(
                    This,
                    nIndex
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

            #region IWICMetadataBlockReader Fields
            public IntPtr GetContainerFormat;

            public IntPtr GetCount;

            public IntPtr GetReaderByIndex;

            public IntPtr GetEnumerator;
            #endregion

            #region Fields
            public IntPtr InitializeFromBlockReader;

            public IntPtr GetWriterByIndex;

            public IntPtr AddWriter;

            public IntPtr SetWriterByIndex;

            public IntPtr RemoveWriterByIndex;
            #endregion
        }
        #endregion
    }
}

