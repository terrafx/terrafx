// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.DXGI
{
    [Guid("645967A4-1392-4310-A798-8053CE3E93FD")]
    unsafe public struct IDXGIAdapter3
    {
        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT RegisterHardwareContentProtectionTeardownStatusEvent(
            [In] IDXGIAdapter3* This,
            [In] HANDLE hEvent,
            [Out] uint* pdwCookie
        );

        public /* static */ delegate void UnregisterHardwareContentProtectionTeardownStatus(
            [In] IDXGIAdapter3* This,
            [In] uint dwCookie
        );

        public /* static */ delegate HRESULT QueryVideoMemoryInfo(
            [In] IDXGIAdapter3* This,
            [In] uint NodeIndex,
            [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup,
            [Out] DXGI_QUERY_VIDEO_MEMORY_INFO* pVideoMemoryInfo
        );

        public /* static */ delegate HRESULT SetVideoMemoryReservation(
            [In] IDXGIAdapter3* This,
            [In] uint NodeIndex,
            [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup,
            [In] ulong Reservation
        );

        public /* static */ delegate HRESULT RegisterVideoMemoryBudgetChangeNotificationEvent(
            [In] IDXGIAdapter3* This,
            [In] HANDLE hEvent,
            [Out] uint* pdwCookie
        );

        public /* static */ delegate void UnregisterVideoMemoryBudgetChangeNotification(
            [In] IDXGIAdapter3* This,
            [In] uint dwCookie
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public IUnknown.QueryInterface QueryInterface;

            public IUnknown.AddRef AddRef;

            public IUnknown.Release Release;

            public IDXGIObject.SetPrivateData SetPrivateData;

            public IDXGIObject.SetPrivateDataInterface SetPrivateDataInterface;

            public IDXGIObject.GetPrivateData GetPrivateData;

            public IDXGIObject.GetParent GetParent;

            public IDXGIAdapter.EnumOutputs EnumOutputs;

            public IDXGIAdapter.GetDesc GetDesc;

            public IDXGIAdapter.CheckInterfaceSupport CheckInterfaceSupport;

            public IDXGIAdapter1.GetDesc1 GetDesc1;

            public IDXGIAdapter2.GetDesc2 GetDesc2;

            public RegisterHardwareContentProtectionTeardownStatusEvent RegisterHardwareContentProtectionTeardownStatusEvent;

            public UnregisterHardwareContentProtectionTeardownStatus UnregisterHardwareContentProtectionTeardownStatus;

            public QueryVideoMemoryInfo QueryVideoMemoryInfo;

            public SetVideoMemoryReservation SetVideoMemoryReservation;

            public RegisterVideoMemoryBudgetChangeNotificationEvent RegisterVideoMemoryBudgetChangeNotificationEvent;

            public UnregisterVideoMemoryBudgetChangeNotification UnregisterVideoMemoryBudgetChangeNotification;
            #endregion
        }
        #endregion
    }
}
