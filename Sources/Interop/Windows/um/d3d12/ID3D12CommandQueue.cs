// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0EC870A6-5D7E-4C22-8CFC-5BAAE07616ED")]
    unsafe public /* blittable */ struct ID3D12CommandQueue
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void UpdateTileMappings(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Resource* pResource,
            [In, ComAliasName("UINT")] uint NumResourceRegions,
            [In, Optional] /* readonly */ D3D12_TILED_RESOURCE_COORDINATE* pResourceRegionStartCoordinates,
            [In, Optional] /* readonly */ D3D12_TILE_REGION_SIZE* pResourceRegionSizes,
            [In, Optional] ID3D12Heap* pHeap,
            [In, ComAliasName("UINT")] uint NumRanges,
            [In, Optional] /* readonly */ D3D12_TILE_RANGE_FLAGS* pRangeFlags,
            [In, Optional, ComAliasName("UINT")] /* readonly */ uint* pHeapRangeStartOffsets,
            [In, Optional, ComAliasName("UINT")] /* readonly */ uint* pRangeTileCounts,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyTileMappings(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Resource* pDstResource,
            [In] /* readonly */ D3D12_TILED_RESOURCE_COORDINATE* pDstRegionStartCoordinate,
            [In] ID3D12Resource* pSrcResource,
            [In] /* readonly */ D3D12_TILED_RESOURCE_COORDINATE* pSrcRegionStartCoordinate,
            [In] /* readonly */ D3D12_TILE_REGION_SIZE* pRegionSize,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ExecuteCommandLists(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("UINT")] uint NumCommandLists,
            [In] /* readonly */ ID3D12CommandList** ppCommandLists
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMarker(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] /* readonly */ void* pData,
            [In, ComAliasName("UINT")] uint Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void BeginEvent(
            [In] ID3D12CommandQueue* This,
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] /* readonly */ void* pData,
            [In, ComAliasName("UINT")] uint Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void EndEvent(
            [In] ID3D12CommandQueue* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Signal(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Fence* pFence,
            [In, ComAliasName("UINT64")] ulong Value
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Wait(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Fence* pFence,
            [In, ComAliasName("UINT64")] ulong Value
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetTimestampFrequency(
            [In] ID3D12CommandQueue* This,
            [Out, ComAliasName("UINT64")] ulong* pFrequency
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetClockCalibration(
            [In] ID3D12CommandQueue* This,
            [Out, ComAliasName("UINT64")] ulong* pGpuTimestamp,
            [Out, ComAliasName("UINT64")] ulong* pCpuTimestamp
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D3D12_COMMAND_QUEUE_DESC GetDesc(
            [In] ID3D12CommandQueue* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

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
