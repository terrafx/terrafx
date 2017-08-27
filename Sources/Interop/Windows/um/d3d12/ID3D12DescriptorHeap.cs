// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("8EFB471D-616C-4F49-90F7-127BB763FA51")]
    public /* blittable */ unsafe struct ID3D12DescriptorHeap
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetDesc(
            [In] ID3D12DescriptorHeap* This,
            [Out] D3D12_DESCRIPTOR_HEAP_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetCPUDescriptorHandleForHeapStart(
            [In] ID3D12DescriptorHeap* This,
            [Out] D3D12_CPU_DESCRIPTOR_HANDLE* pCPUDescriptorHandleForHeapStart
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate void GetGPUDescriptorHandleForHeapStart(
            [In] ID3D12DescriptorHeap* This,
            [Out] D3D12_GPU_DESCRIPTOR_HANDLE* pGPUDescriptorHandleForHeapStart
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public IntPtr GetDesc;

            public IntPtr GetCPUDescriptorHandleForHeapStart;

            public IntPtr GetGPUDescriptorHandleForHeapStart;
            #endregion
        }
        #endregion
    }
}
