// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("0EC870A6-5D7E-4C22-8CFC-5BAAE07616ED")]
    public /* unmanaged */ unsafe struct ID3D12CommandQueue
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3D12CommandQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3D12CommandQueue* This
        );
        #endregion

        #region ID3D12Object Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        );


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetName(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("LPCWSTR")] char* Name
        );
        #endregion

        #region ID3D12DeviceChild Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDevice(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvDevice = null
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _UpdateTileMappings(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Resource* pResource,
            [In, ComAliasName("UINT")] uint NumResourceRegions,
            [In, Optional, ComAliasName("D3D12_TILED_RESOURCE_COORDINATE[]")] D3D12_TILED_RESOURCE_COORDINATE* pResourceRegionStartCoordinates,
            [In, Optional, ComAliasName("D3D12_TILE_REGION_SIZE[]")] D3D12_TILE_REGION_SIZE* pResourceRegionSizes,
            [In, Optional] ID3D12Heap* pHeap,
            [In, ComAliasName("UINT")] uint NumRanges,
            [In, Optional, ComAliasName("D3D12_TILE_RANGE_FLAGS[]")] D3D12_TILE_RANGE_FLAGS* pRangeFlags,
            [In, Optional, ComAliasName("UINT[]")] uint* pHeapRangeStartOffsets,
            [In, Optional, ComAliasName("UINT[]")] uint* pRangeTileCounts,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CopyTileMappings(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Resource* pDstResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pDstRegionStartCoordinate,
            [In] ID3D12Resource* pSrcResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pSrcRegionStartCoordinate,
            [In] D3D12_TILE_REGION_SIZE* pRegionSize,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _ExecuteCommandLists(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("UINT")] uint NumCommandLists,
            [In, ComAliasName("ID3D12CommandList*[]")] ID3D12CommandList** ppCommandLists
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _SetMarker(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] void* pData,
            [In, ComAliasName("UINT")] uint Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _BeginEvent(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] void* pData,
            [In, ComAliasName("UINT")] uint Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _EndEvent(
            [In] ID3D12CommandQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Signal(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Fence* pFence,
            [In, ComAliasName("UINT64")] ulong Value
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Wait(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Fence* pFence,
            [In, ComAliasName("UINT64")] ulong Value
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetTimestampFrequency(
            [In] ID3D12CommandQueue* This,
            [Out, ComAliasName("UINT64")] ulong* pFrequency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetClockCalibration(
            [In] ID3D12CommandQueue* This,
            [Out, ComAliasName("UINT64")] ulong* pGpuTimestamp,
            [Out, ComAliasName("UINT64")] ulong* pCpuTimestamp
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetDesc(
            [In] ID3D12CommandQueue* This,
            [Out] D3D12_COMMAND_QUEUE_DESC* pDesc
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
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
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_Release>(lpVtbl->Release)(
                    This
                );
            }
        }
        #endregion

        #region ID3D12Object Methods
        [return: ComAliasName("HRESULT")]
        public int GetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_GetPrivateData>(lpVtbl->GetPrivateData)(
                    This,
                    guid,
                    pDataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateData(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_SetPrivateData>(lpVtbl->SetPrivateData)(
                    This,
                    guid,
                    DataSize,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetPrivateDataInterface(
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_SetPrivateDataInterface>(lpVtbl->SetPrivateDataInterface)(
                    This,
                    guid,
                    pData
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetName(
            [In, ComAliasName("LPCWSTR")] char* Name
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_SetName>(lpVtbl->SetName)(
                    This,
                    Name
                );
            }
        }
        #endregion

        #region ID3D12DeviceChild Methods
        [return: ComAliasName("HRESULT")]
        public int GetDevice(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvDevice = null
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_GetDevice>(lpVtbl->GetDevice)(
                    This,
                    riid,
                    ppvDevice
                );
            }
        }
        #endregion

        #region Methods
        public void UpdateTileMappings(
            [In] ID3D12Resource* pResource,
            [In, ComAliasName("UINT")] uint NumResourceRegions,
            [In, Optional, ComAliasName("D3D12_TILED_RESOURCE_COORDINATE[]")] D3D12_TILED_RESOURCE_COORDINATE* pResourceRegionStartCoordinates,
            [In, Optional, ComAliasName("D3D12_TILE_REGION_SIZE[]")] D3D12_TILE_REGION_SIZE* pResourceRegionSizes,
            [In, Optional] ID3D12Heap* pHeap,
            [In, ComAliasName("UINT")] uint NumRanges,
            [In, Optional, ComAliasName("D3D12_TILE_RANGE_FLAGS[]")] D3D12_TILE_RANGE_FLAGS* pRangeFlags,
            [In, Optional, ComAliasName("UINT[]")] uint* pHeapRangeStartOffsets,
            [In, Optional, ComAliasName("UINT[]")] uint* pRangeTileCounts,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_UpdateTileMappings>(lpVtbl->UpdateTileMappings)(
                    This,
                    pResource,
                    NumResourceRegions,
                    pResourceRegionStartCoordinates,
                    pResourceRegionSizes,
                    pHeap,
                    NumRanges,
                    pRangeFlags,
                    pHeapRangeStartOffsets,
                    pRangeTileCounts,
                    Flags
                );
            }
        }

        public void CopyTileMappings(
            [In] ID3D12Resource* pDstResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pDstRegionStartCoordinate,
            [In] ID3D12Resource* pSrcResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pSrcRegionStartCoordinate,
            [In] D3D12_TILE_REGION_SIZE* pRegionSize,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_CopyTileMappings>(lpVtbl->CopyTileMappings)(
                    This,
                    pDstResource,
                    pDstRegionStartCoordinate,
                    pSrcResource,
                    pSrcRegionStartCoordinate,
                    pRegionSize,
                    Flags
                );
            }
        }

        public void ExecuteCommandLists(
            [In, ComAliasName("UINT")] uint NumCommandLists,
            [In, ComAliasName("ID3D12CommandList*[]")] ID3D12CommandList** ppCommandLists
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_ExecuteCommandLists>(lpVtbl->ExecuteCommandLists)(
                    This,
                    NumCommandLists,
                    ppCommandLists
                );
            }
        }

        public void SetMarker(
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] void* pData,
            [In, ComAliasName("UINT")] uint Size
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_SetMarker>(lpVtbl->SetMarker)(
                    This,
                    Metadata,
                    pData,
                    Size
                );
            }
        }

        public void BeginEvent(
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] void* pData,
            [In, ComAliasName("UINT")] uint Size
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_BeginEvent>(lpVtbl->BeginEvent)(
                    This,
                    Metadata,
                    pData,
                    Size
                );
            }
        }

        public void EndEvent()
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_EndEvent>(lpVtbl->EndEvent)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Signal(
            [In] ID3D12Fence* pFence,
            [In, ComAliasName("UINT64")] ulong Value
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_Signal>(lpVtbl->Signal)(
                    This,
                    pFence,
                    Value
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Wait(
            [In] ID3D12Fence* pFence,
            [In, ComAliasName("UINT64")] ulong Value
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_Wait>(lpVtbl->Wait)(
                    This,
                    pFence,
                    Value
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetTimestampFrequency(
            [Out, ComAliasName("UINT64")] ulong* pFrequency
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_GetTimestampFrequency>(lpVtbl->GetTimestampFrequency)(
                    This,
                    pFrequency
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetClockCalibration(
            [Out, ComAliasName("UINT64")] ulong* pGpuTimestamp,
            [Out, ComAliasName("UINT64")] ulong* pCpuTimestamp
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                return MarshalFunction<_GetClockCalibration>(lpVtbl->GetClockCalibration)(
                    This,
                    pGpuTimestamp,
                    pCpuTimestamp
                );
            }
        }

        public void GetDesc(
            [Out] D3D12_COMMAND_QUEUE_DESC* pDesc
        )
        {
            fixed (ID3D12CommandQueue* This = &this)
            {
                MarshalFunction<_GetDesc>(lpVtbl->GetDesc)(
                    This,
                    pDesc
                );
            }
        }
        #endregion

        #region Structs
        public /* unmanaged */ struct Vtbl
        {
            #region IUnknown Fields
            public IntPtr QueryInterface;

            public IntPtr AddRef;

            public IntPtr Release;
            #endregion

            #region ID3D12Object Fields
            public IntPtr GetPrivateData;

            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr SetName;
            #endregion

            #region ID3D12DeviceChild Fields
            public IntPtr GetDevice;
            #endregion

            #region Fields
            public IntPtr UpdateTileMappings;

            public IntPtr CopyTileMappings;

            public IntPtr ExecuteCommandLists;

            public IntPtr SetMarker;

            public IntPtr BeginEvent;

            public IntPtr EndEvent;

            public IntPtr Signal;

            public IntPtr Wait;

            public IntPtr GetTimestampFrequency;

            public IntPtr GetClockCalibration;

            public IntPtr GetDesc;
            #endregion
        }
        #endregion
    }
}

