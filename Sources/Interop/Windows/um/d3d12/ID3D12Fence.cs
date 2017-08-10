// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("0A753DCF-C4D8-4B91-ADF6-BE5A60D95A76")]
    unsafe public /* blittable */ struct ID3D12Fence
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("UINT64")]
        public /* static */ delegate ulong GetCompletedValue(
            [In] ID3D12Fence* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int SetEventOnCompletion(
            [In] ID3D12Fence* This,
            [In, ComAliasName("UINT64")] ulong Value,
            [In, ComAliasName("HANDLE")] void* hEvent
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int Signal(
            [In] ID3D12Fence* This,
            [In, ComAliasName("UINT64")] ulong Value
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public ID3D12Pageable.Vtbl BaseVtbl;

            public IntPtr GetCompletedValue;

            public IntPtr SetEventOnCompletion;

            public IntPtr Signal;
            #endregion
        }
        #endregion
    }
}
