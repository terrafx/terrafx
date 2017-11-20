// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;
using static TerraFX.Utilities.InteropUtilities;

namespace TerraFX.Interop
{
    [Guid("77ACCE80-638E-4E65-8895-C1F23386863E")]
    public /* blittable */ unsafe struct ID3D12Device1
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region IUnknown Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _QueryInterface(
            [In] ID3D12Device1* This,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _AddRef(
            [In] ID3D12Device1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("ULONG")]
        public /* static */ delegate uint _Release(
            [In] ID3D12Device1* This
        );
        #endregion

        #region ID3D12Object Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetPrivateData(
            [In] ID3D12Device1* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, Out, ComAliasName("UINT")] uint* pDataSize,
            [Out] void* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateData(
            [In] ID3D12Device1* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In, ComAliasName("UINT")] uint DataSize,
            [In] void* pData = null
        );


        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetPrivateDataInterface(
            [In] ID3D12Device1* This,
            [In, ComAliasName("REFGUID")] Guid* guid,
            [In] IUnknown* pData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetName(
            [In] ID3D12Device1* This,
            [In, ComAliasName("LPCWSTR")] char* Name
        );
        #endregion

        #region ID3D12Device Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetNodeCount(
            [In] ID3D12Device1* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateCommandQueue(
            [In] ID3D12Device1* This,
            [In] D3D12_COMMAND_QUEUE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandQueue
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateCommandAllocator(
            [In] ID3D12Device1* This,
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandAllocator
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateGraphicsPipelineState(
            [In] ID3D12Device1* This,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateComputePipelineState(
            [In] ID3D12Device1* This,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateCommandList(
            [In] ID3D12Device1* This,
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
        public /* static */ delegate int _CheckFeatureSupport(
            [In] ID3D12Device1* This,
            [In] D3D12_FEATURE Feature,
            [In, Out] void* pFeatureSupportData,
            [In, ComAliasName("UINT")] uint FeatureSupportDataSize
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateDescriptorHeap(
            [In] ID3D12Device1* This,
            [In] D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT")]
        public /* static */ delegate uint _GetDescriptorHandleIncrementSize(
            [In] ID3D12Device1* This,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateRootSignature(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] void* pBlobWithRootSignature,
            [In, ComAliasName("SIZE_T")] nuint blobLengthInBytes,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvRootSignature
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CreateConstantBufferView(
            [In] ID3D12Device1* This,
            [In, Optional] D3D12_CONSTANT_BUFFER_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CreateShaderResourceView(
            [In] ID3D12Device1* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_SHADER_RESOURCE_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CreateUnorderedAccessView(
            [In] ID3D12Device1* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] ID3D12Resource* pCounterResource,
            [In, Optional] D3D12_UNORDERED_ACCESS_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CreateRenderTargetView(
            [In] ID3D12Device1* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_RENDER_TARGET_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CreateDepthStencilView(
            [In] ID3D12Device1* This,
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_DEPTH_STENCIL_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CreateSampler(
            [In] ID3D12Device1* This,
            [In] D3D12_SAMPLER_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _CopyDescriptors(
            [In] ID3D12Device1* This,
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
        public /* static */ delegate void _CopyDescriptorsSimple(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint NumDescriptors,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptorRangeStart,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE SrcDescriptorRangeStart,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetResourceAllocationInfo(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint visibleMask,
            [In, ComAliasName("UINT")] uint numResourceDescs,
            [In] D3D12_RESOURCE_DESC* pResourceDescs,
            [Out] D3D12_RESOURCE_ALLOCATION_INFO* pResourceAllocationInfo
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetCustomHeapProperties(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] D3D12_HEAP_TYPE heapType,
            [Out] D3D12_HEAP_PROPERTIES* pCustomHeapProperties
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateCommittedResource(
            [In] ID3D12Device1* This,
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
        public /* static */ delegate int _CreateHeap(
            [In] ID3D12Device1* This,
            [In] D3D12_HEAP_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreatePlacedResource(
            [In] ID3D12Device1* This,
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
        public /* static */ delegate int _CreateReservedResource(
            [In] ID3D12Device1* This,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvResource = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateSharedHandle(
            [In] ID3D12Device1* This,
            [In] ID3D12DeviceChild* pObject,
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In, ComAliasName("DWORD")] uint Access,
            [In, Optional, ComAliasName("LPCWSTR")] char* Name,
            [Out, ComAliasName("HANDLE")] IntPtr* pHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _OpenSharedHandle(
            [In] ID3D12Device1* This,
            [In, ComAliasName("HANDLE")] IntPtr NTHandle,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObj = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _OpenSharedHandleByName(
            [In] ID3D12Device1* This,
            [In, ComAliasName("LPCWSTR")] char* Name,
            [In, ComAliasName("DWORD")] uint Access,
            [Out, ComAliasName("HANDLE")] IntPtr* pNTHandle
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _MakeResident(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _Evict(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateFence(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT64")] ulong InitialValue,
            [In] D3D12_FENCE_FLAGS Flags,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppFence
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _GetDeviceRemovedReason(
            [In] ID3D12Device1* This

        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetCopyableFootprints(
            [In] ID3D12Device1* This,
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
        public /* static */ delegate int _CreateQueryHeap(
            [In] ID3D12Device1* This,
            [In] D3D12_QUERY_HEAP_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetStablePowerState(
            [In] ID3D12Device1* This,
            [In, ComAliasName("BOOL")] int Enable
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreateCommandSignature(
            [In] ID3D12Device1* This,
            [In] D3D12_COMMAND_SIGNATURE_DESC* pDesc,
            [In, Optional] ID3D12RootSignature* pRootSignature,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvCommandSignature = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void _GetResourceTiling(
            [In] ID3D12Device1* This,
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
        public /* static */ delegate void _GetAdapterLuid(
            [In] ID3D12Device1* This,
            [Out] LUID* pAdapterLuid
        );
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _CreatePipelineLibrary(
            [In] ID3D12Device1* This,
            [In] void* pLibraryBlob,
            [In, ComAliasName("SIZE_T")] nuint BlobLength,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineLibrary
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetEventOnMultipleFenceCompletion(
            [In] ID3D12Device1* This,
            [In, ComAliasName("ID3D12Fence*[]")] ID3D12Fence** ppFences,
            [In, ComAliasName("UINT64[]")] ulong* pFenceValues,
            [In, ComAliasName("UINT")] uint NumFences,
            [In] D3D12_MULTIPLE_FENCE_WAIT_FLAGS Flags,
            [In, ComAliasName("HANDLE")] IntPtr hEvent
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int _SetResidencyPriority(
            [In] ID3D12Device1* This,
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects,
            [In, ComAliasName("D3D12_RESIDENCY_PRIORITY[]")] D3D12_RESIDENCY_PRIORITY* pPriorities
        );
        #endregion

        #region IUnknown Methods
        [return: ComAliasName("HRESULT")]
        public int QueryInterface(
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObject
        )
        {
            fixed (ID3D12Device1* This = &this)
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
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_AddRef>(lpVtbl->AddRef)(
                    This
                );
            }
        }

        [return: ComAliasName("ULONG")]
        public uint Release()
        {
            fixed (ID3D12Device1* This = &this)
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
            fixed (ID3D12Device1* This = &this)
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
            fixed (ID3D12Device1* This = &this)
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
            fixed (ID3D12Device1* This = &this)
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
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_SetName>(lpVtbl->SetName)(
                    This,
                    Name
                );
            }
        }
        #endregion

        #region ID3D12Device Methods
        [return: ComAliasName("UINT")]
        public uint GetNodeCount()
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_GetNodeCount>(lpVtbl->GetNodeCount)(
                    This
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateCommandQueue(
            [In] D3D12_COMMAND_QUEUE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandQueue
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateCommandQueue>(lpVtbl->CreateCommandQueue)(
                    This,
                    pDesc,
                    riid,
                    ppCommandQueue
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateCommandAllocator(
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandAllocator
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateCommandAllocator>(lpVtbl->CreateCommandAllocator)(
                    This,
                    Type,
                    riid,
                    ppCommandAllocator
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateGraphicsPipelineState(
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateGraphicsPipelineState>(lpVtbl->CreateGraphicsPipelineState)(
                    This,
                    pDesc,
                    riid,
                    ppPipelineState
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateComputePipelineState(
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateComputePipelineState>(lpVtbl->CreateComputePipelineState)(
                    This,
                    pDesc,
                    riid,
                    ppPipelineState
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateCommandList(
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] D3D12_COMMAND_LIST_TYPE Type,
            [In] ID3D12CommandAllocator* pCommandAllocator,
            [In, Optional] ID3D12PipelineState* pInitialState,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppCommandList
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateCommandList>(lpVtbl->CreateCommandList)(
                    This,
                    NodeMask,
                    Type,
                    pCommandAllocator,
                    pInitialState,
                    riid,
                    ppCommandList
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CheckFeatureSupport(
            [In] D3D12_FEATURE Feature,
            [In, Out] void* pFeatureSupportData,
            [In, ComAliasName("UINT")] uint FeatureSupportDataSize
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CheckFeatureSupport>(lpVtbl->CheckFeatureSupport)(
                    This,
                    Feature,
                    pFeatureSupportData,
                    FeatureSupportDataSize
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateDescriptorHeap(
            [In] D3D12_DESCRIPTOR_HEAP_DESC* pDescriptorHeapDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateDescriptorHeap>(lpVtbl->CreateDescriptorHeap)(
                    This,
                    pDescriptorHeapDesc,
                    riid,
                    ppvHeap
                );
            }
        }

        [return: ComAliasName("UINT")]
        public uint GetDescriptorHandleIncrementSize(
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapType
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_GetDescriptorHandleIncrementSize>(lpVtbl->GetDescriptorHandleIncrementSize)(
                    This,
                    DescriptorHeapType
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateRootSignature(
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] void* pBlobWithRootSignature,
            [In, ComAliasName("SIZE_T")] nuint blobLengthInBytes,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvRootSignature
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateRootSignature>(lpVtbl->CreateRootSignature)(
                    This,
                    NodeMask,
                    pBlobWithRootSignature,
                    blobLengthInBytes,
                    riid,
                    ppvRootSignature
                );
            }
        }

        public void CreateConstantBufferView(
            [In, Optional] D3D12_CONSTANT_BUFFER_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CreateConstantBufferView>(lpVtbl->CreateConstantBufferView)(
                    This,
                    pDesc,
                    DestDescriptor
                );
            }
        }

        public void CreateShaderResourceView(
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_SHADER_RESOURCE_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CreateShaderResourceView>(lpVtbl->CreateShaderResourceView)(
                    This,
                    pResource,
                    pDesc,
                    DestDescriptor
                );
            }
        }

        public void CreateUnorderedAccessView(
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] ID3D12Resource* pCounterResource,
            [In, Optional] D3D12_UNORDERED_ACCESS_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CreateUnorderedAccessView>(lpVtbl->CreateUnorderedAccessView)(
                    This,
                    pResource,
                    pCounterResource,
                    pDesc,
                    DestDescriptor
                );
            }
        }

        public void CreateRenderTargetView(
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_RENDER_TARGET_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CreateRenderTargetView>(lpVtbl->CreateRenderTargetView)(
                    This,
                    pResource,
                    pDesc,
                    DestDescriptor
                );
            }
        }

        public void CreateDepthStencilView(
            [In, Optional] ID3D12Resource* pResource,
            [In, Optional] D3D12_DEPTH_STENCIL_VIEW_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CreateDepthStencilView>(lpVtbl->CreateDepthStencilView)(
                    This,
                    pResource,
                    pDesc,
                    DestDescriptor
                );
            }
        }

        public void CreateSampler(
            [In] D3D12_SAMPLER_DESC* pDesc,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptor
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CreateSampler>(lpVtbl->CreateSampler)(
                    This,
                    pDesc,
                    DestDescriptor
                );
            }
        }

        public void CopyDescriptors(
            [In, ComAliasName("UINT")] uint NumDestDescriptorRanges,
            [In, ComAliasName("D3D12_CPU_DESCRIPTOR_HANDLE[]")] D3D12_CPU_DESCRIPTOR_HANDLE* pDestDescriptorRangeStarts,
            [In, Optional, ComAliasName("UINT[]")] uint* pDestDescriptorRangeSizes,
            [In, ComAliasName("UINT")] uint NumSrcDescriptorRanges,
            [In, ComAliasName("D3D12_CPU_DESCRIPTOR_HANDLE[]")] D3D12_CPU_DESCRIPTOR_HANDLE* pSrcDescriptorRangeStarts,
            [In, Optional, ComAliasName("UINT[]")] uint* pSrcDescriptorRangeSizes,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CopyDescriptors>(lpVtbl->CopyDescriptors)(
                    This,
                    NumDestDescriptorRanges,
                    pDestDescriptorRangeStarts,
                    pDestDescriptorRangeSizes,
                    NumSrcDescriptorRanges,
                    pSrcDescriptorRangeStarts,
                    pSrcDescriptorRangeSizes,
                    DescriptorHeapsType
                );
            }
        }

        public void CopyDescriptorsSimple(
            [In, ComAliasName("UINT")] uint NumDescriptors,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DestDescriptorRangeStart,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE SrcDescriptorRangeStart,
            [In] D3D12_DESCRIPTOR_HEAP_TYPE DescriptorHeapsType
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_CopyDescriptorsSimple>(lpVtbl->CopyDescriptorsSimple)(
                    This,
                    NumDescriptors,
                    DestDescriptorRangeStart,
                    SrcDescriptorRangeStart,
                    DescriptorHeapsType
                );
            }
        }

        public void GetResourceAllocationInfo(
            [In, ComAliasName("UINT")] uint visibleMask,
            [In, ComAliasName("UINT")] uint numResourceDescs,
            [In] D3D12_RESOURCE_DESC* pResourceDescs,
            [Out] D3D12_RESOURCE_ALLOCATION_INFO* pResourceAllocationInfo
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_GetResourceAllocationInfo>(lpVtbl->GetResourceAllocationInfo)(
                    This,
                    visibleMask,
                    numResourceDescs,
                    pResourceDescs,
                    pResourceAllocationInfo
                );
            }
        }

        public void GetCustomHeapProperties(
            [In, ComAliasName("UINT")] uint NodeMask,
            [In] D3D12_HEAP_TYPE heapType,
            [Out] D3D12_HEAP_PROPERTIES* pCustomHeapProperties
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_GetCustomHeapProperties>(lpVtbl->GetCustomHeapProperties)(
                    This,
                    NodeMask,
                    heapType,
                    pCustomHeapProperties
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateCommittedResource(
            [In] D3D12_HEAP_PROPERTIES* pHeapProperties,
            [In] D3D12_HEAP_FLAGS HeapFlags,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialResourceState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riidResource,
            [Out] void** ppvResource = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateCommittedResource>(lpVtbl->CreateCommittedResource)(
                    This,
                    pHeapProperties,
                    HeapFlags,
                    pDesc,
                    InitialResourceState,
                    pOptimizedClearValue,
                    riidResource,
                    ppvResource
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateHeap(
            [In] D3D12_HEAP_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateHeap>(lpVtbl->CreateHeap)(
                    This,
                    pDesc,
                    riid,
                    ppvHeap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreatePlacedResource(
            [In] ID3D12Heap* pHeap,
            [In, ComAliasName("UINT64")] ulong HeapOffset,
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvResource = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreatePlacedResource>(lpVtbl->CreatePlacedResource)(
                    This,
                    pHeap,
                    HeapOffset,
                    pDesc,
                    InitialState,
                    pOptimizedClearValue,
                    riid,
                    ppvResource
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateReservedResource(
            [In] D3D12_RESOURCE_DESC* pDesc,
            [In] D3D12_RESOURCE_STATES InitialState,
            [In, Optional] D3D12_CLEAR_VALUE* pOptimizedClearValue,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvResource = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateReservedResource>(lpVtbl->CreateReservedResource)(
                    This,
                    pDesc,
                    InitialState,
                    pOptimizedClearValue,
                    riid,
                    ppvResource
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateSharedHandle(
            [In] ID3D12DeviceChild* pObject,
            [In, Optional] SECURITY_ATTRIBUTES* pAttributes,
            [In, ComAliasName("DWORD")] uint Access,
            [In, Optional, ComAliasName("LPCWSTR")] char* Name,
            [Out, ComAliasName("HANDLE")] IntPtr* pHandle
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateSharedHandle>(lpVtbl->CreateSharedHandle)(
                    This,
                    pObject,
                    pAttributes,
                    Access,
                    Name,
                    pHandle
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int OpenSharedHandle(
            [In, ComAliasName("HANDLE")] IntPtr NTHandle,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvObj = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_OpenSharedHandle>(lpVtbl->OpenSharedHandle)(
                    This,
                    NTHandle,
                    riid,
                    ppvObj
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int OpenSharedHandleByName(
            [In, ComAliasName("LPCWSTR")] char* Name,
            [In, ComAliasName("DWORD")] uint Access,
            [Out, ComAliasName("HANDLE")] IntPtr* pNTHandle
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_OpenSharedHandleByName>(lpVtbl->OpenSharedHandleByName)(
                    This,
                    Name,
                    Access,
                    pNTHandle
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int MakeResident(
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_MakeResident>(lpVtbl->MakeResident)(
                    This,
                    NumObjects,
                    ppObjects
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int Evict(
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_Evict>(lpVtbl->Evict)(
                    This,
                    NumObjects,
                    ppObjects
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateFence(
            [In, ComAliasName("UINT64")] ulong InitialValue,
            [In] D3D12_FENCE_FLAGS Flags,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppFence
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateFence>(lpVtbl->CreateFence)(
                    This,
                    InitialValue,
                    Flags,
                    riid,
                    ppFence
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int GetDeviceRemovedReason()
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_GetDeviceRemovedReason>(lpVtbl->GetDeviceRemovedReason)(
                    This
                );
            }
        }

        public void GetCopyableFootprints(
            [In] D3D12_RESOURCE_DESC* pResourceDesc,
            [In, ComAliasName("UINT")] uint FirstSubresource,
            [In, ComAliasName("UINT")] uint NumSubresources,
            [In, ComAliasName("UINT64")] ulong BaseOffset,
            [Out, ComAliasName("D3D12_PLACED_SUBRESOURCE_FOOTPRINT[]")] D3D12_PLACED_SUBRESOURCE_FOOTPRINT* pLayouts = null,
            [Out, ComAliasName("UINT[]")] uint* pNumRows = null,
            [Out, ComAliasName("UINT64[]")] ulong* pRowSizeInBytes = null,
            [Out, ComAliasName("UINT64")] ulong* pTotalBytes = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_GetCopyableFootprints>(lpVtbl->GetCopyableFootprints)(
                    This,
                    pResourceDesc,
                    FirstSubresource,
                    NumSubresources,
                    BaseOffset,
                    pLayouts,
                    pNumRows,
                    pRowSizeInBytes,
                    pTotalBytes
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateQueryHeap(
            [In] D3D12_QUERY_HEAP_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvHeap = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateQueryHeap>(lpVtbl->CreateQueryHeap)(
                    This,
                    pDesc,
                    riid,
                    ppvHeap
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetStablePowerState(
            [In, ComAliasName("BOOL")] int Enable
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_SetStablePowerState>(lpVtbl->SetStablePowerState)(
                    This,
                    Enable
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int CreateCommandSignature(
            [In] D3D12_COMMAND_SIGNATURE_DESC* pDesc,
            [In, Optional] ID3D12RootSignature* pRootSignature,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppvCommandSignature = null
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreateCommandSignature>(lpVtbl->CreateCommandSignature)(
                    This,
                    pDesc,
                    pRootSignature,
                    riid,
                    ppvCommandSignature
                );
            }
        }

        public void GetResourceTiling(
            [In] ID3D12Resource* pTiledResource,
            [Out, Optional, ComAliasName("UINT")] uint* pNumTilesForEntireResource,
            [Out, Optional] D3D12_PACKED_MIP_INFO* pPackedMipDesc,
            [Out, Optional] D3D12_TILE_SHAPE* pStandardTileShapeForNonPackedMips,
            [In, Out, Optional, ComAliasName("UINT")] uint* pNumSubresourceTilings,
            [In, ComAliasName("UINT")] uint FirstSubresourceTilingToGet,
            [Out] D3D12_SUBRESOURCE_TILING* pSubresourceTilingsForNonPackedMips
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_GetResourceTiling>(lpVtbl->GetResourceTiling)(
                    This,
                    pTiledResource,
                    pNumTilesForEntireResource,
                    pPackedMipDesc,
                    pStandardTileShapeForNonPackedMips,
                    pNumSubresourceTilings,
                    FirstSubresourceTilingToGet,
                    pSubresourceTilingsForNonPackedMips
                );
            }
        }

        public void GetAdapterLuid(
            [Out] LUID* pAdapterLuid
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                MarshalFunction<_GetAdapterLuid>(lpVtbl->GetAdapterLuid)(
                    This,
                    pAdapterLuid
                );
            }
        }
        #endregion

        #region Methods
        [return: ComAliasName("HRESULT")]
        public int CreatePipelineLibrary(
            [In] void* pLibraryBlob,
            [In, ComAliasName("SIZE_T")] nuint BlobLength,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineLibrary
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_CreatePipelineLibrary>(lpVtbl->CreatePipelineLibrary)(
                    This,
                    pLibraryBlob,
                    BlobLength,
                    riid,
                    ppPipelineLibrary
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetEventOnMultipleFenceCompletion(
            [In, ComAliasName("ID3D12Fence*[]")] ID3D12Fence** ppFences,
            [In, ComAliasName("UINT64[]")] ulong* pFenceValues,
            [In, ComAliasName("UINT")] uint NumFences,
            [In] D3D12_MULTIPLE_FENCE_WAIT_FLAGS Flags,
            [In, ComAliasName("HANDLE")] IntPtr hEvent
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_SetEventOnMultipleFenceCompletion>(lpVtbl->SetEventOnMultipleFenceCompletion)(
                    This,
                    ppFences,
                    pFenceValues,
                    NumFences,
                    Flags,
                    hEvent
                );
            }
        }

        [return: ComAliasName("HRESULT")]
        public int SetResidencyPriority(
            [In, ComAliasName("UINT")] uint NumObjects,
            [In, ComAliasName("ID3D12Pageable*[]")] ID3D12Pageable** ppObjects,
            [In, ComAliasName("D3D12_RESIDENCY_PRIORITY[]")] D3D12_RESIDENCY_PRIORITY* pPriorities
        )
        {
            fixed (ID3D12Device1* This = &this)
            {
                return MarshalFunction<_SetResidencyPriority>(lpVtbl->SetResidencyPriority)(
                    This,
                    NumObjects,
                    ppObjects,
                    pPriorities
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

            #region ID3D12Object Fields
            public IntPtr GetPrivateData;

            public IntPtr SetPrivateData;

            public IntPtr SetPrivateDataInterface;

            public IntPtr SetName;
            #endregion

            #region ID3D12Device Fields
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

            #region Fields
            public IntPtr CreatePipelineLibrary;

            public IntPtr SetEventOnMultipleFenceCompletion;

            public IntPtr SetResidencyPriority;
            #endregion
        }
        #endregion
    }
}

