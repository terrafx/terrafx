// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace TerraFX.Interop
{
    [Guid("1108795C-2772-4BA9-B2A8-D464DC7E2799")]
    unsafe public struct ID3D12FunctionReflection
    {
        #region Constants
        public static readonly Guid IID = typeof(ID3D12FunctionReflection).GUID;
        #endregion

        #region Fields
        public void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12FunctionReflection* This,
            [Out] D3D12_FUNCTION_DESC* pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByIndex(
            [In] ID3D12FunctionReflection* This,
            [In] uint BufferIndex
        );

        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByName(
            [In] ID3D12FunctionReflection* This,
            [In] LPSTR Name
        );

        public /* static */ delegate HRESULT GetResourceBindingDesc(
            [In] ID3D12FunctionReflection* This,
            [In] uint ResourceIndex,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        public /* static */ delegate ID3D12ShaderReflectionVariable* GetVariableByName(
            [In] ID3D12FunctionReflection* This,
            [In] LPSTR Name
        );

        public /* static */ delegate HRESULT GetResourceBindingDescByName(
            [In] ID3D12FunctionReflection* This,
            [In] LPSTR Name,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        // Use D3D_RETURN_PARAMETER_INDEX to get description of the return value.
        public /* static */ delegate ID3D12FunctionParameterReflection* GetFunctionParameter(
            [In] ID3D12FunctionReflection* This,
            [In] int ParameterIndex
        );
        #endregion

        #region Structs
        public struct Vtbl
        {
            #region Fields
            public GetDesc GetDesc;

            public GetConstantBufferByIndex GetConstantBufferByIndex;

            public GetConstantBufferByName GetConstantBufferByName;

            public GetResourceBindingDesc GetResourceBindingDesc;

            public GetVariableByName GetVariableByName;

            public GetResourceBindingDescByName GetResourceBindingDescByName;

            public GetFunctionParameter GetFunctionParameter;
            #endregion
        }
        #endregion
    }
}
