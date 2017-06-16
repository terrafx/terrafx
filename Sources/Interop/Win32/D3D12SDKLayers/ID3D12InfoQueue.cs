// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12sdklayers.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("0742A90B-C387-483F-B946-30A7E4E61458")]
    unsafe public struct ID3D12InfoQueue
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12InfoQueue).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetMessageCountLimit(
            [In] ID3D12InfoQueue* This,
            [In] ulong MessageCountLimit
        );

        public /* static */ delegate void ClearStoredMessages(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT GetMessage(
            [In] ID3D12InfoQueue* This,
            [In] ulong MessageIndex,
            [Out, Optional] D3D12_MESSAGE* pMessage,
            [In, Out] UIntPtr* pMessageByteLength
        );

        public /* static */ delegate ulong GetNumMessagesAllowedByStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate ulong GetNumMessagesDeniedByStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate ulong GetNumStoredMessages(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate ulong GetNumStoredMessagesAllowedByRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate ulong GetNumMessagesDiscardedByMessageCountLimit(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate ulong GetMessageCountLimit(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT AddStorageFilterEntries(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate HRESULT GetStorageFilter(
            [In] ID3D12InfoQueue* This,
            [Out, Optional] D3D12_INFO_QUEUE_FILTER* pFilter,
            [In, Out] UIntPtr* pFilterByteLength
        );

        public /* static */ delegate void ClearStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT PushEmptyStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT PushCopyOfStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT PushStorageFilter(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate void PopStorageFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate uint GetStorageFilterStackSize(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT AddRetrievalFilterEntries(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate HRESULT GetRetrievalFilter(
            [In] ID3D12InfoQueue* This,
            [Out, Optional] D3D12_INFO_QUEUE_FILTER* pFilter,
            [In, Out] UIntPtr* pFilterByteLength
        );

        public /* static */ delegate void ClearRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT PushEmptyRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT PushCopyOfRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT PushRetrievalFilter(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate void PopRetrievalFilter(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate uint GetRetrievalFilterStackSize(
            [In] ID3D12InfoQueue* This
        );

        public /* static */ delegate HRESULT AddMessage(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] D3D12_MESSAGE_ID ID,
            [In] LPSTR pDescription
        );

        public /* static */ delegate HRESULT AddApplicationMessage(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] LPSTR pDescription
        );

        public /* static */ delegate HRESULT SetBreakOnCategory(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category,
            [In] BOOL bEnable
        );

        public /* static */ delegate HRESULT SetBreakOnSeverity(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity,
            [In] BOOL bEnable
        );

        public /* static */ delegate HRESULT SetBreakOnID(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_ID ID,
            [In] BOOL bEnable
        );

        public /* static */ delegate BOOL GetBreakOnCategory(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_CATEGORY Category
        );

        public /* static */ delegate BOOL GetBreakOnSeverity(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_SEVERITY Severity
        );

        public /* static */ delegate BOOL GetBreakOnID(
            [In] ID3D12InfoQueue* This,
            [In] D3D12_MESSAGE_ID ID
        );

        public /* static */ delegate void SetMuteDebugOutput(
            [In] ID3D12InfoQueue* This,
            [In] BOOL bMute
        );

        public /* static */ delegate BOOL GetMuteDebugOutput(
            [In] ID3D12InfoQueue* This
        );
        #endregion

        #region Structs
        public struct Vtbl
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
