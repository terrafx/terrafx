// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using TerraFX.Interop.DXGI;
using TerraFX.Interop.Unknown;

namespace TerraFX.Interop.D3D12
{
    [Guid("553103FB-1FE7-4557-BB38-946D7D0E7CA7")]
    unsafe public struct ID3D12GraphicsCommandList1
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12GraphicsCommandList1).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate void AtomicCopyBufferUINT(
            [In] ID3D12GraphicsCommandList1* This,
            [In] ID3D12Resource* pDstBuffer,
            [In] ulong DstOffset,
            [In] ID3D12Resource* pSrcBuffer,
            [In] ulong SrcOffset,
            [In] uint Dependencies,
            [In] ID3D12Resource** ppDependentResources,
            [In] D3D12_SUBRESOURCE_RANGE_UINT64* pDependentSubresourceRanges
        );

        public /* static */ delegate void AtomicCopyBufferUINT64(
            [In] ID3D12GraphicsCommandList1* This,
            [In] ID3D12Resource* pDstBuffer,
            [In] ulong DstOffset,
            [In] ID3D12Resource* pSrcBuffer,
            [In] ulong SrcOffset,
            [In] uint Dependencies,
            [In] ID3D12Resource* ppDependentResources,
            [In] D3D12_SUBRESOURCE_RANGE_UINT64* pDependentSubresourceRanges
        );

        public /* static */ delegate void OMSetDepthBounds(
            [In] ID3D12GraphicsCommandList1* This,
            [In] float Min,
            [In] float Max
        );

        public /* static */ delegate void SetSamplePositions(
            [In] ID3D12GraphicsCommandList1* This,
            [In] uint NumSamplesPerPixel,
            [In] uint NumPixels,
            [In] D3D12_SAMPLE_POSITION* pSamplePositions
        );

        public /* static */ delegate void ResolveSubresourceRegion(
            [In] ID3D12GraphicsCommandList1* This,
            [In] ID3D12Resource* pDstResource,
            [In] uint DstSubresource,
            [In] uint DstX,
            [In] uint DstY,
            [In] ID3D12Resource* pSrcResource,
            [In] uint SrcSubresource,
            [In, Optional] RECT* pSrcRect,
            [In] DXGI_FORMAT Format,
            [In] D3D12_RESOLVE_MODE ResolveMode
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12GraphicsCommandList.Vtbl BaseVtbl;

            public AtomicCopyBufferUINT AtomicCopyBufferUINT;

            public AtomicCopyBufferUINT64 AtomicCopyBufferUINT64;

            public OMSetDepthBounds OMSetDepthBounds;

            public SetSamplePositions SetSamplePositions;

            public ResolveSubresourceRegion ResolveSubresourceRegion;
            #endregion
        }
        #endregion
    }
}
