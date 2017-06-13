// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("189819F1-1DB6-4B57-BE54-1821339B85F7")]
    unsafe public struct ID3D12Device
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Device).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate uint GetNodeCount(
            [In] ID3D12Device* This
        );

        public /* static */ delegate HRESULT CreateCommandQueue(
            [In] ID3D12Device* This,
            [In] D3D12_COMMAND_QUEUE_DESC* pDesc,
            [In] Guid* riid,
            [Out] void** ppCommandQueue
        );

        public /* static */ delegate HRESULT CreateCommandAllocator(
            [In] ID3D12Device* This,
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In] Guid* riid,
            [Out] void** ppCommandAllocator
        );

        public /* static */ delegate HRESULT CreateGraphicsPipelineState(
            [In] ID3D12Device* This,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In] Guid* riid,
            [Out] void** ppPipelineState
        );

        public /* static */ delegate HRESULT CreateComputePipelineState(
            [In] ID3D12Device* This,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In] Guid* riid,
            [Out] void** ppPipelineState
        );

        public /* static */ delegate HRESULT CreateCommandList(
            [In] ID3D12Device* This,
            [In] uint NodeMask,
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In] ID3D12CommandAllocator* pCommandAllocator,
            [In, Optional] ID3D12PipelineState* pInitialState,
            [In] Guid* riid,
            [Out] void** ppCommandList
        );

        public /* static */ delegate HRESULT CheckFeatureSupport(
            [In] ID3D12Device* This,
            [In] D3D12_FEATURE Feature,
            [In, Out] void* pFeatureSupportData,
            [In] uint FeatureSupportDataSize
        );

        public /* static */ delegate HRESULT CreateDescriptorHeap(
            [In] ID3D12Device* This,
            [In] D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,
            [In] Guid* riid,
            [Out] void** ppvHeap
        );

        public /* static */ delegate uint GetDescriptorHandleIncrementSize(
            [In] ID3D12Device* This,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapType
        );

        public /* static */ delegate HRESULT CreateRootSignature(
            [In] ID3D12Device* This,
            [In] uint NodeMask,
            [In] void* pBlobWithRootSignature,
            [In] UIntPtr blobLengthInBytes,
            [In] Guid* riid,
            [Out] void** ppvRootSignature
        );

        public /* static */ delegate void CreateConstantBufferView(
            [In] ID3D12Device* This,
            [In, Optional] D3D12_CONSTANT_BUFFER_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        public /* static */ delegate void CreateShaderResourceView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_SHADER_RESOURCE_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        public /* static */ delegate void CreateUnorderedAccessView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] ID3D12Resource* pCounterResource,
            [In, Optional] D3D12_UNORDERED_ACCESS_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        public /* static */ delegate void CreateRenderTargetView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_RENDER_TARGET_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        public /* static */ delegate void CreateDepthStencilView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_DEPTH_STENCIL_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        public /* static */ delegate void CreateSampler(
            [In] ID3D12Device* This,
            [In] D3D12_SAMPLER_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        public /* static */ delegate void CopyDescriptors(
            [In] ID3D12Device* This,
            [In] uint NumDestDescriptorRanges,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE* pDestDescriptorRangeStarts,
            [In, Optional] uint* pDestDescriptorRangeSizes,
            [In] uint NumSrcDescriptorRanges,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE* pSrcDescriptorRangeStarts,
            [In, Optional] uint* pSrcDescriptorRangeSizes,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        );

        public /* static */ delegate void CopyDescriptorsSimple(
            [In] ID3D12Device* This,
            [In] uint NumDescriptors,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptorRangeStart,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE SrcDescriptorRangeStart,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        );

        public /* static */ delegate D3D12_RESOURCE_ALLOCATION_INFO GetResourceAllocationInfo(
            [In] ID3D12Device* This,
            [In] uint visibleMask,
            [In] uint numResourceDescs,
            [In] D3D12_RESOURCE_DESC* pResourceDescs
        );

        public /* static */ delegate D3D12_HEAP_PROPERTIES GetCustomHeapProperties(
            [In] ID3D12Device* This,
            [In] uint NodeMask,
            [In] D3D12_HEAP_TYPE heapType
        );

        public /* static */ delegate HRESULT CreateCommittedResource(
            [In] ID3D12Device* This,
            [In] D3D12_HEAP_PROPERTIES* pHeapProperties,
            [In] D3D12_HEAP_FLAGS HeapFlags,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialResourceState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In] Guid* riidResource,
            [Out, Optional] void** ppvResource
        );

        public /* static */ delegate HRESULT CreateHeap(
            [In] ID3D12Device* This,
            [In] D3D12_HEAP_DESC* pDesc,
            [In] Guid* riid,
            [Out, Optional] void** ppvHeap
        );

        public /* static */ delegate HRESULT CreatePlacedResource(
            [In] ID3D12Device* This,
            [In] ID3D12Heap* pHeap,
            [In] ulong HeapOffset,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In] Guid* riid,
            [Out, Optional] void** ppvResource
        );

        public /* static */ delegate HRESULT CreateReservedResource(
            [In] ID3D12Device* This,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In] Guid* riid,
            [Out, Optional] void** ppvResource
        );

        public /* static */ delegate HRESULT CreateSharedHandle(
            [In] ID3D12Device* This,
            [In] ID3D12DeviceChild* pObject,
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In] uint Access,
            [In, Optional] LPWSTR Name,
            [Out] HANDLE* pHandle
        );

        public /* static */ delegate HRESULT OpenSharedHandle(
            [In] ID3D12Device* This,
            [In] HANDLE NTHandle,
            [In] Guid* riid,
            [Out, Optional] void** ppvObj
        );

        public /* static */ delegate HRESULT OpenSharedHandleByName(
            [In] ID3D12Device* This,
            [In] LPWSTR Name,
            [In] uint Access,
            [Out] HANDLE* pNTHandle
        );

        public /* static */ delegate HRESULT MakeResident(
            [In] ID3D12Device* This,
            [In] uint NumObjects,
            [In] ID3D12Pageable** ppObjects
        );

        public /* static */ delegate HRESULT Evict(
            [In] ID3D12Device* This,
            [In] uint NumObjects,
            [In] ID3D12Pageable** ppObjects
        );

        public /* static */ delegate HRESULT CreateFence(
            [In] ID3D12Device* This,
            [In] ulong InitialValue,
            [In] D3D12_FENCE_FLAGS Flags,
            [In] Guid* riid,
            [Out] void** ppFence
        );

        public /* static */ delegate HRESULT GetDeviceRemovedReason(
            [In] ID3D12Device* This
        
        );

        public /* static */ delegate void GetCopyableFootprints(
            [In] ID3D12Device* This,
            [In] D3D12_RESOURCE_DESC* pResourceDesc,
            [In] uint FirstSubresource,
            [In] uint NumSubresources,
            [In] ulong BaseOffset,
            [Out, Optional] D3D12_PLACED_SUBRESOURCE_FOOTPRINT* pLayouts,
            [Out, Optional] uint* pNumRows,
            [Out, Optional] ulong* pRowSizeInBytes,
            [Out, Optional] ulong* pTotalBytes
        );

        public /* static */ delegate HRESULT CreateQueryHeap(
            [In] ID3D12Device* This,
            [In] D3D12_QUERY_HEAP_DESC* pDesc,
            [In] Guid* riid,
            [Out, Optional] void** ppvHeap
        );

        public /* static */ delegate HRESULT SetStablePowerState(
            [In] ID3D12Device* This,
            BOOL Enable
        );

        public /* static */ delegate HRESULT CreateCommandSignature(
            [In] ID3D12Device* This,
            [In] D3D12_COMMAND_SIGNATURE_DESC* pDesc,
            [In, Optional] ID3D12RootSignature* pRootSignature,
            [In] Guid* riid,
            [Out, Optional] void** ppvCommandSignature
        );

        public /* static */ delegate void GetResourceTiling(
            [In] ID3D12Device* This,
            [In] ID3D12Resource* pTiledResource,
            [Out, Optional] uint* pNumTilesForEntireResource,
            [Out, Optional] D3D12_PACKED_MIP_INFO* pPackedMipDesc,
            [Out, Optional] D3D12_TILE_SHAPE* pStandardTileShapeForNonPackedMips,
            [In, Out, Optional] uint* pNumSubresourceTilings,
            [In] uint FirstSubresourceTilingToGet,
            [Out] D3D12_SUBRESOURCE_TILING* pSubresourceTilingsForNonPackedMips
        );

        public /* static */ delegate LUID GetAdapterLuid(
            [In] ID3D12Device* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Object.Vtbl BaseVtbl;

            public GetNodeCount GetNodeCount;

            public CreateCommandQueue CreateCommandQueue;

            public CreateCommandAllocator CreateCommandAllocator;

            public CreateGraphicsPipelineState CreateGraphicsPipelineState;

            public CreateComputePipelineState CreateComputePipelineState;

            public CreateCommandList CreateCommandList;

            public CheckFeatureSupport CheckFeatureSupport;

            public CreateDescriptorHeap CreateDescriptorHeap;

            public GetDescriptorHandleIncrementSize GetDescriptorHandleIncrementSize;

            public CreateRootSignature CreateRootSignature;

            public CreateConstantBufferView CreateConstantBufferView;

            public CreateShaderResourceView CreateShaderResourceView;

            public CreateUnorderedAccessView CreateUnorderedAccessView;

            public CreateRenderTargetView CreateRenderTargetView;

            public CreateDepthStencilView CreateDepthStencilView;

            public CreateSampler CreateSampler;

            public CopyDescriptors CopyDescriptors;

            public CopyDescriptorsSimple CopyDescriptorsSimple;

            public GetResourceAllocationInfo GetResourceAllocationInfo;

            public GetCustomHeapProperties GetCustomHeapProperties;

            public CreateCommittedResource CreateCommittedResource;

            public CreateHeap CreateHeap;

            public CreatePlacedResource CreatePlacedResource;

            public CreateReservedResource CreateReservedResource;

            public CreateSharedHandle CreateSharedHandle;

            public OpenSharedHandle OpenSharedHandle;

            public OpenSharedHandleByName OpenSharedHandleByName;

            public MakeResident MakeResident;

            public Evict Evict;

            public CreateFence CreateFence;

            public GetDeviceRemovedReason GetDeviceRemovedReason;

            public GetCopyableFootprints GetCopyableFootprints;

            public CreateQueryHeap CreateQueryHeap;

            public SetStablePowerState SetStablePowerState;

            public CreateCommandSignature CreateCommandSignature;

            public GetResourceTiling GetResourceTiling;

            public GetAdapterLuid GetAdapterLuid;
            #endregion
        }
        #endregion
    }
}
