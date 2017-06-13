// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("5B160D0F-AC1B-4185-8BA8-B3AE42A5A455")]
    unsafe public struct ID3D12GraphicsCommandList
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12GraphicsCommandList).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Close(
            [In] ID3D12GraphicsCommandList* This
        );

        public /* static */ delegate HRESULT Reset(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12CommandAllocator* pAllocator,
            [In, Optional] ID3D12PipelineState* pInitialState
        );

        public /* static */ delegate void ClearState(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12PipelineState* pPipelineState
        );

        public /* static */ delegate void DrawInstanced(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint VertexCountPerInstance,
            [In] uint InstanceCount,
            [In] uint StartVertexLocation,
            [In] uint StartInstanceLocation
        );

        public /* static */ delegate void DrawIndexedInstanced(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint IndexCountPerInstance,
            [In] uint InstanceCount,
            [In] uint StartIndexLocation,
            [In] int BaseVertexLocation,
            [In] uint StartInstanceLocation
        );

        public /* static */ delegate void Dispatch(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint ThreadGroupCountX,
            [In] uint ThreadGroupCountY,
            [In] uint ThreadGroupCountZ
        );

        public /* static */ delegate void CopyBufferRegion(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstBuffer,
            [In] ulong DstOffset,
            [In] ID3D12Resource* pSrcBuffer,
            [In] ulong SrcOffset,
            [In] ulong NumBytes
        );

        public /* static */ delegate void CopyTextureRegion(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_TEXTURE_COPY_LOCATION* pDst,
            [In] uint DstX,
            [In] uint DstY,
            [In] uint DstZ,
            [In] D3D12_TEXTURE_COPY_LOCATION* pSrc,
            [In, Optional] D3D12_BOX* pSrcBox
        );

        public /* static */ delegate void CopyResource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstResource,
            [In] ID3D12Resource* pSrcResource
        );

        public /* static */ delegate void CopyTiles(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pTiledResource,
            [In] D3D12_TILED_RESOURCE_COORDINATE* pTileRegionStartCoordinate,
            [In] D3D12_TILE_REGION_SIZE* pTileRegionSize,
            [In] ID3D12Resource* pBuffer,
            [In] ulong BufferStartOffsetInBytes,
            [In] D3D12_TILE_COPY_FLAGS Flags
        );

        public /* static */ delegate void ResolveSubresource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pDstResource,
            [In] uint DstSubresource,
            [In] ID3D12Resource* pSrcResource,
            [In] uint SrcSubresource,
            [In] DXGI_FORMAT Format
        );

        public /* static */ delegate void IASetPrimitiveTopology(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D_PRIMITIVE_TOPOLOGY PrimitiveTopology
        );

        public /* static */ delegate void RSSetViewports(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint NumViewports,
            [In] D3D12_VIEWPORT* pViewports
        );

        public /* static */ delegate void RSSetScissorRects(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint NumRects,
            [In] RECT* pRects
        );

        public /* static */ delegate void OMSetBlendFactor(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] float* BlendFactor
        );

        public /* static */ delegate void OMSetStencilRef(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint StencilRef
        );

        public /* static */ delegate void SetPipelineState(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12PipelineState* pPipelineState
        );

        public /* static */ delegate void ResourceBarrier(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint NumBarriers,
            [In] D3D12_RESOURCE_BARRIER* pBarriers
        );

        public /* static */ delegate void ExecuteBundle(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12GraphicsCommandList* pCommandList
        );

        public /* static */ delegate void SetDescriptorHeaps(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint NumDescriptorHeaps,
            [In] ID3D12DescriptorHeap** ppDescriptorHeaps
        );

        public /* static */ delegate void SetComputeRootSignature(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12RootSignature* pRootSignature
        );

        public /* static */ delegate void SetGraphicsRootSignature(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12RootSignature* pRootSignature
        );

        public /* static */ delegate void SetComputeRootDescriptorTable(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE BaseDescriptor
        );

        public /* static */ delegate void SetGraphicsRootDescriptorTable(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE BaseDescriptor
        );

        public /* static */ delegate void SetComputeRoot32BitConstant(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] uint SrcData,
            [In] uint DestOffsetIn32BitValues
        );

        public /* static */ delegate void SetGraphicsRoot32BitConstant(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] uint SrcData,
            [In] uint DestOffsetIn32BitValues
        );

        public /* static */ delegate void SetComputeRoot32BitConstants(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] uint Num32BitValuesToSet,
            [In] void* pSrcData,
            [In] uint DestOffsetIn32BitValues
        );

        public /* static */ delegate void SetGraphicsRoot32BitConstants(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] uint Num32BitValuesToSet,
            [In] void* pSrcData,
            [In] uint DestOffsetIn32BitValues
        );

        public /* static */ delegate void SetComputeRootConstantBufferView(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        public /* static */ delegate void SetGraphicsRootConstantBufferView(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        public /* static */ delegate void SetComputeRootShaderResourceView(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        public /* static */ delegate void SetGraphicsRootShaderResourceView(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        public /* static */ delegate void SetComputeRootUnorderedAccessView(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        public /* static */ delegate void SetGraphicsRootUnorderedAccessView(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint RootParameterIndex,
            [In] D3D12_GPU_VIRTUAL_ADDRESS BufferLocation
        );

        public /* static */ delegate void IASetIndexBuffer(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] D3D12_INDEX_BUFFER_VIEW* pView
        );

        public /* static */ delegate void IASetVertexBuffers(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint StartSlot,
            [In] uint NumViews,
            [In, Optional] D3D12_VERTEX_BUFFER_VIEW* pViews
        );

        public /* static */ delegate void SOSetTargets(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint StartSlot,
            [In] uint NumViews,
            [In, Optional] D3D12_STREAM_OUTPUT_BUFFER_VIEW* pViews
        );

        public /* static */ delegate void OMSetRenderTargets(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint NumRenderTargetDescriptors,
            [In, Optional] D3D12_CPU_DESCRIPTOR_HANDLE* pRenderTargetDescriptors,
            [In] int RTsSingleHandleToDescriptorRange,
            [In, Optional] D3D12_CPU_DESCRIPTOR_HANDLE* pDepthStencilDescriptor
        );

        public /* static */ delegate void ClearDepthStencilView(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE DepthStencilView,
            [In] D3D12_CLEAR_FLAGS ClearFlags,
            [In] float Depth,
            [In] byte Stencil,
            [In] uint NumRects,
            [In] RECT* pRects
        );

        public /* static */ delegate void ClearRenderTargetView(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE RenderTargetView,
            [In] float* ColorRGBA,
            [In] uint NumRects,
            [In] RECT* pRects
        );

        public /* static */ delegate void ClearUnorderedAccessViewUint(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE ViewGPUHandleInCurrentHeap,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE ViewCPUHandle,
            [In] [MarshalAs(UnmanagedType.Interface)] ID3D12Resource pResource,
            [In] uint* Values,
            [In] uint NumRects,
            [In] RECT* pRects
        );

        public /* static */ delegate void ClearUnorderedAccessViewFloat(
            [In] ID3D12GraphicsCommandList* This,
            [In] D3D12_GPU_DESCRIPTOR_HANDLE ViewGPUHandleInCurrentHeap,
            [In] D3D12_CPU_DESCRIPTOR_HANDLE ViewCPUHandle,
            [In] ID3D12Resource* pResource,
            [In] float* Values,
            [In] uint NumRects,
            [In] RECT* pRects
        );

        public /* static */ delegate void DiscardResource(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12Resource* pResource,
            [In, Optional] D3D12_DISCARD_REGION* pRegion
        );

        public /* static */ delegate void BeginQuery(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In] uint Index
        );

        public /* static */ delegate void EndQuery(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In] uint Index
        );

        public /* static */ delegate void ResolveQueryData(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12QueryHeap* pQueryHeap,
            [In] D3D12_QUERY_TYPE Type,
            [In] uint StartIndex,
            [In] uint NumQueries,
            [In] ID3D12Resource* pDestinationBuffer,
            [In] ulong AlignedDestinationBufferOffset
        );

        public /* static */ delegate void SetPredication(
            [In] ID3D12GraphicsCommandList* This,
            [In, Optional] ID3D12Resource* pBuffer,
            [In] ulong AlignedBufferOffset,
            [In] D3D12_PREDICATION_OP Operation
        );

        public /* static */ delegate void SetMarker(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint Metadata,
            [In, Optional] void* pData,
            [In] uint Size
        );

        public /* static */ delegate void BeginEvent(
            [In] ID3D12GraphicsCommandList* This,
            [In] uint Metadata,
            [In, Optional] void* pData,
            [In] uint Size
        );

        public /* static */ delegate void EndEvent(
            [In] ID3D12GraphicsCommandList* This
        );

        public /* static */ delegate void ExecuteIndirect(
            [In] ID3D12GraphicsCommandList* This,
            [In] ID3D12CommandSignature* pCommandSignature,
            [In] uint MaxCommandCount,
            [In] ID3D12Resource* pArgumentBuffer,
            [In] ulong ArgumentBufferOffset,
            [In, Optional] ID3D12Resource* pCountBuffer,
            [In] ulong CountBufferOffset
        );
        #endregion

        #region Structs
        public struct Vtbl
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
