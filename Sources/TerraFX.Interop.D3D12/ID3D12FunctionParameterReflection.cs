// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("EC25F42D-7006-4F2B-B33E-02CC3375733F")]
    unsafe public struct ID3D12FunctionParameterReflection
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12FunctionParameterReflection).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12FunctionParameterReflection* This,
            [Out] D3D12_PARAMETER_DESC* pDesc
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public GetDesc GetDesc;
            #endregion
        }
        #endregion
    }
}
