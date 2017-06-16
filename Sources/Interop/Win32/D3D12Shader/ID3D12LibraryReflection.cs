// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("8E349D19-54DB-4A56-9DC9-119D87BDB804")]
    unsafe public struct ID3D12LibraryReflection
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12LibraryReflection).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12LibraryReflection* This,
            [Out] D3D12_LIBRARY_DESC* pDesc
        );

        public /* static */ delegate ID3D12FunctionReflection* GetFunctionByIndex(
            [In] ID3D12LibraryReflection* This,
            [In] int FunctionIndex
        );
        #endregion

        #region Structs
        public struct Vtbl
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
