// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("C64226A8-9201-46AF-B4CC-53FB9FF7414F")]
    unsafe public struct ID3D12PipelineLibrary
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12PipelineLibrary).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT StorePipeline(
            [In] ID3D12PipelineLibrary* This,
            [In, Optional] LPWSTR pName,
            [In] ID3D12PipelineState* pPipeline
        );

        public /* static */ delegate HRESULT LoadGraphicsPipeline(
            [In] ID3D12PipelineLibrary* This,
            [In] LPWSTR pName,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In] ref /* readonly */ Guid riid,
            [Out] void** ppPipelineState
        );

        public /* static */ delegate HRESULT LoadComputePipeline(
            [In] ID3D12PipelineLibrary* This,
            [In] LPWSTR pName,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In] ref /* readonly */ Guid riid,
            [Out] void** ppPipelineState
        );

        public /* static */ delegate UIntPtr GetSerializedSize(
            [In] ID3D12PipelineLibrary* This
        );

        public /* static */ delegate HRESULT Serialize(
            [In] ID3D12PipelineLibrary* This,
            [Out] void* pData,
            [In] UIntPtr DataSizeInBytes
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12DeviceChild.Vtbl BaseVtbl;

            public StorePipeline StorePipeline;

            public LoadGraphicsPipeline LoadGraphicsPipeline;

            public LoadComputePipeline LoadComputePipeline;

            public GetSerializedSize GetSerializedSize;

            public Serialize Serialize;
            #endregion
        }
        #endregion
    }
}
