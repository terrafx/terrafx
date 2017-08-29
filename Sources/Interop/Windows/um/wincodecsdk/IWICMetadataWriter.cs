// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\wincodecsdk.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("F7836E16-3BE0-470B-86BB-160D0AECD7DE")]
    public /* blittable */ unsafe struct IWICMetadataWriter
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] IWICMetadataWriter* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] IWICMetadataWriter* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] IWICMetadataWriter* This
        );
        #endregion

        #region IWICMetadataReader Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMetadataFormat(
            [In] IWICMetadataWriter* This,
            [Out, ComAliasName("GUID")] Guid* pguidMetadataFormat
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMetadataHandlerInfo(
            [In] IWICMetadataWriter* This,
            [Out] IWICMetadataHandlerInfo** ppIHandler = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetCount(
            [In] IWICMetadataWriter* This,
            [Out, ComAliasName("UINT")] uint* pcCount
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetValueByIndex(
            [In] IWICMetadataWriter* This,
            [In, ComAliasName("UINT")] uint nIndex,
            [In, Out] PROPVARIANT* pvarSchema = null,
            [In, Out] PROPVARIANT* pvarId = null,
            [In, Out] PROPVARIANT* pvarValue = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetValue(
            [In] IWICMetadataWriter* This,
            [In, Optional] PROPVARIANT* pvarSchema,
            [In] PROPVARIANT* pvarId,
            [In, Out] PROPVARIANT* pvarValue = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetEnumerator(
            [In] IWICMetadataWriter* This,
            [Out] IWICEnumMetadataItem** ppIEnumMetadata = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetValue(
            [In] IWICMetadataWriter* This,
            [In, Optional] PROPVARIANT* pvarSchema,
            [In] PROPVARIANT* pvarId,
            [In] PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetValueByIndex(
            [In] IWICMetadataWriter* This,
            [In, ComAliasName("UINT")] uint nIndex,
            [In, Optional] PROPVARIANT* pvarSchema,
            [In] PROPVARIANT* pvarId,
            [In] PROPVARIANT* pvarValue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RemoveValue(
            [In] IWICMetadataWriter* This,
            [In, Optional] PROPVARIANT* pvarSchema,
            [In] PROPVARIANT* pvarId
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int RemoveValueByIndex(
            [In] IWICMetadataWriter* This,
            [In, ComAliasName("UINT")] uint nIndex
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

            #region IWICMetadataReader Fields
            public IntPtr GetMetadataFormat;

            public IntPtr GetMetadataHandlerInfo;

            public IntPtr GetCount;

            public IntPtr GetValueByIndex;

            public IntPtr GetValue;

            public IntPtr GetEnumerator;
            #endregion

            #region Fields
            public IntPtr SetValue;

            public IntPtr SetValueByIndex;

            public IntPtr RemoveValue;

            public IntPtr RemoveValueByIndex;
            #endregion
        }
        #endregion
    }
}
