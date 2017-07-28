// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("8E349D19-54DB-4A56-9DC9-119D87BDB804")]
    unsafe public /* blittable */ struct ID3D12LibraryReflection
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDesc(
            [In] ID3D12LibraryReflection* This,
            [Out] D3D12_LIBRARY_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12FunctionReflection* GetFunctionByIndex(
            [In] ID3D12LibraryReflection* This,
            [In, ComAliasName("INT")] int FunctionIndex
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public GetFunctionByIndex GetFunctionByIndex;
            #endregion
        }
        #endregion
    }
}
