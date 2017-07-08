// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

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
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetMessageCountLimit(
            [In] ID3D12InfoQueue* This,
            [In] UINT64 MessageCountLimit
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearStoredMessages(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMessage(
            [In] ID3D12InfoQueue* This,
            [In] UINT64 MessageIndex,
            [Out, Optional] D3D12_MESSAGE* pMessage,
            [In, Out] SIZE_T* pMessageByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumMessagesAllowedByStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumMessagesDeniedByStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumStoredMessages(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumStoredMessagesAllowedByRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumMessagesDiscardedByMessageCountLimit(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetMessageCountLimit(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddStorageFilterEntries(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetStorageFilter(
            [In] ID3D12InfoQueue* This,
            [Out, Optional] D3D12_INFO_QUEUE_FILTER* pFilter,
            [In, Out] SIZE_T* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushEmptyStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushCopyOfStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushStorageFilter(
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
        public /* static */ delegate UINT GetStorageFilterStackSize(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddRetrievalFilterEntries(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRetrievalFilter(
            [In] ID3D12InfoQueue* This,
            [Out, Optional] D3D12_INFO_QUEUE_FILTER* pFilter,
            [In, Out] SIZE_T* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushEmptyRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushCopyOfRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushRetrievalFilter(
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
        public /* static */ delegate UINT GetRetrievalFilterStackSize(
            [In] ID3D12InfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddMessage(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] D3D12_MESSAGE_ID ID,
            [In] LPCSTR pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddApplicationMessage(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] LPCSTR pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBreakOnCategory(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category,
            [In] BOOL bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBreakOnSeverity(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] BOOL bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBreakOnID(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_ID ID,
            [In] BOOL bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetBreakOnCategory(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetBreakOnSeverity(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetBreakOnID(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_ID ID
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMuteDebugOutput(
            [In] ID3D12InfoQueue* This,
            [In] BOOL bMute
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetMuteDebugOutput(
            [In] ID3D12InfoQueue* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public SetMessageCountLimit SetMessageCountLimit;

            public ClearStoredMessages ClearStoredMessages;

            public GetMessage GetMessage;

            public GetNumMessagesAllowedByStorageFilter GetNumMessagesAllowedByStorageFilter;

            public GetNumMessagesDeniedByStorageFilter GetNumMessagesDeniedByStorageFilter;

            public GetNumStoredMessages GetNumStoredMessages;

            public GetNumStoredMessagesAllowedByRetrievalFilter GetNumStoredMessagesAllowedByRetrievalFilter;

            public GetNumMessagesDiscardedByMessageCountLimit GetNumMessagesDiscardedByMessageCountLimit;

            public GetMessageCountLimit GetMessageCountLimit;

            public AddStorageFilterEntries AddStorageFilterEntries;

            public GetStorageFilter GetStorageFilter;

            public ClearStorageFilter ClearStorageFilter;

            public PushEmptyStorageFilter PushEmptyStorageFilter;

            public PushCopyOfStorageFilter PushCopyOfStorageFilter;

            public PushStorageFilter PushStorageFilter;

            public PopStorageFilter PopStorageFilter;

            public GetStorageFilterStackSize GetStorageFilterStackSize;

            public AddRetrievalFilterEntries AddRetrievalFilterEntries;

            public GetRetrievalFilter GetRetrievalFilter;

            public ClearRetrievalFilter ClearRetrievalFilter;

            public PushEmptyRetrievalFilter PushEmptyRetrievalFilter;

            public PushCopyOfRetrievalFilter PushCopyOfRetrievalFilter;

            public PushRetrievalFilter PushRetrievalFilter;

            public PopRetrievalFilter PopRetrievalFilter;

            public GetRetrievalFilterStackSize GetRetrievalFilterStackSize;

            public AddMessage AddMessage;

            public AddApplicationMessage AddApplicationMessage;

            public SetBreakOnCategory SetBreakOnCategory;

            public SetBreakOnSeverity SetBreakOnSeverity;

            public SetBreakOnID SetBreakOnID;

            public GetBreakOnCategory GetBreakOnCategory;

            public GetBreakOnSeverity GetBreakOnSeverity;

            public GetBreakOnID GetBreakOnID;

            public SetMuteDebugOutput SetMuteDebugOutput;

            public GetMuteDebugOutput GetMuteDebugOutput;
            #endregion
        }
        #endregion
    }
}
