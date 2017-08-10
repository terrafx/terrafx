// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("5B160D0F-AC1B-4185-8BA8-B3AE42A5A455")]
    unsafe public /* blittable */ struct ID3D12GraphicsCommandList
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Close(
            [In] ID3D12GraphicsCommandList* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Reset(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12CommandAllocator* pAllocator,
            [In, Optional] ID3D12PipelineState* pInitialState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearState(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12PipelineState* pPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawInstanced(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint VertexCountPerInstance,
            [In, ComAliasName("UINT")] uint InstanceCount,
            [In, ComAliasName("UINT")] uint StartVertexLocation,
            [In, ComAliasName("UINT")] uint StartInstanceLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawIndexedInstanced(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint IndexCountPerInstance,
            [In, ComAliasName("UINT")] uint InstanceCount,
            [In, ComAliasName("UINT")] uint StartIndexLocation,
            [In, ComAliasName("INT")] int BaseVertexLocation,
            [In, ComAliasName("UINT")] uint StartInstanceLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void Dispatch(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint ThreadGroupCountX,
            [In, ComAliasName("UINT")] uint ThreadGroupCountY,
            [In, ComAliasName("UINT")] uint ThreadGroupCountZ
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyBufferRegion(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstBuffer,
            [In, ComAliasName("UINT64")] ulong DstOffset,
            [In] ID3D12Resource* pSrcBuffer,
            [In, ComAliasName("UINT64")] ulong SrcOffset,
            [In, ComAliasName("UINT64")] ulong NumBytes
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyTextureRegion(
            [In] ID3D12GraphicsCommandList* This,
            [In] /* readonly */ D3D12_TEXTURE_COPY_LOCATION* pDst,
            [In, ComAliasName("UINT")] uint DstX,
            [In, ComAliasName("UINT")] uint DstY,
            [In, ComAliasName("UINT")] uint DstZ,
            [In] /* readonly */ D3D12_TEXTURE_COPY_LOCATION* pSrc,
            [In, Optional] /* readonly */ D3D12_BOX* pSrcBox
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyResource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstResource,
            [In] ID3D12Resource* pSrcResource
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyTiles(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pTiledResource,
            [In] /* readonly */ D3D12_TILED_RESOURCE_COORDINATE* pTileRegionStartCoordinate,
            [In] /* readonly */ D3D12_TILE_REGION_SIZE* pTileRegionSize,
            [In] ID3D12Resource* pBuffer,
            [In, ComAliasName("UINT64")] ulong BufferStartOffsetInBytes,
            [In] D3D12_TILE_COPY_FLAGS Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ResolveSubresource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstResource,
            [In, ComAliasName("UINT")] uint DstSubresource,
            [In] ID3D12Resource* pSrcResource,
            [In, ComAliasName("UINT")] uint SrcSubresource,
            [In] DXGI_FORMAT Format
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void IASetPrimitiveTopology(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D_PRIMITIVE_TOPOLOGY PrimitiveTopology
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void RSSetViewports(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint NumViewports,
            [In] /* readonly */ D3D12_VIEWPORT* pViewports
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void RSSetScissorRects(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint NumRects,
            [In, ComAliasName("D3D12_RECT")] /* readonly */ RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void OMSetBlendFactor(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional, ComAliasName("FLOAT")] /* readonly */ float* BlendFactor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void OMSetStencilRef(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint StencilRef
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetPipelineState(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12PipelineState* pPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ResourceBarrier(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint NumBarriers,
            [In] /* readonly */ D3D12_RESOURCE_BARRIER* pBarriers
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ExecuteBundle(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12GraphicsCommandList* pCommandList
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetDescriptorHeaps(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint NumDescriptorHeaps,
            [In] /* readonly */ ID3D12DescriptorHeap** ppDescriptorHeaps
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootSignature(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12RootSignature* pRootSignature
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootSignature(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12RootSignature* pRootSignature
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootDescriptorTable(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE BaseDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootDescriptorTable(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE BaseDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRoot32BitConstant(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("UINT")] uint SrcData,
            [In, ComAliasName("UINT")] uint DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRoot32BitConstant(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("UINT")] uint SrcData,
            [In, ComAliasName("UINT")] uint DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRoot32BitConstants(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("UINT")] uint Num32BitValuesToSet,
            [In] /* readonly */ void* pSrcData,
            [In, ComAliasName("UINT")] uint DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRoot32BitConstants(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("UINT")] uint Num32BitValuesToSet,
            [In] /* readonly */ void* pSrcData,
            [In, ComAliasName("UINT")] uint DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootConstantBufferView(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")] ulong BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootConstantBufferView(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")] ulong BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootShaderResourceView(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")] ulong BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootShaderResourceView(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")] ulong BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootUnorderedAccessView(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")] ulong BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootUnorderedAccessView(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint RootParameterIndex,
            [In, ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")] ulong BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void IASetIndexBuffer(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] /* readonly */ D3D12_INDEX_BUFFER_VIEW* pView
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void IASetVertexBuffers(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint StartSlot,
            [In, ComAliasName("UINT")] uint NumViews,
            [In, Optional] /* readonly */ D3D12_VERTEX_BUFFER_VIEW* pViews
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SOSetTargets(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint StartSlot,
            [In, ComAliasName("UINT")] uint NumViews,
            [In, Optional] /* readonly */ D3D12_STREAM_OUTPUT_BUFFER_VIEW* pViews
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void OMSetRenderTargets(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint NumRenderTargetDescriptors,
            [In, Optional] /* readonly */ D3D12_CPU_DESCRIPTOR_HANDLE* pRenderTargetDescriptors,
            [In, ComAliasName("INT")] int RTsSingleHandleToDescriptorRange,
            [In, Optional] /* readonly */ D3D12_CPU_DESCRIPTOR_HANDLE* pDepthStencilDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearDepthStencilView(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DepthStencilView,
            [In] D3D12_CLEAR_FLAGS ClearFlags,
            [In, ComAliasName("FLOAT")] float Depth,
            [In, ComAliasName("UINT8")] byte Stencil,
            [In, ComAliasName("UINT")] uint NumRects,
            [In, ComAliasName("D3D12_RECT")] /* readonly */ RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearRenderTargetView(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE RenderTargetView,
            [In, ComAliasName("FLOAT")] /* readonly */ float* ColorRGBA,
            [In, ComAliasName("UINT")] uint NumRects,
            [In, ComAliasName("D3D12_RECT")] /* readonly */ RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearUnorderedAccessViewUint(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE ViewGPUHandleInCurrentHeap,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE ViewCPUHandle,
            [In] [MarshalAs(UnmanagedType.Interface)] ID3D12Resource pResource,
            [In, ComAliasName("UINT")] /* readonly */ uint* Values,
            [In, ComAliasName("UINT")] uint NumRects,
            [In, ComAliasName("D3D12_RECT")] /* readonly */ RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearUnorderedAccessViewFloat(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE ViewGPUHandleInCurrentHeap,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE ViewCPUHandle,
            [In] ID3D12Resource* pResource,
            [In, ComAliasName("FLOAT")] /* readonly */ float* Values,
            [In, ComAliasName("UINT")] uint NumRects,
            [In, ComAliasName("D3D12_RECT")] /* readonly */ RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DiscardResource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pResource,
            [In, Optional] /* readonly */ D3D12_DISCARD_REGION* pRegion
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void BeginQuery(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In, ComAliasName("UINT")] uint Index
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void EndQuery(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In, ComAliasName("UINT")] uint Index
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ResolveQueryData(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In, ComAliasName("UINT")] uint StartIndex,
            [In, ComAliasName("UINT")] uint NumQueries,
            [In] ID3D12Resource* pDestinationBuffer,
            [In, ComAliasName("UINT64")] ulong AlignedDestinationBufferOffset
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetPredication(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12Resource* pBuffer,
            [In, ComAliasName("UINT64")] ulong AlignedBufferOffset,
            [In] D3D12_PREDICATION_OP Operation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMarker(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] /* readonly */ void* pData,
            [In, ComAliasName("UINT")] uint Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void BeginEvent(
            [In] ID3D12GraphicsCommandList* This,
            [In, ComAliasName("UINT")] uint Metadata,
            [In, Optional] /* readonly */ void* pData,
            [In, ComAliasName("UINT")] uint Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void EndEvent(
            [In] ID3D12GraphicsCommandList* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ExecuteIndirect(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12CommandSignature* pCommandSignature,
            [In, ComAliasName("UINT")] uint MaxCommandCount,
            [In] ID3D12Resource* pArgumentBuffer,
            [In, ComAliasName("UINT64")] ulong ArgumentBufferOffset,
            [In, Optional] ID3D12Resource* pCountBuffer,
            [In, ComAliasName("UINT64")] ulong CountBufferOffset
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12CommandList.Vtbl BaseVtbl;

            public IntPtr Close;

            public IntPtr Reset;

            public IntPtr ClearState;

            public IntPtr DrawInstanced;

            public IntPtr DrawIndexedInstanced;

            public IntPtr Dispatch;

            public IntPtr CopyBufferRegion;

            public IntPtr CopyTextureRegion;

            public IntPtr CopyResource;

            public IntPtr CopyTiles;

            public IntPtr ResolveSubresource;

            public IntPtr IASetPrimitiveTopology;

            public IntPtr RSSetViewports;

            public IntPtr RSSetScissorRects;

            public IntPtr OMSetBlendFactor;

            public IntPtr OMSetStencilRef;

            public IntPtr SetPipelineState;

            public IntPtr ResourceBarrier;

            public IntPtr ExecuteBundle;

            public IntPtr SetDescriptorHeaps;

            public IntPtr SetComputeRootSignature;

            public IntPtr SetGraphicsRootSignature;

            public IntPtr SetComputeRootDescriptorTable;

            public IntPtr SetGraphicsRootDescriptorTable;

            public IntPtr SetComputeRoot32BitConstant;

            public IntPtr SetGraphicsRoot32BitConstant;

            public IntPtr SetComputeRoot32BitConstants;

            public IntPtr SetGraphicsRoot32BitConstants;

            public IntPtr SetComputeRootConstantBufferView;

            public IntPtr SetGraphicsRootConstantBufferView;

            public IntPtr SetComputeRootShaderResourceView;

            public IntPtr SetGraphicsRootShaderResourceView;

            public IntPtr SetComputeRootUnorderedAccessView;

            public IntPtr SetGraphicsRootUnorderedAccessView;

            public IntPtr IASetIndexBuffer;

            public IntPtr IASetVertexBuffers;

            public IntPtr SOSetTargets;

            public IntPtr OMSetRenderTargets;

            public IntPtr ClearDepthStencilView;

            public IntPtr ClearRenderTargetView;

            public IntPtr ClearUnorderedAccessViewUint;

            public IntPtr ClearUnorderedAccessViewFloat;

            public IntPtr DiscardResource;

            public IntPtr BeginQuery;

            public IntPtr EndQuery;

            public IntPtr ResolveQueryData;

            public IntPtr SetPredication;

            public IntPtr SetMarker;

            public IntPtr BeginEvent;

            public IntPtr EndEvent;

            public IntPtr ExecuteIndirect;
            #endregion
        }
        #endregion
    }
}
