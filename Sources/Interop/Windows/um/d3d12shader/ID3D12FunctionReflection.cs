// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("1108795C-2772-4BA9-B2A8-D464DC7E2799")]
    unsafe public /* blittable */ struct ID3D12FunctionReflection
    {
        #region Fields
        public readonly Vtbl* lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetDesc(
            [In] ID3D12FunctionReflection* This,
            [Out] D3D12_FUNCTION_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByIndex(
            [In] ID3D12FunctionReflection* This,
            [In, ComAliasName("UINT")] uint BufferIndex
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByName(
            [In] ID3D12FunctionReflection* This,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* Name
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetResourceBindingDesc(
            [In] ID3D12FunctionReflection* This,
            [In, ComAliasName("UINT")] uint ResourceIndex,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionVariable* GetVariableByName(
            [In] ID3D12FunctionReflection* This,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* Name
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        [return: ComAliasName("HRESULT")]
        public /* static */ delegate int GetResourceBindingDescByName(
            [In] ID3D12FunctionReflection* This,
            [In, ComAliasName("LPCSTR")] /* readonly */ sbyte* Name,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12FunctionParameterReflection* GetFunctionParameter(
            [In] ID3D12FunctionReflection* This,
            [In, ComAliasName("INT")] int ParameterIndex
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IntPtr GetDesc;

            public IntPtr GetConstantBufferByIndex;

            public IntPtr GetConstantBufferByName;

            public IntPtr GetResourceBindingDesc;

            public IntPtr GetVariableByName;

            public IntPtr GetResourceBindingDescByName;

            public IntPtr GetFunctionParameter;
            #endregion
        }
        #endregion
    }
}
