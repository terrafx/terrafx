// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("D67441C7-672A-476F-9E82-CD55B44949CE")]
    unsafe public /* blittable */ struct IDXGIInfoQueue
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] UINT64 MessageCountLimit
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMessage(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] UINT64 MessageIndex,
            [Out, Optional] DXGI_INFO_QUEUE_MESSAGE* pMessage,
            [In, Out] SIZE_T* pMessageByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumStoredMessagesAllowedByRetrievalFilters(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumMessagesDiscardedByMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumMessagesAllowedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetNumMessagesDeniedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddStorageFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out] SIZE_T* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT ClearStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushEmptyStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushDenyAllStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushCopyOfStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PopStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetStorageFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddRetrievalFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out] SIZE_T* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushEmptyRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushDenyAllRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushCopyOfRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT PushRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PopRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetRetrievalFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddMessage(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In] DXGI_INFO_QUEUE_MESSAGE_ID ID,
            [In] LPCSTR pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT AddApplicationMessage(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In] LPCSTR pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] BOOL bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In] BOOL bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT SetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_ID ID,
            [In] BOOL bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_ID ID
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer,
            [In] BOOL bMute
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL GetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_DEBUG_ID Producer
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
