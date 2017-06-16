// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("C59598B4-48B3-4869-B9B1-B1618B14A8B7")]
    unsafe public struct ID3D12ShaderReflectionConstantBuffer
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12ShaderReflectionConstantBuffer).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12ShaderReflectionConstantBuffer* This,
            [Out] D3D12_SHADER_BUFFER_DESC* pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionVariable* GetVariableByIndex(
            [In] ID3D12ShaderReflectionConstantBuffer* This,
            [In] uint Index
        );
        public /* static */ delegate ID3D12ShaderReflectionVariable* GetVariableByName(
            [In] ID3D12ShaderReflectionConstantBuffer* This,
            [In] LPSTR Name
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public GetDesc GetDesc;

            public GetVariableByIndex GetVariableByIndex;

            public GetVariableByName GetVariableByName;
            #endregion
        }
        #endregion
    }
}
