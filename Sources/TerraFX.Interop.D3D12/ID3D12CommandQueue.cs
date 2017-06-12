// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("0EC870A6-5D7E-4C22-8CFC-5BAAE07616ED")]
    unsafe public struct ID3D12CommandQueue
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12CommandQueue).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void UpdateTileMappings(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Resource* pResource,
            [In] uint NumResourceRegions,
            [In, Optional] D3D12_TILED_RESOURCE_COORDINATE* pResourceRegionStartCoordinates,
            [In, Optional] D3D12_TILE_REGION_SIZE* pResourceRegionSizes,
            [In, Optional] ID3D12Heap* pHeap,
            [In] uint NumRanges,
            [In, Optional] D3D12_TILE_RANGE_FLAGS* pRangeFlags,
            [In, Optional] uint* pHeapRangeStartOffsets,
            [In, Optional] uint* pRangeTileCounts,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        );

        public /* static */ delegate void CopyTileMappings(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Resource* pDstResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pDstRegionStartCoordinate,
            [In] ID3D12Resource* pSrcResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pSrcRegionStartCoordinate,
            [In] D3D12_TILE_REGION_SIZE* pRegionSize,
            [In] D3D12_TILE_MAPPING_FLAGS Flags
        );

        public /* static */ delegate void ExecuteCommandLists(
            [In] ID3D12CommandQueue* This,
            [In] uint NumCommandLists,
            [In] ID3D12CommandList** ppCommandLists
        );

        public /* static */ delegate void SetMarker(
            [In] ID3D12CommandQueue* This,
            [In] uint Metadata,
            [In, Optional] void* pData,
            [In] uint Size
        );

        public /* static */ delegate void BeginEvent(
            [In] ID3D12CommandQueue* This,
            [In] uint Metadata,
            [In, Optional] void* pData,
            [In] uint Size
        );

        public /* static */ delegate void EndEvent(
            [In] ID3D12CommandQueue* This
        );

        public /* static */ delegate HRESULT Signal(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Fence* pFence,
            [In] ulong Value
        );

        public /* static */ delegate HRESULT Wait(
            [In] ID3D12CommandQueue* This,
            [In] ID3D12Fence* pFence,
            [In] ulong Value
        );

        public /* static */ delegate HRESULT GetTimestampFrequency(
            [In] ID3D12CommandQueue* This,
            [Out] ulong* pFrequency
        );

        public /* static */ delegate HRESULT GetClockCalibration(
            [In] ID3D12CommandQueue* This,
            [Out] ulong* pGpuTimestamp,
            [Out] ulong* pCpuTimestamp
        );

        public /* static */ delegate D3D12_COMMAND_QUEUE_DESC GetDesc(
            [In] ID3D12CommandQueue* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public UpdateTileMappings UpdateTileMappings;

            public CopyTileMappings CopyTileMappings;

            public ExecuteCommandLists ExecuteCommandLists;

            public SetMarker SetMarker;

            public BeginEvent BeginEvent;

            public EndEvent EndEvent;

            public Signal Signal;

            public Wait Wait;

            public GetTimestampFrequency GetTimestampFrequency;

            public GetClockCalibration GetClockCalibration;

            public GetDesc GetDesc;
            #endregion
        }
        #endregion
    }
}
