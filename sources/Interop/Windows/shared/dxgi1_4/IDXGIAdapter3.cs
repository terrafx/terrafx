// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from shared\dxgi1_4.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using TerraFX.Utilities;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("645967A4-1392-4310-A798-8053CE3E93FD")]
    [Unmanaged]
    public unsafe struct IDXGIAdapter3
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] IDXGIAdapter3* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] IDXGIAdapter3* This
        );
        #endregion

        #region IDXGIObject Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetParent(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        );
        #endregion

        #region IDXGIAdapter Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _EnumOutputs(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("UINT")] uint Output,
            [In, Out] IDXGIOutput** ppOutput
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc(
            [In] IDXGIAdapter3* This,
            [Out] DXGI_ADAPTER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CheckInterfaceSupport(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("REFGUID")] Guid* InterfaceName,
            [Out] LARGE_INTEGER* pUMDVersion
        );
        #endregion

        #region IDXGIAdapter1 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc1(
            [In] IDXGIAdapter3* This,
            [Out] DXGI_ADAPTER_DESC1* pDesc
        );
        #endregion

        #region IDXGIAdapter2 Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDesc2(
            [In] IDXGIAdapter3* This,
            [Out] DXGI_ADAPTER_DESC2* pDesc
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterHardwareContentProtectionTeardownStatusEvent(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _UnregisterHardwareContentProtectionTeardownStatus(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("DWORD")] uint dwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryVideoMemoryInfo(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("UINT")] uint NodeIndex,
            [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup,
            [Out] DXGI_QUERY_VIDEO_MEMORY_INFO* pVideoMemoryInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetVideoMemoryReservation(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("UINT")] uint NodeIndex,
            [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup,
            [In, ComAliasName("UINT64")] ulong Reservation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _RegisterVideoMemoryBudgetChangeNotificationEvent(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _UnregisterVideoMemoryBudgetChangeNotification(
            [In] IDXGIAdapter3* This,
            [In, ComAliasName("DWORD")] uint dwCookie
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (IDXGIAdapter3* This = &this)
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
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region IDXGIObject Methods
        [return: ComAliasName("HRESULT")]
        public int SetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_SetPrivateData>(lpVtbl->SetPrivateData)(
                    This,
                    Name,
                    DataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateDataInterface(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In] IUnknown* pUnknown = null
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_SetPrivateDataInterface>(lpVtbl->SetPrivateDataInterface)(
                    This,
                    Name,
                    pUnknown
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* Name,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_GetPrivateData>(lpVtbl->GetPrivateData)(
                    This,
                    Name,
                    pDataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetParent(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppParent
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_GetParent>(lpVtbl->GetParent)(
                    This,
                    riid,
                    ppParent
                );
            }
        }
        #endregion

        #region IDXGIAdapter Methods
        [return: ComAliasName("HRESULT")]
        public int EnumOutputs(
            [In, ComAliasName("UINT")] uint Output,
            [In, Out] IDXGIOutput** ppOutput
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_EnumOutputs>(lpVtbl->EnumOutputs)(
                    This,
                    Output,
                    ppOutput
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDesc(
            [Out] DXGI_ADAPTER_DESC* pDesc
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CheckInterfaceSupport(
            [In, ComAliasName("REFGUID")] Guid* InterfaceName,
            [Out] LARGE_INTEGER* pUMDVersion
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_CheckInterfaceSupport>(lpVtbl->CheckInterfaceSupport)(
                    This,
                    InterfaceName,
                    pUMDVersion
                );
            }
        }
        #endregion

        #region IDXGIAdapter1 Methods
        [return: ComAliasName("HRESULT")]
        public int GetDesc1(
            [Out] DXGI_ADAPTER_DESC1* pDesc
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_GetDesc1>(lpVtbl->GetDesc1)(
                    This,
                    pDesc
                );
            }
        }
        #endregion

        #region IDXGIAdapter2 Methods
        [return: ComAliasName("HRESULT")]
        public int GetDesc2(
            [Out] DXGI_ADAPTER_DESC2* pDesc
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_GetDesc2>(lpVtbl->GetDesc2)(
                    This,
                    pDesc
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int RegisterHardwareContentProtectionTeardownStatusEvent(
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_RegisterHardwareContentProtectionTeardownStatusEvent>(lpVtbl->RegisterHardwareContentProtectionTeardownStatusEvent)(
                    This,
                    hEvent,
                    pdwCookie
                );
            }
        }

        public void UnregisterHardwareContentProtectionTeardownStatus(
            [In, ComAliasName("DWORD")] uint dwCookie
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                MarshalFunction<_UnregisterHardwareContentProtectionTeardownStatus>(lpVtbl->UnregisterHardwareContentProtectionTeardownStatus)(
                    This,
                    dwCookie
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int QueryVideoMemoryInfo(
            [In, ComAliasName("UINT")] uint NodeIndex,
            [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup,
            [Out] DXGI_QUERY_VIDEO_MEMORY_INFO* pVideoMemoryInfo
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_QueryVideoMemoryInfo>(lpVtbl->QueryVideoMemoryInfo)(
                    This,
                    NodeIndex,
                    MemorySegmentGroup,
                    pVideoMemoryInfo
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetVideoMemoryReservation(
            [In, ComAliasName("UINT")] uint NodeIndex,
            [In] DXGI_MEMORY_SEGMENT_GROUP MemorySegmentGroup,
            [In, ComAliasName("UINT64")] ulong Reservation
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_SetVideoMemoryReservation>(lpVtbl->SetVideoMemoryReservation)(
                    This,
                    NodeIndex,
                    MemorySegmentGroup,
                    Reservation
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int RegisterVideoMemoryBudgetChangeNotificationEvent(
            [In, ComAliasName("HANDLE")] IntPtr hEvent,
            [Out, ComAliasName("DWORD")] uint* pdwCookie
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                return MarshalFunction<_RegisterVideoMemoryBudgetChangeNotificationEvent>(lpVtbl->RegisterVideoMemoryBudgetChangeNotificationEvent)(
                    This,
                    hEvent,
                    pdwCookie
                );
            }
        }

        public void UnregisterVideoMemoryBudgetChangeNotification(
            [In, ComAliasName("DWORD")] uint dwCookie
        )
        {
            fixed (IDXGIAdapter3* This = &this)
            {
                MarshalFunction<_UnregisterVideoMemoryBudgetChangeNotification>(lpVtbl->UnregisterVideoMemoryBudgetChangeNotification)(
                    This,
                    dwCookie
                );
            }
        }
        #endregion

        #region Structs
        [Unmanaged]
        public struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region IDXGIObject Fields
            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr GetPrivateData;

            public IntPtr GetParent;
            #endregion

            #region IDXGIAdapter Fields
            public IntPtr EnumOutputs;

            public IntPtr GetDesc;

            public IntPtr CheckInterfaceSupport;
            #endregion

            #region IDXGIAdapter1 Fields
            public IntPtr GetDesc1;
            #endregion

            #region IDXGIAdapter2 Fields
            public IntPtr GetDesc2;
            #endregion

            #region Fields
            public IntPtr RegisterHardwareContentProtectionTeardownStatusEvent;

            public IntPtr UnregisterHardwareContentProtectionTeardownStatus;

            public IntPtr QueryVideoMemoryInfo;

            public IntPtr SetVideoMemoryReservation;

            public IntPtr RegisterVideoMemoryBudgetChangeNotificationEvent;

            public IntPtr UnregisterVideoMemoryBudgetChangeNotification;
            #endregion
        }
        #endregion
    }
}
