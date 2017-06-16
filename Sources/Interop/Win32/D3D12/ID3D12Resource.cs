// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("696442BE-A72E-4059-BC79-5B5C98040FAD")]
    unsafe public struct ID3D12Resource
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12Resource).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT Map(
            [In] ID3D12Resource* This,
            [In] uint Subresource,
            [In, Optional] D3D12_RANGE* pReadRange,
            [Out, Optional] void** ppData
        );

        public /* static */ delegate void Unmap(
            [In] ID3D12Resource* This,
            [In] uint Subresource,
            [In, Optional] D3D12_RANGE* pWrittenRange
        );

        public /* static */ delegate D3D12_RESOURCE_DESC GetDesc(
            [In] ID3D12Resource* This
        );

        public /* static */ delegate D3D12_GPU_VIRTUAL_ADDRESS GetGPUVirtualAddress(
            [In] ID3D12Resource* This
        );

        public /* static */ delegate HRESULT WriteToSubresource(
            [In] ID3D12Resource* This,
            [In] uint DstSubresource,
            [In, Optional] D3D12_BOX* pDstBox,
            [In] void* pSrcData,
            [In] uint SrcRowPitch,
            [In] uint SrcDepthPitch
        );

        public /* static */ delegate HRESULT ReadFromSubresource(
            [In] ID3D12Resource* This,
            [Out] void* pDstData,
            [In] uint DstRowPitch,
            [In] uint DstDepthPitch,
            [In] uint SrcSubresource,
            [In, Optional] D3D12_BOX* pSrcBox
        );

        public /* static */ delegate HRESULT GetHeapProperties(
            [In] ID3D12Resource* This,
            [Out, Optional] D3D12_HEAP_PROPERTIES* pHeapProperties,
            [Out, Optional] D3D12_HEAP_FLAGS* pHeapFlags
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public Map Map;

            public Unmap Unmap;

            public GetDesc GetDesc;

            public GetGPUVirtualAddress GetGPUVirtualAddress;

            public WriteToSubresource WriteToSubresource;

            public ReadFromSubresource ReadFromSubresource;

            public GetHeapProperties GetHeapProperties;
            #endregion
        }
        #endregion
    }
}
