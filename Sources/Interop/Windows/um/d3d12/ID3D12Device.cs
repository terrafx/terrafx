// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("189819F1-1DB6-4B57-BE54-1821339B85F7")]
    public /* blittable */ unsafe struct ID3D12Device
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int QueryInterface(
            [In] ID3D12Device* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint AddRef(
            [In] ID3D12Device* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint Release(
            [In] ID3D12Device* This
        );
        #endregion

        #region ID3D12Object Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetPrivateData(
            [In] ID3D12Device* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrivateData(
            [In] ID3D12Device* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        );


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetPrivateDataInterface(
            [In] ID3D12Device* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetName(
            [In] ID3D12Device* This,
            [In, ComAliasName("LPCWSTR")] char* Name
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetNodeCount(
            [In] ID3D12Device* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCommandQueue(
            [In] ID3D12Device* This,
            [In] D3D12_COMMAND_QUEUE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandQueue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCommandAllocator(
            [In] ID3D12Device* This,
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandAllocator
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateGraphicsPipelineState(
            [In] ID3D12Device* This,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateComputePipelineState(
            [In] ID3D12Device* This,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCommandList(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In] ID3D12CommandAllocator* pCommandAllocator,
            [In, Optional] ID3D12PipelineState* pInitialState,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandList
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CheckFeatureSupport(
            [In] ID3D12Device* This,
            [In] D3D12_FEATURE Feature,
            [In, Out] void* pFeatureSupportData,
            [In, ComAliasName("UINT")] uint FeatureSupportDataSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateDescriptorHeap(
            [In] ID3D12Device* This,
            [In] D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint GetDescriptorHandleIncrementSize(
            [In] ID3D12Device* This,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateRootSignature(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] void* pBlobWithRootSignature,
            [In, ComAliasName("SIZE_T")] nuint blobLengthInBytes,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvRootSignature
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CreateConstantBufferView(
            [In] ID3D12Device* This,
            [In, Optional] D3D12_CONSTANT_BUFFER_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CreateShaderResourceView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_SHADER_RESOURCE_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CreateUnorderedAccessView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] ID3D12Resource* pCounterResource,
            [In, Optional] D3D12_UNORDERED_ACCESS_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CreateRenderTargetView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_RENDER_TARGET_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CreateDepthStencilView(
            [In] ID3D12Device* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_DEPTH_STENCIL_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CreateSampler(
            [In] ID3D12Device* This,
            [In] D3D12_SAMPLER_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyDescriptors(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NumDestDescriptorRanges,
            [In, ComAliasName("D3D12_CPU_DESCRIPTOR_HANDLE[]")] D3D12_CPU_DESCRIPTOR_HANDLE* pDestDescriptorRangeStarts,
            [In, Optional, ComAliasName("UINT[]")] uint* pDestDescriptorRangeSizes,
            [In, ComAliasName("UINT")] uint NumSrcDescriptorRanges,
            [In, ComAliasName("D3D12_CPU_DESCRIPTOR_HANDLE[]")] D3D12_CPU_DESCRIPTOR_HANDLE* pSrcDescriptorRangeStarts,
            [In, Optional, ComAliasName("UINT[]")] uint* pSrcDescriptorRangeSizes,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyDescriptorsSimple(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NumDescriptors,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptorRangeStart,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE SrcDescriptorRangeStart,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetResourceAllocationInfo(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint visibleMask,
            [In, ComAliasName("UINT")] uint numResourceDescs,
            [In] D3D12_RESOURCE_DESC* pResourceDescs,
            [Out] D3D12_RESOURCE_ALLOCATION_INFO* pResourceAllocationInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetCustomHeapProperties(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] D3D12_HEAP_TYPE heapType,
            [Out] D3D12_HEAP_PROPERTIES* pCustomHeapProperties
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCommittedResource(
            [In] ID3D12Device* This,
            [In] D3D12_HEAP_PROPERTIES* pHeapProperties,
            [In] D3D12_HEAP_FLAGS HeapFlags,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialResourceState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riidResource,
            [Out] void** ppvResource = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateHeap(
            [In] ID3D12Device* This,
            [In] D3D12_HEAP_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreatePlacedResource(
            [In] ID3D12Device* This,
            [In] ID3D12Heap* pHeap,
            [In, ComAliasName("UINT64")] ulong HeapOffset,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvResource = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateReservedResource(
            [In] ID3D12Device* This,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvResource = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateSharedHandle(
            [In] ID3D12Device* This,
            [In] ID3D12DeviceChild* pObject,
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In, ComAliasName("DWORD")] uint Access,
            [In, Optional, ComAliasName("LPCWSTR")] char* Name,
            [Out, ComAliasName("HANDLE")] IntPtr* pHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OpenSharedHandle(
            [In] ID3D12Device* This,
            [In, ComAliasName("HANDLE")] IntPtr NTHandle,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObj = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int OpenSharedHandleByName(
            [In] ID3D12Device* This,
            [In, ComAliasName("LPCWSTR")] char* Name,
            [In, ComAliasName("DWORD")] uint Access,
            [Out, ComAliasName("HANDLE")] IntPtr* pNTHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int MakeResident(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Evict(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateFence(
            [In] ID3D12Device* This,
            [In, ComAliasName("UINT64")] ulong InitialValue,
            [In] D3D12_FENCE_FLAGS Flags,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppFence
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDeviceRemovedReason(
            [In] ID3D12Device* This

        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetCopyableFootprints(
            [In] ID3D12Device* This,
            [In] D3D12_RESOURCE_DESC* pResourceDesc,
            [In, ComAliasName("UINT")] uint FirstSubresource,
            [In, ComAliasName("UINT")] uint NumSubresources,
            [In, ComAliasName("UINT64")] ulong BaseOffset,
            [Out, ComAliasName("D3D12_PLACED_SUBRESOURCE_FOOTPRINT[]")] D3D12_PLACED_SUBRESOURCE_FOOTPRINT* pLayouts = null,
            [Out, ComAliasName("UINT[]")] uint* pNumRows = null,
            [Out, ComAliasName("UINT64[]")] ulong* pRowSizeInBytes = null,
            [Out, ComAliasName("UINT64")] ulong* pTotalBytes = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateQueryHeap(
            [In] ID3D12Device* This,
            [In] D3D12_QUERY_HEAP_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetStablePowerState(
            [In] ID3D12Device* This,
            [In, ComAliasName("BOOL")] int Enable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int CreateCommandSignature(
            [In] ID3D12Device* This,
            [In] D3D12_COMMAND_SIGNATURE_DESC* pDesc,
            [In, Optional] ID3D12RootSignature* pRootSignature,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvCommandSignature = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetResourceTiling(
            [In] ID3D12Device* This,
            [In] ID3D12Resource* pTiledResource,
            [Out, Optional, ComAliasName("UINT")] uint* pNumTilesForEntireResource,
            [Out, Optional] D3D12_PACKED_MIP_INFO* pPackedMipDesc,
            [Out, Optional] D3D12_TILE_SHAPE* pStandardTileShapeForNonPackedMips,
            [In, Out, Optional, ComAliasName("UINT")] uint* pNumSubresourceTilings,
            [In, ComAliasName("UINT")] uint FirstSubresourceTilingToGet,
            [Out] D3D12_SUBRESOURCE_TILING* pSubresourceTilingsForNonPackedMips
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetAdapterLuid(
            [In] ID3D12Device* This,
            [Out] LUID* pAdapterLuid
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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

            #region Fields
            public IntPtr GetNodeCount;

            public IntPtr CreateCommandQueue;

            public IntPtr CreateCommandAllocator;

            public IntPtr CreateGraphicsPipelineState;

            public IntPtr CreateComputePipelineState;

            public IntPtr CreateCommandList;

            public IntPtr CheckFeatureSupport;

            public IntPtr CreateDescriptorHeap;

            public IntPtr GetDescriptorHandleIncrementSize;

            public IntPtr CreateRootSignature;

            public IntPtr CreateConstantBufferView;

            public IntPtr CreateShaderResourceView;

            public IntPtr CreateUnorderedAccessView;

            public IntPtr CreateRenderTargetView;

            public IntPtr CreateDepthStencilView;

            public IntPtr CreateSampler;

            public IntPtr CopyDescriptors;

            public IntPtr CopyDescriptorsSimple;

            public IntPtr GetResourceAllocationInfo;

            public IntPtr GetCustomHeapProperties;

            public IntPtr CreateCommittedResource;

            public IntPtr CreateHeap;

            public IntPtr CreatePlacedResource;

            public IntPtr CreateReservedResource;

            public IntPtr CreateSharedHandle;

            public IntPtr OpenSharedHandle;

            public IntPtr OpenSharedHandleByName;

            public IntPtr MakeResident;

            public IntPtr Evict;

            public IntPtr CreateFence;

            public IntPtr GetDeviceRemovedReason;

            public IntPtr GetCopyableFootprints;

            public IntPtr CreateQueryHeap;

            public IntPtr SetStablePowerState;

            public IntPtr CreateCommandSignature;

            public IntPtr GetResourceTiling;

            public IntPtr GetAdapterLuid;
            #endregion
        }
        #endregion
    }
}
