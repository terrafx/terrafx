// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("D67441C7-672A-476F-9E82-CD55B44949CE")]
    public /* blittable */ unsafe struct IDXGIInfoQueue
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("UINT64")] ulong MessageCountLimit
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetMessage(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("UINT64")] ulong MessageIndex,
            [Out, Optional] DXGI_INFO_QUEUE_MESSAGE* pMessage,
            [In, Out, ComAliasName("SIZE_T")] nuint* pMessageByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumStoredMessagesAllowedByRetrievalFilters(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumMessagesDiscardedByMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumMessagesAllowedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetNumMessagesDeniedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddStorageFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ClearStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushEmptyStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushDenyAllStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushCopyOfStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PopStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetStorageFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddRetrievalFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushEmptyRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushDenyAllRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushCopyOfRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int PushRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void PopRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetRetrievalFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddMessage(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int AddApplicationMessage(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("BOOL")] int bMute
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int GetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
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

            public IntPtr GetNumStoredMessagesAllowedByRetrievalFilters;

            public IntPtr GetNumStoredMessages;

            public IntPtr GetNumMessagesDiscardedByMessageCountLimit;

            public IntPtr GetMessageCountLimit;

            public IntPtr GetNumMessagesAllowedByStorageFilter;

            public IntPtr GetNumMessagesDeniedByStorageFilter;

            public IntPtr AddStorageFilterEntries;

            public IntPtr GetStorageFilter;

            public IntPtr ClearStorageFilter;

            public IntPtr PushEmptyStorageFilter;

            public IntPtr PushDenyAllStorageFilter;

            public IntPtr PushCopyOfStorageFilter;

            public IntPtr PushStorageFilter;

            public IntPtr PopStorageFilter;

            public IntPtr GetStorageFilterStackSize;

            public IntPtr AddRetrievalFilterEntries;

            public IntPtr GetRetrievalFilter;

            public IntPtr ClearRetrievalFilter;

            public IntPtr PushEmptyRetrievalFilter;

            public IntPtr PushDenyAllRetrievalFilter;

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
