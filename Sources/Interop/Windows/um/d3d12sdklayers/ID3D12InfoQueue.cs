// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0742A90B-C387-483F-B946-30A7E4E61458")]
    unsafe public /* blittable */ struct ID3D12InfoQueue
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetMessageCountLimit(
            [In] ID3D12InfoQueue* This,
            [In, ComAliasName("UINT64")] ulong MessageCountLimit
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearStoredMessages(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMessage(
            [In] ID3D12InfoQueue* This,
            [In, ComAliasName("UINT64")] ulong MessageIndex,
            [Out, Optional] D3D12_MESSAGE* pMessage,
            [In, Out, ComAliasName("SIZE_T")] nuint* pMessageByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumMessagesAllowedByStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumMessagesDeniedByStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumStoredMessages(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumStoredMessagesAllowedByRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumMessagesDiscardedByMessageCountLimit(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetMessageCountLimit(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddStorageFilterEntries(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetStorageFilter(
            [In] ID3D12InfoQueue* This,
            [Out, Optional] D3D12_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushEmptyStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushCopyOfStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushStorageFilter(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PopStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetStorageFilterStackSize(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddRetrievalFilterEntries(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRetrievalFilter(
            [In] ID3D12InfoQueue* This,
            [Out, Optional] D3D12_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushEmptyRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushCopyOfRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushRetrievalFilter(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PopRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetRetrievalFilterStackSize(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddMessage(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] D3D12_MESSAGE_ID ID,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddApplicationMessage(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetBreakOnCategory(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetBreakOnSeverity(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetBreakOnID(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_ID ID,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetBreakOnCategory(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetBreakOnSeverity(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetBreakOnID(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_ID ID
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMuteDebugOutput(
            [In] ID3D12InfoQueue* This,
            [In, ComAliasName("BOOL")] int bMute
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetMuteDebugOutput(
            [In] ID3D12InfoQueue* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public IntPtr SetMessageCountLimit;

            public IntPtr ClearStoredMessages;

            public IntPtr GetMessage;

            public IntPtr GetNumMessagesAllowedByStorageFilter;

            public IntPtr GetNumMessagesDeniedByStorageFilter;

            public IntPtr GetNumStoredMessages;

            public IntPtr GetNumStoredMessagesAllowedByRetrievalFilter;

            public IntPtr GetNumMessagesDiscardedByMessageCountLimit;

            public IntPtr GetMessageCountLimit;

            public IntPtr AddStorageFilterEntries;

            public IntPtr GetStorageFilter;

            public IntPtr ClearStorageFilter;

            public IntPtr PushEmptyStorageFilter;

            public IntPtr PushCopyOfStorageFilter;

            public IntPtr PushStorageFilter;

            public IntPtr PopStorageFilter;

            public IntPtr GetStorageFilterStackSize;

            public IntPtr AddRetrievalFilterEntries;

            public IntPtr GetRetrievalFilter;

            public IntPtr ClearRetrievalFilter;

            public IntPtr PushEmptyRetrievalFilter;

            public IntPtr PushCopyOfRetrievalFilter;

            public IntPtr PushRetrievalFilter;

            public IntPtr PopRetrievalFilter;

            public IntPtr GetRetrievalFilterStackSize;

            public IntPtr AddMessage;

            public IntPtr AddApplicationMessage;

            public IntPtr SetBreakOnCategory;

            public IntPtr SetBreakOnSeverity;

            public IntPtr SetBreakOnID;

            public IntPtr GetBreakOnCategory;

            public IntPtr GetBreakOnSeverity;

            public IntPtr GetBreakOnID;

            public IntPtr SetMuteDebugOutput;

            public IntPtr GetMuteDebugOutput;
            #endregion
        }
        #endregion
    }
}
