// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("C64226A8-9201-46AF-B4CC-53FB9FF7414F")]
    unsafe public /* blittable */ struct ID3D12PipelineLibrary
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT StorePipeline(
            [In] ID3D12PipelineLibrary* This,
            [In, Optional] LPCWSTR pName,
            [In] ID3D12PipelineState* pPipeline
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT LoadGraphicsPipeline(
            [In] ID3D12PipelineLibrary* This,
            [In] LPCWSTR pName,
            [In] /* readonly */ D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In] REFIID riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT LoadComputePipeline(
            [In] ID3D12PipelineLibrary* This,
            [In] LPCWSTR pName,
            [In] /* readonly */ D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In] REFIID riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate SIZE_T GetSerializedSize(
            [In] ID3D12PipelineLibrary* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT Serialize(
            [In] ID3D12PipelineLibrary* This,
            [Out] void* pData,
            [In] SIZE_T DataSizeInBytes
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
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
