// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\dxgidebug.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [Guid("D67441C7-672A-476F-9E82-CD55B44949CE")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    public interface IDXGIInfoQueue
    {
        #region Methods
        void SetMessageCountLimit(Guid Producer, ulong MessageCountLimit);

        [PreserveSig]
        void ClearStoredMessages(Guid Producer);

        void GetMessage(Guid Producer, ulong MessageIndex, ref DXGI_INFO_QUEUE_MESSAGE pMessage, ref ulong pMessageByteLength);

        [PreserveSig]
        ulong GetNumStoredMessagesAllowedByRetrievalFilters(Guid Producer);

        [PreserveSig]
        ulong GetNumStoredMessages(Guid Producer);

        [PreserveSig]
        ulong GetNumMessagesDiscardedByMessageCountLimit(Guid Producer);

        [PreserveSig]
        ulong GetMessageCountLimit(Guid Producer);

        [PreserveSig]
        ulong GetNumMessagesAllowedByStorageFilter(Guid Producer);

        [PreserveSig]
        ulong GetNumMessagesDeniedByStorageFilter(Guid Producer);

        void AddStorageFilterEntries(Guid Producer, ref DXGI_INFO_QUEUE_FILTER pFilter);

        void GetStorageFilter(Guid Producer, ref DXGI_INFO_QUEUE_FILTER pFilter, ref ulong pFilterByteLength);

        [PreserveSig]
        void ClearStorageFilter(Guid Producer);

        void PushEmptyStorageFilter(Guid Producer);

        void PushDenyAllStorageFilter(Guid Producer);

        void PushCopyOfStorageFilter(Guid Producer);

        void PushStorageFilter(Guid Producer, ref DXGI_INFO_QUEUE_FILTER pFilter);

        [PreserveSig]
        void PopStorageFilter(Guid Producer);

        [PreserveSig]
        uint GetStorageFilterStackSize(Guid Producer);

        void AddRetrievalFilterEntries(Guid Producer, ref DXGI_INFO_QUEUE_FILTER pFilter);

        void GetRetrievalFilter(Guid Producer, ref DXGI_INFO_QUEUE_FILTER pFilter, ref ulong pFilterByteLength);

        [PreserveSig]
        void ClearRetrievalFilter(Guid Producer);

        void PushEmptyRetrievalFilter(Guid Producer);

        void PushDenyAllRetrievalFilter(Guid Producer);

        void PushCopyOfRetrievalFilter(Guid Producer);

        void PushRetrievalFilter(Guid Producer, ref DXGI_INFO_QUEUE_FILTER pFilter);

        [PreserveSig]
        void PopRetrievalFilter(Guid Producer);

        [PreserveSig]
        uint GetRetrievalFilterStackSize(Guid Producer);

        void AddMessage(Guid Producer, DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category, DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity, int ID, [MarshalAs(UnmanagedType.LPStr)] string pDescription);

        void AddApplicationMessage(DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity, [MarshalAs(UnmanagedType.LPStr)] string pDescription);

        void SetBreakOnCategory(Guid Producer, DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category, int bEnable);

        void SetBreakOnSeverity(Guid Producer, DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity, int bEnable);

        void SetBreakOnID(Guid Producer, int ID, int bEnable);

        [PreserveSig]
        int GetBreakOnCategory(Guid Producer, DXGI_INFO_QUEUE_MESSAGE_CATEGORY Category);

        [PreserveSig]
        int GetBreakOnSeverity(Guid Producer, DXGI_INFO_QUEUE_MESSAGE_SEVERITY Severity);

        [PreserveSig]
        int GetBreakOnID(Guid Producer, int ID);

        [PreserveSig]
        void SetMuteDebugOutput(Guid Producer, int bMute);

        [PreserveSig]
        int GetMuteDebugOutput(Guid Producer);
        #endregion
    }
}
