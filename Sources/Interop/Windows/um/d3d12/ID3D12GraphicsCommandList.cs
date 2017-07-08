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
        public /* static */ delegate HRESULT Close(
            [In] ID3D12GraphicsCommandList* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Reset(
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
            [In] UINT VertexCountPerInstance,
            [In] UINT InstanceCount,
            [In] UINT StartVertexLocation,
            [In] UINT StartInstanceLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void DrawIndexedInstanced(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT IndexCountPerInstance,
            [In] UINT InstanceCount,
            [In] UINT StartIndexLocation,
            [In] INT BaseVertexLocation,
            [In] UINT StartInstanceLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void Dispatch(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT ThreadGroupCountX,
            [In] UINT ThreadGroupCountY,
            [In] UINT ThreadGroupCountZ
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyBufferRegion(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstBuffer,
            [In] UINT64 DstOffset,
            [In] ID3D12Resource* pSrcBuffer,
            [In] UINT64 SrcOffset,
            [In] UINT64 NumBytes
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void CopyTextureRegion(
            [In] ID3D12GraphicsCommandList* This,
            [In] /* readonly */ D3D12_TEXTURE_COPY_LOCATION* pDst,
            [In] UINT DstX,
            [In] UINT DstY,
            [In] UINT DstZ,
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
            [In] UINT64 BufferStartOffsetInBytes,
            [In] D3D12_TILE_COPY_FLAGS Flags
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ResolveSubresource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstResource,
            [In] UINT DstSubresource,
            [In] ID3D12Resource* pSrcResource,
            [In] UINT SrcSubresource,
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
            [In] UINT NumViewports,
            [In] /* readonly */ D3D12_VIEWPORT* pViewports
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void RSSetScissorRects(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT NumRects,
            [In] /* readonly */ D3D12_RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void OMSetBlendFactor(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] /* readonly */ FLOAT* BlendFactor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void OMSetStencilRef(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT StencilRef
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
            [In] UINT NumBarriers,
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
            [In] UINT NumDescriptorHeaps,
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
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE BaseDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootDescriptorTable(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE BaseDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRoot32BitConstant(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] UINT SrcData,
            [In] UINT DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRoot32BitConstant(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] UINT SrcData,
            [In] UINT DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRoot32BitConstants(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] UINT Num32BitValuesToSet,
            [In] /* readonly */ void* pSrcData,
            [In] UINT DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRoot32BitConstants(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] UINT Num32BitValuesToSet,
            [In] /* readonly */ void* pSrcData,
            [In] UINT DestOffsetIn32BitValues
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootConstantBufferView(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootConstantBufferView(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootShaderResourceView(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootShaderResourceView(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetComputeRootUnorderedAccessView(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetGraphicsRootUnorderedAccessView(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
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
            [In] UINT StartSlot,
            [In] UINT NumViews,
            [In, Optional] /* readonly */ D3D12_VERTEX_BUFFER_VIEW* pViews
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SOSetTargets(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT StartSlot,
            [In] UINT NumViews,
            [In, Optional] /* readonly */ D3D12_STREAM_OUTPUT_BUFFER_VIEW* pViews
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void OMSetRenderTargets(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT NumRenderTargetDescriptors,
            [In, Optional] /* readonly */ D3D12_CPU_DESCRIPTOR_HANDLE* pRenderTargetDescriptors,
            [In] INT RTsSingleHandleToDescriptorRange,
            [In, Optional] /* readonly */ D3D12_CPU_DESCRIPTOR_HANDLE* pDepthStencilDescriptor
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearDepthStencilView(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DepthStencilView,
            [In] D3D12_CLEAR_FLAGS ClearFlags,
            [In] FLOAT Depth,
            [In] UINT8 Stencil,
            [In] UINT NumRects,
            [In] /* readonly */ D3D12_RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearRenderTargetView(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE RenderTargetView,
            [In] /* readonly */ FLOAT* ColorRGBA,
            [In] UINT NumRects,
            [In] /* readonly */ D3D12_RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearUnorderedAccessViewUint(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE ViewGPUHandleInCurrentHeap,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE ViewCPUHandle,
            [In] [MarshalAs(UnmanagedType.Interface)] ID3D12Resource pResource,
            [In] /* readonly */ UINT* Values,
            [In] UINT NumRects,
            [In] /* readonly */ D3D12_RECT* pRects
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ClearUnorderedAccessViewFloat(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE ViewGPUHandleInCurrentHeap,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE ViewCPUHandle,
            [In] ID3D12Resource* pResource,
            [In] /* readonly */ FLOAT* Values,
            [In] UINT NumRects,
            [In] /* readonly */ D3D12_RECT* pRects
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
            [In] UINT Index
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void EndQuery(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In] UINT Index
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void ResolveQueryData(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In] UINT StartIndex,
            [In] UINT NumQueries,
            [In] ID3D12Resource* pDestinationBuffer,
            [In] UINT64 AlignedDestinationBufferOffset
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetPredication(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12Resource* pBuffer,
            [In] UINT64 AlignedBufferOffset,
            [In] D3D12_PREDICATION_OP Operation
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void SetMarker(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT Metadata,
            [In, Optional] /* readonly */ void* pData,
            [In] UINT Size
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void BeginEvent(
            [In] ID3D12GraphicsCommandList* This,
            [In] UINT Metadata,
            [In, Optional] /* readonly */ void* pData,
            [In] UINT Size
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
            [In] UINT MaxCommandCount,
            [In] ID3D12Resource* pArgumentBuffer,
            [In] UINT64 ArgumentBufferOffset,
            [In, Optional] ID3D12Resource* pCountBuffer,
            [In] UINT64 CountBufferOffset
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12CommandList.Vtbl BaseVtbl;

            public Close Close;

            public Reset Reset;

            public ClearState ClearState;

            public DrawInstanced DrawInstanced;

            public DrawIndexedInstanced DrawIndexedInstanced;

            public Dispatch Dispatch;

            public CopyBufferRegion CopyBufferRegion;

            public CopyTextureRegion CopyTextureRegion;

            public CopyResource CopyResource;

            public CopyTiles CopyTiles;

            public ResolveSubresource ResolveSubresource;

            public IASetPrimitiveTopology IASetPrimitiveTopology;

            public RSSetViewports RSSetViewports;

            public RSSetScissorRects RSSetScissorRects;

            public OMSetBlendFactor OMSetBlendFactor;

            public OMSetStencilRef OMSetStencilRef;

            public SetPipelineState SetPipelineState;

            public ResourceBarrier ResourceBarrier;

            public ExecuteBundle ExecuteBundle;

            public SetDescriptorHeaps SetDescriptorHeaps;

            public SetComputeRootSignature SetComputeRootSignature;

            public SetGraphicsRootSignature SetGraphicsRootSignature;

            public SetComputeRootDescriptorTable SetComputeRootDescriptorTable;

            public SetGraphicsRootDescriptorTable SetGraphicsRootDescriptorTable;

            public SetComputeRoot32BitConstant SetComputeRoot32BitConstant;

            public SetGraphicsRoot32BitConstant SetGraphicsRoot32BitConstant;

            public SetComputeRoot32BitConstants SetComputeRoot32BitConstants;

            public SetGraphicsRoot32BitConstants SetGraphicsRoot32BitConstants;

            public SetComputeRootConstantBufferView SetComputeRootConstantBufferView;

            public SetGraphicsRootConstantBufferView SetGraphicsRootConstantBufferView;

            public SetComputeRootShaderResourceView SetComputeRootShaderResourceView;

            public SetGraphicsRootShaderResourceView SetGraphicsRootShaderResourceView;

            public SetComputeRootUnorderedAccessView SetComputeRootUnorderedAccessView;

            public SetGraphicsRootUnorderedAccessView SetGraphicsRootUnorderedAccessView;

            public IASetIndexBuffer IASetIndexBuffer;

            public IASetVertexBuffers IASetVertexBuffers;

            public SOSetTargets SOSetTargets;

            public OMSetRenderTargets OMSetRenderTargets;

            public ClearDepthStencilView ClearDepthStencilView;

            public ClearRenderTargetView ClearRenderTargetView;

            public ClearUnorderedAccessViewUint ClearUnorderedAccessViewUint;

            public ClearUnorderedAccessViewFloat ClearUnorderedAccessViewFloat;

            public DiscardResource DiscardResource;

            public BeginQuery BeginQuery;

            public EndQuery EndQuery;

            public ResolveQueryData ResolveQueryData;

            public SetPredication SetPredication;

            public SetMarker SetMarker;

            public BeginEvent BeginEvent;

            public EndEvent EndEvent;

            public ExecuteIndirect ExecuteIndirect;
            #endregion
        }
        #endregion
    }
}
