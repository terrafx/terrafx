// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop.DXGI
{
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [SuppressUnmanagedCodeSecurity]
    [Guid("645967A4-1392-4310-A798-8053CE3E93FD")]
    public interface IDXGIAdapter3 : IDXGIAdapter2
    {
        new void SetPrivateData([In] ref Guid Name, [In] uint DataSize, [In] IntPtr pData);

        new void SetPrivateDataInterface([In] ref Guid Name, [MarshalAs(UnmanagedType.IUnknown), In] object pUnknown);

        new void GetPrivateData([In] ref Guid Name, [In, Out] ref uint pDataSize, [Out] IntPtr pData);

        new IntPtr GetParent([In] ref Guid riid);

        new void EnumOutputs([In] uint Output, [MarshalAs(UnmanagedType.Interface), In, Out] ref IDXGIOutput ppOutput);

        new void GetDesc(out DXGI_ADAPTER_DESC pDesc);

        new void CheckInterfaceSupport([In] ref Guid InterfaceName, out long pUMDVersion);

        new void GetDesc1(out DXGI_ADAPTER_DESC1 pDesc);

        new void GetDesc2(out DXGI_ADAPTER_DESC2 pDesc);

        void RegisterHardwareContentProtectionTeardownStatusEvent([In] IntPtr hEvent, out uint pdwCookie);

        [PreserveSig]
        void UnregisterHardwareContentProtectionTeardownStatus([In] uint dwCookie);

        void QueryVideoMemoryInfo([In] uint NodeIndex, [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup, out DXGI_QUERY_VIDEO_MEMORY_INFO pVideoMemoryInfo);

        void SetVideoMemoryReservation([In] uint NodeIndex, [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup, [In] ulong Reservation);

        void RegisterVideoMemoryBudgetChangeNotificationEvent([In] IntPtr hEvent, out uint pdwCookie);

        [PreserveSig]
        void UnregisterVideoMemoryBudgetChangeNotification([In] uint dwCookie);
    }
}
