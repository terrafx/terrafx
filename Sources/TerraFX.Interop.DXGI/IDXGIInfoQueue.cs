// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("D67441C7-672A-476F-9E82-CD55B44949CE")]
    unsafe public struct IDXGIInfoQueue
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT SetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] ulong MessageCountLimit
        );

        public /* static */ delegate void ClearStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT GetMessage(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] ulong MessageIndex,
            [Out, Optional] DXGI_INFO_QUEUE_MESSAGE* pMessage,
            [In, Out] UIntPtr* pMessageByteLength
        );

        public /* static */ delegate ulong GetNumStoredMessagesAllowedByRetrievalFilters(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate ulong GetNumStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate ulong GetNumMessagesDiscardedByMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate ulong GetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate ulong GetNumMessagesAllowedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate ulong GetNumMessagesDeniedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT AddStorageFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate HRESULT GetStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out] UIntPtr* pFilterByteLength
        );

        public /* static */ delegate HRESULT ClearStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushEmptyStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushDenyAllStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushCopyOfStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate void PopStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate uint GetStorageFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT AddRetrievalFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate HRESULT GetRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out] UIntPtr* pFilterByteLength
        );

        public /* static */ delegate void ClearRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushEmptyRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushDenyAllRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushCopyOfRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT PushRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        public /* static */ delegate void PopRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate uint GetRetrievalFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );

        public /* static */ delegate HRESULT AddMessage(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In] DXGI_INFO_QUEUE_MESSAGE_ID ID,
            [In] byte* pDescription
        );

        public /* static */ delegate HRESULT AddApplicationMessage(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In] byte* pDescription
        );

        public /* static */ delegate HRESULT SetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] BOOL bEnable
        );

        public /* static */ delegate HRESULT SetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In] BOOL bEnable
        );

        public /* static */ delegate HRESULT SetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_ID ID,
            [In] BOOL bEnable
        );

        public /* static */ delegate BOOL GetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category
        );

        public /* static */ delegate BOOL GetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity
        );

        public /* static */ delegate BOOL GetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_ID ID
        );

        public /* static */ delegate void SetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer,
            [In] BOOL bMute
        );

        public /* static */ delegate BOOL GetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In] Guid Producer
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.QueryInterface QueryInterface;

            public IUnknown.AddRef AddRef;

            public IUnknown.Release Release;

            public SetMessageCountLimit SetMessageCountLimit;

            public ClearStoredMessages ClearStoredMessages;

            public GetMessage GetMessage;

            public GetNumStoredMessagesAllowedByRetrievalFilters GetNumStoredMessagesAllowedByRetrievalFilters;

            public GetNumStoredMessages GetNumStoredMessages;

            public GetNumMessagesDiscardedByMessageCountLimit GetNumMessagesDiscardedByMessageCountLimit;

            public GetMessageCountLimit GetMessageCountLimit;

            public GetNumMessagesAllowedByStorageFilter GetNumMessagesAllowedByStorageFilter;

            public GetNumMessagesDeniedByStorageFilter GetNumMessagesDeniedByStorageFilter;

            public AddStorageFilterEntries AddStorageFilterEntries;

            public GetStorageFilter GetStorageFilter;

            public ClearStorageFilter ClearStorageFilter;

            public PushEmptyStorageFilter PushEmptyStorageFilter;

            public PushDenyAllStorageFilter PushDenyAllStorageFilter;

            public PushCopyOfStorageFilter PushCopyOfStorageFilter;

            public PushStorageFilter PushStorageFilter;

            public PopStorageFilter PopStorageFilter;

            public GetStorageFilterStackSize GetStorageFilterStackSize;

            public AddRetrievalFilterEntries AddRetrievalFilterEntries;

            public GetRetrievalFilter GetRetrievalFilter;

            public ClearRetrievalFilter ClearRetrievalFilter;

            public PushEmptyRetrievalFilter PushEmptyRetrievalFilter;

            public PushDenyAllRetrievalFilter PushDenyAllRetrievalFilter;

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
