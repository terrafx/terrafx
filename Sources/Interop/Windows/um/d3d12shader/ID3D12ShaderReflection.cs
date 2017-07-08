// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License MIT. See License.md in the repository root for more information.

// Ported from um\d3d12shader.h in the Windows SDK for Windows 10.0.15063.0
// Original source is Copyright © Microsoft. All rights reserved.

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace TerraFX.Interop
{
    [Guid("5A58797D-A72C-478D-8BA2-EFC6B0EFE88E")]
    unsafe public /* blittable */ struct ID3D12ShaderReflection
    {
        #region Fields
        public readonly void* /* Vtbl* */ lpVtbl;
        #endregion

        #region Delegates
        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetDesc(
            [In] ID3D12ShaderReflection* This,
            [Out] D3D12_SHADER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByIndex(
            [In] ID3D12ShaderReflection* This,
            [In] UINT Index
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionConstantBuffer* GetConstantBufferByName(
            [In] ID3D12ShaderReflection* This,
            [In] LPCSTR Name
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetResourceBindingDesc(
            [In] ID3D12ShaderReflection* This,
            [In] UINT ResourceIndex,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetInputParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In] UINT ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetOutputParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In] UINT ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetPatchConstantParameterDesc(
            [In] ID3D12ShaderReflection* This,
            [In] UINT ParameterIndex,
            [Out] D3D12_SIGNATURE_PARAMETER_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate ID3D12ShaderReflectionVariable* GetVariableByName(
            [In] ID3D12ShaderReflection* This,
            [In] LPCSTR Name
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetResourceBindingDescByName(
            [In] ID3D12ShaderReflection* This,
            [In] LPCSTR Name,
            [Out] D3D12_SHADER_INPUT_BIND_DESC* pDesc
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetMovInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetMovcInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetConversionInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetBitwiseInstructionCount(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate D3D_PRIMITIVE GetGSInputPrimitive(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate BOOL IsSampleFrequencyShader(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetNumInterfaceSlots(
            [In] ID3D12ShaderReflection* This
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate HRESULT GetMinFeatureLevel(
            [In] ID3D12ShaderReflection* This,
            [Out] D3D_FEATURE_LEVEL* pLevel
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT GetThreadGroupSize(
            [In] ID3D12ShaderReflection* This,
            [Out, Optional] UINT* pSizeX,
            [Out, Optional] UINT* pSizeY,
            [Out, Optional] UINT* pSizeZ
        );

        [SuppressUnmanagedCodeSecurity]
        [UnmanagedFunctionPointer(CallingConvention.ThisCall, BestFitMapping = false, CharSet = CharSet.Unicode, SetLastError = false, ThrowOnUnmappableChar = false)]
        public /* static */ delegate UINT64 GetRequiresFlags(
            [In] ID3D12ShaderReflection* This
        );
        #endregion

        #region Structs
        public /* blittable */ struct Vtbl
        {
            #region Fields
            public IUnknown.Vtbl BaseVtbl;

            public GetDesc GetDesc;

            public GetConstantBufferByIndex GetConstantBufferByIndex;

            public GetConstantBufferByName GetConstantBufferByName;

            public GetResourceBindingDesc GetResourceBindingDesc;

            public GetInputParameterDesc GetInputParameterDesc;

            public GetOutputParameterDesc GetOutputParameterDesc;

            public GetPatchConstantParameterDesc GetPatchConstantParameterDesc;

            public GetVariableByName GetVariableByName;

            public GetResourceBindingDescByName GetResourceBindingDescByName;

            public GetMovInstructionCount GetMovInstructionCount;

            public GetMovcInstructionCount GetMovcInstructionCount;

            public GetConversionInstructionCount GetConversionInstructionCount;

            public GetBitwiseInstructionCount GetBitwiseInstructionCount;

            public GetGSInputPrimitive GetGSInputPrimitive;

            public IsSampleFrequencyShader IsSampleFrequencyShader;

            public GetNumInterfaceSlots GetNumInterfaceSlots;

            public GetMinFeatureLevel GetMinFeatureLevel;

            public GetThreadGroupSize GetThreadGroupSize;

            public GetRequiresFlags GetRequiresFlags;
            #endregion
        }
        #endregion
    }
}
