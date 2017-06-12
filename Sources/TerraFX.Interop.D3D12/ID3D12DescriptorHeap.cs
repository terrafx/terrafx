// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop.D3D12
{
    [Guid("8EFB471D-616C-4F49-90F7-127BB763FA51")]
    unsafe public struct ID3D12DescriptorHeap
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12DescriptorHeap).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate D3D12_DESCRIPTOR_HEAP_DESC GetDesc(
            [In] ID3D12DescriptorHeap* This
        );

        public /* static */ delegate D3D12_CPU_DESCRIPTOR_HANDLE GetCPUDescriptorHandleForHeapStart(
            [In] ID3D12DescriptorHeap* This
        );

        public /* static */ delegate D3D12_GPU_DESCRIPTOR_HANDLE GetGPUDescriptorHandleForHeapStart(
            [In] ID3D12DescriptorHeap* This
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public GetCPUDescriptorHandleForHeapStart GetCPUDescriptorHandleForHeapStart;

            public GetGPUDescriptorHandleForHeapStart GetGPUDescriptorHandleForHeapStart;
            #endregion
        }
        #endregion
    }
}
