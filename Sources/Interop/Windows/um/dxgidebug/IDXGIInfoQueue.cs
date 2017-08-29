// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("D67441C7-672A-476F-9E82-CD55B44949CE")]
    public /* blittable */ unsafe struct IDXGIInfoQueue
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIInfoQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIInfoQueue* This
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("UINT64")] ulong MessageCountLimit
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ClearStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetMessage(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("UINT64")] ulong MessageIndex,
            [Out, Optional] DXGI_INFO_QUEUE_MESSAGE* pMessage,
            [In, Out, ComAliasName("SIZE_T")] nuint* pMessageByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetNumStoredMessagesAllowedByRetrievalFilters(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetNumStoredMessages(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetNumMessagesDiscardedByMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetMessageCountLimit(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetNumMessagesAllowedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong _GetNumMessagesDeniedByStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AddStorageFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _ClearStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushEmptyStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushDenyAllStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushCopyOfStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _PopStorageFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetStorageFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AddRetrievalFilterEntries(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ClearRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushEmptyRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushDenyAllRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushCopyOfRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _PushRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _PopRetrievalFilter(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetRetrievalFilterStackSize(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AddMessage(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID,
            [In, ComAliasName("LPCSTR")] sbyte* pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _AddApplicationMessage(
            [In] IDXGIInfoQueue* This,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("LPCSTR")] sbyte* pDescription
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID,
            [In, ComAliasName("BOOL")] int bEnable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _GetBreakOnCategory(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _GetBreakOnSeverity(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _GetBreakOnID(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("BOOL")] int bMute
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("BOOL")]
        public /* static */ delegate int _GetMuteDebugOutput(
            [In] IDXGIInfoQueue* This,
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
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
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int SetMessageCountLimit(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("UINT64")] ulong MessageCountLimit
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_SetMessageCountLimit>(lpVtbl->SetMessageCountLimit)(
                    This,
                    Producer,
                    MessageCountLimit
                );
            }
        }

        public void ClearStoredMessages(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                MarshalFunction<_ClearStoredMessages>(lpVtbl->ClearStoredMessages)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetMessage(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("UINT64")] ulong MessageIndex,
            [Out, Optional] DXGI_INFO_QUEUE_MESSAGE* pMessage,
            [In, Out, ComAliasName("SIZE_T")] nuint* pMessageByteLength
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetMessage>(lpVtbl->GetMessage)(
                    This,
                    Producer,
                    MessageIndex,
                    pMessage,
                    pMessageByteLength
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetNumStoredMessagesAllowedByRetrievalFilters(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetNumStoredMessagesAllowedByRetrievalFilters>(lpVtbl->GetNumStoredMessagesAllowedByRetrievalFilters)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetNumStoredMessages(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetNumStoredMessages>(lpVtbl->GetNumStoredMessages)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetNumMessagesDiscardedByMessageCountLimit(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetNumMessagesDiscardedByMessageCountLimit>(lpVtbl->GetNumMessagesDiscardedByMessageCountLimit)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetMessageCountLimit(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetMessageCountLimit>(lpVtbl->GetMessageCountLimit)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetNumMessagesAllowedByStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetNumMessagesAllowedByStorageFilter>(lpVtbl->GetNumMessagesAllowedByStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT64")]
        public ulong GetNumMessagesDeniedByStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetNumMessagesDeniedByStorageFilter>(lpVtbl->GetNumMessagesDeniedByStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AddStorageFilterEntries(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_AddStorageFilterEntries>(lpVtbl->AddStorageFilterEntries)(
                    This,
                    Producer,
                    pFilter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetStorageFilter>(lpVtbl->GetStorageFilter)(
                    This,
                    Producer,
                    pFilter,
                    pFilterByteLength
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int ClearStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_ClearStorageFilter>(lpVtbl->ClearStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushEmptyStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushEmptyStorageFilter>(lpVtbl->PushEmptyStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushDenyAllStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushDenyAllStorageFilter>(lpVtbl->PushDenyAllStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushCopyOfStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushCopyOfStorageFilter>(lpVtbl->PushCopyOfStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushStorageFilter>(lpVtbl->PushStorageFilter)(
                    This,
                    Producer,
                    pFilter
                );
            }
        }

        public void PopStorageFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                MarshalFunction<_PopStorageFilter>(lpVtbl->PopStorageFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetStorageFilterStackSize(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetStorageFilterStackSize>(lpVtbl->GetStorageFilterStackSize)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AddRetrievalFilterEntries(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_AddRetrievalFilterEntries>(lpVtbl->AddRetrievalFilterEntries)(
                    This,
                    Producer,
                    pFilter
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [Out, Optional] DXGI_INFO_QUEUE_FILTER* pFilter,
            [In, Out, ComAliasName("SIZE_T")] nuint* pFilterByteLength
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetRetrievalFilter>(lpVtbl->GetRetrievalFilter)(
                    This,
                    Producer,
                    pFilter,
                    pFilterByteLength
                );
            }
        }

        public void ClearRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                MarshalFunction<_ClearRetrievalFilter>(lpVtbl->ClearRetrievalFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushEmptyRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushEmptyRetrievalFilter>(lpVtbl->PushEmptyRetrievalFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushDenyAllRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushDenyAllRetrievalFilter>(lpVtbl->PushDenyAllRetrievalFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushCopyOfRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushCopyOfRetrievalFilter>(lpVtbl->PushCopyOfRetrievalFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int PushRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_FILTER* pFilter
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_PushRetrievalFilter>(lpVtbl->PushRetrievalFilter)(
                    This,
                    Producer,
                    pFilter
                );
            }
        }

        public void PopRetrievalFilter(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                MarshalFunction<_PopRetrievalFilter>(lpVtbl->PopRetrievalFilter)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetRetrievalFilterStackSize(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetRetrievalFilterStackSize>(lpVtbl->GetRetrievalFilterStackSize)(
                    This,
                    Producer
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AddMessage(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID,
            [In, ComAliasName("LPCSTR")] sbyte* pDescription
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_AddMessage>(lpVtbl->AddMessage)(
                    This,
                    Producer,
                    Category,
                    Severity,
                    ID,
                    pDescription
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int AddApplicationMessage(
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("LPCSTR")] sbyte* pDescription
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_AddApplicationMessage>(lpVtbl->AddApplicationMessage)(
                    This,
                    Severity,
                    pDescription
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetBreakOnCategory(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category,
            [In, ComAliasName("BOOL")] int bEnable
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_SetBreakOnCategory>(lpVtbl->SetBreakOnCategory)(
                    This,
                    Producer,
                    Category,
                    bEnable
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetBreakOnSeverity(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity,
            [In, ComAliasName("BOOL")] int bEnable
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_SetBreakOnSeverity>(lpVtbl->SetBreakOnSeverity)(
                    This,
                    Producer,
                    Severity,
                    bEnable
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetBreakOnID(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID,
            [In, ComAliasName("BOOL")] int bEnable
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_SetBreakOnID>(lpVtbl->SetBreakOnID)(
                    This,
                    Producer,
                    ID,
                    bEnable
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int GetBreakOnCategory(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetBreakOnCategory>(lpVtbl->GetBreakOnCategory)(
                    This,
                    Producer,
                    Category
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int GetBreakOnSeverity(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In] DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetBreakOnSeverity>(lpVtbl->GetBreakOnSeverity)(
                    This,
                    Producer,
                    Severity
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int GetBreakOnID(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("DXGI_INFO_QUEUE_MESSAGE_ID")] int ID
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetBreakOnID>(lpVtbl->GetBreakOnID)(
                    This,
                    Producer,
                    ID
                );
            }
        }

        public void SetMuteDebugOutput(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer,
            [In, ComAliasName("BOOL")] int bMute
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                MarshalFunction<_SetMuteDebugOutput>(lpVtbl->SetMuteDebugOutput)(
                    This,
                    Producer,
                    bMute
                );
            }
        }

        [return: ComAliasName("BOOL")]
        public int GetMuteDebugOutput(
            [In, ComAliasName("DXGI_DEBUG_ID")] Guid Producer
        )
        {
            fixed (IDXGIInfoQueue* This = &this)
            {
                return MarshalFunction<_GetMuteDebugOutput>(lpVtbl->GetMuteDebugOutput)(
                    This,
                    Producer
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

