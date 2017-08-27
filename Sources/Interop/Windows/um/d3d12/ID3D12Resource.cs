// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("696442BE-A72E-4059-BC79-5B5C98040FAD")]
    public /* blittable */ unsafe struct ID3D12Resource
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Map(
            [In] ID3D12Resource* This,
            [In, ComAliasName("UINT")] uint Subresource,
            [In] D3D12_RANGE* pReadRange = null,
            [Out] void** ppData = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void Unmap(
            [In] ID3D12Resource* This,
            [In, ComAliasName("UINT")] uint Subresource,
            [In] D3D12_RANGE* pWrittenRange = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDesc(
            [In] ID3D12Resource* This,
            [Out] D3D12_RESOURCE_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("D3D12_GPU_VIRTUAL_ADDRESS")]
        public /* static */ delegate ulong GetGPUVirtualAddress(
            [In] ID3D12Resource* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int WriteToSubresource(
            [In] ID3D12Resource* This,
            [In, ComAliasName("UINT")] uint DstSubresource,
            [In, Optional] D3D12_BOX* pDstBox,
            [In] void* pSrcData,
            [In, ComAliasName("UINT")] uint SrcRowPitch,
            [In, ComAliasName("UINT")] uint SrcDepthPitch
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int ReadFromSubresource(
            [In] ID3D12Resource* This,
            [Out] void* pDstData,
            [In, ComAliasName("UINT")] uint DstRowPitch,
            [In, ComAliasName("UINT")] uint DstDepthPitch,
            [In, ComAliasName("UINT")] uint SrcSubresource,
            [In] D3D12_BOX* pSrcBox = null
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetHeapProperties(
            [In] ID3D12Resource* This,
            [Out] D3D12_HEAP_PROPERTIES* pHeapProperties = null,
            [Out] D3D12_HEAP_FLAGS* pHeapFlags = null
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public IntPtr Map;

            public IntPtr Unmap;

            public IntPtr GetDesc;

            public IntPtr GetGPUVirtualAddress;

            public IntPtr WriteToSubresource;

            public IntPtr ReadFromSubresource;

            public IntPtr GetHeapProperties;
            #endregion
        }
        #endregion
    }
}
