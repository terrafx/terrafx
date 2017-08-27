// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("C64226A8-9201-46AF-B4CC-53FB9FF7414F")]
    public /* blittable */ unsafe struct ID3D12PipelineLibrary
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int StorePipeline(
            [In] ID3D12PipelineLibrary* This,
            [In, Optional, ComAliasName("LPCWSTR")] char* pName,
            [In] ID3D12PipelineState* pPipeline
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadGraphicsPipeline(
            [In] ID3D12PipelineLibrary* This,
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_GRAPHICS_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int LoadComputePipeline(
            [In] ID3D12PipelineLibrary* This,
            [In, ComAliasName("LPCWSTR")] char* pName,
            [In] D3D12_COMPUTE_PIPELINE_STATE_DESC* pDesc,
            [In, ComAliasName("REFIID")] Guid* riid,
            [Out] void** ppPipelineState
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("SIZE_T")]
        public /* static */ delegate nuint GetSerializedSize(
            [In] ID3D12PipelineLibrary* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Serialize(
            [In] ID3D12PipelineLibrary* This,
            [Out] void* pData,
            [In, ComAliasName("SIZE_T")] nuint DataSizeInBytes
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12DeviceChild.Vtbl BaseVtbl;

            public IntPtr StorePipeline;

            public IntPtr LoadGraphicsPipeline;

            public IntPtr LoadComputePipeline;

            public IntPtr GetSerializedSize;

            public IntPtr Serialize;
            #endregion
        }
        #endregion
    }
}
